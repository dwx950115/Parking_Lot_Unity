#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(StairsGenerator))]
public class StairsGeneratorEditor : Editor {

	private StairsGenerator ColumnControllerParking;
	private BuilderEngineParking Engine;

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

		StairsGenerator myScript = (StairsGenerator)target;

		if (GUILayout.Button ("Build Stairs", GUILayout.Height (30))) {
			myScript.CreateStairs ();
		}

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Stair Type");
		if(GUILayout.Button("P"))	myScript.stairID--;
		if(GUILayout.Button("N"))	myScript.stairID++;
		myScript.stairID = EditorGUILayout.IntSlider(myScript.stairID,1,Engine.stair.Length-1);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Stair Material");
		if(GUILayout.Button("P"))	myScript.stairMaterialID--;
		if(GUILayout.Button("N"))	myScript.stairMaterialID++;
		myScript.stairMaterialID = EditorGUILayout.IntSlider(myScript.stairMaterialID,1,Engine.stair_materials.Length-1);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Stair Number");
		if(GUILayout.Button("P"))	myScript.stairCount--;
		if(GUILayout.Button("N"))	myScript.stairCount++;
		myScript.stairCount = EditorGUILayout.IntSlider(myScript.stairCount,1,25);
		EditorGUILayout.EndHorizontal ();
	}
}
#endif
