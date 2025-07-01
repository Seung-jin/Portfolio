using UnityEngine;
using UnityEngine.UI;

public class UIOption : UIBase {
	[SerializeField] private Button _exitBtn;

	public override void Init() {
		base.Init();
		_exitBtn.onClick.AddListener(Hide);
	}
}