#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderEngineParking : MonoBehaviour {

	public GameObject[] walls;
	public Material[] wall_materials;

	public GameObject[] ground;
	public Material[] ground_materials;

	public GameObject[] ceiling;
	public Material[] ceiling_materials;

	public GameObject[] stair;
	public Material[] stair_materials;
}
#endif
