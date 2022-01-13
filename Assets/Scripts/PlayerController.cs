using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MultiplayerDemo
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float _moveSpeed;
        [SerializeField]
        private Rigidbody _rigidBody;
        [SerializeField]
        private Transform _gun;
        [SerializeField]
        private Bullet _bulletPrefab;

        private Queue<Transform> _bulletPool = new Queue<Transform>();

        private Vector2 _moveDirection2d;

        public Transform Target { get; set; }

        private void Start()
        {
            FindObjectOfType<GameManager>().PlayerAdded(this);
            if (name.Contains(PhotonNetwork.NickName))
                StartCoroutine(CreateBullet());
        }

        private void FixedUpdate()
        {
            if (name.Contains(PhotonNetwork.NickName))
            {
                if (_moveDirection2d != Vector2.zero)
                {
                    var updatedVelocity = new Vector3(_moveDirection2d.x, _rigidBody.velocity.y, _moveDirection2d.y) * _moveSpeed * Time.fixedDeltaTime;
                    _rigidBody.velocity = updatedVelocity;
                }
                if (Target != null)
                {
                    var originalRotation = transform.rotation;
                    transform.LookAt(Target);
                    var newRotation = transform.rotation;
                    newRotation.eulerAngles.Set(0, newRotation.eulerAngles.y, 0);
                    transform.rotation = originalRotation;
                    transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, 20 * Time.deltaTime);
                }

                if (transform.position.y < -10)
                    FindObjectOfType<GameManager>().OnQuit();
            }
        }

        private IEnumerator CreateBullet()
        {
            while (this.isActiveAndEnabled)
            {
                while (_bulletPool.Count > 30)
                {
                    var removedBullet = _bulletPool.Dequeue();
                    Destroy(removedBullet.gameObject);
                }
                var bullet = PhotonNetwork.Instantiate("Bullet", Vector3.zero, transform.rotation);
                bullet.transform.position = _gun.position;
                bullet.transform.rotation = transform.rotation;
                _bulletPool.Enqueue(bullet.transform);
                yield return new WaitForSeconds(0.3f);
            }
        }

        public void Move(InputAction.CallbackContext context)
        {
            if (name.Contains(PhotonNetwork.NickName))
            {
                var vector = context.ReadValue<Vector2>();
                _moveDirection2d = new Vector2(vector.x, vector.y);
            }
        }
    }
}