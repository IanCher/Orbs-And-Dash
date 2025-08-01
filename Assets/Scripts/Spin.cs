using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour {

    public Vector3 spinSpeeds;

	void Update () {
        transform.Rotate(Vector3.up, Time.deltaTime * spinSpeeds.y);		
        transform.Rotate(Vector3.left, Time.deltaTime * spinSpeeds.x);		
        transform.Rotate(Vector3.forward, Time.deltaTime * spinSpeeds.z);		
	}
}