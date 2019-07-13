using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoorController : MonoBehaviour {

	private bool closed = true;

	void FixedUpdate(){
		if(Input.GetKeyDown(KeyCode.Backspace)){
			if(closed){
				OpenDoor();
			} else {
				CloseDoor();
			}
			Debug.Log ("Elevator Event");
		}
	}

	public void OpenDoor(){
		GetComponent<Animator>().SetBool("Open", true);
	}

	public void CloseDoor(){
		GetComponent<Animator>().SetBool("Close", true);
	}

	public void DoorOpened(){
		GetComponent<Animator>().SetBool("Open", false);
		GetComponent<Animator>().SetBool("Close", false);
		closed = false;
	}

	public void DoorClosed(){
		GetComponent<Animator>().SetBool("Open", false);
		GetComponent<Animator>().SetBool("Close", false);
		closed = true;
	}
}
