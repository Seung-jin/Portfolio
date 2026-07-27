using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FactoryExample : MonoBehaviour {
	[SerializeField] private BulletFactory _bulletFactory;
	[SerializeField] private TMP_InputField _bulletIdInputField;
	[SerializeField] private int _bulletId = 1;
	[SerializeField] private int _createCount = 1;

	private readonly List<BulletObject> _activeBullets = new List<BulletObject>();

	public void CreateBullet() {
		if (_bulletFactory == null) {
			return;
		}

		ResetBullets();

		int bulletId = GetBulletId();

		for (int i = 0; i < _createCount; i++) {
			BulletObject bullet = _bulletFactory.GetBullet(bulletId);

			if (bullet == null) {
				continue;
			}

			bullet.transform.SetAsLastSibling();
			_activeBullets.Add(bullet);
		}
	}

	public void ResetBullets() {
		if (_bulletFactory == null) {
			return;
		}

		for (int i = _activeBullets.Count - 1; i >= 0; i--) {
			_bulletFactory.ReturnBullet(_activeBullets[i]);
		}

		_activeBullets.Clear();
	}

	private int GetBulletId() {
		if (_bulletIdInputField != null && int.TryParse(_bulletIdInputField.text, out int inputId)) {
			return inputId;
		}

		return _bulletId;
	}
}
