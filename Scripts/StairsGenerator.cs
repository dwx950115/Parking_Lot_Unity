#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsGenerator : MonoBehaviour {

	public int stairID;
	public int stairMaterialID;

	private BuilderEngineParking Engine;

	public int stairCount;

	public void RemoveAllChildrens(){
		for (int i = 0; i < 20; i++) {
			foreach (Transform child in transform) {
				DestroyImmediate (child.gameObject);
			}
		}
	}

	public void CreateStairs(){
		PrepareParameters ();
		RemoveAllChildrens ();
		for (int i = 0; i < stairCount; i++) {
			CreateStair (i);
		}
	}



	private void CreateStair(int _i){
		GameObject Stair = Instantiate (Engine.stair [stairID], new Vector3(0f, 0.25f * _i, 0.3f * _i), Quaternion.Euler (0, 270, 0), transform);
		Stair.transform.localScale = Vector3.one;
		Stair.name = "Stair";
		Stair.transform.parent = transform;
		Stair.transform.localPosition = new Vector3(0f, 0.25f * _i, 0.3f * _i);
		Stair.transform.localRotation = Quaternion.Euler (0, 270, 0);
		if (stairMaterialID != 0 && Stair.transform.childCount == 0) {
			Stair.GetComponent<MeshRenderer> ().sharedMaterial = Engine.stair_materials [stairMaterialID];
			BoxCollider Collider = Stair.AddComponent<BoxCollider> ();
		} else {
			foreach (Transform child in Stair.transform) {
				if(child.gameObject.name == "Stair"){
					child.gameObject.GetComponent<MeshRenderer> ().sharedMaterial = Engine.stair_materials [stairMaterialID];
				}
			}
		}
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
