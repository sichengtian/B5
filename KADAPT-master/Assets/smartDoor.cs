using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smartDoor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            if (Input.GetKey(KeyCode.F))
            {
                 this.GetComponent<Animator>().SetBool("Open", true);
            }
        }
    }
}
