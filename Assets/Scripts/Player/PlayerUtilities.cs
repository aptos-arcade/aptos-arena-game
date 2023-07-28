using System.Collections;
using System.Collections.Generic;
using Characters;
using Commands;
using Gameplay;
using Photon.Pun;
using UnityEngine;
using Weapons;
using static Photon.PlayerPropertyKeys;

namespace Player
{
    public class PlayerUtilities
    {
        private readonly PlayerScript player;

        private readonly List<Command> commands = new();
        
        public bool IsOnGround => player.PlayerComponents.FootCollider.IsTouchingLayers(player.PlayerComponents.Ground.value);
        public bool IsOnPlatform => player.PlayerComponents.FootCollider.IsTouchingLayers(player.PlayerComponents.Platform.value);
        public bool IsGrounded => IsOnGround || IsOnPlatform;
        private bool IsFalling => player.PlayerComponents.RigidBody.velocity.y < 0 && !IsGrounded;
        
        public bool IsDashing => player.PlayerComponents.Animator.CurrentAnimationBody == "Body_Dash";
        public bool IsDodging => player.PlayerComponents.Animator.CurrentAnimationBody == "Body_Dodge";
        public bool IsStunned => player.PlayerComponents.Animator.CurrentAnimationBody == "Body_Stunned";
        public bool IsShielding => player.PlayerComponents.Animator.CurrentAnimationBody == "Body_Shield";

        private Coroutine hurtCoroutine;
        private Coroutine shieldStunCoroutine;

        public PlayerUtilities(PlayerScript player)
        {
            this.player = player;
            commands.Add(new JumpCommand(player, KeyCode.UpArrow));
            commands.Add(new DropCommand(player, KeyCode.DownArrow));
            commands.Add(new ShootCommand(player, KeyCode.Space));
            commands.Add(new JabMeleeCommand(player, KeyCode.S));
            commands.Add(new UpMeleeCommand(player, KeyCode.W));
            commands.Add(new SideMeleeCommand(player, KeyCode.A, -1));
            commands.Add(new SideMeleeCommand(player, KeyCode.D, 1));
            commands.Add(new ShieldCommand(player, KeyCode.LeftShift));
            commands.Add(new DashCommand(player, KeyCode.LeftArrow, -1));
            commands.Add(new DashCommand(player, KeyCode.RightArrow, 1));
        }

        public void HandleInput()
        {
            if (!player.PlayerComponents.PhotonView.IsMine || player.PlayerState.IsDisabled) return;

            if (IsStunned && Input.anyKeyDown)
            {
                player.PlayerComponents.Animator.TryPlayAnimation("Body_Idle");
                player.PlayerComponents.Animator.TryPlayAnimation("Legs_Idle");
            }

            if (player.PlayerState.CanMove)
            {
                var x = Input.GetAxisRaw("Horizontal");
                if (x != 0 || !player.PlayerUtilities.IsDashing)
                {
                    player.PlayerState.Direction = new Vector2(x, 0);
                }
            }
            else if(!player.PlayerUtilities.IsDodging)
            {
                player.PlayerState.Direction = Vector2.zero;
            }

            foreach (var command in commands)
            {
                if (Input.GetKeyDown(command.Key))
                {
                    command.GetKeyDown();
                }

                if (Input.GetKeyUp(command.Key))
                {
                    command.GetKeyUp();
                }

                if (Input.GetKey(command.Key))
                {
                    command.GetKey();
                }
            }
        }

        public void HandleAir()
        {
            if(IsFalling)
            {
                player.PlayerComponents.Animator.TryPlayAnimation("Body_Fall");
                player.PlayerComponents.Animator.TryPlayAnimation("Legs_Fall");
            }
            if(IsGrounded)
            {
                player.PlayerState.CanDoubleJump = true;
            }
        }

        public void HandleDeath()
        {
            if (player.photonView.IsMine && !player.PlayerState.IsDead && (Mathf.Abs(player.transform.position.x) > 30 || Mathf.Abs(player.transform.position.y) > 16))
            {
                OnDeath();
            }
        }

        public void HandleEnergy()
        {
            if(player.PlayerState.MeleeEnergy < 1)
            {
                player.PlayerState.MeleeEnergy += Time.deltaTime / player.PlayerStats.MeleeEnergyRegenTime;
            }
            if(player.PlayerState.RangedEnergy < 1)
            {
                player.PlayerState.RangedEnergy += Time.deltaTime / player.PlayerStats.RangedEnergyRegenTime;
            }
            if(player.PlayerState.ShieldEnergy < 1 && !player.PlayerReferences.PlayerShield.gameObject.activeSelf)
            {
                player.PlayerState.ShieldEnergy += Time.deltaTime / player.PlayerStats.ShieldEnergyRegenTime;
            }
        }
        
        private void OnDeath()
        {
            MatchManager.Instance.SetPlayerCameraActive(false);
            PhotonNetwork.Instantiate(player.PlayerReferences.ExplosionPrefab.name, player.transform.position, Quaternion.identity);
            player.photonView.RPC("OnDeath", RpcTarget.AllBuffered);
            MatchManager.Instance.SetEnergyUIActive(false);
            MatchManager.PlayerDeathSend(player.photonView.OwnerActorNr, player.PlayerState.StrikerActorNumber);
            player.PlayerState.StrikerActorNumber = 0;
        }

        public void StrikerCollision(Striker striker)
        {
            if(hurtCoroutine != null) player.StopCoroutine(hurtCoroutine);
            hurtCoroutine = player.StartCoroutine(HurtCoroutine(striker));
            player.photonView.RPC(
                "OnStrike",
                RpcTarget.AllBuffered,
                striker.KnockBackSignedDirection,
                striker.strikerData.KnockBack,
                striker.strikerData.Damage,
                striker.photonView.OwnerActorNr
            );
        }

        private IEnumerator HurtCoroutine(Striker striker)
        {
            player.PlayerComponents.PhotonView.RPC("HurtEffect", RpcTarget.AllBuffered, true);
            yield return new WaitForSeconds(striker.strikerData.StunTime * player.PlayerState.DamageMultiplier);
            player.PlayerComponents.PhotonView.RPC("HurtEffect", RpcTarget.AllBuffered, false);
            hurtCoroutine = null;
        }

        public void ShieldCollision(Striker striker)
        {
            player.photonView.RPC("OnShieldStrike", RpcTarget.AllBuffered, striker.strikerData.Damage);
        }
        
        public void StunEffect(bool stunned, Color effectColor)
        {
            if (stunned)
            {
                player.PlayerComponents.Animator.TryPlayAnimation("Body_Stunned");
                player.PlayerComponents.Animator.TryPlayAnimation("Legs_Stunned");
            }
            player.PlayerState.IsDisabled = stunned;
            player.PlayerState.CanMove = !stunned;
            for (var i = 0; i < player.PlayerComponents.PlayerSprites.Count; i++)
            {
                player.PlayerComponents.PlayerSprites[i].color = stunned 
                    ? effectColor
                    : player.PlayerComponents.PlayerSpriteColors[i];
            }
        }

        public void ShieldHit(PlayerShield shield)
        {
            if(shieldStunCoroutine != null) player.StopCoroutine(shieldStunCoroutine);
            shieldStunCoroutine = player.StartCoroutine(ShieldStunCoroutine(shield));
        }

        private IEnumerator ShieldStunCoroutine(PlayerShield shield)
        {
            player.PlayerComponents.PhotonView.RPC("ShieldStunEffect", RpcTarget.AllBuffered, true);
            yield return new WaitForSeconds(shield.ShieldStunDuration);
            player.PlayerComponents.PhotonView.RPC("ShieldStunEffect", RpcTarget.AllBuffered, false);
            shieldStunCoroutine = null;
        }

        public void DodgeEffect(bool dodging)
        {
            player.PlayerState.CanMove = !dodging;
            player.PlayerComponents.BodyCollider.enabled = !dodging;
            player.PlayerComponents.RigidBody.gravityScale = dodging ? 0 : 5;
            foreach (var renderer in player.PlayerComponents.PlayerSprites)
            {
                var color = renderer.color;
                color.a = dodging ? 0.5f : 1;
                renderer.color = color;
            }
        }

        public IEnumerator SpawnCoroutine(Vector3 spawnPosition)
        {
            player.transform.position = spawnPosition;
            MatchManager.Instance.SetPlayerCameraActive(true);
            var portal = PhotonNetwork.Instantiate(
                player.PlayerReferences.Portal.name,
                spawnPosition,
                Quaternion.identity
            );
            yield return new WaitForSeconds(2.5f);
            PhotonNetwork.Destroy(portal);
            if(player.photonView.IsMine) MatchManager.Instance.SetEnergyUIActive(true);
            player.photonView.RPC("OnRevive", RpcTarget.AllBuffered);
            player.photonView.RPC("TriggerInvincibility", RpcTarget.AllBuffered, true);
            yield return new WaitForSeconds(5f);
            if (player.PlayerState.IsInvincible)
            {
                player.photonView.RPC("TriggerInvincibility", RpcTarget.AllBuffered, false);
            }
        }

        public void GetSpriteRenderers()
        {
            foreach (Transform transform in player.PlayerReferences.PlayerMesh.transform)
            {
                var spriteRenderer = transform.GetComponent<SpriteRenderer>();
                player.PlayerComponents.PlayerSprites.Add(spriteRenderer);
                player.PlayerComponents.PlayerSpriteColors.Add(spriteRenderer.color);
            }
        }

        private void SetPlayerEnabled(bool enabled)
        {
            player.PlayerComponents.PlayerSprites.ForEach(sprite => sprite.enabled = enabled);
        }
        
        public void DeathRevive(bool isRevive)
        {
            SetPlayerEnabled(isRevive);
            
            player.PlayerReferences.PlayerCanvas.SetActive(isRevive);

            player.PlayerComponents.RigidBody.gravityScale = isRevive ? 5 : 0;
            player.PlayerComponents.FootCollider.enabled = isRevive;
            player.PlayerComponents.BodyCollider.enabled = isRevive;

            player.PlayerState.IsDead = !isRevive;
            player.PlayerState.CanMove = isRevive;
            player.PlayerState.MeleeEnergy = isRevive ? 1 : 0;
            player.PlayerState.RangedEnergy = isRevive ? 1 : 0;

            if (isRevive)
            {
                player.PlayerReferences.WeaponObjects[(int)player.PlayerState.Weapon].Equip();
            }
            else
            {
                player.PlayerActions.UnEquipWeapons();
            }
        }

        public void JumpImpl(float jumpForce)
        {
            player.PlayerComponents.RigidBody.velocity = new Vector2(player.PlayerComponents.RigidBody.velocity.x, 0);
            player.PlayerComponents.RigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Force);
            PlayOneShotAudio(player.PlayerReferences.JumpAudioClip);
        }

        public void HandleAnimation(string animation)
        {
            if (!player.photonView.IsMine) return;
            switch (animation)
            {
                case "Shoot":
                    player.PlayerActions.Shoot();
                    break;
                case "Side_Melee":
                    player.PlayerActions.SideMelee();
                    break;
                case "Up_Melee":
                    player.PlayerActions.UpMelee();
                    break;
                case "Jab_Melee":
                    player.PlayerActions.JabMelee();
                    break;
                case "Down_Melee":
                    player.PlayerActions.DownMelee();
                    break;
                case "Jump":
                    player.PlayerActions.Jump();
                    break;
                case "Double_Jump":
                    player.PlayerActions.DoubleJump();
                    break;
                case "Shield":
                    player.PlayerActions.TriggerShield(true);
                    break;
                case "Dodge":
                    player.StartCoroutine(player.PlayerActions.DodgeCoroutine());
                    break;
                case "Dash":
                    player.PlayerActions.Dash();
                    break;
                case "FastFall":
                    player.PlayerActions.FastFall();
                    break;
            }
        }
        
        public bool IsSameTeam(PhotonView other)
        {
            return (CharactersEnum)other.Owner.CustomProperties[TeamKey] ==
                   (CharactersEnum)player.photonView.Owner.CustomProperties[TeamKey];
        }
        
        public void TriggerInvincibility(bool isInvincible)
        {
            if(player.PlayerState.IsInvincible == isInvincible) return;
            player.PlayerState.IsInvincible = isInvincible;
            foreach (var renderer in player.PlayerComponents.PlayerSprites)
            {
                var color = renderer.color;
                color.a = isInvincible ? 0.5f : 1;
                renderer.color = color;
            }
        }

        public void PlayOneShotAudio(AudioClip clip)
        {
            player.PlayerComponents.OneShotAudioSource.Stop();
            player.PlayerComponents.OneShotAudioSource.PlayOneShot(clip);
        }

    }
}
