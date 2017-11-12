using UnityEngine;
using System.Collections;

public class walking : MonoBehaviour {
	private float forward, turn, run, fire;
	private Animator anim;
	// Use this for initialization
	void Start () {
		anim=GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		forward = Input.GetAxis ("Vertical");
		anim.SetFloat ("walk", forward);
		Debug.Log (forward);
		turn = Input.GetAxis ("Horizontal");
		anim.SetFloat ("turn", turn);
		run = Input.GetAxis ("Run");
		anim.SetFloat ("sprint", run);
		fire = Input.GetAxis ("Fire1");
		//if (fire > 0.0f)
		//	anim.enabled = false;
	}
}
