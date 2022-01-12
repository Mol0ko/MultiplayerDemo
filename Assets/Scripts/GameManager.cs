using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MultiplayerDemo
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        private PlayerController _player1;
        private PlayerController _player2;

        private void Start()
        {
            var playerPosition = new Vector3(
                Random.Range(-10, 10),
                0.5f,
                Random.Range(-10, 10));
            PhotonNetwork.Instantiate(
                "Player" + PhotonNetwork.NickName,
                playerPosition,
                new Quaternion());
        }

        public void PlayerAdded(PlayerController player)
        {
            if (player.name == "Player1")
                _player1 = player;
            else if (player.name == "Player2")
                _player2 = player;

            if (_player1 != null && _player2 != null) {
                _player1.Target = _player2.transform;
                _player2.Target = _player1.transform;
            }
        }

        public void OnQuit()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}