using System.Collections;
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

        private Vector2 _moveDirection2d;

        public Transform Target { get; set; }

        private void Start() {
            FindObjectOfType<GameManager>().PlayerAdded(this);
            StartCoroutine(FocusOnTarget());
        }

        private void FixedUpdate()
        {
            if (_moveDirection2d != Vector2.zero)
            {
                var updatedVelocity = new Vector3(_moveDirection2d.x, _rigidBody.velocity.y, _moveDirection2d.y) * _moveSpeed * Time.fixedDeltaTime;
                _rigidBody.velocity = updatedVelocity;
            }
        }

        private IEnumerator FocusOnTarget() {
            while(Target != null) {
                transform.LookAt(Target);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                yield return new WaitForSeconds(0.5f);
            }
        }

        public void Move(InputAction.CallbackContext context)
        {
            var vector = context.ReadValue<Vector2>();
            _moveDirection2d = new Vector2(vector.x, vector.y);
        }
    }
}