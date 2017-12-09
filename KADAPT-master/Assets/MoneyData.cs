using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoneyData : MonoBehaviour {

	public bool taken;

	// Use this for initialization
	void Start () {
		taken = false;	
	}
	
	// Update is called once per frame
	void Update () {
		if (taken == true) {
			this.GetComponent<MeshRenderer> ().enabled = false;
			//this.GetComponent<GameObject> ().SetActive (false);
			this.GetComponent<NavMeshObstacle>().enabled=false;
			this.GetComponent<BoxCollider> ().enabled = false;

		} else {
			this.GetComponent<MeshRenderer> ().enabled = true;
			//this.GetComponent<GameObject> ().SetActive (true);
			this.GetComponent<NavMeshObstacle>().enabled=true;
			this.GetComponent<BoxCollider> ().enabled = true;
		}
	}
}
