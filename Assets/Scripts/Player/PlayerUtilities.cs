using System.Collections;
using System.Collections.Generic;
using Commands;
using Gameplay;
using Photon.Pun;
using UnityEngine;
using Weapons;

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
        }

        public void HandleInput()
        {
            if (!player.PlayerComponents.PhotonView.IsMine || !player.PlayerState.CanMove) return;
            player.PlayerState.Direction = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

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
            if (player.photonView.IsMine && player.PlayerState.CanMove && (Mathf.Abs(player.transform.position.x) > 30 || Mathf.Abs(player.transform.position.y) > 16))
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
        }
        
        private void OnDeath()
        {
            PhotonNetwork.Instantiate(player.PlayerReferences.ExplosionPrefab.name, player.transform.position, Quaternion.identity);
            player.photonView.RPC("OnDeath", RpcTarget.AllBuffered);
            MatchManager.Instance.SetEnergyUIActive(false);
            MatchManager.PlayerDeathSend(player.photonView.Owner.ActorNumber);
        }

        public void StrikerCollision(Striker striker)
        {
            player.photonView.RPC(
                "OnStrike",
                RpcTarget.AllBuffered,
                striker.KnockBackSignedDirection,
                striker.KnockBackForce,
                striker.Damage
            );
        }

        public void HandleStrike(Vector2 direction, float knockBack, float damage)
        {
            player.PlayerComponents.RigidBody.AddForce(direction.normalized *
                                                       (knockBack * Mathf.Pow(player.PlayerState.DamageMultiplier,
                                                           player.PlayerStats.KnockBackPower)));
            player.PlayerState.DamageMultiplier += damage;
            player.PlayerReferences.DamageDisplay.text = ((player.PlayerState.DamageMultiplier - 1) * 100).ToString("F0") + "%";
            player.StartCoroutine(player.PlayerComponents.PlayerCamera.Shake(0.2f, 0.1f));
            player.PlayerComponents.HitAudioSource.Play();
        }

        public void HurtEffect(bool hurt)
        {
            player.PlayerComponents.BodyCollider.enabled = !hurt;
            foreach (var renderer in player.PlayerComponents.PlayerSprites)
            {
                renderer.color = hurt ? Color.red : Color.white;
            }
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
            player.PlayerReferences.WeaponObjects[(int)player.PlayerState.Weapon].GetPhotonView()
                .RPC(isRevive ? "Equip" : "UnEquip", RpcTarget.AllBuffered);
            
            player.PlayerComponents.RigidBody.gravityScale = isRevive ? 5 : 0;
            player.PlayerComponents.FootCollider.enabled = isRevive;
            player.PlayerComponents.BodyCollider.enabled = isRevive;

            player.PlayerState.CanMove = isRevive;
            player.PlayerState.MeleeEnergy = isRevive ? 1 : 0;
            player.PlayerState.RangedEnergy = isRevive ? 1 : 0;
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
            }
        }

    }
}
