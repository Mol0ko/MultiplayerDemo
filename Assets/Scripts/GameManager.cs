using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MultiplayerDemo
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private CameraFollowing _camera;
        [SerializeField]
        private GameObject _gameOverBanner;
        [SerializeField]
        private Text _winner;

        private PlayerController _player1;
        private PlayerController _player2;

        public bool GameOvered = false;

        private void Start()
        {
            if (Screen.fullScreen)
                Screen.fullScreen = false;
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
            if (player.name.Contains("1"))
                _player1 = player;
            else if (player.name.Contains("2"))
                _player2 = player;

            if (_player1 != null && _player2 != null)
            {
                _player1.Target = _player2.transform;
                _player2.Target = _player1.transform;
            }

            if (player.IsMe)
                _camera.Target = player.transform;
        }

        public void GameOver(PlayerController looser)
        {
            GameOvered = true;
            if (looser.name.Contains("1"))
                _winner.text = "Player2";
            else if (looser.name.Contains("2"))
                _winner.text = "Player1";
            _gameOverBanner.SetActive(true);
            StartCoroutine(ShowMenuAfterDelay());
        }

        private IEnumerator ShowMenuAfterDelay()
        {
            yield return new WaitForSeconds(4);
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene("Menu");
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