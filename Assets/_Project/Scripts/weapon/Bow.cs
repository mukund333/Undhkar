using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : AbstractBehavior
{
	public Transform bowTransform;
	
	public  Arrow projectile;
	public  GameObject crossHair = null;
	private CoolDown coolDown;
    
    protected override void Awake()
    {
		base.Awake ();
		coolDown = GetComponent<CoolDown>();
		crossHair = this.gameObject.transform.GetChild(0).gameObject;
		bowTransform =  this.gameObject.transform.GetChild(1);
    
    }

    
   void Update()
    {
		 var fire = inputState.GetButtonValue(inputButtons [0]);
		
		
		if (fire && coolDown.Check()!=false)
        {
			
			coolDown.Reset();
			//projectile.arrowDirection = (transform.position - crossHairTransform.position) * -1;
			//projectile.arrowDirection = (float) inputState.direction;
			Arrow clone = Instantiate(projectile,bowTransform.position ,  Quaternion.identity);
			var dir =  (transform.position - crossHair.transform.position)*-1;
			projectile.arrowDirection = dir;
			clone.arrowDirection = dir;
			clone.transform.Rotate(0.0f,0.0f,Mathf.Atan2(dir.y,dir.x)*Mathf.Rad2Deg);
			
			
		}
		
			
		
    }
}
