using System.Collections.Generic;
using UnityEngine;

	public class UIBase : MonoBehaviour {
		public UIType uiType;
		public BackButtonType backButtonType;

		public virtual void Init() { }

		public virtual void Show(Dictionary<string, object> param = null) {
			gameObject.SetActive(true);
		}

		public virtual void AfterShow() { }

		public virtual void Hide() {
			gameObject.SetActive(false);
		}

		public virtual void OnBack() {
			UIManager.Hide(uiType);
		}

		public virtual void Refresh() { }
	}