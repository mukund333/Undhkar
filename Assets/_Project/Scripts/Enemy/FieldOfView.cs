using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
   public float viewRadius;
   [Range(0,360)]
   public float viewAngle;
   
   public LayerMask targetMask;
   public LayerMask obstacleMask;
   
   public List<Transform> visibleTargets = new List<Transform>();
   
   
   void Start()
   {
	   StartCoroutine("FindTargetsWithDelay",0.1f);
   }
   
   
   IEnumerator FindTargetsWithDelay(float delay)
   {
	   while(true)
	   {
		   yield return new WaitForSeconds(delay);
		   FindVisibleTargets();
	   }
   }
   
   
   void FindVisibleTargets()
   {
	   visibleTargets.Clear();
	 Collider2D[] targetInViewRadius = Physics2D.OverlapCircleAll(transform.position,viewRadius,targetMask);
	 
	 for(int i =0;i<targetInViewRadius.Length;i++)
	 {
		 Transform target = targetInViewRadius[i].transform;
		 Vector3 dirToTarget = (target.position - transform.position).normalized;
		 
		 if(Vector3.Angle(transform.up,dirToTarget)<viewAngle/2)
		 {
			 float dstToTarget = Vector3.Distance(transform.position,target.position);
			 
			 if(!Physics2D.Raycast(transform.position,dirToTarget,dstToTarget,obstacleMask))
			 {
					visibleTargets.Add(target);
			 }
		 }
	 
	 }
   }
   
   
   public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
   {
	   if(!angleIsGlobal)
	   {
		   angleInDegrees += transform.eulerAngles.y;
	   }
	   return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),Mathf.Cos(angleInDegrees * Mathf.Deg2Rad),0);
   }
   
}
