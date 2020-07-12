using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightBehaviour : MonoBehaviour
{
	public bool lightswitch = true;
	private SpriteRenderer spriteR;
	
	public GameObject restartButton;
	
	bool initSwitch;
	Sprite initSprite;
	
	public Sprite lightVisible;
	public Sprite lightInvisible;

	
    // Start is called before the first frame update
    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
		
        restartButton.GetComponent<Button>().onClick.AddListener(Reset);
		
		initSwitch = lightswitch;
		initSprite = spriteR.sprite;
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

	void buttonTriggered()
	{
		if(lightswitch)
		{
			LightsOff();		
		}
		else
		{
			LightsOn();	
		}	
	}
	
	void Reset()
	{
		spriteR.sprite = initSprite;
		lightswitch = initSwitch;
	}
}
