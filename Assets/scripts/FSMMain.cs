using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSMMain : MonoBehaviour {
	float selectMovement;
	float survivor, killer,temp;
	float velocity=0.5f;
	int[] state;
	public int action;
	public Vector3 movement;
	Camera mycamera;
	static GameObject[] opponents;
	public Rigidbody projectile;
	RaycastHit hit;
	float gunSpeed=0.25f;
	GameObject temphit;
	float totaltime=0f;
	float radius=8f;
	int noOfPoints=20;
	public GameObject attacker;
	NavMeshAgent agent;
	NavMeshHit hit1;
	NavMeshPath path;
	/*
	1 - wander
	2 - evade
	3 - follow
	4 - shoot
	
	 */
	//SPEED OF TURN
	//SPEED OF WALK
	//PROB. OF WANDER/HIDE
	//distance from position last 10 seconds ago
	public GameObject target;
	// Use this for initialization
	void Start () {
		survivor = 0.9f;
		action = 1;
		opponents = GameObject.FindGameObjectsWithTag ("Player");
		state=new int[8+1];
		movement=new Vector3((Random.value*10f-2f),0f,(Random.value*10f-2f));
		movement+=transform.position;
		mycamera=gameObject.GetComponentInChildren<Camera>();
		agent=gameObject.GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		path = new NavMeshPath ();
	}

	void lookForEnemy(int[] state){
		for (int i=0; i<opponents.Length; i++) {
			temp=Vector3.Angle(opponents[i].transform.position-transform.position,transform.forward);

			if(Mathf.Abs(temp)<=60f){
				float distance=Vector3.Distance(transform.position,opponents[i].transform.position);
				if(opponents[i]!=gameObject && checkIfEnemyIsPresent(opponents[i],distance)){
					target=opponents[i];
					action=4;

					return;
				
				}
			}
		
		}
		action = 1;
	}

	void findCover(){
		float dist = 2f;
		int numOfValues = 0;
		while (true) {
			Vector3 ran2,ran = Random.insideUnitSphere * dist;
			ran.y=transform.position.y;
			NavMesh.SamplePosition(ran,out hit1,transform.position.y,NavMesh.AllAreas);
			ran2=hit1.position;


			if(Vector3.Distance(ran2,ran)>0.1f){
				movement=ran2;
				movement.y=transform.position.y;
				break;
			}
			numOfValues++;
			if(numOfValues>dist*4){
				dist+=2f;
				numOfValues=0;
			}
		}
	}

	void walkAlongCover(){
		
	}

	bool checkIfEnemyIsPresent(GameObject enemy, float distance){
		bool val = Physics.Raycast (transform.position+transform.up*0.25f, enemy.transform.position - transform.position, out hit, distance + 1f);
		if (val) {
			temphit = hit.transform.root.gameObject;
			//Debug.Log(temphit.name+","+temphit.tag);
			return (temphit.tag.Equals ("Player") || temphit.tag.Equals ("mainplayer"));
		}
		return false;
	}

	void selectAction(){
		if (action == 1) {
			//Debug.Log ("Wander");
			wander ();
			lookForEnemy (state);

		} else if (action == 4) {
			//Debug.Log ("attack");
			attackEnemy ();
		} else if (action == 2) {
			//Debug.Log ("evade "+gameObject.name);
			Debug.DrawLine(transform.position,(attacker.transform.position-transform.position).normalized*(Vector3.Magnitude(attacker.transform.position-transform.position)+1f));
			if(Physics.Raycast(transform.position,attacker.transform.position-transform.position,out hit,Vector3.Distance(attacker.transform.position,transform.position)+1f)){

				if(!hit.transform.gameObject.tag.Equals("Player")){
					agent.ResetPath();
					agent.enabled=false;
					action=1;
					attacker=null;
				}
			}
			NavMesh.CalculatePath(transform.position, movement, NavMesh.AllAreas, path);
			for (int i = 0; i < path.corners.Length-1; i++)
				Debug.DrawLine(path.corners[i], path.corners[i+1], Color.red);
		}
	}

	public void evade(){
		Vector3 candidatepos,bestpos=Vector3.up;
		float val,mindist = 99999;
		for (int i=0; i<50; i++) {
			candidatepos=Random.insideUnitSphere*5f;
			candidatepos.y=transform.position.y;
			if(NavMesh.SamplePosition(candidatepos,out hit1,transform.position.y,NavMesh.AllAreas)){
				candidatepos=hit1.position;
			}
			candidatepos.y=transform.position.y;
			if(Physics.Raycast(candidatepos,attacker.transform.position-candidatepos,out hit,Vector3.Magnitude(attacker.transform.position-candidatepos)+1f)){
				if(!hit.transform.root.gameObject.tag.Equals("Player")){
					agent.enabled=true;
					agent.SetDestination(candidatepos);
					val=0;

					if(agent.remainingDistance<mindist){
						mindist=agent.remainingDistance;
						bestpos=candidatepos;
					}
					agent.ResetPath();
					agent.enabled=false;
				}
			}
		}
		movement = bestpos;
		agent.enabled = true;
		agent.SetDestination (bestpos);
		GameObject.CreatePrimitive (PrimitiveType.Cube).transform.position = movement + transform.up*3f;
		action = 2;

		Debug.Log (gameObject.name+" evading-"+bestpos);

	}

	void attackEnemy(){
		Quaternion dirn = Quaternion.LookRotation (target.transform.position - transform.position);
		transform.rotation = Quaternion.Lerp (transform.rotation, dirn, Time.deltaTime*5f);
		Vector3 diff=dirn.eulerAngles-transform.rotation.eulerAngles;
		if (diff.magnitude < 10f&&gunSpeed>0.25f) {
			Rigidbody clone = Instantiate (projectile, transform.position+transform.forward, transform.rotation) as Rigidbody;
			clone.gameObject.GetComponent<bullet> ().player = gameObject;
			clone.AddForce ((transform.forward+transform.right*(Random.value-0.5f)*0.03f+transform.up*(Random.value-0.5f)*0.06f) * 2000f);
			gunSpeed=0f;
			
		}
		if (target!=null && (checkIfEnemyIsPresent(target,Vector3.Distance(transform.position,target.transform.position))==false)) {
			Debug.Log(checkIfEnemyIsPresent(target,Vector3.Distance(transform.position,target.transform.position))+","+hit.transform.root.gameObject.name);
			Debug.DrawLine(transform.position+transform.up*0.25f,target.transform.position+transform.up*0.25f);
			target=null;
			action=1;
			gunSpeed=0.26f;

		}
		//Debug.Log(target.GetComponent<manageLife> ().life+","+gameObject.GetComponent<manageLife> ().life+","+checkIfEnemyIsPresent(target,Vector3.Distance(transform.position,target.transform.position)));

		if(gunSpeed<=0.25f)
			gunSpeed += Time.deltaTime;
	}

	void wander(){
		Debug.DrawLine (transform.position,transform.position+(movement - transform.position).normalized*2f);
		if (Physics.Raycast (transform.position, movement - transform.position, 2f)
			|| Vector3.Distance (transform.position, movement) < 1f) {
			movement = new Vector3 ((Random.value * 50f-25f), 0f, (Random.value * 50f-25f));
			movement = transform.position+movement.normalized*8f;
		} else {
			Quaternion dirn = Quaternion.LookRotation (movement - transform.position);
			transform.rotation = Quaternion.Lerp (transform.rotation, dirn, Time.deltaTime * 1f);
			transform.position+=(movement-transform.position).normalized*velocity*0.1f;
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		selectAction ();
	}
}
