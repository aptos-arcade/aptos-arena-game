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

        private void Update()
        {
            SetRangedEnergy();
            SetSwordEnergy();
        }

        private void SetRangedEnergy()
        {
            var energy = MatchManager.Instance.Player.PlayerState.RangedEnergy;
            gunEnergyFill.color = energy < MatchManager.Instance.Player.PlayerStats.RangedAttackEnergyCost
                ? Color.red
                : Color.green;
            gunEnergySlider.value = energy;
        }

        private void SetSwordEnergy()
        {
            var energy = MatchManager.Instance.Player.PlayerState.MeleeEnergy;
            swordEnergyFill.color = energy < MatchManager.Instance.Player.PlayerStats.MeleeAttackEnergyCost
                ? Color.red
                : Color.green;
            swordEnergySlider.value = energy;
        }
    }
}