using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class GuardController : MonoBehaviour
{
    Animator anim;
    private NavMeshAgent agent;
    public Vector3 Destination1;
    public Vector3 Destination2;
    public float range = 5.0F;
    bool goBack = false;
    bool targetInSight = false;
    public GameObject killByGuard;

    // Use this for initialization
    void Start()
    {
        killByGuard.SetActive(false);
        anim = this.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(Destination1);
        anim.SetFloat("InputVertical", 0.5F);
    }

    // Update is called once per frame
    void Update()
    {
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
                    hit.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    this.GetComponent<Animator>().SetBool("isFighting", true);
                    StartCoroutine(Example(hit));
                }
            }
        }

        if (agent.remainingDistance < 2.0)
        {
            if (goBack == false)
            {
                anim.SetFloat("InputVertical", 0.0F);
                agent.SetDestination(Destination2);
                anim.SetFloat("InputVertical", 0.5F);
                goBack = true;
            }
            else
            {
                anim.SetFloat("InputVertical", 0.0F);
                agent.SetDestination(Destination1);
                anim.SetFloat("InputVertical", 0.5F);
                goBack = false;
            }
        }
    }

    IEnumerator Example(RaycastHit hit)
    {
        yield return new WaitForSeconds(0.5f);
        hit.transform.GetComponent<Animator>().SetTrigger("isDeath");
        hit.transform.GetComponent<NavMeshAgent>().isStopped = true;
        yield return new WaitForSeconds(1.0f);
        killByGuard.SetActive(true);
        Time.timeScale = 0;
    }
}
