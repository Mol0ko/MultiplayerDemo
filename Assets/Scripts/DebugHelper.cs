using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MultiplayerDemo
{
    public class DebugHelper : MonoBehaviour
    {
        private static Text _logText;

        void OnStart() {
            _logText = GameObject.FindObjectsOfType<Text>().FirstOrDefault(obj => obj.tag == "Console");
        }

        public static void Log(object message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
#else
            _logText.text += "\n" + message;
#endif
        }
    }
}