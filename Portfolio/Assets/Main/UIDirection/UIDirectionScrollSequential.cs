using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class UIDirectionScrollSequential : UIDirection {
	private ScrollRect _scrollRect;
	private Transform[] _childTransformArr;

	[SerializeField, Header("스크롤 끝으로 이동 여부")]
	private bool _isMoveToLast;

	[SerializeField, Header("다음 오브젝트 나오기까지 걸리는 시간")]
	private float _sequentialDelay;

	private void Awake() {
		_scrollRect = GetComponent<ScrollRect>();
		_childTransformArr = _scrollRect.content.GetComponentsInChildren<Transform>();
	}

	protected override async UniTask PlayAsync() {
		// _scrollRect.verticalNormalizedPosition = 0f;
		foreach (Transform childTransform in _childTransformArr) {
			childTransform.gameObject.SetActive(false);
		}

		foreach (Transform childTransform in _childTransformArr) {
			childTransform.gameObject.SetActive(true);
			if (_isMoveToLast) {
				if (_scrollRect.vertical) {
					_scrollRect.verticalNormalizedPosition = 1f;
				}

				if (_scrollRect.horizontal) {
					_scrollRect.horizontalNormalizedPosition = 1f;
				}
			}

			await UniTask.WaitForSeconds(_sequentialDelay, cancellationToken: _cancellationToken.Token);
		}

		IsPlaying = false;
		_endCb?.Invoke();
	}

	public override void Skip() {
		base.Skip();
		foreach (Transform childTransform in _childTransformArr) {
			childTransform.gameObject.SetActive(true);
		}

		_scrollRect.verticalNormalizedPosition = (_scrollRect.vertical && _isMoveToLast) ? 1f : 0f;
		_scrollRect.horizontalNormalizedPosition = (_scrollRect.horizontal && _isMoveToLast) ? 1f : 0f;
	}
}