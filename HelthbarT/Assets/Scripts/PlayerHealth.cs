using UnityEngine;
using UnityEngine.Events;
using System;

public class PlayerHealth : MonoBehaviour {
  [SerializeField] private float _maxHealth;
  [SerializeField] private float _damageValue;
  [SerializeField] private float _healValue;
  [SerializeField] private UnityEvent _setHealth;

  public static Action<float> onSetHealth;
  private float _currentHealth;

  void OnEnable() {
    _currentHealth = _maxHealth;

    if (onSetHealth == null) {
      Debug.Log("NULL");
    }

    onSetHealth?.Invoke(_maxHealth);
  }

  public void HealthDamage() {

    if (_currentHealth - _damageValue > 0) {
      _currentHealth -= _damageValue;
    } else {
      _currentHealth = 0;
    }

    onSetHealth?.Invoke(_currentHealth);
  }

  public void HealthHeal() {

    if(_currentHealth + _healValue < _maxHealth) {
      _currentHealth += _healValue;
    } else {
      _currentHealth = _maxHealth;
    }

    onSetHealth?.Invoke(_currentHealth);
  }
}
