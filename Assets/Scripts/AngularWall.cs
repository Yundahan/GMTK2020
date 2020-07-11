using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngularWall : MonoBehaviour
{
	public PolygonCollider2D polygonCollider;
	
	HashSet<GameObject> alreadyTurned;
	
	HashSet<GameObject> ignoreX;
	HashSet<GameObject> ignoreY;
	
    // Start is called before the first frame update
    void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
		ignoreX = new HashSet<GameObject>();
		ignoreY = new HashSet<GameObject>();
		alreadyTurned = new HashSet<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.name == "Sleepyboi" || col.gameObject.name == "Ghost")
		{
			float distX = Mathf.Abs(col.gameObject.transform.position.x - transform.position.x);
			float distY = Mathf.Abs(col.gameObject.transform.position.y - transform.position.y);
			
			if(distX <= 0.1f)
			{
				ignoreX.Add(col.gameObject);
			}
			else if(distY <= 0.1f)
			{
				ignoreY.Add(col.gameObject);
			}
		}
	}
	
	void OnTriggerStay2D(Collider2D col)
	{
		if(alreadyTurned.Contains(col.gameObject))
		{
			return;
		}
		
		if(col.gameObject.name == "Sleepyboi" || col.gameObject.name == "Ghost")
		{
			float distX = Mathf.Abs(col.gameObject.transform.position.x - transform.position.x);
			float distY = Mathf.Abs(col.gameObject.transform.position.y - transform.position.y);
			
			if(distX > 0.1f && distY > 0.1f || distX > 0.1f && ignoreY.Contains(col.gameObject) || distY > 0.1f && ignoreX.Contains(col.gameObject))
			{
				return;
			}
			
			float rotation = Mathf.Round(transform.rotation.eulerAngles.z);
			alreadyTurned.Add(col.gameObject);
			
			if(rotation == 0 || rotation == 180)
			{
				col.gameObject.SendMessage("Turn", -1f);
			}
			else
			{
				col.gameObject.SendMessage("Turn", 1f);
			}
			
			ignoreX.Remove(col.gameObject);
			ignoreY.Remove(col.gameObject);
		}
	}
	
	void OnTriggerExit2D(Collider2D col)
	{
		if(col.gameObject.name == "Sleepyboi" || col.gameObject.name == "Enemy")
		{
			ResetTurned(col.gameObject);
		}
	}
	
	void ResetTurned(GameObject go)
	{
		alreadyTurned.Remove(go);
		float distX = Mathf.Abs(go.transform.position.x - transform.position.x);
		float distY = Mathf.Abs(go.transform.position.y - transform.position.y);
		
		if(distX <= 0.1f)
		{
			ignoreX.Add(go);
		}
		else if(distY <= 0.1f)
		{
			ignoreY.Add(go);
		}
		
		if(distX <= 0.1f && distY <= 0.1f)
		{
			ignoreX.Remove(go);
			ignoreY.Remove(go);
		}
	}
}
