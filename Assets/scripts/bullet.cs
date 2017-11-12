using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class bullet : MonoBehaviour {
	public GameObject player;
	float time=2f;
	GameObject temp;
	void OnTriggerEnter(Collider other){
		temp = other.transform.root.gameObject;
		Debug.Log ("shoot,"+temp.name);
		if (temp.tag.Equals ("Player")) {
			temp.GetComponent<manageLife> ().reduceLife (10, player);
			Destroy (gameObject);
		}
	}

	void Start(){
	}

	void Update(){
		if(time<0f)
			Destroy(gameObject);
		time -= Time.deltaTime;
	}
}
