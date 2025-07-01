using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDirectionExample : MonoBehaviour {
	[SerializeField] private UIDirectionTextSequential _textSequential;
	[SerializeField] private TMP_InputField _textSequentialInputField;
	[SerializeField] private Button _textSequentialPlayBtn;
	[SerializeField] private Button _textSequentialSkipBtn;

	[SerializeField] private UIDirectionScrollSequential _scrollSequential;
	[SerializeField] private Button _scrollSequentialPlayBtn;

	private void Awake() {
		_textSequentialPlayBtn.onClick.AddListener(OnClickTextSequentialPlayBtn);
		_textSequentialSkipBtn.onClick.AddListener(OnClickTextSequentialSkipBtn);

		_scrollSequentialPlayBtn.onClick.AddListener(OnClickScrollSequentialPlayBtn);
	}

	private void OnClickTextSequentialPlayBtn() {
		_textSequential.Play(_textSequentialInputField.text);
	}

	private void OnClickTextSequentialSkipBtn() {
		_textSequential.Skip();
	}

	private void OnClickScrollSequentialPlayBtn() {
		_scrollSequential.Play();
	}
}