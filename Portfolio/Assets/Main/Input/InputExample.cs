using System.Collections;
using System.Collections.Generic;
using System.Text;
#if DOTWEEN
using DG.Tweening;
#endif
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]
public class InputExample : MonoBehaviour {
	private const string MOVE_ACTION_NAME = "Move";
	private const string SHAKE_ACTION_NAME = "Shake";
	private const float SHAKE_DURATION = 0.3f;
	private const float SHAKE_STRENGTH = 0.5f;
	private const int SHAKE_VIBRATO = 20;
	private const float MOVE_SPEED = 3f;
	private const int MAX_LOG_COUNT = 30;

	[SerializeField] private PlayerInput _playerInput;
	[SerializeField] private Transform _target;
	[SerializeField] private TextMeshProUGUI _logText;
	[SerializeField] private ScrollRect _logScrollRect;

	private readonly List<string> _logs = new List<string>();
	private readonly StringBuilder _logBuilder = new StringBuilder();

	private InputLogic _inputLogic;
	private InputAction _moveAction;
	private InputAction _shakeAction;
	private Coroutine _shakeCoroutine;
	private Vector2 _moveInput;
	private Key _shakeKey = Key.A;

	private void Awake() {
		if (_playerInput == null) {
			_playerInput = GetComponent<PlayerInput>();
		}

		if (_playerInput == null || _playerInput.actions == null) {
			Debug.LogError("[InputExample] PlayerInput actions is null.");
			return;
		}

		_playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
		_moveAction = _playerInput.actions.FindAction(MOVE_ACTION_NAME, true);
		_shakeAction = _playerInput.actions.FindAction(SHAKE_ACTION_NAME, true);

		_inputLogic = new InputLogic(_shakeAction);
		_inputLogic.SetShakeKey(_shakeKey);
		RefreshLog();
	}

	private void OnEnable() {
		if (_moveAction == null || _shakeAction == null) {
			return;
		}

		_moveAction.performed += OnMove;
		_moveAction.canceled += OnMove;
		_shakeAction.performed += OnShake;
		_playerInput.actions.Enable();
	}

	private void OnDisable() {
		if (_moveAction == null || _shakeAction == null) {
			return;
		}

		_moveAction.performed -= OnMove;
		_moveAction.canceled -= OnMove;
		_shakeAction.performed -= OnShake;
		_playerInput.actions.Disable();
		_moveInput = Vector2.zero;
	}

	private void Update() {
		Move(_moveInput);
	}

	public void SetShakeKeyA() {
		SetShakeKey(Key.A);
	}

	public void SetShakeKeyS() {
		SetShakeKey(Key.S);
	}

	public void SetShakeKeyD() {
		SetShakeKey(Key.D);
	}

	public void SetShakeKeyF() {
		SetShakeKey(Key.F);
	}

	public void SetShakeKey(Key shakeKey) {
		shakeKey = InputLogic.NormalizeShakeKey(shakeKey);
		if (_shakeKey == shakeKey) {
			return;
		}

		_shakeKey = shakeKey;
		_inputLogic.SetShakeKey(_shakeKey);
		AddLog($"흔들기 키 변경: {_shakeKey}");
	}

	public void ClearLog() {
		_logs.Clear();
		RefreshLog();
	}

	private void OnMove(InputAction.CallbackContext context) {
		_moveInput = context.ReadValue<Vector2>();
	}

	private void OnShake(InputAction.CallbackContext context) {
		string pressedKey = context.control != null ? context.control.displayName : _shakeKey.ToString();
		AddLog($"흔들기 성공: {pressedKey}");
		Shake();
	}

	private void Move(Vector2 direction) {
		if (_target == null || direction == Vector2.zero) {
			return;
		}

		_target.position += (Vector3)(direction.normalized * (MOVE_SPEED * Time.deltaTime));
	}

	private void Shake() {
		if (_target == null) {
			return;
		}

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

	private void AddLog(string message) {
		Debug.Log(message);

		_logs.Add(message);

		while (_logs.Count > MAX_LOG_COUNT) {
			_logs.RemoveAt(0);
		}

		RefreshLog();
		ScrollToBottom();
	}

	private void RefreshLog() {
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
			_target.localPosition = startPosition + (Vector3)UnityEngine.Random.insideUnitCircle * strength;
			yield return null;
		}

		_target.localPosition = startPosition;
		_shakeCoroutine = null;
	}
#endif
}
