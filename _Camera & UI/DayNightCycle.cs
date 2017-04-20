using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.RotateAround (Vector3.zero, Vector3.right, 1.5f*Time.deltaTime); //rotate around zero point along the right axis at the speed
		transform.LookAt (Vector3.zero);


	}
}
