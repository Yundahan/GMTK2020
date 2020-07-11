using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngularWall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerEnter2D(Collider2D col)
	{
		float rotation = Mathf.Round(transform.rotation.eulerAngles.z);
		
		if(col.gameObject.name == "Sleepyboi")
		{
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
}
