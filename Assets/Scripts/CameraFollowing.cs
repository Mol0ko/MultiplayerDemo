
using UnityEngine;

namespace MultiplayerDemo
{
    public class CameraFollowing : MonoBehaviour
    {
        private float _cameraHeight = 20.0f;
        public Transform Target { get; set; }

        void Update()
        {
            if (Target != null && Target.position.y >= 0)
            {
                var position = Target.position;
                position.y += _cameraHeight;
                position.z -= 2f;
                transform.position = position;
            }
        }
    }
}