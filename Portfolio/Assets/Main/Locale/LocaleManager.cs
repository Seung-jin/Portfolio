using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LocaleManager {
	private static readonly HashSet<SystemLanguage> USE_LANGUAGE = new() {
		SystemLanguage.English, SystemLanguage.Korean, SystemLanguage.Japanese
	};

	private static Dictionary<string, string> _dict;

	public static SystemLanguage CurrentLanguage { get; private set; } = SystemLanguage.English;

	public static async UniTask SetCurrentLanguage(SystemLanguage language) {
		if (!USE_LANGUAGE.Contains(language)) {
			Debug.LogWarning($"[LocaleManager] SetCurrentLanguage : {language} is not supported.");
			language = SystemLanguage.English;
		}

		if (CurrentLanguage.Equals(language)) {
			return;
		}

		CurrentLanguage = language;
		await LoadLocale();
	}

	public static async UniTask LoadLocale() {
		var request = Addressables.LoadAssetAsync<TextAsset>("Locale/TextFile");
		await request;

		var tsv = TSVParser.Parse(request.Result.text);
		var keyIndex = Array.IndexOf(tsv[0], "key");
		var valueIndex = Array.IndexOf(tsv[0], CurrentLanguage.ToString());

		_dict = tsv.Skip(1).ToDictionary(
		                                 line => line[keyIndex],
		                                 line => line[valueIndex]
		                                );
	}

	public static string Get(string key) {
		return _dict.GetValueOrDefault(key, key);
	}

	public static string GetValue(string key, params object[] values) {
		string data = Get(key);
		return string.IsNullOrEmpty(data) ? key : string.Format(key, values);
	}
}