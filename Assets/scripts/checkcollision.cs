using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkcollision : MonoBehaviour {
	GameObject parent;
	void OnCollisionEnter(Collision other){
		parent.GetComponent<movePlayer>().collideflag = 0f;

	}

	// Use this for initialization
	void Start () {
		parent = gameObject.GetComponentInParent<Transform> ().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
