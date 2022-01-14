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
        [SerializeField]
        private TextMesh _hpText;
        [SerializeField]
        private PhotonView _photonView;

        private GameManager _gameManager;
        private Queue<Transform> _bulletPool = new Queue<Transform>();

        private Vector2 _moveDirection2d;

        public Transform Target { get; set; }

        private int _hp = 10;

        public bool IsMe
            => name.Contains(PhotonNetwork.NickName);

        private string _hpTextString
        {
            get
            {
                var result = "";
                for (var i = 0; i < _hp; i++)
                    result += "|";
                return result;
            }
        }

        private void Start()
        {
            _hpText.text = _hpTextString;
            _hpText.color = Color.green;
            _gameManager = FindObjectOfType<GameManager>();
            _gameManager?.PlayerAdded(this);
            if (IsMe)
                StartCoroutine(CreateBullet());
        }

        private void FixedUpdate()
        {
            if (IsMe)
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
                    _gameManager?.GameOver(looser: this);
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
                bullet.tag = "bullet-" + (name.Contains("1") ? "1" : "2");
                bullet.transform.position = _gun.position;
                bullet.transform.rotation = transform.rotation;
                _bulletPool.Enqueue(bullet.transform);
                yield return new WaitForSeconds(0.3f);
            }
        }

        public void Move(InputAction.CallbackContext context)
        {
            if (IsMe)
            {
                var vector = context.ReadValue<Vector2>();
                _moveDirection2d = new Vector2(vector.x, vector.y);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var collidingObjectTag = other.gameObject.tag;
            var prefixMatched = collidingObjectTag.Contains("bullet");
            var postfixMatched = name.Contains("1") ? collidingObjectTag.Contains("2") : collidingObjectTag.Contains("1");
            if (prefixMatched && postfixMatched && !_gameManager.GameOvered)
                _photonView.RPC("AddDamage", RpcTarget.All);

        }

        [PunRPC]
        private void AddDamage()
        {
            _hp = _hp <= 0 ? 0 : _hp - 1;
            _hpText.text = _hpTextString;
            if (_hp <= 0)
                _gameManager?.GameOver(looser: this);
            else if (_hp < 3)
                _hpText.color = Color.red;
            else if (_hp < 6)
                _hpText.color = Color.yellow;
            else
                _hpText.color = Color.green;
        }
    }
}