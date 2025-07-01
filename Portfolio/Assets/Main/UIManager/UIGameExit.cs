using UnityEngine;
using UnityEngine.UI;

public class UIGameExit : UIBase {
	[SerializeField] private Button _exitBtn;
	[SerializeField] private Button _closeBtn;

	public override void Init() {
		base.Init();
		_exitBtn.onClick.AddListener(OnClickExitBtn);
		_closeBtn.onClick.AddListener(Hide);
	}

	private void OnClickExitBtn() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
		Application.Quit();
	}
}