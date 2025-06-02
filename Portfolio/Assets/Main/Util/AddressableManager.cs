using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Locusts.Util {
	public class AddressableManager : Singleton<AddressableManager> {
		/// <summary>
		/// Addressable의 Key를 통해 리소스를 로드.
		/// </summary>
		public static async UniTask<TObject> LoadAssetAsync<TObject>(string key) {
			float startAt = Time.realtimeSinceStartup;
			var op = Addressables.LoadAssetAsync<TObject>(key);
			try {
				await op;
			} catch (Exception e) {
				Debug.LogWarning("[AddressableManager] LoadAssetAsync Exception : " + e.Message);
			}

			Debug.LogWarning($"[AddressableManager] LoadAssetAsync {op.Status} : {(Time.realtimeSinceStartup - startAt)}sec");
			return op.Result;
		}

		/// <summary>
		/// 불러올 리소스들의 키 리스트를 받아서 한번에 로드.
		/// </summary>
		public static async UniTask<IList<TObject>> LoadAssetAsyncList<TObject>(IEnumerable<string> keyList, Action<TObject> callback) {
			float startAt = Time.realtimeSinceStartup;
			try {
				var op = Addressables.LoadAssetsAsync(keyList, callback, Addressables.MergeMode.Union);
				await op;
				Debug.LogWarning($"[AddressableManager] LoadAssetAsync {op.Status} : {(Time.realtimeSinceStartup - startAt)}sec.\nCount : {op.Result.Count} / {keyList.Count()}");
				return op.Result;
			} catch (Exception e) {
				Debug.LogWarning("[AddressableManager] LoadAssetAsync Exception : " + e.Message);
				return null;
			}
		}

		/// <summary>
		/// 불러올 리소스들의 키 리스트를 받아서 한번에 로드.
		/// </summary>
		public static async UniTask<Dictionary<string, TObject>> LoadAssetAsyncDic<TObject>(IEnumerable<string> keyList, Action<TObject> callback) {
			var resourceList = await LoadAssetAsyncList(keyList, callback);
			if (resourceList is null) {
				return null;
			}

			var result = new Dictionary<string, TObject>();
			int i = 0;
			foreach (string key in keyList) {
				result.Add(key, resourceList[i]);
				i++;
			}

			return result;
		}
	}
}