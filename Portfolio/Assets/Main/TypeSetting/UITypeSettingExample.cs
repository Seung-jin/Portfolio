using TMPro;
using UnityEngine;

public class UITypeSettingExample : MonoBehaviour {
	[SerializeField] private UITypeSetting _test1Type;
	[SerializeField] private TextMeshProUGUI _test1BtnText;
	private int _test1BtnCount;

	public void OnChangeTest1Toggle1(bool isOn) {
		if (isOn) {
			_test1Type.Active(true);
		}
	}

	public void OnChangeTest1Toggle2(bool isOn) {
		if (isOn) {
			_test1Type.Active(false);
		}
	}

	public void OnClickBtn() {
		_test1BtnCount++;
		_test1BtnText.text = $"{_test1BtnCount} Clicked!";
	}

	[SerializeField] private UITypeSetting _gradType;

	public void OnChangeGradeToggle(int grade) {
		_gradType.Active(grade);
	}
}