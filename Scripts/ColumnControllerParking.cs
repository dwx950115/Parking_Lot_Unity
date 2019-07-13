using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnControllerParking : MonoBehaviour {

	private GameObject ColumnUpObject;

	public void newHeight(float _y){
		if (ColumnUpObject == null) {
			for (int i = 0; i < transform.childCount; i++) {
				if (transform.GetChild (i).name == "Column_Up") {
					ColumnUpObject = transform.GetChild (i).gameObject;
				}
			}
		}

		ColumnUpObject.transform.localScale = new Vector3 (ColumnUpObject.transform.localScale.x, _y, ColumnUpObject.transform.localScale.z);
		GetComponent<BoxCollider>().size = new Vector3(GetComponent<BoxCollider>().size.x, _y+1 , GetComponent<BoxCollider>().size.z);
		GetComponent<BoxCollider>().center = new Vector3(GetComponent<BoxCollider>().center.x, (_y+1)/2f , GetComponent<BoxCollider>().center.z);
	}

	public float TakeHeight(){
		if (ColumnUpObject == null) {
			for (int i = 0; i < transform.childCount; i++) {
				if (transform.GetChild (i).name == "Column_Up") {
					ColumnUpObject = transform.GetChild (i).gameObject;
				}
			}
		}

		return ColumnUpObject.transform.localScale.y+1;
	}
}
