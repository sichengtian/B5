using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class GameMeta : MonoBehaviour {


	private int CurrentValue;
	private int TargetValue;

	public Text Money;
	public Text TimeT;
    public bool isWanted = false;
	private float timeRemaining;

	GameObject[] citizens;
	GameObject[] moneys;
	System.Random rnd;

	public bool policeCalled;
	public Light pLight;

	public GameObject wantedPanel;

    public GameObject win;
    public GameObject timeOut;

	// Use this for initialization
	void Start () {
		CurrentValue = 0;
		TargetValue = 10000;
		timeRemaining = 180;
		citizens = GameObject.FindGameObjectsWithTag ("Citizen");
		moneys = GameObject.FindGameObjectsWithTag ("Money");
		rnd = new System.Random ();
		refreshEnv ();
		policeCalled = false;
		pLight.color = Color.clear;
        win.SetActive(false);
        timeOut.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("Wanted:"+isWanted);
		Money.text=CurrentValue.ToString();
		//detect click
		if(Input.GetMouseButtonDown(0)){
			this.GetComponent<Animator>().SetBool("isReach",true);
			Vector3 tmp = new Vector3 (Camera.main.transform.position.x, 0.4f, Camera.main.transform.position.z);

			Ray newRay = new Ray (tmp, transform.forward);
			RaycastHit hit;
			if (Physics.Raycast (newRay, out hit)) {

				Vector3 directionToTarget = this.transform.position - hit.transform.position;
				float distance = directionToTarget.magnitude;

				if(hit.transform.name.Contains("Citizen") && distance<=4){
					stealFrom (hit.transform.GetComponent<CitizenController>());

				}
				if (hit.transform.name.Contains ("Money") && distance <= 4) {
					takeMoney (hit.transform.GetComponent<MoneyData>());
				}
				if (hit.transform.name.Contains ("special") && distance <= 4) {
					//Debug.Log ("hhhhhhh");
					int lower = Mathf.CeilToInt(TargetValue*0.045f);
					int higher = Mathf.CeilToInt(TargetValue * 0.065f);
					int amountStolen=rnd.Next (lower, higher);
					CurrentValue += amountStolen;
					Destroy (hit.transform.gameObject);
				}

			}

		}
		if(Input.GetMouseButtonDown(1)){
			this.GetComponent<Animator>().SetBool("isFighting",true);
			Vector3 tmp = new Vector3 (Camera.main.transform.position.x, 0.4f, Camera.main.transform.position.z);
			Ray newRay = new Ray (tmp, transform.forward);
			RaycastHit hit;
			if (Physics.Raycast (newRay, out hit)) {

				Vector3 directionToTarget = this.transform.position - hit.transform.position;
				float distance = directionToTarget.magnitude;

				if(hit.transform.name.Contains("Citizen") && distance<=4){
					Collider[] hitColliders = Physics.OverlapSphere(hit.transform.position, 20.0f);
					int i = 0;
					while (i < hitColliders.Length)
					{
						if(hitColliders[i].transform.position != hit.transform.position && hitColliders[i].CompareTag("Citizen")){
							Vector3 direction = hitColliders [i].transform.position - hit.transform.position;
							Ray newRay2 = new Ray(hit.transform.position,direction);
							RaycastHit hit2;
							if (Physics.Raycast (newRay2, out hit2)) {
								if(hit2.transform.name.Contains("Citizen")){
									hit2.transform.GetComponent<CitizenController> ().callPolice = true;
                                    Debug.Log("true");
								}
							}
						}
						i++;
					}
					StartCoroutine(Example(hit));
				}
			}
		}
		if(this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Fight540RoundHouse")){
			this.GetComponent<Animator>().SetBool("isFighting",false);
		}
		if(this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Reaching_RightHand")){
			this.GetComponent<Animator>().SetBool("isReach",false);
		}

		//timer
		timeRemaining -= Time.deltaTime;
		if (timeRemaining > 0) {
			TimeT.text = timeRemaining.ToString();

			if (isWanted == true) {

				wantedPanel.SetActive (true);

				pLight.intensity = 5;
				if (timeRemaining % 0.3 >= 0.2) {
					pLight.color=Color.red;
				} else {
					pLight.color=Color.blue;
				}
			} else {
				pLight.color=Color.clear;
				wantedPanel.SetActive (false);
			}


			if (timeRemaining % 100 <= 0.05) {
				refreshEnv ();
			}
		} else {
			TimeT.text = "Game Over";
            timeOut.SetActive(true);
            Time.timeScale = 0;
        }
        if(CurrentValue >= 10000)
        {
            win.SetActive(true);
            Time.timeScale = 0;
        }

	}

	IEnumerator Example(RaycastHit hit)
	{
		yield return new WaitForSeconds(0.5f);
		hit.transform.GetComponent<Animator>().SetTrigger("isDeath");
		hit.transform.GetComponent<NavMeshAgent>().isStopped = true;
		int lower = Mathf.CeilToInt(TargetValue*0.02f);
		int higher = Mathf.CeilToInt(TargetValue * 0.05f);
		int amountStolen=rnd.Next (lower, higher);
		CurrentValue += amountStolen;
		yield return new WaitForSeconds(5.0f);
		Destroy(hit.transform.gameObject);

	}


	void stealFrom(CitizenController citizen){
		//play animation

		//set citizen as stolen
		if(citizen.stolen==false){
			citizen.stolen=true;
			//add amount to self
			int chance = rnd.Next (1, 10);
			if (chance > 1) {
				//got money
				int lower = Mathf.CeilToInt(TargetValue*0.02f);
				int higher = Mathf.CeilToInt(TargetValue * 0.03f);
				int amountStolen=rnd.Next (lower, higher);
				CurrentValue += amountStolen;
			}	
		}
	}

	void refreshEnv(){
		//refresh all citizen status
		foreach(GameObject c in citizens){
			if (c != null) {
				c.GetComponent<CitizenController> ().stolen = false;
			}
		}
		//refresh bank money
		foreach(GameObject m in moneys){
			float x=(float)rnd.Next(-140,-73);
			float z=(float)rnd.Next(80,135);
			m.transform.position = new Vector3 (x, 0.5f, z);
		}

	}
	void takeMoney(MoneyData money){
		int lower = Mathf.CeilToInt(TargetValue*0.05f);
		int higher = Mathf.CeilToInt(TargetValue * 0.06f);
		int amountStolen=rnd.Next (lower, higher);
		if (money.taken == false) {
			CurrentValue += amountStolen;
			money.taken=true;
		}


	}
}
