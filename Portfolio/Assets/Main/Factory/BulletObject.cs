using TMPro;
using UnityEngine;

public class BulletObject : PoolingObject<BulletObject> {
	[SerializeField] private TMP_Text _idText;

	public int Id { get; private set; }

	private void Awake() {
		if (_idText == null) {
			_idText = GetComponentInChildren<TMP_Text>();
		}
	}

	public void SetId(int id) {
		Id = id;
		RefreshText();
	}

	public override void OnGetFromPool() {
		base.OnGetFromPool();
		RefreshText();
	}

	public override void OnReleaseToPool() {
		Id = 0;
		RefreshText();
		base.OnReleaseToPool();
	}

	private void RefreshText() {
		if (_idText == null) {
			return;
		}

		_idText.text = Id > 0 ? $"Bullet {Id}" : string.Empty;
	}
}
