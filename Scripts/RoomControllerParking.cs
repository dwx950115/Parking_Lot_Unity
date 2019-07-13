#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RoomControllerParking : MonoBehaviour {

	private BuilderEngineParking Engine;

	[Header("Room Parameters")]
	[Range(2.5f,5)]
	public float Wall_Height;
	[Range(1,50)]
	public int Room_Lenght;
	[Range(1,50)]
	public int Room_Width;
	[Header("Wall Settings")]
	public int Wall_ID;
	public int Wall_Material_ID;
	public bool ApplyMaterial_Wall;
	[Header("Ground Settings")]
	public int Ground_ID;
	public int Ground_Material_ID;
	public bool ApplyMaterial_Ground;
	[Header("Ceiling Settings")]
	public int Ceiling_ID;
	public int Ceiling_Material_ID;
	public bool ApplyMaterial_Ceiling;

	public bool CreateReflectionProbe;

	public void Build(){
		if (Engine == null) {
			if (GameObject.Find ("BuilderEngineParking").GetComponent<BuilderEngineParking> () != null) {
				Engine = GameObject.Find ("BuilderEngineParking").GetComponent<BuilderEngineParking> ();
			} else {
				Debug.LogWarning ("Builder Engine Parking Component not found! Return");
				return;
			}
		}

		RemoveAllChildrens ();
		CreateRoom (Room_Lenght, Room_Width, Wall_Height);
	}

	private void CreateRoom(int _Room_Lenght, int _Room_Width, float _Room_Height){
		if (Wall_ID == 0) {
			Debug.LogWarning ("Wall requst");
			return;
		}
		//wall one
		for (int i = 0; i < _Room_Lenght; i++) {
			CreateWall (new Vector3(i,0,-0.5f), Quaternion.Euler (0, 0, 0), _Room_Height, i);
		}
		//wall two
		for (int i = 0; i < _Room_Lenght; i++) {
			CreateWall (new Vector3(i,0,_Room_Width-0.5f), Quaternion.Euler (0, 180, 0), _Room_Height, i);
		}
		//wall three
		for (int i = 0; i < _Room_Width; i++) {
			CreateWall (new Vector3(-0.5f,0,i), Quaternion.Euler (0, 90, 0), _Room_Height, i);
		}
		//wall four
		for (int i = 0; i < _Room_Width; i++) {
			CreateWall (new Vector3(_Room_Lenght-0.5f,0,i), Quaternion.Euler (0, 270, 0), _Room_Height, i);
		}

		CreateCeilingAndGround (_Room_Height);
		if (CreateReflectionProbe)
			CreateReflectionProbeVoid ();
	}

	private void CreateWall(Vector3 _position, Quaternion _rotation, float _height = 3, int _i = 0){
		// Instantiate Wall
		GameObject Wall = Instantiate (Engine.walls [Wall_ID], _position, Quaternion.Euler (0, 0, 0), transform);
		Wall.name = "Wall" + _i;
		Vector3 position = _position;
		Wall.transform.localPosition = position;
		Wall.AddComponent<WallControllerParking> ();
		BoxCollider Collider = Wall.AddComponent<BoxCollider> ();
		Vector3 ColliderSize = Collider.size;
		ColliderSize.y = _height;
		ColliderSize.z = 0.05f;
		Vector3 ColliderCenter = Collider.center;
		ColliderCenter.y = _height / 2f;
		Wall.GetComponent<BoxCollider> ().size = ColliderSize;
		Wall.GetComponent<BoxCollider> ().center = ColliderCenter;
		if (Wall_Material_ID != 0) {
			if (Wall.transform.childCount == 0) {
				Wall.GetComponent<MeshRenderer> ().sharedMaterial = Engine.wall_materials [Wall_Material_ID];
				Vector2 texScale;
				texScale.x = Wall.transform.localScale.x;
				texScale.y = Wall.transform.localScale.y;
				Wall.GetComponent<MeshRenderer> ().sharedMaterial.mainTextureScale = texScale;
			} else {
				for (int i = 0; i < Wall.transform.childCount; i++) {
					Wall.transform.GetChild(i).gameObject.GetComponent<MeshRenderer> ().sharedMaterial = Engine.wall_materials [Wall_Material_ID];
					Vector2 texScale;
					texScale.x = Wall.transform.localScale.x;
					texScale.y = Wall.transform.localScale.y;
					Wall.transform.GetChild(i).gameObject.GetComponent<MeshRenderer> ().sharedMaterial.mainTextureScale = texScale;
				}
			}
		}
		Wall.transform.localRotation = _rotation;

		int instMethod = 0;
		for (int i = 0; i < Wall.transform.childCount; i++) {
			if (Wall.transform.GetChild (i).name == "Wall_UpPart") {
				Wall = Wall.transform.GetChild (i).gameObject;
				instMethod = 1;
			}
			if (instMethod == 0) {
				if (Wall.transform.GetChild (i).name == "Wall_Up") { 
					for (int y = 0; y < Wall.transform.childCount; y++) {
						Wall.transform.GetChild (y).transform.localPosition = Vector3.zero;
					}
					Wall = Wall.transform.GetChild (i).gameObject;
					instMethod = 2;
				}
			}
		}

		if (instMethod == 0) {
			Wall.transform.localScale = new Vector3(1, _height, 1);
		}

		if (instMethod == 1) {
			Vector3 scale = Wall.transform.lossyScale;
			scale.x = 1;
			scale.z = 1;
			scale.y = _height - 1;
			Wall.transform.localScale = scale;
		}
		if (instMethod == 2) {
			Wall.transform.localPosition = new Vector3 (Wall.transform.localPosition.x, _height, Wall.transform.localPosition.z);
			Wall.transform.localScale = Vector3.one;
		}

		Undo.RegisterCreatedObjectUndo (Wall, "Created go");
	}

	private void CreateCeilingAndGround(float _height){
		for (int i = 0; i < Room_Width; i++) {
			for (int y = 0; y < Room_Lenght; y++) {
				if (Ground_ID != 0) {
					GameObject Ground = Instantiate (Engine.ground [Ground_ID], Vector3.zero, Quaternion.Euler (0, 0, 0), transform);
					Ground.transform.localScale = Vector3.one;
					Ground.name = "Ground";
					Ground.transform.localPosition = new Vector3 (y, 0, i);
					BoxCollider Collider = Ground.AddComponent<BoxCollider> ();
					Vector3 ColliderSize = Collider.size;
					ColliderSize.y = 0.05f;
					Ground.GetComponent<BoxCollider> ().size = ColliderSize;
					Ground.GetComponent<BoxCollider> ().center = new Vector3 (0, -ColliderSize.y/2, 0);
					if (ApplyMaterial_Ground) {
						if (Wall_Material_ID != 0 && Ground_Material_ID == 0) {
							Ground.GetComponent<MeshRenderer> ().sharedMaterial = Engine.wall_materials [Wall_Material_ID];
						} else {
							Ground.GetComponent<MeshRenderer> ().sharedMaterial = Engine.ground_materials [Ground_Material_ID];
						}
					}

					Undo.RegisterCreatedObjectUndo (Ground, "Created go");
				}

				if (Ceiling_ID != 0) {
					GameObject Ceiling = Instantiate (Engine.ceiling [Ceiling_ID], Vector3.zero, Quaternion.Euler (180, 0, 0), transform);
					Ceiling.transform.localScale = Vector3.one;
					Ceiling.name = "Ceiling";
					Ceiling.transform.localPosition = new Vector3 (y, _height, i);
					BoxCollider Collider = Ceiling.AddComponent<BoxCollider> ();
					Vector3 ColliderSize = Collider.size;
					ColliderSize.y = 0.05f;
					Ceiling.GetComponent<BoxCollider> ().size = ColliderSize;
					if (ApplyMaterial_Ceiling) {
						if (Wall_Material_ID != 0 && Ceiling_Material_ID == 0) {
							Ceiling.GetComponent<MeshRenderer> ().sharedMaterial = Engine.wall_materials [Wall_Material_ID];
						} else {
							Ceiling.GetComponent<MeshRenderer> ().sharedMaterial = Engine.ceiling_materials [Ceiling_Material_ID];
						}
					}

					Undo.RegisterCreatedObjectUndo (Ceiling, "Created go");
				}
			}
		}
	}

	private void CreateReflectionProbeVoid(){
		GameObject probeGameObject = new GameObject("The Reflection Probe");
		ReflectionProbe probeComponent = probeGameObject.AddComponent<ReflectionProbe>() as ReflectionProbe;
		probeComponent.resolution = 256;
		probeComponent.size = new Vector3(Room_Lenght+0.15f, Wall_Height+0.15f, Room_Width+0.15f);
		probeGameObject.transform.parent = transform;
		probeGameObject.transform.localPosition = new Vector3((Room_Lenght-1f)/2, Wall_Height/2f, (Room_Width-1f)/2);
		probeComponent.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.EveryFrame;
	}

	public void RemoveAllChildrens(){
		for (int i = 0; i < 20; i++) {
			foreach (Transform child in transform) {
				Undo.DestroyObjectImmediate (child.gameObject);
			}
		}
	}
}
#endif
