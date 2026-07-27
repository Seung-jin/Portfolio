using System.Collections;
using System.Collections.Generic;
using System.Text;
#if DOTWEEN
using DG.Tweening;
#endif
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum ShakeInputKey {
	A,
	S,
	D,
	F
}

public class InputExample : MonoBehaviour {
	private const float SHAKE_DURATION = 0.3f;
	private const float SHAKE_STRENGTH = 0.5f;
	private const int SHAKE_VIBRATO = 20;

	[SerializeField] private KeyInputBinder _keyInputBinder;
	[SerializeField] private Transform _target;
	[SerializeField] private float _moveSpeed = 3f;
	[SerializeField] private ShakeInputKey _shakeKey = ShakeInputKey.A;
	[SerializeField] private UnityEvent _shakeCallback = new UnityEvent();
	[SerializeField] private TextMeshProUGUI _logText;
	[SerializeField] private ScrollRect _logScrollRect;
	[SerializeField] private int _maxLogCount = 30;

	private Coroutine _shakeCoroutine;
	private readonly List<string> _logs = new List<string>();
	private readonly StringBuilder _logBuilder = new StringBuilder();

	public ShakeInputKey CurrentShakeKey => _shakeKey;

	private void Awake() {
		if (_keyInputBinder == null) {
			_keyInputBinder = GetComponent<KeyInputBinder>();
		}

		RefreshLogText();
	}

	private void OnEnable() {
		RegisterDefaultInput();
		RegisterShakeInput();
		RegisterFailedShakeInput();
	}

	private void OnDisable() {
		UnregisterDefaultInput();
		UnregisterShakeInput();
		UnregisterFailedShakeInput();
	}

	public void SetShakeKeyA() {
		SetShakeKey(ShakeInputKey.A);
	}

	public void SetShakeKeyS() {
		SetShakeKey(ShakeInputKey.S);
	}

	public void SetShakeKeyD() {
		SetShakeKey(ShakeInputKey.D);
	}

	public void SetShakeKeyF() {
		SetShakeKey(ShakeInputKey.F);
	}

	public void SetShakeKey(ShakeInputKey shakeKey) {
		if (_shakeKey == shakeKey) {
			return;
		}

		UnregisterShakeInput();
		UnregisterFailedShakeInput();
		_shakeKey = shakeKey;
		RegisterShakeInput();
		RegisterFailedShakeInput();
		AddLog($"흔들기 키 변경: {_shakeKey}");
	}

	public void AddShakeCallback(UnityAction callback) {
		_shakeCallback.AddListener(callback);
	}

	public void RemoveShakeCallback(UnityAction callback) {
		_shakeCallback.RemoveListener(callback);
	}

	public void ClearLog() {
		_logs.Clear();
		RefreshLogText();
	}

	public void Shake() {
		if (_target == null) {
			return;
		}

		_shakeCallback?.Invoke();

#if DOTWEEN
		_target.DOKill();
		_target.DOShakePosition(SHAKE_DURATION, SHAKE_STRENGTH, SHAKE_VIBRATO);
#else
		if (_shakeCoroutine != null) {
			StopCoroutine(_shakeCoroutine);
		}

		_shakeCoroutine = StartCoroutine(ShakeFallback());
#endif
	}

	public void MoveUp() {
		Move(Vector3.up);
	}

	public void MoveDown() {
		Move(Vector3.down);
	}

	public void MoveLeft() {
		Move(Vector3.left);
	}

	public void MoveRight() {
		Move(Vector3.right);
	}

	private void Move(Vector3 direction) {
		if (_target == null) {
			return;
		}

		_target.position += direction * (_moveSpeed * Time.deltaTime);
	}

	private void RegisterDefaultInput() {
		if (_keyInputBinder == null) {
			return;
		}

		_keyInputBinder.AddListener(Key.UpArrow, InputTriggerType.Hold, MoveUp);
		_keyInputBinder.AddListener(Key.DownArrow, InputTriggerType.Hold, MoveDown);
		_keyInputBinder.AddListener(Key.LeftArrow, InputTriggerType.Hold, MoveLeft);
		_keyInputBinder.AddListener(Key.RightArrow, InputTriggerType.Hold, MoveRight);
	}

	private void UnregisterDefaultInput() {
		if (_keyInputBinder == null) {
			return;
		}

		_keyInputBinder.RemoveListener(Key.UpArrow, InputTriggerType.Hold, MoveUp);
		_keyInputBinder.RemoveListener(Key.DownArrow, InputTriggerType.Hold, MoveDown);
		_keyInputBinder.RemoveListener(Key.LeftArrow, InputTriggerType.Hold, MoveLeft);
		_keyInputBinder.RemoveListener(Key.RightArrow, InputTriggerType.Hold, MoveRight);
	}

	private void RegisterShakeInput() {
		if (_keyInputBinder == null) {
			return;
		}

		_keyInputBinder.AddListener(ToKey(_shakeKey), InputTriggerType.Pressed, OnShakeKeyPressed);
	}

	private void UnregisterShakeInput() {
		if (_keyInputBinder == null) {
			return;
		}

		_keyInputBinder.RemoveListener(ToKey(_shakeKey), InputTriggerType.Pressed, OnShakeKeyPressed);
	}

	private void RegisterFailedShakeInput() {
		if (_keyInputBinder == null) {
			return;
		}

		if (_shakeKey != ShakeInputKey.A) {
			_keyInputBinder.AddListener(Key.A, InputTriggerType.Pressed, OnShakeFailedA);
		}

		if (_shakeKey != ShakeInputKey.S) {
			_keyInputBinder.AddListener(Key.S, InputTriggerType.Pressed, OnShakeFailedS);
		}

		if (_shakeKey != ShakeInputKey.D) {
			_keyInputBinder.AddListener(Key.D, InputTriggerType.Pressed, OnShakeFailedD);
		}

		if (_shakeKey != ShakeInputKey.F) {
			_keyInputBinder.AddListener(Key.F, InputTriggerType.Pressed, OnShakeFailedF);
		}
	}

	private void UnregisterFailedShakeInput() {
		if (_keyInputBinder == null) {
			return;
		}

		_keyInputBinder.RemoveListener(Key.A, InputTriggerType.Pressed, OnShakeFailedA);
		_keyInputBinder.RemoveListener(Key.S, InputTriggerType.Pressed, OnShakeFailedS);
		_keyInputBinder.RemoveListener(Key.D, InputTriggerType.Pressed, OnShakeFailedD);
		_keyInputBinder.RemoveListener(Key.F, InputTriggerType.Pressed, OnShakeFailedF);
	}

	private void OnShakeKeyPressed() {
		AddLog("흔들기 성공");
		Shake();
	}

	private void OnShakeFailedA() {
		AddShakeFailedLog(ShakeInputKey.A);
	}

	private void OnShakeFailedS() {
		AddShakeFailedLog(ShakeInputKey.S);
	}

	private void OnShakeFailedD() {
		AddShakeFailedLog(ShakeInputKey.D);
	}

	private void OnShakeFailedF() {
		AddShakeFailedLog(ShakeInputKey.F);
	}

	private void AddShakeFailedLog(ShakeInputKey inputKey) {
		if (inputKey == _shakeKey) {
			return;
		}

		AddLog($"흔들기 실패. 현재 키: {_shakeKey}");
	}

	private Key ToKey(ShakeInputKey shakeInputKey) {
		switch (shakeInputKey) {
			case ShakeInputKey.A:
				return Key.A;
			case ShakeInputKey.S:
				return Key.S;
			case ShakeInputKey.D:
				return Key.D;
			case ShakeInputKey.F:
				return Key.F;
			default:
				return Key.A;
		}
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

#if !DOTWEEN
	private IEnumerator ShakeFallback() {
		Vector3 startPosition = _target.localPosition;
		float elapsedTime = 0f;

		while (elapsedTime < SHAKE_DURATION) {
			elapsedTime += Time.deltaTime;
			float strength = Mathf.Lerp(SHAKE_STRENGTH, 0f, elapsedTime / SHAKE_DURATION);
			_target.localPosition = startPosition + (Vector3)Random.insideUnitCircle * strength;
			yield return null;
		}

		_target.localPosition = startPosition;
		_shakeCoroutine = null;
	}
#endif
}
