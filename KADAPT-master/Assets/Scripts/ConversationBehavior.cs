using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TreeSharpPlus;
using UnityEngine.UI;
using System.Xml.Schema;

public class ConversationBehavior : MonoBehaviour {
    public Transform MeetingPointTyrone;
    public Transform MeetingPointDaniel;
    public GameObject participant1;
    public GameObject participant2;
    public Transform MeetingPointTyrone2;
    public Transform MeetingPointDaniel2;
    public GameObject Door;
	public GameObject Door2;
	public GameObject Door3;
	public GameObject Door4;
	public GameObject money;
    private BehaviorAgent behaviorAgent;
    public GameObject textbox;
    public Text text;
	public GameObject Button1;
	public GameObject Button2;
	public GameObject Button3;
	public GameObject Guard;
	public GameObject Guard2;
	public GameObject Guard3;
    public Transform MeetingPointTyrone3;
    public Transform MeetingPointDaniel3;
    public Transform MeetingPointTyrone4;
    public Transform MeetingPointDaniel4;
    public Transform MeetingPointTyrone5;
    public Transform MeetingPointDaniel5;
	public Transform TDancingPoint;
	public Transform DDancingPoint;


    private int loopCount=0;
	private Vector3 Ttarget;
	private Vector3 Dtarget;

	GameObject[] Buttons;
	GameObject[] Guards;
	GameObject[] Doors;
    //Transform[] MeetingPointsTyrone;
	//Transform[] MeetingPointsDaniel;
    // Use this for initialization
    void Start () {
        //MeetingPointsTyrone = new Transform[] { MeetingPointTyrone3, MeetingPointTyrone4, MeetingPointTyrone5 };
        //MeetingPointsDaniel = new Transform[] { MeetingPointDaniel3, MeetingPointDaniel4, MeetingPointDaniel5 };
        Doors = new GameObject[]{Door2, Door3, Door4 };
		Buttons=new GameObject[]{Button1,Button2,Button3};
		Guards=new GameObject[]{Guard,Guard2,Guard3};
		behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }
	
	// Update is called once per frame
	void Update () {
		Guard.transform.forward = participant1.transform.position - Guard.transform.position;
		Guard2.transform.forward = participant1.transform.position - Guard2.transform.position;
		Guard3.transform.forward = participant1.transform.position - Guard3.transform.position;
		Debug.Log(loopCount);
    }
    protected Node ApproachAndWait(GameObject participant, Vector3 target)
    {
        Debug.Log("hello");
        Val<Vector3> position = Val.V(() => target);
        return  new Sequence(participant.GetComponent<BehaviorMecanim>().Node_OrientTowards(position), participant.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
    }
    protected Node TurnTowardEachOther(GameObject participant, GameObject participant2)
    {
        
        Val<Vector3> position = Val.V(() => participant2.transform.position);
        
        return new Sequence(participant.GetComponent<BehaviorMecanim>().Node_OrientTowards(position),
 
                            new LeafWait(10));
    }
    protected Node WaveAndTalk1(GameObject participant)
    {
       
        return new Sequence(participant.GetComponent<BehaviorMecanim>().Node_HandAnimation("CHEER", true),
                            new LeafWait(1000),
                            participant.GetComponent<BehaviorMecanim>().Node_HandAnimation("CHEER", false),
                            new LeafWait(1000))
                            ;
    }
    protected Node WaveAndTalk2(GameObject participant)
    {

        return new Sequence(participant.GetComponent<BehaviorMecanim>().Node_HandAnimation("WONDERFUL", true),
                            new LeafWait(1000),
                            participant.GetComponent<BehaviorMecanim>().Node_HandAnimation("WONDERFUL", false),
                            new LeafWait(1000))
                            ;
    }
	protected Node highFive(GameObject participant)
	{
		return new Sequence(this.touch(participant),new LeafWait(100),participant.GetComponent<BehaviorMecanim>().Node_HandAnimation("WONDERFUL", true),
			new LeafWait(1000),
			participant.GetComponent<BehaviorMecanim>().Node_HandAnimation("WONDERFUL", false),this.disTouch(participant),
			new LeafWait(1000))
			;
	}
	protected Node touch(GameObject participant)
	{
		return new LeafInvoke(() => { participant.GetComponent<selfIK>().isOn = true; });
	}
	protected Node disTouch(GameObject participant)
	{
		return new LeafInvoke(() => { participant.GetComponent<selfIK>().isOn = false; });
	}
    protected Node enableTextBox(GameObject textBox)
    {
        return new LeafInvoke(() => { textBox.SetActive(true); });
    }
    protected Node disableTextBox(GameObject textBox)
    {
        return new LeafInvoke(() => { textBox.SetActive(false); });
    }
	protected Node TyroneSpeaks (GameObject participant,string speech)
    {
        return new LeafInvoke(() => {

            text.text = speech;

        });
    }
	protected Node DanielSpeaks (GameObject participant,string speech)
    {
        return new LeafInvoke(() => {

            text.text = speech;

        });
    }
    protected Node TurnTowardDoor(GameObject participant, GameObject door)
    {
        Val<Vector3> position = Val.V(() => door.transform.position);
        return new Sequence(participant.GetComponent<BehaviorMecanim>().Node_OrientTowards(position), new LeafWait(1000));
    }
    protected Node AutoOpenDoor (GameObject door)
    {
        return new LeafInvoke(()=> {
				
                Animator anim = door.GetComponent<Animator>();
                anim.SetBool("Open", true);
				//loopCount++;
            
            });
    }
    protected Node PressButton(GameObject participant, GameObject button) {
        Vector3 position = button.transform.position;
        position.z -= 0.12F;
        return new Sequence
        (
            new LeafInvoke(() => { participant.GetComponent<CrossfadeFBBIK>().solver.leftHandEffector.position = position; }),
            new DecoratorLoop(50, new LeafInvoke(
                        () => { participant.GetComponent<CrossfadeFBBIK>().solver.leftHandEffector.positionWeight += 0.02F; }
                        )),
            new DecoratorLoop(50, new LeafInvoke(
                        () => { participant.GetComponent<CrossfadeFBBIK>().solver.leftHandEffector.positionWeight -= 0.02F; }
                        ))
        );
	}
    protected Node guardGoAway (GameObject guard)
    {
        return new LeafInvoke(() => { guard.SetActive(false); });
    }
	protected Node Fight(GameObject participant,GameObject guard){


        return new Sequence(participant.GetComponent<BehaviorMecanim>().Node_BodyAnimation("FIGHT", true),
            new LeafWait(500),
            participant.GetComponent<BehaviorMecanim>().Node_HandAnimation("HITSTEALTH", true),
            new LeafWait(200),
            guard.GetComponent<BehaviorMecanim>().Node_BodyAnimation("DYING", true),
            new LeafWait(500),
            guard.GetComponent<BehaviorMecanim>().Node_BodyAnimation("DYING", false),
            new LeafWait(500),
            guardGoAway(guard),
			participant.GetComponent<BehaviorMecanim>().Node_HandAnimation("HITSTEALTH",false),
			participant.GetComponent<BehaviorMecanim>().Node_BodyAnimation("FIGHT",false),
			new LeafWait(500));
	}
	protected Node updatePositions(){
		return new LeafInvoke(() => {
			if(loopCount<2){
				loopCount++;
			}
		});
	}
	protected Node Dance(GameObject participant){
		return new Sequence(participant.GetComponent<BehaviorMecanim> ().Node_BodyAnimation ("BREAKDANCE", true),
							new LeafWait(1000),
							participant.GetComponent<BehaviorMecanim> ().Node_BodyAnimation ("BREAKDANCE", false),
							new LeafWait(1000)
							);
	}
	protected Node Dance1(GameObject participant){
		return new Sequence(participant.GetComponent<BehaviorMecanim> ().Node_HandAnimation("CHEER", true),
			new LeafWait(1000),
			participant.GetComponent<BehaviorMecanim> ().Node_HandAnimation("CHEER", false),
			new LeafWait(1000)
		);
	}

    protected Node BuildTreeRoot()
    {
        Node roaming = new Sequence(
                            new SequenceParallel(
                                this.ApproachAndWait(participant1, this.MeetingPointTyrone.position),
                                this.ApproachAndWait(participant2, this.MeetingPointDaniel.position)),

                            new SequenceParallel(
                                this.TurnTowardEachOther(participant1, participant2),
                                this.TurnTowardEachOther(participant2, participant1)),

                            new Sequence(this.enableTextBox(textbox), new SequenceParallel(this.WaveAndTalk1(participant1), TyroneSpeaks(participant1, "Tyrone: Yo what's up homie! You ready for this big score??")), this.disableTextBox(textbox)),
                            new Sequence(this.enableTextBox(textbox), new SequenceParallel(this.WaveAndTalk2(participant2), DanielSpeaks(participant2, "Daniel: Hell yeah! Let's do this!")), this.disableTextBox(textbox)),

                            new SequenceParallel(
                                this.TurnTowardDoor(participant1, Door),
                                this.TurnTowardDoor(participant2, Door)),

                            new Sequence(AutoOpenDoor(Door)),

                            new SequenceParallel(
                                this.ApproachAndWait(participant1, this.MeetingPointTyrone2.position),
                                this.ApproachAndWait(participant2, this.MeetingPointDaniel2.position)),

                            new SequenceParallel(
                                this.TurnTowardEachOther(participant1, participant2),
                                this.TurnTowardEachOther(participant2, participant1)),
							new Sequence(this.enableTextBox(textbox), new SequenceParallel(this.WaveAndTalk1(participant1), this.WaveAndTalk1(participant2),TyroneSpeaks(participant1, "Tyrone: Let's go and beat them up!")), this.disableTextBox(textbox)),
                            new SequenceParallel(
                                this.highFive(participant1),
                                this.highFive(participant2)),


                            //need to loop 3 times for 3 doors

                                new Sequence(
                                    new SequenceParallel(
										this.ApproachAndWait(participant1, MeetingPointTyrone3.position),
										this.ApproachAndWait(participant2, MeetingPointDaniel3.position)),
                                    new Sequence(
                                        this.Fight(participant1, Guard)
                                    ),
                                    new Sequence(this.enableTextBox(textbox), new SequenceParallel(this.WaveAndTalk1(participant1), TyroneSpeaks(participant1, "Tyrone: Open the door!")), this.disableTextBox(textbox)),
                                    new Sequence(this.PressButton(participant2, Button1)),
                                    new Sequence(this.AutoOpenDoor(Door2)),
                                    //this should update loopCount index
                                    new Sequence(this.updatePositions()),
                                    new LeafWait(1000)
                                ),
								new Sequence(
									new SequenceParallel(
										this.ApproachAndWait(participant1, MeetingPointTyrone4.position),
										this.ApproachAndWait(participant2, MeetingPointDaniel4.position)),
									new Sequence(
										this.Fight(participant1, Guard2)
									),
									new Sequence(this.enableTextBox(textbox), new SequenceParallel(this.WaveAndTalk1(participant1), TyroneSpeaks(participant1, "Tyrone: Open the door!")), this.disableTextBox(textbox)),
									new Sequence(this.PressButton(participant2, Button2)),
									new Sequence(this.AutoOpenDoor(Door3)),
									//this should update loopCount index
									new Sequence(this.updatePositions()),
									new LeafWait(1000)
								),
								new Sequence(
									new SequenceParallel(
										this.ApproachAndWait(participant1, MeetingPointTyrone5.position),
										this.ApproachAndWait(participant2, MeetingPointDaniel5.position)),
									new Sequence(
										this.Fight(participant1, Guard3)
									),
									new Sequence(this.enableTextBox(textbox), new SequenceParallel(this.WaveAndTalk1(participant1), TyroneSpeaks(participant1, "Tyrone: Open the door!")), this.disableTextBox(textbox)),
									new Sequence(this.PressButton(participant2, Button3)),
									new Sequence(this.AutoOpenDoor(Door4)),
									//this should update loopCount index
									new Sequence(this.updatePositions()),
									new LeafWait(1000)
								),

								//walk toward money and then dance to money
								new SequenceParallel(
									this.ApproachAndWait(participant1,TDancingPoint.position),
									this.ApproachAndWait(participant2,DDancingPoint.position)
								),
								new Sequence(this.enableTextBox(textbox), new SequenceParallel(this.WaveAndTalk2(participant1), TyroneSpeaks(participant1, "Daniel: We will be so rich! Let's dance!")), this.disableTextBox(textbox)),
								//infinitely dance to money
								new DecoratorLoop(
									new Sequence(
										this.TurnTowardEachOther(participant1,money),this.TurnTowardEachOther(participant2,money),
										new SequenceParallel(
											new SequenceParallel(
												new probNode(this.Dance(participant1),this.Dance1(participant1),0.7),
												new probNode(this.Dance(participant2),this.Dance1(participant2),0.3)
											)
										)    	
									) 
								)
                            
                       );              
        return roaming;
    }
}
