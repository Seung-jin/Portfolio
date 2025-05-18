using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UITypeSetting))]
public class UITypeSettingEditor : Editor {
	private int _id;

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		UITypeSetting typeSetting = target as UITypeSetting;

		EditorGUILayout.BeginVertical();
		_id = EditorGUILayout.IntField("ID", _id);

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("<-", GUILayout.Width(50), GUILayout.Height(20))) {
			if (_id > 0) {
				_id--;
			}
		}

		if (GUILayout.Button("Active")) {
			typeSetting.Active(_id);
			EditorUtility.SetDirty(target);
		}

		if (GUILayout.Button("->", GUILayout.Width(50), GUILayout.Height(20))) {
			if (_id < typeSetting.infoList.Count - 1) {
				_id++;
			}
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.EndVertical();

	}
}