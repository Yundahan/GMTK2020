using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterController2D : MonoBehaviour
{
    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    float speed = 5f;
	
	public int maxCollisions = 5;
	private int collisionCount = 0;

    //[SerializeField, Tooltip("Acceleration while grounded.")]
    //float walkAcceleration = 75;

	public Button startButton;
	public GameObject restartButton;
	
	public GameObject wall;
	public GameObject ground;
	public GameObject orangePortal; //Orange on right wall facing left is default
	public GameObject bluePortal;  //Blue on left wall facing right is default
	public GameObject MusicController;
	
    private BoxCollider2D boxCollider;
	private Animator animator;
	private Rigidbody2D rb;
	
	public GameObject angularWall;
	
	private BoxCollider2D bluePortalCollider;
	private BoxCollider2D orangePortalCollider;

    private Vector2 velocity;
	
	private bool walking = false;
	private bool falling = false;
	private bool dead = false;
	
	private int bridgeCounter = 0;
	
	private float xSpeed = 1f;
	private float ySpeed = 0f;
	
	private float initXPos;
	private float initYPos;
	private float initXSpeed;
	private float initYSpeed;
	
	public Sprite deadSprite;
	private Sprite initSprite;
	
	
	/*public AudioSource MainThemePiano;
	public AudioSource MainThemeOrchestra;
	public AudioSource DeathTheme ;*/
	
	private Vector3 defaultRotation = new Vector3(0,0,0f);
	private Vector3 quarterRotationClockwise = new Vector3(0,0,270f);
	private Vector3 halfRotation = new Vector3 (0,0,180f);
	private Vector3 threeQuarterRotationClockwise = new Vector3(0,0,90f);
	
	public Text collisionsRemaining;
	
	private void Awake()
    {      
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
		animator.enabled = false;
		bluePortalCollider = bluePortal.GetComponent<BoxCollider2D>();
		orangePortalCollider = orangePortal.GetComponent<BoxCollider2D>();
		
		rb.isKinematic = true;
        startButton.GetComponent<Button>().onClick.AddListener(StartWalking);
        restartButton.GetComponent<Button>().onClick.AddListener(Restart);
		
		initXSpeed = xSpeed;
		initYSpeed = ySpeed;
		initXPos = transform.position.x;
		initYPos = transform.position.y;
		initSprite = GetComponent<SpriteRenderer>().sprite;
		
		MusicController.SendMessage("Awake");
		
		collisionsRemaining.text = maxCollisions.ToString();
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
			velocity.y -= 0.1f;
			return;
		}
		
		if(collisionCount >= maxCollisions)
		{
			WakeUpStill();
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
			animator.SetFloat("xSpeed", xSpeed);
			animator.SetFloat("ySpeed", ySpeed);
			angularWall.SendMessage("ResetTurned", gameObject);
			collisionCount++;
			collisionsRemaining.text = (maxCollisions - collisionCount).ToString();
		}
		
		if(col.gameObject == orangePortal)
		{
			
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
					animator.SetFloat("xSpeed", newHorizontalMovement);
					animator.SetFloat("ySpeed", newVerticalMovement);
					//Debug.Log("QuarterRotationBlueOut");
					
				}
				if(Mathf.Abs(relativePortalPosition.z - halfRotation.z) <0.01f)
				{
					xSpeed *= -1f;
					ySpeed *= -1f;
					animator.SetFloat("xSpeed", xSpeed);
					animator.SetFloat("ySpeed", ySpeed);
					//Debug.Log("HalfRotationBlueOut");
				}
				if(Mathf.Abs(relativePortalPosition.z - threeQuarterRotationClockwise.z) <0.01f)
				{
					float newVerticalMovement = (1f*xSpeed);
					float newHorizontalMovement =  (-1f*ySpeed);
					xSpeed = newHorizontalMovement;
					ySpeed = newVerticalMovement;
					animator.SetFloat("xSpeed", newHorizontalMovement);
					animator.SetFloat("ySpeed", newVerticalMovement);
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
					animator.SetFloat("xSpeed", newHorizontalMovement);
					animator.SetFloat("ySpeed", newVerticalMovement);
					//Debug.Log("QuarterRotationOrangeOut");
				}
				if(Mathf.Abs(relativePortalPosition.z - halfRotation.z) <0.01f)
				{
					xSpeed *= -1f;
					ySpeed *= -1f;
					animator.SetFloat("xSpeed", xSpeed);
					animator.SetFloat("ySpeed", ySpeed);
					//Debug.Log("HalfRotationOrangeOut");
				}
				if(Mathf.Abs(relativePortalPosition.z - threeQuarterRotationClockwise.z) <0.01f)
				{
					float newVerticalMovement = (-1f*xSpeed);
					float newHorizontalMovement =  (1f*ySpeed);
					xSpeed = newHorizontalMovement;
					ySpeed = newVerticalMovement;
					animator.SetFloat("xSpeed", newHorizontalMovement);
					animator.SetFloat("ySpeed", newVerticalMovement);
					//Debug.Log("ThreeQuarterRotationOrangeOut");
				}
				
			}
		}
	}
	
	void StartWalking()
	{
		animator.enabled = true;
		walking = true;
		startButton.GetComponent<Button>().interactable = false;
		MusicController.SendMessage("StartWalking");
	}
	
	void Restart()
	{
		walking = false;
		falling = false;
		dead = false;
		xSpeed = initXSpeed;
		ySpeed = initYSpeed;
		animator.SetFloat("xSpeed", initXSpeed);
		animator.SetFloat("ySpeed", initYSpeed);
		bridgeCounter = 0;
		collisionCount = 0;
		startButton.GetComponent<Button>().interactable = true;
		restartButton.SetActive(false);
		transform.position = new Vector2(initXPos, initYPos);
		animator.enabled = false;
		GetComponent<SpriteRenderer>().sprite = initSprite;
		MusicController.SendMessage("Restart");
		collisionsRemaining.text = maxCollisions.ToString();
	}
	
	public void FallDown()
	{
		if(bridgeCounter > 0 || falling)
		{
			return;
		}
		
		walking = false;
		falling = true;
		velocity = new Vector2(0f, -1f);
		WakeUp();
	}
	
	public void WakeUpStill()
	{
		dead = true;
		velocity = new Vector2(0f, 0f);
		xSpeed = initXSpeed;
		ySpeed = initXSpeed;
		WakeUp();
	}
	
	public void WakeUp()
	{
		restartButton.SetActive(true);
		MusicController.SendMessage("WakeUp");
		animator.enabled = false;
		GetComponent<SpriteRenderer>().sprite = deadSprite;
		angularWall.SendMessage("Reset");
	}	
	
	public void ChangeBridgeCounter(int value)
	{
		bridgeCounter += value;
	}
	
	public int GetBridgeCounter()
	{
		return bridgeCounter;
	}
	
	public bool GetWalking()
	{
		return walking;
	}
	
	public void Turn(float value)
	{
		float tempx = xSpeed;
		xSpeed = ySpeed * value;
		ySpeed = tempx * value;
		animator.SetFloat("xSpeed", xSpeed);
		animator.SetFloat("ySpeed", ySpeed);
		collisionCount++;
		collisionsRemaining.text = (maxCollisions - collisionCount).ToString();
	}
}
