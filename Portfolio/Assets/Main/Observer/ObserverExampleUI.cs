using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObserverExampleUI : MonoBehaviour {
	[SerializeField] private ObserverExample _observerExample;
	[SerializeField] private TextMeshProUGUI _logText;
	[SerializeField] private ScrollRect _logScrollRect;
	[SerializeField] private int _maxLogCount = 30;

	private readonly List<string> _logs = new List<string>();
	private readonly StringBuilder _logBuilder = new StringBuilder();

	private bool _isSubscribed;

	private void Awake() {
		RefreshLogText();
	}

	private void OnEnable() {
		Subscribe();
	}

	private void OnDisable() {
		Unsubscribe();
	}

	public void ClearLog() {
		_logs.Clear();
		RefreshLogText();
	}

	private void Subscribe() {
		if (_isSubscribed || _observerExample == null) {
			return;
		}

		_observerExample.AddListener(BattleEventType.Attack, OnAttack);
		_observerExample.AddListener(BattleEventType.Damaged, OnDamaged);
		_observerExample.AddListener(BattleEventType.Healed, OnHealed);
		_observerExample.AddListener(BattleEventType.Dead, OnDead);
		_observerExample.AddListener(BattleEventType.BattleReset, OnBattleReset);
		_observerExample.AddListener(BattleEventType.Message, OnMessage);

		_isSubscribed = true;
	}

	private void Unsubscribe() {
		if (!_isSubscribed || _observerExample == null) {
			return;
		}

		_observerExample.RemoveListener(BattleEventType.Attack, OnAttack);
		_observerExample.RemoveListener(BattleEventType.Damaged, OnDamaged);
		_observerExample.RemoveListener(BattleEventType.Healed, OnHealed);
		_observerExample.RemoveListener(BattleEventType.Dead, OnDead);
		_observerExample.RemoveListener(BattleEventType.BattleReset, OnBattleReset);
		_observerExample.RemoveListener(BattleEventType.Message, OnMessage);

		_isSubscribed = false;
	}

	private void OnAttack(params object[] args) {
		string attackerName = (string)args[0];
		string targetName = (string)args[1];
		int damage = (int)args[2];

		AddLog($"{attackerName} 공격. 대상: {targetName}, 피해: {damage}");
	}

	private void OnDamaged(params object[] args) {
		string targetName = (string)args[0];
		int damage = (int)args[1];
		int currentHealth = (int)args[2];
		int maxHealth = (int)args[3];

		AddLog($"{targetName} 피해. HP: {currentHealth}/{maxHealth}");
	}

	private void OnHealed(params object[] args) {
		string targetName = (string)args[0];
		int healAmount = (int)args[1];
		int currentHealth = (int)args[2];
		int maxHealth = (int)args[3];

		AddLog($"{targetName} 회복. 회복량: {healAmount}, HP: {currentHealth}/{maxHealth}");
	}

	private void OnDead(params object[] args) {
		string targetName = (string)args[0];

		AddLog($"{targetName} 사망");
	}

	private void OnBattleReset(params object[] args) {
		string targetName = (string)args[0];
		int currentHealth = (int)args[1];
		int maxHealth = (int)args[2];

		AddLog($"전투 초기화. {targetName} HP: {currentHealth}/{maxHealth}");
	}

	private void OnMessage(params object[] args) {
		AddLog((string)args[0]);
	}

	private void AddLog(string message) {
		Debug.Log(message);

		_logs.Add(message);

		while (_logs.Count > _maxLogCount) {
			_logs.RemoveAt(0);
		}

		RefreshLogText();
		ScrollToBottom();
	}

	private void RefreshLogText() {
		if (_logText == null) {
			return;
		}

		_logBuilder.Clear();

		for (int i = 0; i < _logs.Count; i++) {
			_logBuilder.AppendLine(_logs[i]);
		}

		_logText.text = _logBuilder.ToString();
	}

	private void ScrollToBottom() {
		if (_logScrollRect == null) {
			return;
		}

		Canvas.ForceUpdateCanvases();
		_logScrollRect.verticalNormalizedPosition = 0f;
	}
}
