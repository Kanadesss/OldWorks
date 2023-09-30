using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
  private Animator _animator;

  void Start () {
    _animator = GetComponent<Animator>();
  }
  
  public void Walk() {
    _animator.SetFloat("WalkSpeed", 1);
  }

  public void Idle() {
    _animator.SetFloat("WalkSpeed", 0);
    JumpStop();
  }

  public void Jump() {
    _animator.SetBool("IsJumping", true);
  }

  public void JumpStop() {
    _animator.SetBool("IsJumping", false);
  }

}
