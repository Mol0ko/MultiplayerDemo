using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace MultiplayerDemo
{
    public class BulletEmitter : MonoBehaviour
    {
        [SerializeField]
        private Bullet _bulletPrefab;

        private Queue<Transform> _bulletPool = new Queue<Transform>();

        void Start()
        {
            StartCoroutine(CreateBulletRoutine());
        }

        private IEnumerator CreateBulletRoutine()
        {
            while (this.isActiveAndEnabled)
            {
                while (_bulletPool.Count > 30)
                {
                    var removedBullet = _bulletPool.Dequeue();
                    Destroy(removedBullet.gameObject);
                }
                var bullet = PhotonNetwork.Instantiate("Bullet", Vector3.zero, transform.localRotation);
                bullet.transform.SetParent(transform);
                bullet.transform.localPosition = Vector3.zero;
                _bulletPool.Enqueue(bullet.transform);
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}