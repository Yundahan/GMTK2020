using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBehaviour : MonoBehaviour


{
	private bool lightswitch = true;
	private SpriteRenderer spriteR;
	
	public Sprite lightVisible;
	public Sprite lightInvisible;

	
    // Start is called before the first frame update
    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.name == "Sleepyboi" && lightswitch)
		{
			col.gameObject.SendMessage("WakeUpStill");
		}
		else if(col.gameObject.name == "Ghost" && lightswitch)
		{
			col.gameObject.SendMessage("Kill");
		}
	}
	
	void LightsOn()
	{
	lightswitch = true;	
	spriteR.sprite = lightVisible;
		
	}
	
	void LightsOff()
	{
	 lightswitch = false;
	 spriteR.sprite = lightInvisible;	
	}	
	
}
