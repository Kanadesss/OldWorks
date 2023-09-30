using UnityEngine;

public class ItemPick : MonoBehaviour {
  private int _itemCount = 1;

  public void DestroyItem() {
    Destroy(gameObject);
  }

  public int GetItemCount() {
    return _itemCount;
  }

  private void OnTriggerEnter2D(Collider2D collider) {

    if (collider.gameObject.CompareTag("Player")) {
      DestroyItem();
    }

  }

}
