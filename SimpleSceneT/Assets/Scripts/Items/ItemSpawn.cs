using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ItemSpawn : MonoBehaviour {
  [SerializeField] private GameObject _item;
  [SerializeField] private int _itemCount;
  [SerializeField] private float _respawnTime;
  [SerializeField] private UnityEvent _respawnEvent;

  void Start() {

    for (int itemsSpawned = 0; itemsSpawned < _itemCount; itemsSpawned++) {
      SpawnItem();
    }
    
  }

  private void OnTriggerEnter2D(Collider2D collider) {

    if (collider.gameObject.CompareTag("Player")) {
      _respawnEvent.Invoke();
      RespawnItem();
    }

  }

  public void RespawnItem() {
    StartCoroutine(RespawnDelay());
  }

  private IEnumerator RespawnDelay() {
    yield return new WaitForSeconds(_respawnTime);
    SpawnItem();
  }

  private void SpawnItem() {
    Instantiate(_item, transform.position, Quaternion.identity);
  }
}
