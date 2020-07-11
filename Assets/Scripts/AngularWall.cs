using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngularWall : MonoBehaviour
{
	public PolygonCollider2D polygonCollider;
	
	private bool alreadyTurned = false;
	
    // Start is called before the first frame update
    void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerStay2D(Collider2D col)
	{
		if(alreadyTurned)
		{
			return;
		}
		
		if(col.gameObject.name == "Sleepyboi")
		{
			float distX = Mathf.Abs(col.gameObject.transform.position.x - transform.position.x);
			float distY = Mathf.Abs(col.gameObject.transform.position.y - transform.position.y);
			
			if(distX > 0.1f && distY > 0.1f)
			{
				return;
			}
			
			float rotation = Mathf.Round(transform.rotation.eulerAngles.z);
			alreadyTurned = true;
			
			if(rotation == 0 || rotation == 180)
			{
				col.gameObject.SendMessage("Turn", -1f);
			}
			else
			{
				col.gameObject.SendMessage("Turn", 1f);
			}
		}
	}
	
	void OnTriggerExit2D(Collider2D col)
	{
		if(col.gameObject.name == "Sleepyboi")
		{
			ResetTurned();
		}
	}
	
	void ResetTurned()
	{
		alreadyTurned = false;
	}
}
