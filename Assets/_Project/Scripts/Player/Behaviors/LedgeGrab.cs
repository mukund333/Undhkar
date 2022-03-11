using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrab : AbstractBehavior
{
	public bool drawDebugRaycasts = true;	//Should the environment checks be visualized
	
	public float hangingJumpForce = 15f;	//Force of wall hanging jump
	
	[Header("Environment Check Properties")]
	public float eyeHeight = 1.5f;			//Height of wall checks
	public float reachOffset = .7f;			//X offset for wall grabbing
	public float headClearance = .5f;		//Space needed above the player's head
	public float grabDistance = .4f;		//The reach distance for wall grabs
	public float footOffset = .4f;			//X Offset of feet raycast
	public LayerMask groundLayer;			//Layer of the ground
	
	public bool isHanging;					//Is player hanging?
	public bool isHeadBlocked;
	public bool isOnGround;					//Is the player on the ground?
	
	
	Rigidbody2D rigidBody;					//The rigidbody component
	
	public float playerHeight;						//Height of the player
	
	int direction = 1;						//Direction player is facing
	
	Vector2 colliderStandSize;				//Size of the standing collider
	Vector2 colliderStandOffset;			//Offset of the standing collider
	
	const float smallAmount = .05f;			//A small amount used for hanging position
	
	void Start ()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		
		
	}
	
	void FixedUpdate()
	{
		//Check the environment to determine status
		PhysicsCheck();
		OnHanging();
		
	}
	
	void PhysicsCheck()
	{
		//Start by assuming the player isn't on the ground and the head isn't blocked
		isHeadBlocked = false;
		
				//Cast the ray to check above the player's head
		RaycastHit2D headCheck = Raycast(new Vector2(0f, playerHeight), Vector2.up, headClearance);

		//If that ray hits, the player's head is blocked
		if (headCheck)
			isHeadBlocked = true;

		//Determine the direction of the wall grab attempt
		Vector2 grabDir = new Vector2(direction, 0f);

		//Cast three rays to look for a wall grab
		RaycastHit2D blockedCheck = Raycast(new Vector2(footOffset * direction, playerHeight), grabDir, grabDistance);
		RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset * direction, playerHeight), Vector2.down, grabDistance);
		RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), grabDir, grabDistance);
		
		
		//If the player is off the ground AND is not hanging AND is falling AND
		//found a ledge AND found a wall AND the grab is NOT blocked...
		if (!isOnGround && !isHanging && rigidBody.velocity.y < 0f && 
			ledgeCheck && wallCheck && !blockedCheck)
		{ 
			//...we have a ledge grab. Record the current position...
			Vector3 pos = transform.position;
			//...move the distance to the wall (minus a small amount)...
			pos.x += (wallCheck.distance - smallAmount) * direction;
			//...move the player down to grab onto the ledge...
			pos.y -= ledgeCheck.distance;
			//...apply this position to the platform...
			transform.position = pos;
			//...set the rigidbody to static...
			rigidBody.bodyType = RigidbodyType2D.Static;
			//...finally, set isHanging to true
			isHanging = true;
		}
	}	
		
		//These two Raycast methods wrap the Physics2D.Raycast() and provide some extra
	//functionality
	RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length)
	{
		//Call the overloaded Raycast() method using the ground layermask and return 
		//the results
		return Raycast(offset, rayDirection, length, groundLayer);
	}

	RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask mask)
	{
		//Record the player's position
		Vector2 pos = transform.position;

		//Send out the desired raycasr and record the result
		RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, mask);

		//If we want to show debug raycasts in the scene...
		if (drawDebugRaycasts)
		{
			//...determine the color based on if the raycast hit...
			Color color = hit ? Color.red : Color.green;
			//...and draw the ray in the scene view
			Debug.DrawRay(pos + offset, rayDirection * length, color);
		}

		//Return the results of the raycast
		return hit;
	}
	
	void OnHanging()
	{
		
		var down 	= inputState.GetButtonValue (inputButtons [0]);
		var canJump = inputState.GetButtonValue (inputButtons [1]);
			//If the player is currently hanging...
		if (isHanging)
		{
			//If crouch is pressed...
			if (down)
			{
				//...let go...
				isHanging = false;
				//...set the rigidbody to dynamic and exit
				rigidBody.bodyType = RigidbodyType2D.Dynamic;
				return;
			}

			//If jump is pressed...
			if (canJump)
			{
				//...let go...
				isHanging = false;
				//...set the rigidbody to dynamic and apply a jump force...
				rigidBody.bodyType = RigidbodyType2D.Dynamic;
				rigidBody.AddForce(new Vector2(1f, hangingJumpForce), ForceMode2D.Impulse);
				//...and exit
				return;
			}
		}
	}
	
	
}
