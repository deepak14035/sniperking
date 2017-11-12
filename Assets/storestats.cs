using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class storestats : MonoBehaviour {
	public GameObject[] players;
	// Use this for initialization
	void Start () {
		players = GameObject.FindGameObjectsWithTag ("Player");

	}
	void OnApplicationQuit(){
		int i;
		StreamWriter writer=new StreamWriter("stats.txt",true);
		for (i=0; i<players.Length; i++) {
			writer.WriteLine(players[i].GetComponent<manageLife>().kills+","+players[i].GetComponent<manageLife>().deaths);
			writer.Flush();
		}
		writer.Close ();
	}

}
