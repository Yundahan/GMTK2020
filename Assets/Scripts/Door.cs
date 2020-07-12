using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	public Sprite open;
	public Sprite closed;
	
	private Sprite initSprite;
	
	public bool isOpen = false;
	
	private bool initOpen;
	
	public Vector3 outcomePos;
	
    // Start is called before the first frame update
    void Start()
    {
		initOpen = isOpen;
		
		if(isOpen)
		{
			initSprite = open;
			GetComponent<SpriteRenderer>().sprite = open;
		}
		else
		{
			initSprite = closed;
			GetComponent<SpriteRenderer>().sprite = closed;
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.name == "Sleepyboi" && open)
		{
			col.gameObject.transform.position = outcomePos;
		}
	}
	
	void Reset()
	{
		GetComponent<SpriteRenderer>().sprite = initSprite;
		isOpen = initOpen;
	}

	void buttonTriggered()
	{
		if(isOpen)
		{
			isOpen = false;
			GetComponent<SpriteRenderer>().sprite = closed;
		}
		else
		{
			isOpen = true;
			GetComponent<SpriteRenderer>().sprite = open;
		}
	}
}
