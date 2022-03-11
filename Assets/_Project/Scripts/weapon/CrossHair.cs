using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CrossHair : AbstractBehavior
{
	public float cosX;
	public Transform pivot = null;
	public Transform crossHairTransform = null;
	
	private Quaternion destRot = Quaternion.identity;
	//Distance to maintian form pivot
	public float pivotDistance = 50f;
	public float rotSpeed = 100f;
	[SerializeField] float RotZ = 0f;
	[SerializeField] float Angle;
	
	public Vector3 crossHairPosition;	
	float Hortz =0;
	
	public bool isAiming;
	
	protected override void Awake()
    {
		base.Awake ();
		
		crossHairTransform = this.gameObject.transform.GetChild(0);
		pivot = this.transform;
	}
	
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    void Update()
    {
         Hortz = Input.GetAxis("Horizontal");
		
		// var right = inputState.GetButtonValue (inputButtons [0]);
		// var left  = inputState.GetButtonValue (inputButtons [1]);
		
		isAiming = true;
		ToggleScripts (!isAiming);
		transform.localScale = new Vector3 ((float)inputState.direction, 1, 1);
		// if (right || left)
		// {
			
			// Hortz = (float)inputState.direction;
			
		// }else{
			// Hortz = 0f;
			
		// }
		
		//DefaultAim();
		CalculateOrbit();
		FaceDirection();
		
		
    }
	//stregth aim left and right
	void DefaultAim()
	{
		if(inputState.direction == Directions.Right)
		{
			crossHairTransform.position = pivot.position + Vector3.right * -pivotDistance;
		}
		if(inputState.direction == Directions.Right)
		{
			crossHairTransform.position = pivot.position + Vector3.right * pivotDistance;
		}
		
		
		//crossHairTransform.position = pivot.position + Vector3.right * -pivotDistance;
	}
	
	
	
	//Aim
	void CalculateOrbit()
	{
		
		
		RotZ += Hortz  * Time.deltaTime * rotSpeed *-1;
		RotZ = RotZ % 360;
		Quaternion ZRot = Quaternion.Euler(0f,0f,RotZ+Angle);
		crossHairTransform.rotation = ZRot;
		
		Angle = RotZ * Mathf.Deg2Rad;
	    cosX= Mathf.Cos(Angle);
		//Adjust position
		crossHairPosition = pivot.position + crossHairTransform.rotation * Vector3.right * -pivotDistance;
		crossHairTransform.position = crossHairPosition;
	
	}
	
	void FaceDirection()
	{
		if(cosX > 0)
		{
			inputState.direction = Directions.Left;
		}else if(cosX < 0)
		{
			inputState.direction = Directions.Right;
		}
			
		
		
	}
	
	
	
}
