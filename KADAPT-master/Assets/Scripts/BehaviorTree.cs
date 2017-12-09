using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TreeSharpPlus;
using UnityEngine.AI;


public class BehaviorTree : MonoBehaviour {
    public GameObject player;
    public GameObject police1;
    public GameObject police2;
    public GameObject police3;
    private BehaviorAgent behaviorAgent;
    private Vector3 PlayerPosition = new Vector3(0,0,0);

	// Use this for initialization
	void Start () {
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }
	
	// Update is called once per frame
	void Update () {}



    protected Node BuildTreeRoot()
    {
        Node root = new Sequence(
                new SequenceParallel(
                    new DecoratorLoop(updatePlayerPosition()),
                    new Sequence(
                        PoliceBorn(),
                        PoliceCome()
                        )
                    ),
                new DecoratorLoop(new LeafWait(1000))
            );
        return root;
    }
    protected Node PoliceCome()
    {
        return new Sequence(
            new LeafInvoke(()=> {
				NavMeshPath path = new NavMeshPath();
				police1.GetComponent<NavMeshAgent>().CalculatePath(new Vector3(PlayerPosition.x-4,0,PlayerPosition.z-4),path);
				police1.GetComponent<NavMeshAgent>().SetPath(path);
				police1.GetComponent<Animator>().SetBool("callingPolice",true);
				NavMeshPath path2 = new NavMeshPath();
				police2.GetComponent<NavMeshAgent>().CalculatePath(new Vector3(PlayerPosition.x-3,0,PlayerPosition.z+2),path2);
				police2.GetComponent<NavMeshAgent>().SetPath(path2);
				police2.GetComponent<Animator>().SetBool("callingPolice",true);
				NavMeshPath path3 = new NavMeshPath();
				police3.GetComponent<NavMeshAgent>().CalculatePath(new Vector3(PlayerPosition.x+4,0,PlayerPosition.z-1),path2);
				police3.GetComponent<NavMeshAgent>().SetPath(path3);
				police3.GetComponent<Animator>().SetBool("callingPolice",true);
                police1.GetComponent<NavMeshAgent>().speed = 4;
                police2.GetComponent<NavMeshAgent>().speed = 5;
                police3.GetComponent<NavMeshAgent>().speed = 6;
            }));
    }
    protected Node PoliceBorn()
    {
        return new Sequence(
            CheckWantStatus(),
            SpawnPolice()
        );
    }
    protected bool isPlayerWanted()
    {
        bool wanted = player.GetComponent<GameMeta>().isWanted;

        print("Player is wanted: "+wanted);

        if (wanted == true)
        {
            return false;
        }
        else return true;
    }
    protected Node CheckWantStatus()
    {
        return new DecoratorForceStatus(RunStatus.Success, new DecoratorLoop(new LeafAssert(() => isPlayerWanted())));
    }
    protected Node SpawnPolice()
    {
        return new LeafInvoke(()=> {
            police1.SetActive(true);
            police2.SetActive(true);
            police3.SetActive(true);
        });
    }

    protected Node updatePlayerPosition()
    {
        return new LeafInvoke(()=> {
            print(PlayerPosition);
            PlayerPosition = player.transform.position;

        });
    }



}
