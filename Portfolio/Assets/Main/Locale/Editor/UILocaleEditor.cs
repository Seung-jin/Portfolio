using TMPro;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UILocale))]
public class UILocaleEditor : Editor {
	public override void OnInspectorGUI() {
		UILocale uiLocale = (UILocale)target;

		DrawDefaultInspector();

		// EditorGUILayout.Space();
		// EditorGUILayout.LabelField("=== UILocale Tools ===", EditorStyles.boldLabel);

		// if (uiLocale.DataList != null && uiLocale.DataList.Count > 0) {
		// 	EditorGUILayout.LabelField($"DataList Count: {uiLocale.DataList.Count}");
		// 	EditorGUI.indentLevel++;
		//
		// 	foreach (var data in uiLocale.DataList) {
		// 		EditorGUILayout.BeginHorizontal();
		// 		EditorGUILayout.ObjectField("Text", data.Text, typeof(TextMeshProUGUI), true);
		// 		EditorGUILayout.LabelField($"Key: {data.Key}");
		// 		EditorGUILayout.EndHorizontal();
		// 	}
		//
		// 	EditorGUI.indentLevel--;
		// } else {
		// 	EditorGUILayout.HelpBox("DataList is empty.", MessageType.Info);
		// }

		EditorGUILayout.Space();

		if (GUILayout.Button("Load All Text")) {
			Undo.RecordObject(uiLocale, "Load All Text");
			uiLocale.LoadAllText();

			EditorUtility.SetDirty(uiLocale);
		}
	}
}