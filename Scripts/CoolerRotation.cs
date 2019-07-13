using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolerRotation : MonoBehaviour {

    public float speed = 10;
    private void FixedUpdate()
    {
        transform.Rotate(0, 0, Time.deltaTime * speed);
    }
}
