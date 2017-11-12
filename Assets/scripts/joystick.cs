using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class joystick : MonoBehaviour, IDragHandler, IPointerUpHandler,IPointerDownHandler {
	private Image bgimg,joystickimg;
	public Vector3 inputvector;

	public virtual void OnPointerDown(PointerEventData ped){
		OnDrag (ped);
	}
	public virtual void OnPointerUp(PointerEventData ped){
		joystickimg.rectTransform.anchoredPosition = Vector2.zero;
		inputvector = Vector3.zero;
	}
	public virtual void OnDrag(PointerEventData ped){
		Vector2 pos;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle 
		    (bgimg.rectTransform, ped.position, ped.pressEventCamera, out pos)) {
			pos.x = pos.x / bgimg.rectTransform.sizeDelta.x;
			pos.y = pos.y / bgimg.rectTransform.sizeDelta.y;
			inputvector = new Vector3 ((2 * pos.x) + 1, 0, (2 * pos.y) - 1);
			if (inputvector.magnitude > 1f) {
				inputvector = inputvector.normalized;
			}
			joystickimg.rectTransform.anchoredPosition = new Vector2 (inputvector.x * bgimg.rectTransform.sizeDelta.x / 3,
			                                      inputvector.z * bgimg.rectTransform.sizeDelta.y / 3);
			//Debug.Log (inputvector);

		}
		//Debug.Log (pos);
	}

	// Use this for initialization
	void Start () {
		bgimg = GetComponent<Image> ();
		joystickimg = transform.GetChild (0).GetComponent<Image> ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}





































