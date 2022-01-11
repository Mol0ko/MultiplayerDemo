using UnityEngine;

namespace MultiplayerDemo
{
    public class Bullet : MonoBehaviour
    {
        private void Update()
        {
            transform.localPosition += Vector3.forward * 5f * Time.deltaTime;
        }
    }
}