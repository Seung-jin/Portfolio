using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 탄환 ID에 맞는 풀에서 탄환 오브젝트를 가져오는 팩토리.
/// </summary>
public class BulletFactory : MonoBehaviour {
	[SerializeField] private List<BulletPoolData> _bulletPoolDataList;
	[SerializeField] private Transform _poolParent;

	private readonly Dictionary<int, ObjectPooler<BulletObject>> _poolers = new Dictionary<int, ObjectPooler<BulletObject>>();

	private void Awake() {
		CreatePoolers();
	}

	/// <summary>
	/// ID에 해당하는 탄환을 풀에서 가져온다.
	/// </summary>
	public BulletObject GetBullet(int id) {
		if (!_poolers.TryGetValue(id, out ObjectPooler<BulletObject> pooler)) {
			Debug.LogWarning($"Bullet pool not found. Id: {id}");
			return null;
		}

		BulletObject bullet = pooler.Get();
		bullet.SetId(id);
		return bullet;
	}

	/// <summary>
	/// 사용한 탄환을 다시 풀로 반환한다.
	/// </summary>
	public void ReturnBullet(BulletObject bullet) {
		if (bullet == null) {
			return;
		}

		bullet.Release();
	}

	private void CreatePoolers() {
		if (_bulletPoolDataList == null) {
			return;
		}

		for (int i = 0; i < _bulletPoolDataList.Count; i++) {
			BulletPoolData data = _bulletPoolDataList[i];

			if (data.Prefab == null || _poolers.ContainsKey(data.Id)) {
				continue;
			}

			_poolers.Add(data.Id, new ObjectPooler<BulletObject>(data.Prefab, _poolParent));
		}
	}
}

/// <summary>
/// 탄환 ID와 프리팹을 묶는 풀 생성 데이터.
/// </summary>
[Serializable]
public class BulletPoolData {
	public int Id;
	public BulletObject Prefab;
}
