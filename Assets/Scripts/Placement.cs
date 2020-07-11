using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Placement : MonoBehaviour
{
	public GameObject[] allButtons;
	
	public GameObject bridge;
	public GameObject angularWall;
	
	public Button placeBridge;
	public Button placeAngularWall;
	
	public Button rotateBridge;
	public Button rotateAngularWall;
	
	enum ObjectType
	{
		None,
		Bridge,
		AngularWall
	}
	
	ObjectType currentObject = ObjectType.None;
	
    // Start is called before the first frame update
    void Start()
    {
        placeBridge.GetComponent<Button>().onClick.AddListener(PlaceBridge);
        rotateBridge.GetComponent<Button>().onClick.AddListener(RotateBridge);
        placeAngularWall.GetComponent<Button>().onClick.AddListener(PlaceAngularWall);
        rotateAngularWall.GetComponent<Button>().onClick.AddListener(RotateAngularWall);
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
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
					bridge.transform.position = new Vector3(Mathf.Floor(newPos.x) + 0.5f, Mathf.Floor(newPos.y) + 0.5f, 0);
					break;
				case ObjectType.AngularWall:
					angularWall.transform.position = new Vector3(Mathf.Floor(newPos.x) + 0.5f, Mathf.Floor(newPos.y) + 0.5f, 0);
					break;
				default:
					break;
			}
		}
    }
	
	void PlaceBridge()
	{
		currentObject = ObjectType.Bridge;
	}
	
	void PlaceAngularWall()
	{
		currentObject = ObjectType.AngularWall;
	}
	
	void RotateBridge()
	{
		bridge.transform.eulerAngles = new Vector3(bridge.transform.eulerAngles.x, bridge.transform.eulerAngles.y, bridge.transform.eulerAngles.z + 90);
	}
	
	void RotateAngularWall()
	{
		angularWall.transform.eulerAngles = new Vector3(angularWall.transform.eulerAngles.x, angularWall.transform.eulerAngles.y, angularWall.transform.eulerAngles.z + 90);
	}
}
