using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public enum InputTriggerType {
	Pressed,
	Hold,
	Released
}

/// <summary>
/// New Input System의 Key 입력에 UnityEvent를 연결하는 바인더.
/// </summary>
public class KeyInputBinder : MonoBehaviour {
	[SerializeField] private bool _inputEnabled = true;
	[SerializeField] private List<KeyInputEvent> _keyInputEventList = new();

	private void Update() {
		if (!_inputEnabled || Keyboard.current == null) {
			return;
		}

		foreach (KeyInputEvent keyInputEvent in _keyInputEventList) {
			keyInputEvent.TryInvoke();
		}
	}

	public void SetInputEnabled(bool inputEnabled) {
		_inputEnabled = inputEnabled;
	}

	public void AddListener(Key key, InputTriggerType triggerType, UnityAction callback) {
		KeyInputEvent keyInputEvent = FindOrCreateEvent(key, triggerType);
		keyInputEvent.AddListener(callback);
	}

	public void RemoveListener(Key key, InputTriggerType triggerType, UnityAction callback) {
		KeyInputEvent keyInputEvent = FindEvent(key, triggerType);
		keyInputEvent?.RemoveListener(callback);
	}

	private KeyInputEvent FindOrCreateEvent(Key key, InputTriggerType triggerType) {
		KeyInputEvent keyInputEvent = FindEvent(key, triggerType);
		if (keyInputEvent != null) {
			return keyInputEvent;
		}

		keyInputEvent = new KeyInputEvent(key, triggerType);
		_keyInputEventList.Add(keyInputEvent);
		return keyInputEvent;
	}

	private KeyInputEvent FindEvent(Key key, InputTriggerType triggerType) {
		foreach (KeyInputEvent keyInputEvent in _keyInputEventList) {
			if (keyInputEvent.Key == key && keyInputEvent.TriggerType == triggerType) {
				return keyInputEvent;
			}
		}

		return null;
	}
}

[Serializable]
public class KeyInputEvent {
	[SerializeField] private Key _key = Key.Space;
	[SerializeField] private InputTriggerType _triggerType = InputTriggerType.Pressed;
	[SerializeField] private UnityEvent _event = new UnityEvent();

	public Key Key => _key;
	public InputTriggerType TriggerType => _triggerType;

	public KeyInputEvent(Key key, InputTriggerType triggerType) {
		_key = key;
		_triggerType = triggerType;
		_event = new UnityEvent();
	}

	public void TryInvoke() {
		KeyControl keyControl = Keyboard.current[_key];
		if (keyControl == null || !IsTriggered(keyControl)) {
			return;
		}

		_event?.Invoke();
	}

	public void AddListener(UnityAction callback) {
		if (_event == null) {
			_event = new UnityEvent();
		}

		_event.AddListener(callback);
	}

	public void RemoveListener(UnityAction callback) {
		if (_event == null) {
			return;
		}

		_event.RemoveListener(callback);
	}

	private bool IsTriggered(KeyControl keyControl) {
		switch (_triggerType) {
			case InputTriggerType.Pressed:
				return keyControl.wasPressedThisFrame;
			case InputTriggerType.Hold:
				return keyControl.isPressed;
			case InputTriggerType.Released:
				return keyControl.wasReleasedThisFrame;
			default:
				return false;
		}
	}
}
