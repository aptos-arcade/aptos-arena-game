using Photon.Pun;
using UnityEngine.SceneManagement;

namespace Matchmaking
{
    public class RoomsManager : MonoBehaviourPunCallbacks
    {
        public override void OnJoinedRoom()
        {
            SceneManager.LoadScene("MatchmakingScene");
        }
    }
}
