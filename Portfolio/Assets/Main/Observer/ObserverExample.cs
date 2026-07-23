using UnityEngine;

public enum BattleEventType {
	Attack,
	Damaged,
	Healed,
	Dead,
	BattleReset,
	Message
}

public class ObserverExample : MonoBehaviour {
	[SerializeField] private string _attackerName = "Player";
	[SerializeField] private string _targetName = "Enemy";
	[SerializeField] private int _attackPower = 10;
	[SerializeField] private int _healAmount = 15;
	[SerializeField] private int _maxHealth = 100;

	private readonly Observer<BattleEventType> _observer = new Observer<BattleEventType>();

	private int _currentHealth;
	private bool _isDead;

	private void Awake() {
		_currentHealth = _maxHealth;
	}

	public void AddListener(BattleEventType eventType, Observer<BattleEventType>.ParamEventHandler listener) {
		_observer.AddListener(eventType, listener);
	}

	public void AddListener(BattleEventType eventType, Observer<BattleEventType>.EventHandler listener) {
		_observer.AddListener(eventType, listener);
	}

	public void RemoveListener(BattleEventType eventType, Observer<BattleEventType>.ParamEventHandler listener) {
		_observer.RemoveListener(eventType, listener);
	}

	public void RemoveListener(BattleEventType eventType, Observer<BattleEventType>.EventHandler listener) {
		_observer.RemoveListener(eventType, listener);
	}

	public void Attack() {
		if (_isDead) {
			_observer.TriggerEvent(BattleEventType.Message, $"{_targetName} is already dead.");
			return;
		}

		_observer.TriggerEvent(BattleEventType.Attack, _attackerName, _targetName, _attackPower);
		TakeDamage(_attackPower);
	}

	public void Heal() {
		if (_isDead) {
			_observer.TriggerEvent(BattleEventType.Message, $"{_targetName} is dead and cannot be healed.");
			return;
		}

		_currentHealth = Mathf.Min(_maxHealth, _currentHealth + _healAmount);
		_observer.TriggerEvent(BattleEventType.Healed, _targetName, _healAmount, _currentHealth, _maxHealth);
	}

	public void ResetBattle() {
		_currentHealth = _maxHealth;
		_isDead = false;

		_observer.TriggerEvent(BattleEventType.BattleReset, _targetName, _currentHealth, _maxHealth);
	}

	private void TakeDamage(int damage) {
		_currentHealth = Mathf.Max(0, _currentHealth - damage);
		_observer.TriggerEvent(BattleEventType.Damaged, _targetName, damage, _currentHealth, _maxHealth);

		if (_currentHealth > 0) {
			return;
		}

		_isDead = true;
		_observer.TriggerEvent(BattleEventType.Dead, _targetName);
	}
}
