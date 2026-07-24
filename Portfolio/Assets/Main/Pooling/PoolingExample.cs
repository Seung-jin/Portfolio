using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PoolingExample : MonoBehaviour {
	[SerializeField] private TestPoolingObject _prefab;
	[SerializeField] private Transform _poolParent;
	[SerializeField] private TMP_InputField _countInputField;
	[SerializeField] private int _createCount = 5;

	private readonly List<TestPoolingObject> _activeObjects = new List<TestPoolingObject>();

	private ObjectPooler<TestPoolingObject> _pooler;
	private int _nextId = 1;

	private void Awake() {
		if (_prefab == null) {
			return;
		}

		_pooler = new ObjectPooler<TestPoolingObject>(_prefab, _poolParent);
	}

	public void Create() {
		if (_pooler == null) {
			return;
		}

		ResetObjects();

		int count = GetCreateCount();

		for (int i = 0; i < count; i++) {
			TestPoolingObject poolingObject = _pooler.Get();
			poolingObject.transform.SetAsLastSibling();
			poolingObject.SetId(_nextId);

			_activeObjects.Add(poolingObject);
			_nextId++;
		}
	}

	public void ResetObjects() {
		if (_pooler == null) {
			return;
		}

		for (int i = _activeObjects.Count - 1; i >= 0; i--) {
			_pooler.Release(_activeObjects[i]);
		}

		_activeObjects.Clear();
		_nextId = 1;
	}

	public void ClearPool() {
		ResetObjects();
		_pooler?.Clear();
	}

	private int GetCreateCount() {
		if (_countInputField != null && int.TryParse(_countInputField.text, out int inputCount)) {
			return Mathf.Max(0, inputCount);
		}

		return Mathf.Max(0, _createCount);
	}
}
