using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {
    private UnitySteeringController steering;
    // Use this for initialization
    void Start () {
        steering = GetComponent<UnitySteeringController>();
    }
	
	// Update is called once per frame
	void Update () {
        float Vertical = Input.GetAxis("Vertical");
        float Horizontal = Input.GetAxis("Horizontal");
        steering.Target = transform.position + Quaternion.LookRotation(transform.forward) * new Vector3(Horizontal / 2, 0, Vertical) * 5;
        steering.maxSpeed = 3;
        if (Input.GetKey(KeyCode.LeftShift)) steering.maxSpeed = 6;
        if (Vertical == 0 && Horizontal != 0)
        {
            steering.orientationBehavior = OrientationBehavior.None;
            steering.SetDesiredOrientation(transform.position + Quaternion.Euler(0, Horizontal * 50, 0) * transform.forward * 5);
            if (steering.IsFacing()) steering.orientationBehavior = OrientationBehavior.LookForward;
        }
    }
}
