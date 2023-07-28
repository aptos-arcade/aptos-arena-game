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

        [Header("Sword Energy")]
        [SerializeField] private Slider swordEnergySlider;
        [SerializeField] private Image swordEnergyFill;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip noEnergyAudioClip;
        
        private Color gunEnergyColor;
        private Color swordEnergyColor;
        private Color dashEnergyColor;
        
        private AudioSource audioSource;
        
        private Coroutine noSwordEnergyCoroutine;
        private Coroutine noRangedEnergyCoroutine;
        private Coroutine noDashEnergyCoroutine;
        
        public enum EnergyType
        {
            Gun,
            Sword,
        }
        
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            
            gunEnergyColor = gunEnergyFill.color;
            swordEnergyColor = swordEnergyFill.color;
        }

        private void Update()
        {
            SetRangedEnergy();
            SetSwordEnergy();
        }

        private void SetRangedEnergy()
        {
            gunEnergySlider.value = MatchManager.Instance.Player.PlayerState.RangedEnergy;
        }

        private void SetSwordEnergy()
        {
            swordEnergySlider.value = MatchManager.Instance.Player.PlayerState.MeleeEnergy;
        }

        public void NoEnergy(EnergyType energyType)
        {
            switch (energyType)
            {
                case EnergyType.Gun:
                    if (noRangedEnergyCoroutine != null) StopCoroutine(noRangedEnergyCoroutine);
                    noRangedEnergyCoroutine = StartCoroutine(NoRangedEnergyCor());
                    break;
                case EnergyType.Sword:
                    if(noSwordEnergyCoroutine != null) StopCoroutine(noSwordEnergyCoroutine);
                    noSwordEnergyCoroutine = StartCoroutine(NoSwordEnergyCor());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(energyType), energyType, null);
            }
        }
        
        private IEnumerator NoRangedEnergyCor()
        {
            gunEnergyFill.color = Color.red;
            PlayNoEnergySound();
            yield return new WaitForSeconds(0.5f);
            gunEnergyFill.color = gunEnergyColor;
            noRangedEnergyCoroutine = null;
        }

        private IEnumerator NoSwordEnergyCor()
        {
            swordEnergyFill.color = Color.red;
            PlayNoEnergySound();
            yield return new WaitForSeconds(0.5f);
            swordEnergyFill.color = swordEnergyColor;
            noSwordEnergyCoroutine = null;
        }

        private void PlayNoEnergySound()
        {
            audioSource.Stop();
            audioSource.PlayOneShot(noEnergyAudioClip);
        }
        
        
    }
}