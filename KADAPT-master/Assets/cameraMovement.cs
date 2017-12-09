using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour {

	public float sensitivity = 5f;
	public float maxYAngle = 80f;
	private Vector2 currentRotation;

	public float speed;
	private Rigidbody rb;

	private bool isCursorLocked;


	private Camera fpsCam;


	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 7F;
	public float sensitivityY = 7F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationY = 0F;

	LineRenderer line;



	void Start(){
		rb = GetComponent<Rigidbody> ();
		ToggleCursorState ();
		fpsCam = GetComponentInParent<Camera> ();
	}

	void Update(){
		if (axes == RotationAxes.MouseXAndY){
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

			transform.localEulerAngles = new Vector3(0, rotationX, 0);
			Camera.main.transform.localEulerAngles = new Vector3(-rotationY,0, 0);

		}else if (axes == RotationAxes.MouseX){
			transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
		}else{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

			transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
			Camera.main.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);

		}

		//hide cursor
		CheckForInput ();
		CheckIfCursorShouldBeLocked ();


	}

	void ToggleCursorState(){
		isCursorLocked = !isCursorLocked;
	}

	void CheckForInput(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			ToggleCursorState ();
		}
	}

	void CheckIfCursorShouldBeLocked(){
		if (isCursorLocked) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		} else {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	void OnCollisionExit(Collision collision)
	{
		this.GetComponent<Rigidbody>().velocity = Vector3.zero;
		this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
	}

}
