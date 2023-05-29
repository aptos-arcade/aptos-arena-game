using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UIComponents
{
    public class SceneButton : MonoBehaviour
    {
    
        [SerializeField] private Global.Scenes sceneToLoad;

        private Button button;

        // Start is called before the first frame update
        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() => SceneManager.LoadScene((int)sceneToLoad));
        }
    }
}
