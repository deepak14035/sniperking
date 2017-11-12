using UnityEngine;
using System.Collections;

public class mainmenuscript : MonoBehaviour {

	public Rigidbody projectile;
	void Update () {
		//transform.Translate (new Vector3(0.0f, 0.0f, 10.0f) * Time.deltaTime*Input.GetAxis("Fire1"), Camera.main.transform);
		
		if (Input.GetButtonDown ("Fire1")) {
			Rigidbody clone = Instantiate (projectile, transform.position, transform.rotation) as Rigidbody;
			clone.AddForce (transform.forward*2000);
		}
	}
}
