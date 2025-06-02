using UnityEngine;
using UnityEngine.UI;

namespace Locusts.UI {
	public class UIMain : UIBase {
		[SerializeField] private Button _optionBtn;

		public override void Init() {
			base.Init();
			_optionBtn.onClick.AddListener(OnClickOptionBtn);
		}

		private void OnClickOptionBtn() {
			UIManager.Show(UIType.Option);
		}
	}
}