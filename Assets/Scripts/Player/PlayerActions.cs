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
            else if(player.PlayerComponents.RigidBody.velocity.magnitude < 0.1f)
            {
                player.PlayerComponents.Animator.TryPlayAnimation("Body_Idle");
                player.PlayerComponents.Animator.TryPlayAnimation("Legs_Idle");
                player.PlayerComponents.RunAudioSource.Stop();
            }
        }

        public void Jump()
        {
            if (player.PlayerUtilities.IsGrounded)
            {
                JumpImpl(player.PlayerStats.JumpForce);
            }
            else if(player.PlayerState.CanDoubleJump)
            {
                player.PlayerState.CanDoubleJump = false;
                JumpImpl(player.PlayerStats.DoubleJumpForce);
            }
        }

        private void JumpImpl(float jumpForce)
        {
            player.PlayerComponents.RigidBody.velocity = new Vector2(player.PlayerComponents.RigidBody.velocity.x, 0);
            player.PlayerComponents.RigidBody.AddForce(new Vector2(0, jumpForce));
            player.PlayerComponents.Animator.TryPlayAnimation("Legs_Jump");
            player.PlayerComponents.Animator.TryPlayAnimation("Body_Jump");
            player.PlayerComponents.JumpAudioSource.Play();
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

            player.PlayerReferences.WeaponObjects[(int)player.PlayerState.Weapon].GetComponent<PhotonView>().RPC("Equip", RpcTarget.AllBuffered);
        }

        private void PlayWeaponSound()
        {
            
            player.PlayerReferences.WeaponObjects[(int)player.PlayerState.Weapon].GetComponent<Weapons.Weapon>().PlaySound();
        }

        public void Drop()
        {
            player.StartCoroutine(DropCoroutine());
        }

        private IEnumerator DropCoroutine()
        {
            player.PlayerComponents.FootCollider.enabled = false;
            yield return new WaitForSeconds(0.2f);
            player.PlayerComponents.FootCollider.enabled = true;
        }

        public void Shoot(string animation)
        {
            if (animation != "Shoot" || !player.photonView.IsMine) return;
            
            var projectile = PhotonNetwork.Instantiate(player.PlayerReferences.ProjectilePrefab.name,
                player.PlayerReferences.GunBarrel.position, Quaternion.identity);
            var direction = new Vector2(player.transform.localScale.x, 0);
            projectile.GetComponent<PhotonView>().RPC("SetDirection", RpcTarget.All, direction);
            PlayWeaponSound();
            player.PlayerState.RangedEnergy -= player.PlayerStats.RangedAttackEnergyCost;
        }

        public void Melee(string animation)
        {
            if (animation != "Melee" || !player.photonView.IsMine) return;
            PlayWeaponSound();
            player.PlayerState.MeleeEnergy -= player.PlayerStats.MeleeAttackEnergyCost;
        }
    }
}
