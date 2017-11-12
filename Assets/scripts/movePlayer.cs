using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class movePlayer : MonoBehaviour {
	public Rigidbody projectile;
	public static int score;
	public Text displayScore,displaykills,displaydeaths;
	int flag=0;
	public joystick jstick;
	private Vector3 _mouseReference;
	private Vector3 _mouseOffset,_rotation;
	bool rotateflag=false;
	public Image jostick,fire;
	GameObject camera;
	public int kills=0,deaths=0;
	GameObject cylinder,target;
	Vector3 direction,targetposition;
	public float collideflag=1f;
	float time=0.5f,fieldofview=60f;
	RaycastHit hit;
	int scopeflag=0;
	// Use this for initialization
	void Start () {
		score = 0;
		camera = transform.GetChild (0).gameObject;
		displaykills.text += "0";
		displaydeaths.text += "0";
		cylinder = transform.Find ("Capsule").gameObject;
	}
	
	int move=0;
	
	void FixedUpdate () {
		direction = cylinder.transform.forward * jstick.inputvector.z + cylinder.transform.right * jstick.inputvector.x;
		Debug.DrawLine (cylinder.transform.position + direction.normalized, cylinder.transform.position + direction);
		if (!Physics.Raycast (cylinder.transform.position, direction, 1f))
			transform.position += (transform.forward * jstick.inputvector.z * 0.05f) + ((transform.right * jstick.inputvector.x * 0.05f));
		collideflag = 1f;
		if (rotateflag) {
			//if(!RectTransformUtility.RectangleContainsScreenPoint(jostick.rectTransform,Input.mousePosition)){
			Touch[] myTouches = Input.touches;
			foreach (Touch touch in Input.touches) {
				if (!RectTransformUtility.RectangleContainsScreenPoint (jostick.rectTransform, touch.position)) {
					_mouseOffset = touch.position;
					_mouseOffset = (_mouseOffset - _mouseReference);
					
					// apply rotation
					_rotation.y = (_mouseOffset.x) * 0.15f;
					camera.transform.Rotate (new Vector3 (-0.1f, 0f, 0f) * _mouseOffset.y);
					Quaternion q = camera.transform.localRotation;
					// rotate
					transform.Rotate (_rotation);
					_mouseReference = touch.position;
					
					Debug.Log ("rotate");
					break;
				}
			}
		}
		if (time <= 0.2f)
			time += Time.deltaTime;
		foreach (Touch touch in Input.touches) {
			if (touch.phase.Equals(TouchPhase.Began)&&!rotateflag && !RectTransformUtility.RectangleContainsScreenPoint (jostick.rectTransform, touch.position)) {
				_mouseReference = touch.position;
				rotateflag = true;
				Debug.Log ("about to rotate");
			}
			if (touch.phase.Equals(TouchPhase.Began)&&RectTransformUtility.RectangleContainsScreenPoint (fire.rectTransform, Input.mousePosition)) {
				Rigidbody clone = Instantiate (projectile, transform.position + 2 * camera.transform.forward, transform.rotation) as Rigidbody;
				clone.AddForce (camera.transform.forward * 3000f);
				clone.gameObject.GetComponent<bullet> ().player = gameObject;
				time = 0f;
			}
			if (touch.phase.Equals(TouchPhase.Ended)&& rotateflag && !RectTransformUtility.RectangleContainsScreenPoint (jostick.rectTransform, touch.position)) {
				rotateflag = false;
				
			}
			if ((touch.phase.Equals(TouchPhase.Began)||touch.phase.Equals(TouchPhase.Stationary)||touch.phase.Equals(TouchPhase.Moved))&&RectTransformUtility.RectangleContainsScreenPoint (fire.rectTransform, Input.mousePosition)) {
				if (time >= 0.2f) {
					Rigidbody clone = Instantiate (projectile, transform.position + 2 * camera.transform.forward, transform.rotation) as Rigidbody;
					clone.AddForce (camera.transform.forward * 3000f);
					clone.gameObject.GetComponent<bullet> ().player = gameObject;
					time = 0f;
				}
			}
		}
			if (Physics.Raycast (transform.position + camera.transform.forward, camera.transform.forward, out hit, 50)
				&& hit.transform.gameObject.tag.Equals ("Player") && Vector3.Distance (hit.transform.position, transform.position) > 12f) {
				fieldofview -= 3f;
				if (fieldofview < 20f) {
					fieldofview = 20f;
					scopeflag = 1;
				}
				camera.GetComponent<Camera> ().fieldOfView = fieldofview;
				targetposition = hit.transform.position;
				target = hit.transform.gameObject;
				Debug.Log ("scoping");
			} else if (scopeflag == 1) {
				if (Vector3.Magnitude (targetposition - target.transform.position) > 2f) {
					fieldofview += 3f;
					if (fieldofview > 60f) {
						fieldofview = 60f;
						scopeflag = 0;
					}
					camera.GetComponent<Camera> ().fieldOfView = fieldofview;
					Debug.Log ("oos");
				}
			}
		
		//}
	}
}
