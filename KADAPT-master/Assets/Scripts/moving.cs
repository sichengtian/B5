using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving : MonoBehaviour {

	private Collider rb;

	void Start () {
		rb = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.A)){
			rb.transform.position += Vector3.left * 10 * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.D)){
			rb.transform.position += Vector3.right * 10 * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.W)){
			rb.transform.position += Vector3.forward * 10 * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.S)){
			rb.transform.position += Vector3.back * 10 * Time.deltaTime;
		}
	}
}
