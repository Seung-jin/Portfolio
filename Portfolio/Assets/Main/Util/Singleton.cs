using UnityEngine;

namespace Locusts.Util {
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
		private static T _instance;

		public static T Instance {
			get {
				if (_instance is null) {
					_instance = FindFirstObjectByType(typeof(T)) as T;
				}

				return _instance;
			}
		}

		protected virtual void Awake() {
			DontDestroyOnLoad(this);
		}
	}
}