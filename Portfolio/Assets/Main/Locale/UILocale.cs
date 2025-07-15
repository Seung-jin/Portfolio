using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UILocale : MonoBehaviour {
	public List<Data> DataList = new();

	public void SetLocale() {
		foreach (var data in DataList) {
			if (data.Text == null) {
				continue;
			}

			data.Text.text = LocaleManager.Get(data.Key);
		}
	}

	public void LoadAllText() {
		DataList = GetComponentsInChildren<TextMeshProUGUI>()
		           .Select(x => new Data { Text = x, Key = x.name })
		           .ToList();
	}

	[Serializable]
	public class Data {
		public TextMeshProUGUI Text;
		public string Key;
	}
}