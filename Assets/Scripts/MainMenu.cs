using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace MultiplayerDemo
{
    public class MainMenu : MonoBehaviourPunCallbacks
    {
        void Start()
        {
#if UNITY_EDITOR
            PhotonNetwork.NickName = "1";
#else
            PhotonNetwork.NickName = "2";
#endif
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = "0.0.1";
            PhotonNetwork.ConnectUsingSettings();
        }

        public void CreateRoom() {
            var roomOptions = new RoomOptions() {
                MaxPlayers = 2
            };
            PhotonNetwork.CreateRoom("room1", roomOptions);
        }

        public void JoinRoom() {
            PhotonNetwork.JoinRoom("room1");
        }

        public void Quit() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public override void OnConnectedToMaster() {
            DebugHelper.Log("OnConnectedToMaster");
            base.OnConnectedToMaster();
        }

        public override void OnJoinedRoom() {
            PhotonNetwork.LoadLevel("MainScene");
            base.OnJoinedRoom();
        }
    }
}