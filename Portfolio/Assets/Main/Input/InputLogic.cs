using UnityEngine.InputSystem;

public class InputLogic {
	private readonly InputAction _shakeAction;

	public InputLogic(InputAction shakeAction) {
		_shakeAction = shakeAction;
	}

	public void SetShakeKey(Key key) {
		if (_shakeAction == null) {
			return;
		}

		key = NormalizeShakeKey(key);
		bool wasEnabled = _shakeAction.enabled;
		if (wasEnabled) {
			_shakeAction.Disable();
		}

		_shakeAction.ApplyBindingOverride(ToKeyPath(key));

		if (wasEnabled) {
			_shakeAction.Enable();
		}
	}

	public static Key NormalizeShakeKey(Key key) {
		if (key == Key.A || key == Key.S || key == Key.D || key == Key.F) {
			return key;
		}

		return Key.A;
	}

	private string ToKeyPath(Key key) {
		return $"<Keyboard>/{key.ToString().ToLower()}";
	}
}
