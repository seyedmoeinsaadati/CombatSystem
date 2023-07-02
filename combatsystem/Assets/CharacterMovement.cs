using Moein.Core;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Move speed of the character in m/s")]
    public float moveSpeed = 2.0f;

    [Tooltip("Sprint speed of the character in m/s")]
    public float aimingSpeed = 5.335f;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float rotationSmoothTime = 0.12f;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform aimingTarget;
    [SerializeField] private Rig aimingRig;

    private const string AnimatorMoveXKey = "MoveX";
    private const string AnimatorMoveZKey = "MoveZ";
    private const string AnimatorMoveSpeedKey = "MoveSpeed";
    private const string Aiming_Bool_Key = "aiming";

    private float targetSpeed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    [SerializeField] private float _verticalVelocity;

    private Vector2 moveInput;
    private bool aiming;

    private void Reset()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
        // ThirdPersonMove();
    }

    private void ThirdPersonMove()
    {
        Vector3 move = new Vector3(moveInput.y, 0.0f, moveInput.x);
        // move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        // move.y = 0;
        controller.Move(move * (Time.deltaTime * aimingSpeed));

        SetActiveAiming(true);

        AimingMovement(moveInput.x, moveInput.y);

    }


    private void Move()
    {
        Vector3 inputDirection = new Vector3(moveInput.x, 0.0f, moveInput.y).normalized;
        _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        targetSpeed = aiming ? aimingSpeed : moveSpeed;
        if (moveInput == Vector2.zero) targetSpeed = 0.0f;

        if (aiming)
        {
            aimingRig.weight = 1;
            _animationBlend = targetSpeed / aimingSpeed;
            if (_animationBlend < 0.01f) _animationBlend = 0f;
            var direction = aimingTarget.position - transform.position;
            direction.y = 0;

            if (direction.magnitude > _verticalVelocity)
            {
                _animationBlend *= -1;
            }

            _verticalVelocity = direction.magnitude;

            inputDirection = inputDirection.RotateXZ(135 * Mathf.Deg2Rad, true);
            AimingMovement(inputDirection.z, inputDirection.x);

            if (targetSpeed > 0)
                transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            aimingRig.weight = 0;
            _animationBlend = targetSpeed / aimingSpeed;
            if (_animationBlend < 0.01f) _animationBlend = 0f;
            Movement(_animationBlend);

            if (moveInput != Vector2.zero)
            {
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, rotationSmoothTime);
                transform.rotation = Quaternion.Euler(0.0f, _targetRotation, 0.0f);
            }
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        controller.Move(targetDirection.normalized * (targetSpeed * Time.deltaTime));

    }

    #region Animation

    private void SetActiveAiming(bool active)
    {
        animator.SetBool(Aiming_Bool_Key, active);
    }


    private void AimingMovement(float forward, float right)
    {
        animator.SetFloat(AnimatorMoveXKey, right);
        animator.SetFloat(AnimatorMoveZKey, forward);
    }

    private void Movement(float forward)
    {
        animator.SetFloat(AnimatorMoveSpeedKey, forward);
    }

    #endregion

    #region Movement Input

    public void Move(Vector2 input)
    {
        moveInput = input;
    }

    public void Sprint(bool aimingState)
    {

        if (aiming != aimingState)
            SetActiveAiming(aimingState);

        aiming = aimingState;
    }

    #endregion

}
