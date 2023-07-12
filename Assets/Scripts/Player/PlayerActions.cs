using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace Player
{
    public class PlayerActions
    {

        private readonly PlayerScript player;

        public PlayerActions(PlayerScript player)
        {
            this.player = player;
        }

        public void Move(Transform transform)
        {
            if (player.PlayerState.IsStunned)
            {
                player.PlayerComponents.RunAudioSource.Stop();
                return;
            }

            float targetSpeed;
            if (player.PlayerUtilities.IsDashing)
            {
                targetSpeed = player.PlayerState.Direction.x * player.PlayerStats.DashVelocity;
            }
            else if (player.PlayerUtilities.IsDodging)
            {
                targetSpeed = player.PlayerState.Direction.x * player.PlayerStats.DodgeVelocity;
            }
            else
            {
                targetSpeed = player.PlayerState.Direction.x * player.PlayerStats.Speed;
            }

            var speedDiff = targetSpeed - player.PlayerComponents.RigidBody.velocity.x;
            var accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? player.PlayerStats.Acceleration : player.PlayerStats.Deceleration;
            var movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, player.PlayerStats.VelPower) * Mathf.Sign(speedDiff);
            player.PlayerComponents.RigidBody.AddForce(movement * Vector2.right);

            if(player.PlayerState.Direction.x != 0)
            {
                var direction = player.PlayerState.Direction.x < 0 ? -1 : 1;
                transform.localScale = new Vector3(direction, 1, 1);
                player.PlayerReferences.PlayerCanvas.transform.localScale = new Vector3(direction, 1, 1);
                player.PlayerComponents.Animator.TryPlayAnimation("Body_Walk");
                player.PlayerComponents.Animator.TryPlayAnimation("Legs_Walk");
                if (!player.PlayerComponents.RunAudioSource.isPlaying)
                {
                    player.PlayerComponents.RunAudioSource.Play();
                }
            }
            else if(player.PlayerComponents.RigidBody.velocity.magnitude < 0.1f && player.PlayerUtilities.IsGrounded)
            {
                player.PlayerComponents.Animator.TryPlayAnimation("Body_Idle");
                player.PlayerComponents.Animator.TryPlayAnimation("Legs_Idle");
            }
            
            if(!player.PlayerUtilities.IsGrounded || player.PlayerComponents.RigidBody.velocity.magnitude < 0.1f || 
               player.PlayerUtilities.IsDashing || player.PlayerUtilities.IsDodging)
            {
                player.PlayerComponents.RunAudioSource.Stop();
            }
        }

        public void TryJump()
        {
            if (player.PlayerUtilities.IsGrounded)
            {
                player.PlayerComponents.Animator.TryPlayAnimation("Legs_Jump");
                player.PlayerComponents.Animator.TryPlayAnimation("Body_Jump");
            }
            else if(player.PlayerState.CanDoubleJump)
            {
                player.PlayerComponents.Animator.TryPlayAnimation("Legs_Double_Jump");
                player.PlayerComponents.Animator.TryPlayAnimation("Body_Double_Jump");
            }
        }

        public void Attack()
        {
            if (player.PlayerState.IsInvincible)
            {
                player.photonView.RPC("TriggerInvincibility", RpcTarget.AllBuffered, false);
            }
            player.PlayerComponents.Animator.TryPlayAnimation("Legs_Attack");
            player.PlayerComponents.Animator.TryPlayAnimation("Body_Attack");
        }

        public void TrySwapWeapon(Global.Weapons weapon)
        {
            if(weapon == player.PlayerState.Weapon) return;
            player.PlayerState.Weapon = weapon;
            player.PlayerComponents.Animator.SetWeapon((int)player.PlayerState.Weapon);
            SwapWeapon();
        }

        public void UnEquipWeapons()
        {
            foreach (var weapon in player.PlayerReferences.WeaponObjects)
            {
                if(weapon.gameObject.activeSelf) 
                    weapon.photonView.RPC("UnEquip", RpcTarget.AllBuffered);
            }
        }

        private void SwapWeapon()
        {
            UnEquipWeapons();
            player.PlayerReferences.WeaponObjects[(int)player.PlayerState.Weapon].GetComponent<PhotonView>()
                .RPC("Equip", RpcTarget.AllBuffered);
        }

        private void PlayWeaponSound(AudioClip clip)
        {
            player.PlayerReferences.WeaponObjects[(int)player.PlayerState.Weapon].GetComponent<Weapons.Weapon>()
                .PlaySound(clip);
        }

        public void Drop()
        {
            player.StartCoroutine(DropCoroutine());
        }

        private IEnumerator DropCoroutine()
        {
            player.PlayerComponents.FootCollider.enabled = false;
            yield return new WaitForSeconds(0.25f);
            player.PlayerComponents.FootCollider.enabled = true;
        }

        public void Shoot()
        {
            player.PlayerReferences.Gun.Shoot(player);
            PlayWeaponSound(player.PlayerReferences.ShootAudioClip);
            player.PlayerState.RangedEnergy -= player.PlayerReferences.Gun.ProjectilePrefab.strikerData.Energy;
        }

        public void SideMelee()
        {
            PlayWeaponSound(player.PlayerReferences.SideMeleeAudioClip);
            player.PlayerState.MeleeEnergy -= player.PlayerStats.SideMeleeAttack.Energy;
        }
        
        public void UpMelee()
        {
            PlayWeaponSound(player.PlayerReferences.UpMeleeAudioClip);
            player.PlayerState.MeleeEnergy -= player.PlayerStats.UpMeleeAttack.Energy;
        }
        
        public void JabMelee()
        {
            PlayWeaponSound(player.PlayerReferences.JabMeleeAudioClip);
            player.PlayerState.MeleeEnergy -= player.PlayerStats.JabMeleeAttack.Energy;
        }
        
        public void DownMelee()
        {
            PlayWeaponSound(player.PlayerReferences.SideMeleeAudioClip);
            player.PlayerState.MeleeEnergy -= player.PlayerStats.DownMeleeAttack.Energy;
        }

        public void Jump()
        {
            player.PlayerUtilities.JumpImpl(player.PlayerStats.JumpForce);
        }
        
        public void DoubleJump()
        {
            player.PlayerState.CanDoubleJump = false;
            player.PlayerUtilities.JumpImpl(player.PlayerStats.DoubleJumpForce);
        }

        public void TryShield()
        {
            player.PlayerComponents.Animator.TryPlayAnimation("Body_Shield");
            player.PlayerComponents.Animator.TryPlayAnimation("Legs_Shield");
        }

        public void TriggerShield(bool active)
        {
            player.PlayerReferences.PlayerShield.photonView.RPC("TriggerShield", RpcTarget.AllBuffered, active);
        }

        public void TryDodge()
        {
            player.PlayerComponents.Animator.TryPlayAnimation("Body_Dodge");
            player.PlayerComponents.Animator.TryPlayAnimation("Legs_Dodge");
        }

        public IEnumerator DodgeCoroutine()
        {
            player.PlayerUtilities.PlayOneShotAudio(player.PlayerReferences.DodgeAudioClip);
            player.PlayerState.CanDodge = false;
            player.PlayerComponents.RigidBody.velocity = Vector2.zero;
            player.photonView.RPC("DodgeEffect", RpcTarget.AllBuffered, true);
            yield return new WaitForSeconds(0.35f);
            player.photonView.RPC("DodgeEffect", RpcTarget.AllBuffered, false);
        }
        
        public void TryDash()
        {
            player.PlayerComponents.Animator.TryPlayAnimation("Body_Dash");
            player.PlayerComponents.Animator.TryPlayAnimation("Legs_Dash");
        }

        public void Dash()
        {
            player.PlayerUtilities.PlayOneShotAudio(player.PlayerReferences.DashAudioClip);
            player.PlayerComponents.RigidBody.velocity = Vector2.zero;
        }

        public void FastFall()
        {
            player.PlayerComponents.RigidBody.velocity = new Vector2(player.PlayerComponents.RigidBody.velocity.x, 0);
            player.PlayerComponents.RigidBody.AddForce(Vector2.down * player.PlayerStats.FastFallForce);
        }
    }
}
