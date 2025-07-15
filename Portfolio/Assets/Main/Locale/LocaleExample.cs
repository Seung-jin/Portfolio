using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocaleExample : MonoBehaviour {
	[SerializeField] private UILocale _textLocale1;
	[SerializeField] private List<Toggle> _example1ToggleList;

	[SerializeField] private UILocale _textLocale2;

	private async void Awake() {
		try {
			await LocaleManager.LoadLocale();

			SetLocale();
		} catch (Exception e) {
			throw;
		}

		_example1ToggleList[0].onValueChanged.AddListener(isOn => ChangeCurrentLanguage(isOn, SystemLanguage.English));
		_example1ToggleList[1].onValueChanged.AddListener(isOn => ChangeCurrentLanguage(isOn, SystemLanguage.Korean));
		_example1ToggleList[2].onValueChanged.AddListener(isOn => ChangeCurrentLanguage(isOn, SystemLanguage.Japanese));
	}

	public async void ChangeCurrentLanguage(bool isOn, SystemLanguage language) {
		if (isOn) {
			await LocaleManager.SetCurrentLanguage(language);
			SetLocale();
		}
	}

	private void SetLocale() {
		_textLocale1.SetLocale();
		_textLocale2.SetLocale();
	}
}