using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushButton : MonoBehaviour


{
	private SpriteRenderer spriteR;
	
	public Sprite buttonPressed;
	public Sprite buttonUnpressed;
	public GameObject triggeredObject;

	
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
		triggeredObject.SendMessage("buttonTriggered");
		spriteR.sprite = buttonPressed;
	}
	
	void OnTriggerExit2D (Collider2D col)
	{
		spriteR.sprite = buttonUnpressed;

	}	
	
}
