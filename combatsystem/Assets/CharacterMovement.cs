using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    [Header("Movement")]
    [Tooltip("Move speed of the character in m/s")]
    public float moveSpeed = 2.0f;

    [Tooltip("Sprint speed of the character in m/s")]
    public float sprintSpeed = 5.335f;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float rotationSmoothTime = 0.12f;

    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController controller;

    private const string MovementBlendKey = "movement";

    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;

    private Vector2 move;
    private bool sprint;

    private void Reset()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float targetSpeed = sprint ? moveSpeed : sprintSpeed;

        if (move == Vector2.zero) targetSpeed = 0.0f;

        _speed = targetSpeed;

        _animationBlend = _speed / sprintSpeed;
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        Vector3 inputDirection = new Vector3(move.x, 0.0f, move.y).normalized;

        if (move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, _targetRotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        // move the player

        controller.Move(targetDirection.normalized * (_speed * Time.deltaTime));
        //transform.Translate(inputDirection * (_speed * Time.deltaTime), Space.World);

        animator.SetFloat(MovementBlendKey, _animationBlend);
    }


    #region Movement Input

    public void Move(Vector2 input)
    {
        move = input;
    }

    public void Sprint(bool spriteState)
    {
        sprint = spriteState;
    }

    #endregion

}
