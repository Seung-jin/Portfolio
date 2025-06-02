using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Locusts.Util;
using UnityEngine;

public class UIManager : Singleton<UIManager> {
	[SerializeField] private Transform _uiParent;

	private static readonly Dictionary<UIType, string> _uiDataDic = new() {
		{ UIType.Main, "UIMain.prefab" },
		{ UIType.Option, "UIOption.prefab" },
		{ UIType.GameExit, "UIGameExit.prefab" },
	};

	private static Dictionary<UIType, UIBase> _uiBaseDic = new();

	/// <summary>
	/// 현재 보여지고 있는 UI.
	/// </summary>
	private static List<UIType> _showUIList = new();

	/// <summary>
	/// UI 보여질 때 순서 저장.
	/// </summary>
	private static List<UIType> _showWaitQueue = new();

	public static async void Show(UIType uiType, Dictionary<string, object> param = null) {
		_showWaitQueue.Add(uiType);
		if (!_uiBaseDic.ContainsKey(uiType)) {
			await Load(uiType);
		}

		await UniTask.WaitUntil(() => _showWaitQueue.First().Equals(uiType));
		var uiBase = _uiBaseDic.GetValueOrDefault(uiType);
		uiBase.transform.SetAsLastSibling();
		uiBase.Show(param);
		if (_showUIList.Contains(uiType)) {
			_showUIList.Remove(uiType);
		}

		_showUIList.Add(uiType);

		await UniTask.WaitUntil(() => uiBase.gameObject.activeInHierarchy);
		uiBase.AfterShow();
		_showWaitQueue.Remove(uiType);
	}

	private static async UniTask Load(UIType uiType) {
		Debug.Log("[UIManager] Load " + uiType);
		if (!_uiDataDic.TryGetValue(uiType, out var uiData)) {
			return;
		}

		try {
			var loaded = await AddressableManager.LoadAssetAsync<GameObject>("UIPrefab/" + uiData);
			var uiPrefab = GameObject.Instantiate(loaded);
			if (uiPrefab is null) {
				Debug.LogError("[UIManager] uiPrefab is null. uiType is " + uiType);
				return;
			}

			var script = uiPrefab.GetComponent<UIBase>();
			script.uiType = uiType;
			_uiBaseDic.Add(uiType, script);
			script.transform.SetParent(Instance._uiParent, false);
			script.Init();
		} catch (Exception e) {
			Debug.LogError("[UIManager] Load error. " + e);
			throw;
		}
	}

	public static void Hide(UIType uiType) {
		var uiBase = _uiBaseDic.GetValueOrDefault(uiType);
		if (uiBase is null) {
			return;
		}

		uiBase.Hide();
		_showUIList.Remove(uiType);
	}

	public static void OnBack(UIType uiType) {
		UIBase uiBase = _uiBaseDic.GetValueOrDefault(uiType);
		if (uiBase is null) {
			return;
		}

		switch (uiBase.backButtonType) {
			case BackButtonType.Close:
				uiBase.OnBack();
				break;
			case BackButtonType.GameExit:
				// 게임 종료 팝업.
				break;
		}
	}

	public static void Refresh(UIType uiType) {
		if (!_uiBaseDic.ContainsKey(uiType)) {
			return;
		}

		_uiBaseDic.GetValueOrDefault(uiType).Refresh();
	}

	public static bool IsShow(UIType uiType) {
		return _showUIList.Contains(uiType);
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (_showUIList.Count > 0) {
				OnBack(_showUIList.Last());
			}
		}
	}
}