using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class EnergyUIController : MonoBehaviour
    {

        [Header("Gun Energy")]
        [SerializeField] private Slider gunEnergySlider;
        [SerializeField] private Image gunEnergyFill;
        [SerializeField] private Color gunEnergyColor;

        [Header("Sword Energy")]
        [SerializeField] private Slider swordEnergySlider;
        [SerializeField] private Image swordEnergyFill;
        [SerializeField] private Color swordEnergyColor;
        
        [Header("Audio Clips")]
        [SerializeField] private AudioClip noEnergyAudioClip;
        
        
        private AudioSource audioSource;
        
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            SetRangedEnergy();
            SetSwordEnergy();
        }

        private void SetRangedEnergy()
        {
            var energy = MatchManager.Instance.Player.PlayerState.RangedEnergy;
            gunEnergySlider.value = energy;
        }

        private void SetSwordEnergy()
        {
            var energy = MatchManager.Instance.Player.PlayerState.MeleeEnergy;
            swordEnergySlider.value = energy;
        }
        
        public void NoEnergy(Global.Weapons weapon)
        {
            switch (weapon)
            {
                case Global.Weapons.Gun:
                    StartCoroutine(NoRangedEnergyCor());
                    break;
                case Global.Weapons.Sword:
                    StartCoroutine(NoSwordEnergyCor());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(weapon), weapon, null);
            }
        }
        
        private IEnumerator NoRangedEnergyCor()
        {
            gunEnergyFill.color = Color.red;
            PlayNoEnergySound();
            yield return new WaitForSeconds(0.5f);
            gunEnergyFill.color = gunEnergyColor;
        }

        private IEnumerator NoSwordEnergyCor()
        {
            swordEnergyFill.color = Color.red;
            PlayNoEnergySound();
            yield return new WaitForSeconds(0.5f);
            swordEnergyFill.color = swordEnergyColor;
        }
            
        private void PlayNoEnergySound()
        {
            audioSource.Stop();
            audioSource.PlayOneShot(noEnergyAudioClip);
        }
        
        
    }
}