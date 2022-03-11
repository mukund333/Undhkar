using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDown : MonoBehaviour
{
	
	
	[Space]
	[Header(" ")]
    [SerializeField] private float cooldownTimer=0.5f;
    [SerializeField] private float cooldownTime;
    [SerializeField] private bool  isCool = true;
	
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

	
	public  bool Check()
    {
        return isCool;     
    }

    public  void Reset()
    {
        isCool = false;
        cooldownTime = 0.0f;   
    }
	
	
	private void Update()
    {
 
        if (!isCool)
        {
			
            cooldownTime += Time.deltaTime;
            if(cooldownTime > cooldownTimer)
            {	
                isCool = true;
                
            }
        }
    }
}
