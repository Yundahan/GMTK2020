using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngularWall : MonoBehaviour
{
	public PolygonCollider2D polygonCollider;
	
	private bool alreadyTurned = false;
	
	private bool ignoreX = false;
	private bool ignoreY = false;
	
    // Start is called before the first frame update
    void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.name == "Sleepyboi")
		{
			float distX = Mathf.Abs(col.gameObject.transform.position.x - transform.position.x);
			float distY = Mathf.Abs(col.gameObject.transform.position.y - transform.position.y);
			
			if(distX <= 0.1f)
			{
				ignoreX = true;
			}
			else if(distY <= 0.1f)
			{
				ignoreY = true;
			}
		}
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
			
			if(distX > 0.1f && distY > 0.1f || distX > 0.1f && ignoreY || distY > 0.1f && ignoreX)
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
			
			ignoreX = false;
			ignoreY = false;
		}
	}
	
	void OnTriggerExit2D(Collider2D col)
	{
		if(col.gameObject.name == "Sleepyboi")
		{
			ResetTurned(col.gameObject);
		}
	}
	
	void ResetTurned(GameObject go)
	{
		alreadyTurned = false;
		float distX = Mathf.Abs(go.transform.position.x - transform.position.x);
		float distY = Mathf.Abs(go.transform.position.y - transform.position.y);
		
		if(distX <= 0.1f)
		{
			ignoreX = true;
		}
		else if(distY <= 0.1f)
		{
			ignoreY = true;
		}
		
		if(distX <= 0.1f && distY <= 0.1f)
		{
			ignoreX = false;
			ignoreY = false;
		}
	}
}
