using UnityEngine;

public class PlayerInventory : MonoBehaviour {
  private int _itemCount;

  public void AddItem(ItemPick item) {
    _itemCount += item.GetItemCount();
    Debug.Log("Items count = " + _itemCount);
  }

  private void Start() {
    _itemCount = 0;
  }

}
