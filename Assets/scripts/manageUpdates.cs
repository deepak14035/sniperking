using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class manageUpdates : MonoBehaviour {
	Text text;
	string[] lines;
	float time=3f;
	// Use this for initialization
	void Start () {
		text=GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if (text.text.Length > 1f) {
			lines = text.text.Split (new char[]{'\n'});
			if (lines.Length > 0) {
				time -= Time.deltaTime;
				if (time <= 0f) {
					time=3f;
					string text2="";
					for(int i=1;i<lines.Length;i++){
						text2+=lines[i]+"\n";
					}
					text.text=text2;
				}
			}
		}
	}
}
