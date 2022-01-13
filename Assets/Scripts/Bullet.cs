using UnityEngine;

namespace MultiplayerDemo
{
    public class Bullet : MonoBehaviour
    {
        private void Update()
        {
            transform.position += transform.forward * 7f * Time.deltaTime;
        }
    }
}