using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class manageLife : MonoBehaviour {
	Image healthbar;
	public float life=100f;
	public Text displayScore;
	float timeremaining=3f;
	static Vector3[] positions;
	int tflag=0;
	public GameObject attacker;
	string tag;
	public int kills=0,deaths=0;
	public void reduceLife(int val,GameObject source){
		life -= val;
		attacker=source;
		gameObject.GetComponent<FSMMain> ().attacker = source;
		gameObject.GetComponent<FSMMain>().attacker=source;
		gameObject.GetComponent<FSMMain>().evade();

		//if(gameObject.tag.Equals("mainplayer"))
		//	Debug.Log ("life reduced");
		if (life <= 0f) {
			life = -1f;
			displayScore.text+="\n"+gameObject.name+" destroyed by "+source.name;
			source.GetComponent<FSMMain>().action=1;

			if(gameObject.tag.Equals("mainplayer")){
				gameObject.GetComponent<movePlayer>().deaths++;
				string[] score=gameObject.GetComponent<movePlayer>().displaydeaths.text.Split(new char[] { ':'});
				score[1]=":"+gameObject.GetComponent<movePlayer>().deaths+"";
				gameObject.GetComponent<movePlayer>().displaydeaths.text=score[0]+score[1];
				attacker=null;
			}
			if(source.tag.Equals("mainplayer")){
				source.GetComponent<movePlayer>().kills++;
				string[] score=source.GetComponent<movePlayer>().displaykills.text.Split(new char[] { ':'});
				score[1]=":"+source.GetComponent<movePlayer>().kills+"";
				source.GetComponent<movePlayer>().displaykills.text=score[0]+score[1];
				attacker=null;
			}
			deaths++;
			source.GetComponent<manageLife>().kills++;
			respawn();
		}
		healthbar.fillAmount = life / 100f;
		
	}

	public void respawn(){
		Debug.Log ("respawning "+gameObject.name);
		gameObject.tag = "Respawn";
		int pos=Mathf.FloorToInt(Random.value*5f);
		transform.position=positions[pos];
		transform.Find("Capsule").GetComponent<MeshRenderer> ().enabled = false;
		transform.Find("Capsule").GetComponent<CapsuleCollider> ().enabled = false;
		transform.Find("Canvas").GetComponent<Canvas> ().enabled = false;
		MeshRenderer[] arr=transform.Find("Ak47").GetComponentsInChildren<MeshRenderer> ();
		for (int i=0; i<arr.Length; i++) {
			arr[i].enabled=false;
		}
		tflag = 1;
		timeremaining = 3f;
		gameObject.GetComponent<FSMMain>().action=0;

	}

	// Use this for initialization
	void Start () {
		healthbar = transform.Find("Canvas").Find("red").Find("green").gameObject.GetComponent<Image>();
		positions=new Vector3[5];
		positions [0] = new Vector3 (7.2f,2.5f,9.2f);
		positions [1] = new Vector3 (7.2f,2.5f,16.3f);
		positions [2] = new Vector3 (25f,2.5f,25f);
		positions [3] = new Vector3 (4f,2.5f,-16f);
		positions [4] = new Vector3 (-15f,2.5f,-15f);
		tag = gameObject.tag;

	}
	
	// Update is called once per frame
	void Update () {
		if (tflag == 1) {
			timeremaining-=Time.deltaTime;
			//Debug.Log("time remaining-"+timeremaining);
			if(timeremaining<=0f){
				life=100f;
				transform.Find("Capsule").GetComponent<MeshRenderer> ().enabled = true;
				transform.Find("Capsule").GetComponent<CapsuleCollider> ().enabled = true;
				transform.Find("Canvas").GetComponent<Canvas> ().enabled = true;
				MeshRenderer[] arr=transform.Find("Ak47").GetComponentsInChildren<MeshRenderer> ();
				for (int i=0; i<arr.Length; i++) {
					arr[i].enabled=true;
				}
				gameObject.tag=tag;
				healthbar.fillAmount = life / 100f;
				timeremaining=3f;
				tflag=0;
				gameObject.GetComponent<FSMMain>().action=1;

			}
		}
	}
}
