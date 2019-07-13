#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ColumnControllerParking))]
public class ColumnControllerParkingEditor : Editor {

	private ColumnControllerParking ColumnControllerParking;
	private float Height;

	void OnEnable(){
		ColumnControllerParking myScript = (ColumnControllerParking)target;
		Height = myScript.TakeHeight ();
	}

	public override void OnInspectorGUI()
	{
		ColumnControllerParking myScript = (ColumnControllerParking)target;

		GUILayout.Label ("Column Height");
		Height = EditorGUILayout.Slider(Height,2,5);

		if (GUILayout.Button ("Rebuild Column", GUILayout.Height (30))) {
			myScript.newHeight (Height-1);
		}
	}
}
#endif
