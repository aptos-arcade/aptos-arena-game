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
            if (player.PlayerState.IsStunned) return;
            var targetSpeed = player.PlayerState.Direction.x * player.PlayerStats.Speed;
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

        private void SwapWeapon()
        {
            foreach (var weapon in player.PlayerReferences.WeaponObjects)
            {
                if(weapon.activeSelf) weapon.GetComponent<PhotonView>().RPC("UnEquip", RpcTarget.AllBuffered);
            }

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
            player.PlayerState.RangedEnergy -= player.PlayerStats.RangedAttack.Energy;
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

        public void Jump()
        {
            player.PlayerUtilities.JumpImpl(player.PlayerStats.JumpForce);
        }
        
        public void DoubleJump()
        {
            player.PlayerState.CanDoubleJump = false;
            player.PlayerUtilities.JumpImpl(player.PlayerStats.DoubleJumpForce);
        }
    }
}
