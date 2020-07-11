using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    public float speed;
	

    //[SerializeField, Tooltip("Acceleration while grounded.")]
    //float walkAcceleration = 75;

	public Button startButton;
	public GameObject restartButton;
	
	public GameObject wall;
	public GameObject ground;
	public GameObject orangePortal; //Orange on right wall facing left is default
	public GameObject bluePortal;  //Blue on left wall facing right is default
	
    private BoxCollider2D boxCollider;
	private Rigidbody2D rigidbody;
	
	public GameObject angularWall;
	
	private BoxCollider2D bluePortalCollider;
	private BoxCollider2D orangePortalCollider;

    private Vector2 velocity;
	
	private bool walking = false;
	private bool falling = false;
	private bool dead = false;
	
	public float xSpeed;
	public float ySpeed;
	
	private float initXPos;
	private float initYPos;
	private float initXSpeed;
	private float initYSpeed;
	
	private Vector3 defaultRotation = new Vector3(0,0,0f);
	private Vector3 quarterRotationClockwise = new Vector3(0,0,270f);
	private Vector3 halfRotation = new Vector3 (0,0,180f);
	private Vector3 threeQuarterRotationClockwise = new Vector3(0,0,90f);
	
	private void Awake()
    {      
        boxCollider = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
		bluePortalCollider = bluePortal.GetComponent<BoxCollider2D>();
		orangePortalCollider = orangePortal.GetComponent<BoxCollider2D>();
		
		rigidbody.isKinematic = true;
        startButton.GetComponent<Button>().onClick.AddListener(StartWalking);
        restartButton.GetComponent<Button>().onClick.AddListener(Restart);
		
		initXSpeed = xSpeed;
		initYSpeed = ySpeed;
		initXPos = transform.position.x;
		initYPos = transform.position.y;
    }

    private void Update()
    {
		if(!walking && !falling)
		{
			return;
		}
		
		if(dead)
		{
			return;
		}
		
		if(!walking)
		{
			transform.Translate(velocity * Time.deltaTime);
			velocity.y -= 1f;
			return;
		}
		
		
		transform.Translate(velocity * Time.deltaTime);
		velocity.x = speed * xSpeed;
		velocity.y = speed * ySpeed;
		
		/*
		float moveInputHorizontal = Input.GetAxisRaw("Horizontal");
		velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInputHorizontal, walkAcceleration * Time.deltaTime);
		float moveInputVertical = Input.GetAxisRaw("Vertical");
		velocity.y = Mathf.MoveTowards(velocity.y, speed * moveInputVertical, walkAcceleration * Time.deltaTime);
		*/
    }
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject == wall)
		{
			xSpeed *= -1f;
			ySpeed *= -1f;
			angularWall.SendMessage("ResetTurned", gameObject);
		}
		
		if(col.gameObject == orangePortal)
		{
			//Debug.Log("Ich hab ein Portal gefunden");
			Vector2 bluePortalPosition = new Vector2(0,0);
			bluePortalPosition = bluePortal.transform.position;
			Vector3 relativePortalPosition = new Vector3(0,0,0);
			relativePortalPosition = (bluePortal.transform.rotation.eulerAngles - orangePortal.transform.rotation.eulerAngles);
				if (relativePortalPosition.z<0)
				{
						relativePortalPosition.z = (relativePortalPosition.z + 360f);
				}	
			if (Mathf.Abs(bluePortal.transform.rotation.eulerAngles.z -defaultRotation.z) <0.01f)
			{
				Vector2 portalPush = new Vector2 (2f,0);
				transform.position = (bluePortalCollider.offset + bluePortalPosition + portalPush);
			}
			else if (Mathf.Abs(bluePortal.transform.rotation.eulerAngles.z -quarterRotationClockwise.z) <0.01f)
			{	
				Vector2 portalPush = new Vector2 (0,-2f);
				transform.position = (bluePortalCollider.offset + bluePortalPosition + portalPush);
			}
			else if (Mathf.Abs(bluePortal.transform.rotation.eulerAngles.z -halfRotation.z) <0.01f)
			{	
				Vector2 portalPush = new Vector2 (-2f,0);
				transform.position = (bluePortalCollider.offset + bluePortalPosition + portalPush);
			}
			else if (Mathf.Abs(bluePortal.transform.rotation.eulerAngles.z -threeQuarterRotationClockwise.z) <0.01f)
			{	
				Vector2 portalPush = new Vector2 (0,2f);
				transform.position = (bluePortalCollider.offset + bluePortalPosition + portalPush);
			}

			if((Mathf.Abs(relativePortalPosition.z - defaultRotation.z) >0.01f))
			{
				//Debug.Log(relativePortalPosition+ " OrangeIn");	
				if(Mathf.Abs(relativePortalPosition.z - quarterRotationClockwise.z) <0.01f)
				{
					float newVerticalMovement = (-1f*xSpeed);
					float newHorizontalMovement =  (1f*ySpeed);
					xSpeed = newHorizontalMovement;
					ySpeed = newVerticalMovement;
					//Debug.Log("QuarterRotationBlueOut");
					
				}
				if(Mathf.Abs(relativePortalPosition.z - halfRotation.z) <0.01f)
				{
					xSpeed *= -1f;
					ySpeed *= -1f;
					//Debug.Log("HalfRotationBlueOut");
				}
				if(Mathf.Abs(relativePortalPosition.z - threeQuarterRotationClockwise.z) <0.01f)
				{
					float newVerticalMovement = (1f*xSpeed);
					float newHorizontalMovement =  (-1f*ySpeed);
					xSpeed = newHorizontalMovement;
					ySpeed = newVerticalMovement;
					//Debug.Log("ThreeQuarterRotationBlueOut");
				}
				
			}
		}
		if(col.gameObject == bluePortal)
		{
			
			Vector2 orangePortalPosition = new Vector2(0,0);
			orangePortalPosition = orangePortal.transform.position;

			
			Vector3 relativePortalPosition = new Vector3(0,0,0);
			relativePortalPosition = (bluePortal.transform.rotation.eulerAngles - orangePortal.transform.rotation.eulerAngles);
				if (relativePortalPosition.z <0)
				{
						relativePortalPosition.z = (relativePortalPosition.z + 360f);
				}
	
			if (Mathf.Abs(orangePortal.transform.rotation.eulerAngles.z - defaultRotation.z) <0.01f)
			{
				Vector2 portalPush = new Vector2 (-2f,0);
				transform.position = (orangePortalCollider.offset + orangePortalPosition + portalPush);
			}
			else if (Mathf.Abs(orangePortal.transform.rotation.eulerAngles.z - quarterRotationClockwise.z) <0.01f)
			{	
				Vector2 portalPush = new Vector2 (0,2f);
				transform.position = (orangePortalCollider.offset + orangePortalPosition + portalPush);
			}
			else if (Mathf.Abs(orangePortal.transform.rotation.eulerAngles.z - halfRotation.z) <0.01f)
			{	
				Vector2 portalPush = new Vector2 (2f,0);
				transform.position = (orangePortalCollider.offset + orangePortalPosition + portalPush);
			}
			else if (Mathf.Abs(orangePortal.transform.rotation.eulerAngles.z - threeQuarterRotationClockwise.z) <0.01f)
			{	
				Vector2 portalPush = new Vector2 (0,-2f);
				transform.position = (orangePortalCollider.offset + orangePortalPosition + portalPush);
				//Debug.Log("ThreeQuarterPortalPush");
			}	
			
			if(relativePortalPosition != defaultRotation)
			{
				//Debug.Log(relativePortalPosition + " BlueIn");	
				if(Mathf.Abs(relativePortalPosition.z - quarterRotationClockwise.z) <0.01f)
				{
					float newVerticalMovement = (1f*xSpeed);
					float newHorizontalMovement =  (-1f*ySpeed);
					xSpeed = newHorizontalMovement;
					ySpeed = newVerticalMovement;
					//Debug.Log("QuarterRotationOrangeOut");
				}
				if(Mathf.Abs(relativePortalPosition.z - halfRotation.z) <0.01f)
				{
					xSpeed *= -1f;
					ySpeed *= -1f;
					//Debug.Log("HalfRotationOrangeOut");
				}
				if(Mathf.Abs(relativePortalPosition.z - threeQuarterRotationClockwise.z) <0.01f)
				{
					float newVerticalMovement = (-1f*xSpeed);
					float newHorizontalMovement =  (1f*ySpeed);
					xSpeed = newHorizontalMovement;
					ySpeed = newVerticalMovement;
					//Debug.Log("ThreeQuarterRotationOrangeOut");
				}
				
			}
		}
	}
	
	void StartWalking()
	{
		walking = true;
	}
	
	void Restart()
	{
		walking = false;
		falling = false;
		xSpeed = initXSpeed;
		ySpeed = initYSpeed;
		startButton.GetComponent<Button>().interactable = true;
		restartButton.SetActive(false);
		transform.position = new Vector2(initXPos, initYPos);
		velocity = new Vector2(0f, 0f);
	}
	
	public void Turn(float value)
	{
		float tempx = xSpeed;
		xSpeed = ySpeed * value;
		ySpeed = tempx * value;
	}
}
