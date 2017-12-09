using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CitizenController : MonoBehaviour {

	Animator anim;
	private NavMeshAgent citizen;
	GameObject[] citizens;
	private NavMeshPath navMeshPath;

	public bool stolen;

	public bool callPolice = false;
    
	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator> ();
		citizen = this.GetComponent<NavMeshAgent> ();
		anim.SetFloat ("InputVertical", 0.5f);
		citizens=GameObject.FindGameObjectsWithTag ("Citizen");

		Debug.Log (citizens.Length);
		navMeshPath=  new NavMeshPath ();
		stolen = false;

	}
	
	// Update is called once per frame
	void Update () {
		anim.SetFloat ("InputVertical", 0.5f);
		foreach (GameObject c in citizens) {
			if (c != null) {
				if (c.GetComponent<NavMeshAgent> ().remainingDistance < 0.2) {
                    if (c.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Run"))
                    {
                        calledPolice(c);
                    }
                    c.GetComponent<NavMeshAgent> ().SetDestination (CalcNewPoint (c.GetComponent<NavMeshAgent> ()));
				}
			}
		}
		if (callPolice == true) {
            callingPolice ();
		}
	}

	Vector3 CalcNewPoint(NavMeshAgent c){
		System.Random rnd = new System.Random ();
		Vector3 tmpPoint;

		float x=0, z=0;

		x = (float)rnd.Next (-140, 140);
		z = (float)rnd.Next (-140, 140);

		tmpPoint = new Vector3 (x, 0, z);


		return tmpPoint;
	}

	bool Walkable(Vector3 p,NavMeshAgent c){
		Debug.Log (p);
		Debug.Log (c);
		c.CalculatePath (p, navMeshPath);
		if (navMeshPath.status != NavMeshPathStatus.PathComplete) {
			return false;
		} else {
			return true;
		}
	}

	void callingPolice(){
        Vector3 des = new Vector3(104.9f, 0, -64.9f);
        NavMeshPath path = new NavMeshPath();
        this.GetComponent<NavMeshAgent>().CalculatePath(des, path);
        this.GetComponent<NavMeshAgent>().SetPath(path);
        this.GetComponent<Animator>().SetBool("callingPolice",true);
        this.GetComponent<NavMeshAgent>().speed = 10;
	}

    void calledPolice(GameObject citizen)
    {
        citizen.GetComponent<CitizenController>().callPolice = false;
        citizen.GetComponent<Animator>().SetBool("callingPolice", false);
        citizen.GetComponent<NavMeshAgent>().speed = 2;
        citizen.GetComponent<NavMeshAgent>().ResetPath();
        GameObject player= GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<GameMeta>().isWanted = true;
        print("in citizencontrol" + player.GetComponent<GameMeta>().isWanted);
		GameObject.FindGameObjectWithTag("Player").GetComponent<GameMeta> ().policeCalled = true;
        /*
        GameObject[] polices = GameObject.FindGameObjectsWithTag("Police");
        foreach(GameObject police in polices)
        {
            //set goto scene bool to true
        }
        */
    }
}
