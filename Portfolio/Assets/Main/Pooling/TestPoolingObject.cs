using TMPro;
using UnityEngine;

public class TestPoolingObject : PoolingObject<TestPoolingObject> {
	[SerializeField] private TMP_Text _idText;

	private int _id;

	private void Awake() {
		if (_idText == null) {
			_idText = GetComponentInChildren<TMP_Text>();
		}
	}

	public void SetId(int id) {
		_id = id;
		RefreshText();
	}

	public override void OnGetFromPool() {
		base.OnGetFromPool();
		RefreshText();
	}

	public override void OnReleaseToPool() {
		_id = 0;
		RefreshText();
		base.OnReleaseToPool();
	}

	private void RefreshText() {
		if (_idText == null) {
			return;
		}

		_idText.text = _id > 0 ? _id.ToString() : string.Empty;
	}
}
