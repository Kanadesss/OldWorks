using UnityEngine;

public class PlayerMovement : MonoBehaviour {
  [Header("Speeds")]
  [SerializeField] private float walkSpeed;
  [SerializeField] private float jumpForce;

  [Header("Ground")]
  [SerializeField] private Transform _groundCheck;
  [SerializeField] private LayerMask _WhatIsGround;

  [Header("Animator")]
  [SerializeField] private PlayerAnimator _playerAnimator;

  private MoveState _moveState;
  private Transform _transform;
  private Rigidbody2D _rigidbody;
  private float _groundCheckRadius = 0.2f;
  private bool _facingRight = true;
  private bool _isGrounded = true;

  enum MoveState {
    Idle,
    Walk,
    Jump,
  }

  void Start() {
    _transform = GetComponent<Transform>();
    _rigidbody = GetComponent<Rigidbody2D>();
    _moveState = MoveState.Idle;
  }

  void FixedUpdate() {
    _isGrounded = false;
    Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, _groundCheckRadius, _WhatIsGround);

    foreach (Collider2D collider in colliders) {

      if (collider.gameObject != gameObject) {
        _isGrounded = true;
        StopJump();
      }

    }

  }

  public void Move(float walkDirection, bool jump) {
    Walk(walkDirection);

    if (jump) {
      Jump();
    }

    SetPlayerDirection(walkDirection);
  }

  private void Walk(float walkDirection) {

    if (_moveState != MoveState.Jump) {

      if (walkDirection == 0) {
        Idle();
      } else {
        _moveState = MoveState.Walk;
        _playerAnimator.Walk();
      }

    }

    Vector3 movement = new Vector3(walkDirection, 0, 0);
    _transform.Translate(movement * walkSpeed);
  }

  private void Jump() {

    if (_moveState != MoveState.Jump && _isGrounded) {
      _rigidbody.AddForce(Vector2.up * jumpForce);
      _moveState = MoveState.Jump;
      _playerAnimator.Jump();
    }

  }

  private void StopJump() {

    if (_isGrounded && _moveState == MoveState.Jump) {
      _moveState = MoveState.Idle;
      _playerAnimator.JumpStop();
      Idle();
    }

  }

  private void Idle() {
    _moveState = MoveState.Idle;
    _playerAnimator.Idle();
  }

  private void SetPlayerDirection(float direction) {

    if (direction > 0 && !_facingRight) {
      _facingRight = !_facingRight;
      Flip();
    } else if (direction < 0 && _facingRight) {
      _facingRight = !_facingRight;
      Flip();
    }

  }

  private void Flip() {
    Vector3 scale = _transform.localScale;
    scale.x *= -1;
    _transform.localScale = scale;
  }

}
