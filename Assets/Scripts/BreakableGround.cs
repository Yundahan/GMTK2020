using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableGround : MonoBehaviour
{
	bool broken = false;
	
	public GameObject groundCollider; //required!!
	
	public Sprite brokenSprite;
	
	private Sprite initSprite;
	
	Vector3 initGroundColliderPos;
	
    // Start is called before the first frame update
    void Start()
    {
        initGroundColliderPos = groundCollider.transform.position;
		initSprite = GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerExit2D(Collider2D col)
	{
		if(col.gameObject.name == "Sleepyboi")
		{
			broken = true;
			groundCollider.transform.position = new Vector3(5000f, 5000f, 0f);
			GetComponent<SpriteRenderer>().sprite = brokenSprite;
		}
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if(broken && col.gameObject.name == "Sleepyboi")
		{
			col.gameObject.SendMessage("WakeUpStill");
		}
	}
	
	void Reset()
	{
		broken = false;
		groundCollider.transform.position = initGroundColliderPos;
		GetComponent<SpriteRenderer>().sprite = initSprite;
	}
}
