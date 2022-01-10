using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private Rigidbody _rigidBody;

    private Vector2 _moveDirection2d;

    void Start()
    {

    }

    private void FixedUpdate()
    {
        if (_moveDirection2d != Vector2.zero)
        {
            var updatedVelocity = new Vector3(_moveDirection2d.x, _rigidBody.velocity.y, _moveDirection2d.y) * _moveSpeed * Time.fixedDeltaTime;
            _rigidBody.velocity = updatedVelocity;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        var vector = context.ReadValue<Vector2>();
        _moveDirection2d = new Vector2(vector.x, vector.y);
    }
}
