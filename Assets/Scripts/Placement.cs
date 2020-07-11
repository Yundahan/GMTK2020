using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Placement : MonoBehaviour
{
	public GameObject[] allButtons;
	public Button bridgeButton;
	public GameObject bridge;
	
	enum ObjectType
	{
		None,
		Bridge
	}
	
	ObjectType currentObject = ObjectType.None;
	
    // Start is called before the first frame update
    void Start()
    {
        bridgeButton.GetComponent<Button>().onClick.AddListener(PlaceBridges);
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		newPos.z = 0;
		
		foreach(GameObject go in allButtons)
		{
			Bounds buttonBounds = go.GetComponent<BoxCollider2D>().bounds;
			
			if(newPos.x < buttonBounds.max.x &&
			   newPos.x > buttonBounds.min.x &&
			   newPos.y < buttonBounds.max.y &&
			   newPos.y > buttonBounds.min.y &&
			   go.activeSelf)
			{
				return;
			}
		}
		
		if(Input.GetMouseButtonDown(0))
		{
			switch(currentObject)
			{
				case ObjectType.Bridge:
					bridge.transform.position = newPos;
					break;
				default:
					break;
			}
		}
    }
	
	void PlaceBridges()
	{
		currentObject = ObjectType.Bridge;
	}
}
