#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CanEditMultipleObjects]
[CustomEditor(typeof(WallControllerParking))]
public class WallContollerParkingEditor : Editor {

	private int WallID_wcp;
	private int WallMID_wcp;
	private BuilderEngineParking Engine;

	string[] options = new string[]
	{
		"Closed", "Open", "Aperture"
	};

	void OnEnable(){
		if(EditorPrefs.HasKey ("WallID_wcp"))
			WallID_wcp = EditorPrefs.GetInt ("WallID_wcp");
		if(EditorPrefs.HasKey ("WallMID_wcp"))
			WallMID_wcp = EditorPrefs.GetInt ("WallMID_wcp");
		WallControllerParking myScript = (WallControllerParking)target;
		myScript.Wall_ID = WallID_wcp;
		myScript.Wall_Material_ID = WallMID_wcp;
	}

	void OnDisable(){
		WallControllerParking myScript = (WallControllerParking)target;
		WallID_wcp = myScript.Wall_ID;
		WallMID_wcp = myScript.Wall_Material_ID;
		EditorPrefs.SetInt ("WallID_wcp", WallID_wcp);
		EditorPrefs.SetInt ("WallMID_wcp", WallMID_wcp);
	}

	public override void OnInspectorGUI()
	{
		if (Engine == null) {
			if (GameObject.Find ("BuilderEngineParking").GetComponent<BuilderEngineParking> () != null) {
				Engine = GameObject.Find ("BuilderEngineParking").GetComponent<BuilderEngineParking> ();
			} else {
				Debug.LogWarning ("Builder Engine Parking Component not found! Return");
				return;
			}
		}

		WallControllerParking myScript = (WallControllerParking)target;
		GameObject[] obj = Selection.gameObjects;
		if (GUILayout.Button ("Rebuild Wall", GUILayout.Height (30))) {
			/*myScript.Wall_ID = WallID_wcp;
			myScript.Wall_Material_ID = WallMID_wcp;*/
			myScript.RebuildWallParking ();
		}

		WallID_wcp = EditorGUILayout.Popup("Type", WallID_wcp, options);
		myScript.Wall_ID = WallID_wcp + 1;

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Material ID");
		if(GUILayout.Button("-"))	myScript.Wall_Material_ID = myScript.Wall_Material_ID - 1;
		if(GUILayout.Button("+"))	myScript.Wall_Material_ID = myScript.Wall_Material_ID + 1;
		myScript.Wall_Material_ID = EditorGUILayout.IntSlider(myScript.Wall_Material_ID,0,Engine.wall_materials.Length-1);
		EditorGUILayout.EndHorizontal ();
	}
}
#endif