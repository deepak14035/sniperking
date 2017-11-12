using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class enemyAI : MonoBehaviour {
	int start=0;
	GameObject[] players;
	RaycastHit hit;
	GameObject target;
	int targetflag=0;
	float mindist,shoottime=0f;
	int move=0;
	Vector3 movement;
	public Rigidbody projectile;
	public GameObject mainplayer;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (start == 0) {
			GameObject[] p1=GameObject.FindGameObjectsWithTag("Player");
			players=new GameObject[p1.Length+1];
			for(int i=0;i<p1.Length;i++){
				players[i]=p1[i];
			}
			players[p1.Length]=mainplayer;
			start=1;
		}
		mindist = 999f;
		for (int i=0; i<players.Length; i++) {
			float distance=Vector3.Distance(transform.position,players[i].transform.position);
			if(Physics.Raycast(transform.position,players[i].transform.position-transform.position,out hit,distance+1f)){
				if((hit.transform.gameObject.tag.Equals("Player")||hit.transform.gameObject.tag.Equals("mainplayer"))&&distance<mindist){
					targetflag=1;
					target=hit.transform.parent.gameObject;
					//if(target.tag.Equals("mainplayer"))
					//	Debug.Log (gameObject.name+"--");
					mindist=distance;
				}
			}
		}
		if (targetflag == 1 &&target.GetComponent<manageLife> ().life > 0f) {
			Quaternion dirn = Quaternion.LookRotation (target.transform.position - transform.position);
			transform.rotation = Quaternion.Lerp (transform.rotation, dirn, Time.deltaTime*3f);
			Vector3 diff=dirn.eulerAngles-transform.rotation.eulerAngles;
			if (diff.magnitude < 10f&&shoottime>0.75f) {
				Rigidbody clone = Instantiate (projectile, transform.position+transform.forward, transform.rotation) as Rigidbody;
				clone.gameObject.GetComponent<bullet> ().player = gameObject;
				clone.AddForce (transform.forward * 3000f);
				shoottime=0f;
				//if(target.tag.Equals("mainplayer"))
				//	Debug.Log (gameObject.name+"-shot!!!!"+diff.magnitude);

			}
		}
		if(shoottime<0.75f)
			shoottime += Time.deltaTime;
		if (move == 0) {
			movement=new Vector3((Random.value*10f)-5f,0f,(Random.value*10f)-5f);
			movement+=transform.position;
			move=1;
		}
		if (move == 1) {
			transform.position=Vector3.Lerp(transform.position,movement,Time.deltaTime*0.5f);
			if(Physics.Raycast(transform.position,movement-transform.position,1f)
			   ||Vector3.Distance(transform.position,movement)<1f){
				move=0;
			}
		}
		if (target != null) {
			if (target.GetComponent<manageLife> ().life <= 0f) {
				targetflag = 0;
				target=null;
			}
		}
	}
}
