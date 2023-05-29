using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class GameMenuManager : MonoBehaviour
    {

        [SerializeField] private GameObject gameMenu;
        [SerializeField] private Button leaveButton;
    
        // Start is called before the first frame update
        private void Start()
        {
            leaveButton.onClick.AddListener(MatchManager.Instance.LeaveRoom);
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) ToggleGameMenu();
        }
    
        private void ToggleGameMenu()
        {
            gameMenu.SetActive(!gameMenu.activeSelf);
        }
    }
}
