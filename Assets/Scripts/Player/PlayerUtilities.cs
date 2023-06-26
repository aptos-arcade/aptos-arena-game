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
            commands.Add(new DashCommand(player, KeyCode.LeftArrow));
            commands.Add(new DashCommand(player, KeyCode.RightArrow));
        }

        public void HandleInput()
        {
            if (!player.PlayerComponents.PhotonView.IsMine) return;
            
            if(Input.anyKeyDown) player.PlayerState.IsStunned = false;

            if (player.PlayerState.CanMove)
            {
                var x = Input.GetAxisRaw("Horizontal");
                player.PlayerState.Direction = new Vector2(x, 0);
            } 
            else
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
                player.PlayerState.CanDodge = true;
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
            PhotonNetwork.Instantiate(player.PlayerReferences.ExplosionPrefab.name, player.transform.position, Quaternion.identity);
            player.photonView.RPC("OnDeath", RpcTarget.AllBuffered);
            MatchManager.Instance.SetEnergyUIActive(false);
            MatchManager.PlayerDeathSend(player.photonView.OwnerActorNr, player.PlayerState.StrikerActorNumber);
            player.PlayerState.StrikerActorNumber = 0;
        }

        public void StrikerCollision(Striker striker)
        {
            player.StartCoroutine(HurtCoroutine());
            player.photonView.RPC(
                "OnStrike",
                RpcTarget.AllBuffered,
                striker.KnockBackSignedDirection,
                striker.KnockBackForce,
                striker.Damage,
                striker.photonView.OwnerActorNr
            );
        }

        public void ShieldCollision(Striker striker)
        {
            player.photonView.RPC("OnShieldStrike", RpcTarget.AllBuffered, striker.Damage);
        }

        public void HurtEffect(bool hurt)
        {
            if (hurt)
            {
                player.PlayerState.IsStunned = true;
            }
            player.PlayerState.CanMove = !hurt;
            player.PlayerState.IsInvincible = hurt;
            foreach (var renderer in player.PlayerComponents.PlayerSprites)
            {
                renderer.color = hurt ? Color.red : Color.white;
            }
        }
        
        private IEnumerator HurtCoroutine()
        {
            player.PlayerComponents.PhotonView.RPC("HurtEffect", RpcTarget.AllBuffered, true);
            yield return new WaitForSeconds(0.5f);
            player.PlayerComponents.PhotonView.RPC("HurtEffect", RpcTarget.AllBuffered, false);
        }

        public void DodgeEffect(bool dodging)
        {
            player.PlayerState.CanMove = !dodging;
            player.PlayerState.IsDodging = dodging;
            player.PlayerComponents.BodyCollider.enabled = !dodging;
            player.PlayerComponents.RigidBody.gravityScale = dodging ? 0 : 5;
            foreach (var renderer in player.PlayerComponents.PlayerSprites)
            {
                var dodgeColor = Color.white;
                dodgeColor.a = 0.5f;
                renderer.color = dodging ? dodgeColor : Color.white;
            }
        }
        
        public void DashEffect(bool dashing)
        {
            player.PlayerState.CanMove = !dashing;
            player.PlayerState.IsDashing = dashing;
        }

        public IEnumerator SpawnCoroutine(Vector3 spawnPosition)
        {
            player.transform.position = spawnPosition;
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
                player.PlayerComponents.PlayerSprites.Add(transform.GetComponent<SpriteRenderer>());
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
            player.PlayerState.IsStunned = !isRevive;
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
            player.PlayerComponents.JumpAudioSource.Play();
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
                    player.StartCoroutine(player.PlayerActions.DashCoroutine());
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

    }
}
