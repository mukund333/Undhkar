using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	Rigidbody2D rb2d;
	public float weaponThrust;
	public float weaponGravity;
	public Vector3 arrowDirection;
	//public float arrowDirection;
	
	
	[Space]
	[Header("CurvePhysics Settings")]
	[SerializeField] private float 			 accThrust;
	[SerializeField] private float 			 accTimeThrust;
	[SerializeField] private AnimationCurve  accCurveThrust;
	
	[Space]
	[Header("GravityCurve Settings")]
	[SerializeField] private float 			 accGravity;
	[SerializeField] private float 			 accTimeGravity;
	[SerializeField] private AnimationCurve  accCurveGravity;
	
	
	
	
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
	
	void FixedUpdate()
	{
		  
			// //body2d.AddForce(transform.right * arrowSpeed * arrowDirection,ForceMode2D.Impulse);        
			// var dir = transform.right * arrowSpeed * arrowDirection;
			// body2d.velocity = new Vector2(dir.x,dir.y);
			// //body2d.velocity = new Vector2(arrowDirection.x,arrowDirection.y);
			 Destroy(gameObject, 5f);
			 
			// accThrust = accThrust + 1f / accTimeThrust * Time.deltaTime;
			// rb2d.velocity = transform.right * (weaponThrust * accCurveThrust.Evaluate(accThrust));//*arrowDirection;
			// accThrust = Mathf.Clamp(accThrust, 0f, 1f);
        
			// if (accThrust > 0f)
			// {
				// accThrust = this.rb2d.velocity.magnitude / weaponThrust;
			// }
			
			rb2d.velocity = arrowDirection * weaponThrust;
			ApplyGravityCurve();
    }
	
	private void ApplyGravityCurve()
	{
			accGravity = accGravity + 1f / accTimeGravity * Time.deltaTime;
			rb2d.gravityScale = weaponGravity * accCurveGravity.Evaluate(accGravity);
			accGravity = Mathf.Clamp(accGravity, 0f, 1f);
	}
	
}
