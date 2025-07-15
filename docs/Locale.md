## Locale
게임에서 언어 변경에 대응하기 위한 스크립트<br>
Key/Value 조합을 통해 언어별로 번역을 저장하여 사용

---
### 언어 변경 시 UI에서 즉시 수정
![언어 변경](Locale.gif "언어 변경")

```csharp
// LocaleManager.cs
// 현재 언어 변경
public async void ChangeCurrentLanguage(bool isOn, SystemLanguage language) {
	if (isOn) {
		await LocaleManager.SetCurrentLanguage(language);
		SetLocale();
	}
}

// UILocale.cs의 SetLocale() 호출.
private void SetLocale() {
	_textLocale1.SetLocale();
	_textLocale2.SetLocale();
}

// UILocale.cs
// 등록된 text에 연결된 Key값의 Value를 불러와서 문구 설정
public void SetLocale() {
	foreach (var data in DataList) {
		if (data.Text == null) {
			continue;
		}

		data.Text.text = LocaleManager.Get(data.Key);
	}
}
```

---
### 자식 오브젝트의 모든 텍스트 불러오기
텍스트인 자식 오브젝트들을 불러오고 Key를 연결할 수 있는 기능<br>
누구나 쉽게 사용 가능

![텍스트 로드](Locale2.gif "텍스트 로드")