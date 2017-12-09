using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class policeController : MonoBehaviour {

	public GameObject player;
	System.Random rnd;

	private float wantedTime;
	private bool goBack = false;

    public GameObject caught;
	void Start () {
		rnd = new System.Random ();
		wantedTime = 60;
        caught.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		if (GameObject.FindGameObjectWithTag ("Player").GetComponent<GameMeta> ().isWanted) {
			goBack = true;
			wantedTime -= Time.deltaTime;
			//Debug.Log (wantedTime);
			if (this.GetComponent<NavMeshAgent> ().remainingDistance < 0.2) {
				this.GetComponent<NavMeshAgent> ().speed = 5;
				this.GetComponent<Animator> ().SetFloat ("InputVertical", 0.5f);
				updateDestination ();
			}


            //check for vision
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector3 directionToTarget = this.transform.position - player.transform.position;
            float angle = Vector3.Angle(transform.forward, directionToTarget);
            float distance = directionToTarget.magnitude;

            if (angle > 90 && angle < 180 && distance < 5)
            {
                Debug.Log("into if");
                Vector3 tmp = new Vector3(this.transform.position.x, 0.4f, this.transform.position.z);
                Ray newRay = new Ray(tmp, this.transform.forward);
                RaycastHit hit;
                if (Physics.Raycast(newRay, out hit))
                {
                    if (hit.transform.name.Contains("3rdPersonController"))
                    {
                        caught.SetActive(true);
                        Time.timeScale = 0;
                    }
                }
            }


        } else {
			wantedTime = 60;
			if (goBack == true) {
				this.GetComponent<NavMeshAgent> ().ResetPath ();
				Vector3 des = new Vector3 (107.9f, 0, -68.9f);
				NavMeshPath path = new NavMeshPath ();
				this.GetComponent<NavMeshAgent> ().CalculatePath (des, path);
				this.GetComponent<NavMeshAgent> ().SetPath (path);


				if (this.GetComponent<NavMeshAgent> ().remainingDistance < 0.5) {
					goBack = false;
					this.GetComponent<NavMeshAgent> ().ResetPath ();
					this.GetComponent<Animator> ().SetFloat ("InputVertical", 0f);
				}
			}
		}
		if (wantedTime <= 0) {
			GameObject.FindGameObjectWithTag ("Player").GetComponent<GameMeta> ().isWanted= false;
		}
	}

	void updateDestination(){
		System.Random rnd = new System.Random ();
		Vector3 tmpPoint;
		float x=0, z=0;
		x = (float)rnd.Next (-6, 6);
		z = (float)rnd.Next (-6, 6);
		NavMeshPath path = new NavMeshPath ();
		tmpPoint = new Vector3 (player.transform.position.x+x, 0,player.transform.position.z+z);
		this.GetComponent<NavMeshAgent> ().CalculatePath (tmpPoint, path);
		this.GetComponent<NavMeshAgent> ().SetPath (path);
	}
		
}
