using UnityEngine;

public class CubeKeyboardContoller : MonoBehaviour {
  [SerializeField] private CubeMovement _cubeMovement;
  [SerializeField] private Camera _cameraCurrent;

  private void FixedUpdate() {

    if (Input.GetKey(KeyCode.W)) {
      Vector3 rotatiotnAxis = _cameraCurrent.transform.right;
      _cubeMovement.Move(rotatiotnAxis);
    } else if (Input.GetKey(KeyCode.S)) {
      Vector3 rotatiotnAxis = _cameraCurrent.transform.right * -1;
      _cubeMovement.Move(rotatiotnAxis);
    }

  }
}
