using System.Collections;
using UnityEngine;

namespace MultiplayerDemo
{
    public class BulletEmitter : MonoBehaviour
    {
        [SerializeField]
        private Bullet _bulletPrefab;

        void Start()
        {
            StartCoroutine(CreateBulletRoutine());
        }

        private IEnumerator CreateBulletRoutine()
        {
            while (this.isActiveAndEnabled)
            {
                var bullet = GameObject.Instantiate<Bullet>(_bulletPrefab, transform);
                bullet.transform.localPosition = Vector3.zero;
                bullet.transform.localRotation = transform.localRotation;
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}