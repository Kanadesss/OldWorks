using UnityEngine;

public class EnemyMove : MonoBehaviour {
  [SerializeField] private float _runSpeed;
  [SerializeField] private Transform[] _wayPoints;
  
  private int wayPoint;
  private SpriteRenderer _spriteRenderer;

  private void Start() {
    wayPoint = 0;
    _spriteRenderer = GetComponent<SpriteRenderer>();
  }

  private void Update() {
    transform.position = Vector3.MoveTowards(transform.position, _wayPoints[wayPoint].position, _runSpeed * Time.deltaTime);

    if (Vector2.Distance(transform.position, _wayPoints[wayPoint].position) <= 1f) { 
      _spriteRenderer.flipX = !_spriteRenderer.flipX;
      ++wayPoint;
      wayPoint %= _wayPoints.Length;
    }

  }
}
