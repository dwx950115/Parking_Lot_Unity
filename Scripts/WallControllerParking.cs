#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WallControllerParking : MonoBehaviour {

	public int Wall_ID;
	public int Wall_Material_ID;
	private BuilderEngineParking Engine;

	public void RemoveAllChildrens(){
		for (int i = 0; i < 20; i++) {
			foreach (Transform child in transform) {
				DestroyImmediate (child.gameObject);
			}
		}
	}

	public void RebuildWallParking(){
		PrepareParameters ();
		if (Engine != null) {
			RemoveAllChildrens ();
		

			CreateWall (transform.localPosition, transform.localRotation, 3);
		}
	}

	private void CreateWall(Vector3 _position, Quaternion _rotation, float _height = 3){
		if (Wall_ID == 1 || Wall_ID == 2) {
			if (transform.parent.GetComponent<RoomControllerParking> () != null) {
				_height = transform.parent.GetComponent<RoomControllerParking> ().Wall_Height;
			}
			// Instantiate Wall
			GameObject Wall = Instantiate (Engine.walls [Wall_ID], _position, Quaternion.Euler (0, 0, 0), transform);
			Wall.transform.localScale = Vector3.one;
			Wall.name = transform.name;
			Wall.transform.localPosition = Vector3.zero;
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
						Wall.transform.GetChild (i).gameObject.GetComponent<MeshRenderer> ().sharedMaterial = Engine.wall_materials [Wall_Material_ID];
						Vector2 texScale;
						texScale.x = Wall.transform.localScale.x;
						texScale.y = Wall.transform.localScale.y;
						Wall.transform.GetChild (i).gameObject.GetComponent<MeshRenderer> ().sharedMaterial.mainTextureScale = texScale;
					}
				}
			}

			Wall.transform.localRotation = Quaternion.Euler (Vector3.zero);

			Wall.transform.parent = transform.parent;

			Wall.transform.localPosition = new Vector3(Wall.transform.localPosition.x, 0f, Wall.transform.localPosition.z);

			int instMethod = 0;

			for (int i = 0; i < Wall.transform.childCount; i++) {
				if (Wall.transform.GetChild (i).name == "Wall_UpPart") {
					Selection.activeGameObject = Wall;
					Wall = Wall.transform.GetChild (i).gameObject;
					instMethod = 1;
				}
				if (instMethod == 0) {
					if (Wall.transform.GetChild (i).name == "Wall_Up") { 
						for (int y = 0; y < Wall.transform.childCount; y++) {
							Wall.transform.GetChild (y).transform.localPosition = Vector3.zero;
						}
						Selection.activeGameObject = Wall;
						Wall = Wall.transform.GetChild (i).gameObject;
						instMethod = 2;
					}
				}
			}

			if (instMethod == 0) {
				Wall.transform.localScale = new Vector3 (1, _height, 1);
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

			DestroyImmediate (this.gameObject);
		}
		if (Wall_ID == 3) {
			CreateApperture (_position, _rotation, _height);
		}
	}

	private void CreateApperture(Vector3 _position, Quaternion _rotation, float _height = 3){
		if (transform.parent.GetComponent<RoomControllerParking> () != null) {
			_height = transform.parent.GetComponent<RoomControllerParking> ().Wall_Height;
		}
		// Instantiate Wall
		GameObject Wall = Instantiate (Engine.walls [Wall_ID], _position, Quaternion.Euler (0, 0, 0), transform);
		Wall.transform.localScale = new Vector3(1,_height - 2, 1);
		Wall.name = transform.name;
		Wall.transform.localPosition = new Vector3 (0, _height, 0);;
		Wall.AddComponent<WallControllerParking> ();
		BoxCollider Collider = Wall.AddComponent<BoxCollider> ();
		Vector3 ColliderSize = Collider.size;
		ColliderSize.y = _height-2;
		ColliderSize.z = 0.05f;
		Vector3 ColliderCenter = Collider.center;
		ColliderCenter.y = -(_height-2) / 2f;
		Wall.GetComponent<BoxCollider> ().size = ColliderSize;
		Wall.GetComponent<BoxCollider> ().center = ColliderCenter;
		Wall.GetComponent<MeshRenderer> ().sharedMaterial = Engine.wall_materials [Wall_Material_ID];

		Wall.transform.localRotation = Quaternion.Euler (Vector3.zero);
		Wall.transform.parent = transform.parent;


		DestroyImmediate (this.gameObject);

		Selection.activeGameObject = Wall;
	}

	public void PrepareParameters(){
		if (Engine == null) {
			if (GameObject.Find ("BuilderEngineParking").GetComponent<BuilderEngineParking> () != null) {
				Engine = GameObject.Find ("BuilderEngineParking").GetComponent<BuilderEngineParking> ();
			} else {
				Debug.LogWarning ("Builder Engine Parking Component not found! Return");
				return;
			}
		}
	}
}
#endif