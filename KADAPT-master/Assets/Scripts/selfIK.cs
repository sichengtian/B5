using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class selfIK : MonoBehaviour {

	protected Animator animator;

	public bool isOn = false;
	public Transform otherGuyLeftHand;

	void Start () 
	{
		animator = GetComponent<Animator>();
	}

	void OnAnimatorIK ()
	{
		if (isOn) {
			animator.SetIKPositionWeight (AvatarIKGoal.LeftHand, 1);
			animator.SetIKRotationWeight (AvatarIKGoal.LeftHand, 1);
			animator.SetIKPosition (AvatarIKGoal.LeftHand, otherGuyLeftHand.position);
			animator.SetIKRotation (AvatarIKGoal.LeftHand, otherGuyLeftHand.rotation);

		} else {
			animator.SetIKPositionWeight (AvatarIKGoal.LeftHand, 0);
			animator.SetIKPositionWeight (AvatarIKGoal.LeftHand, 0);
		}
	}
}
