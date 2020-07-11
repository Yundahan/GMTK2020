using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Placement : MonoBehaviour
{
	public GameObject canvas;
	private BoxCollider2D[] allButtons;
	
	public GameObject wall;
	private BoxCollider2D[] allWallColliders;
	public GameObject ground;
	private BoxCollider2D[] allGroundColliders;
	
	public CharacterController2D charController;
	
	public GameObject bridge;
	public GameObject angularWall;
	public GameObject bluePortal;
	public GameObject orangePortal;
	
	public Button placeBridge;
	public Button placeAngularWall;
	public Button placePortal;
	
	public Button rotateBridge;
	public Button rotateAngularWall;
	
	enum ObjectType
	{
		None,
		Bridge,
		AngularWall,
		Portal
	}
	
	ObjectType currentObject = ObjectType.None;
	
    // Start is called before the first frame update
    void Start()
    {
        placeBridge.GetComponent<Button>().onClick.AddListener(PlaceBridge);
        rotateBridge.GetComponent<Button>().onClick.AddListener(RotateBridge);
        placeAngularWall.GetComponent<Button>().onClick.AddListener(PlaceAngularWall);
        rotateAngularWall.GetComponent<Button>().onClick.AddListener(RotateAngularWall);
		placePortal.GetComponent<Button>().onClick.AddListener(PlacePortal);
		
		allButtons = canvas.GetComponentsInChildren<BoxCollider2D>();
		allWallColliders = wall.GetComponentsInChildren<BoxCollider2D>();
		allGroundColliders = ground.GetComponentsInChildren<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		foreach(BoxCollider2D col in allButtons)
		{
			Bounds buttonBounds = col.bounds;
			
			if(newPos.x < buttonBounds.max.x &&
			   newPos.x > buttonBounds.min.x &&
			   newPos.y < buttonBounds.max.y &&
			   newPos.y > buttonBounds.min.y &&
			   col.gameObject.activeSelf)
			{
				return;
			}
		}
		
		if(Input.GetMouseButtonDown(0))
		{
			switch(currentObject)
			{
				case ObjectType.Bridge:
					if(charController.GetBridgeCounter() > 0)
					{
						return;
					}
					
					bridge.transform.position = new Vector3(Mathf.Floor(newPos.x) + 0.5f, Mathf.Floor(newPos.y) + 0.5f, 0);//success
					break;
				case ObjectType.AngularWall:
					newPos.z = 0;
					
					foreach(BoxCollider2D col in allGroundColliders)
					{
						float tempDist = Vector3.Distance(col.bounds.ClosestPoint(newPos), newPos);
						
						if(tempDist <= 0.01f)
						{
							angularWall.transform.position = new Vector3(Mathf.Floor(newPos.x) + 0.5f, Mathf.Floor(newPos.y) + 0.5f, 0);
							return;//success
						}
					}
					break;
				case ObjectType.Portal:
					newPos.z = 0;
					float dist = 100000f;
					Vector3 point = new Vector3(0f, 0f, 0f);
					
					foreach(BoxCollider2D col in allWallColliders)
					{
						Vector3 closestPoint = col.bounds.ClosestPoint(newPos);
						float tempDist = Vector3.Distance(closestPoint, newPos);
						
						if(tempDist < dist)
						{
							dist = tempDist;
							point = closestPoint;
						}
					}
					
					Vector3 direction = newPos - point;
					float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
					angle = Mathf.Round(angle / 90) * 90;
					
					if(angle < 0)
					{
						angle += 360;
					}
					
					bluePortal.transform.position = point;
					bluePortal.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));//success
					break;
				default:
					break;
			}
		}
		
		if(Input.GetMouseButtonDown(1))
		{
			if(currentObject == ObjectType.Portal)
			{
				newPos.z = 0;
				float dist = 5000f;
				Vector3 point = new Vector3(0f, 0f, 0f);
				
				foreach(BoxCollider2D col in allWallColliders)
				{
					Vector3 closestPoint = col.bounds.ClosestPoint(newPos);
					float tempDist = Vector3.Distance(closestPoint, newPos);
					
					if(tempDist < dist)
					{
						dist = tempDist;
						point = closestPoint;
					}
				}
				
				Vector3 direction = newPos - point;
				float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
				angle += 180;
				angle = Mathf.Round(angle / 90) * 90;
				orangePortal.transform.position = point;
				orangePortal.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));//success
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
	
	void PlacePortal()
	{
		currentObject = ObjectType.Portal;
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
