#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(RoomControllerParking))]
public class RoomControllerParkingEditor : Editor {

	private BuilderEngineParking Engine;

	AnimBool m_ShowWalls;
	AnimBool m_ShowGround;
	AnimBool m_ShowCeiling;

	void OnEnable(){
		m_ShowWalls = new AnimBool(true);
		m_ShowWalls.valueChanged.AddListener(Repaint);
		m_ShowGround = new AnimBool(true);
		m_ShowGround.valueChanged.AddListener(Repaint);
		m_ShowCeiling = new AnimBool(true);
		m_ShowCeiling.valueChanged.AddListener(Repaint);
	}

	public override void OnInspectorGUI()
	{
		RoomControllerParking myScript = (RoomControllerParking)target;

		if (Engine == null) {
			if (GameObject.Find ("BuilderEngineParking").GetComponent<BuilderEngineParking> () != null) {
				Engine = GameObject.Find ("BuilderEngineParking").GetComponent<BuilderEngineParking> ();
			} else {
				Debug.LogWarning ("Builder Engine Parking Component not found! Return");
				return;
			}
		}

		GUILayout.Label ("Current version of Builder Engine Parking - alpha", GUILayout.Height (20));

		if (GUILayout.Button ("Delete Room")) {
			myScript.RemoveAllChildrens ();
		}

		GUILayout.Label ("\n", GUILayout.Height (5));

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Rebuild Room", GUILayout.Height (60))) {
			myScript.Build ();
		}
		EditorGUILayout.BeginVertical ();
		GUILayout.Label ("\n", GUILayout.Height (10));
		//AutoRebuid = EditorGUILayout.ToggleLeft ("AutoRebuid", AutoRebuid, GUILayout.Width (85));
		myScript.CreateReflectionProbe = EditorGUILayout.ToggleLeft ("Ref Probe", myScript.CreateReflectionProbe, GUILayout.Width (85));
		EditorGUILayout.EndVertical ();
		EditorGUILayout.EndHorizontal ();

		GUILayout.Label ("Room Parameters", EditorStyles.boldLabel);
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Lenght");
		if(GUILayout.Button("-"))	myScript.Room_Lenght--;
		if(GUILayout.Button("+"))	myScript.Room_Lenght++;
		myScript.Room_Lenght = EditorGUILayout.IntSlider(myScript.Room_Lenght,1,50);
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Width");
		if(GUILayout.Button("-"))	myScript.Room_Width--;
		if(GUILayout.Button("+"))	myScript.Room_Width++;
		myScript.Room_Width = EditorGUILayout.IntSlider(myScript.Room_Width,1,50);
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Height");
		if(GUILayout.Button("-"))	myScript.Wall_Height = Mathf.Round(myScript.Wall_Height*10)/10-0.1f;
		if(GUILayout.Button("+"))	myScript.Wall_Height= Mathf.Round(myScript.Wall_Height*10)/10+0.1f;
		myScript.Wall_Height = EditorGUILayout.Slider(myScript.Wall_Height,2.1f,5);
		EditorGUILayout.EndHorizontal ();

		GUILayout.Label ("\nFilling Parameters", EditorStyles.boldLabel);

		//Walls Parameters
		m_ShowWalls.target = EditorGUILayout.ToggleLeft("Create Walls", m_ShowWalls.target, EditorStyles.boldLabel);
		if (!m_ShowWalls.target)
			myScript.Wall_ID = 0;
		if (Engine.walls.Length == 0 && m_ShowWalls.target) {
			Debug.LogWarning ("Assign 'Walls' in Builder Engine");
			m_ShowWalls.target = false;
		}

		if (EditorGUILayout.BeginFadeGroup(m_ShowWalls.faded))
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField("Wall Type");
			if(GUILayout.Button("P"))	myScript.Wall_ID--;
			if(GUILayout.Button("N"))	myScript.Wall_ID++;
			myScript.Wall_ID = EditorGUILayout.IntSlider(myScript.Wall_ID,1,Engine.walls.Length-1);
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
			myScript.ApplyMaterial_Wall = EditorGUILayout.BeginToggleGroup ("Apply Material?", myScript.ApplyMaterial_Wall);
			EditorGUILayout.BeginHorizontal ();
			if(GUILayout.Button("P"))	myScript.Wall_Material_ID--;
			if(GUILayout.Button("N"))	myScript.Wall_Material_ID++;
			myScript.Wall_Material_ID = EditorGUILayout.IntSlider(myScript.Wall_Material_ID,0,Engine.wall_materials.Length-1);
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.EndToggleGroup ();
			EditorGUILayout.EndHorizontal ();
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.EndFadeGroup();

		if(m_ShowWalls.target)
			GUILayout.Label ("\n");

		//Ground Parameters
		m_ShowGround.target = EditorGUILayout.ToggleLeft("Create Ground", m_ShowGround.target, EditorStyles.boldLabel);
		if (!m_ShowGround.target)
			myScript.Ground_ID = 0;
		if (Engine.ground.Length == 0 && m_ShowGround.target) {
			Debug.LogWarning ("Assign 'Ground' in Builder Engine");
			m_ShowGround.target = false;
		}

		if (EditorGUILayout.BeginFadeGroup(m_ShowGround.faded))
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField("Ground Type");
			if(GUILayout.Button("P"))	myScript.Ground_ID--;
			if(GUILayout.Button("N"))	myScript.Ground_ID++;
			myScript.Ground_ID = EditorGUILayout.IntSlider(myScript.Ground_ID,1,Engine.ground.Length-1);
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
			myScript.ApplyMaterial_Ground = EditorGUILayout.BeginToggleGroup ("Apply Material?", myScript.ApplyMaterial_Ground);
			EditorGUILayout.BeginHorizontal ();
			if(GUILayout.Button("P"))	myScript.Ground_Material_ID--;
			if(GUILayout.Button("N"))	myScript.Ground_Material_ID++;
			myScript.Ground_Material_ID = EditorGUILayout.IntSlider(myScript.Ground_Material_ID,0,Engine.ground_materials.Length-1);
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.EndToggleGroup ();
			EditorGUILayout.EndHorizontal ();
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.EndFadeGroup();

		if(m_ShowGround.target)
			GUILayout.Label ("\n");

		//Ceiling Parameters
		m_ShowCeiling.target = EditorGUILayout.ToggleLeft("Create Ceiling", m_ShowCeiling.target, EditorStyles.boldLabel);
		if (!m_ShowCeiling.target)
			myScript.Ceiling_ID = 0;
		if (Engine.ceiling.Length == 0 && m_ShowCeiling.target) {
			Debug.LogWarning ("Assign 'Ceiling' in Builder Engine");
			m_ShowCeiling.target = false;
		}

		if (EditorGUILayout.BeginFadeGroup(m_ShowCeiling.faded))
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField("Ceiling Type");
			if(GUILayout.Button("P"))	myScript.Ceiling_ID--;
			if(GUILayout.Button("N"))	myScript.Ceiling_ID++;
			myScript.Ceiling_ID = EditorGUILayout.IntSlider(myScript.Ceiling_ID,1,Engine.ceiling.Length-1);
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
			myScript.ApplyMaterial_Ceiling = EditorGUILayout.BeginToggleGroup ("Apply Material?", myScript.ApplyMaterial_Ceiling);
			EditorGUILayout.BeginHorizontal ();
			if(GUILayout.Button("P"))	myScript.Ceiling_Material_ID--;
			if(GUILayout.Button("N"))	myScript.Ceiling_Material_ID++;
			myScript.Ceiling_Material_ID = EditorGUILayout.IntSlider(myScript.Ceiling_Material_ID,0,Engine.ceiling_materials.Length-1);
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.EndToggleGroup ();
			EditorGUILayout.EndHorizontal ();
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.EndFadeGroup();

		if(m_ShowCeiling.target)
			GUILayout.Label ("\n");
	}
}
#endif