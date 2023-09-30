using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
  [SerializeField] private Slider _slider;
  [Range(1, 10)][SerializeField]private float _fillSpeed;

  private float _healthTarget;
  private float _signDifference;
  private bool _isHealthSet;

  void Awake() {
    PlayerHealth.onSetHealth += SetMaxHealth;
  }

  void OnDisable() {
    PlayerHealth.onSetHealth -= SetHealth;
  }

  void Start() {
    _isHealthSet = true;
    PlayerHealth.onSetHealth -= SetMaxHealth;
    PlayerHealth.onSetHealth += SetHealth;
  }

  void Update() {

    if (!_isHealthSet) {   
      SetIntermediateHealth(_fillSpeed * Time.deltaTime);
    }

  }

  private void SetMaxHealth(float maxHealth) {
    _slider.maxValue = maxHealth;
    _slider.value = maxHealth;
  }

  private void SetHealth(float health) {
    _isHealthSet = false;
    _healthTarget = health;
    _signDifference = _slider.value - _healthTarget < 0 ? -1 : 1;
  }

  private void SetIntermediateHealth(float fillSpeed) {
    _slider.value -= _signDifference * fillSpeed;
    
    if (Mathf.Abs(_slider.value - _healthTarget) <= 0.2f) {     
      _slider.value = _healthTarget;
      _isHealthSet = true;
    }

  }
}
