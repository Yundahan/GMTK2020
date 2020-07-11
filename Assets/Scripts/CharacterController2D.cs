using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterController2D : MonoBehaviour
{
    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    float speed = 5f;

    //[SerializeField, Tooltip("Acceleration while grounded.")]
    //float walkAcceleration = 75;

	public Button startButton;
	public GameObject restartButton;
	
	public GameObject wall;
	public GameObject ground;
	public GameObject orangePortal; //Orange on right wall facing left is default
	public GameObject bluePortal;  //Blue on left wall facing right is default
	
	
    private BoxCollider2D boxCollider;
	private Animator animator;
	private Rigidbody2D rigidbody;
	
	public GameObject angularWall;
	
	private BoxCollider2D bluePortalCollider;
	private BoxCollider2D orangePortalCollider;

    private Vector2 velocity;
	
	private bool walking = false;
	private bool falling = false;
	
	private int bridgeCounter = 0;
	
	private float xSpeed = 1f;
	private float ySpeed = 0f;
	
	private float initXPos;
	private float initYPos;
	private float initXSpeed;
	private float initYSpeed;
	
	public AudioSource MainThemePiano;
	public AudioSource MainThemeOrchestra;
	public AudioSource DeathTheme ;
	
	private Vector3 defaultRotation = new Vector3(0,0,0);
	private Vector3 quarterRotationClockwise = new Vector3(0,0,270);
	private Vector3 halfRotation = new Vector3 (0,0,180);
	private Vector3 threeQuarterRotationClockwise = new Vector3(0,0,90);
	
	private void Awake()
    {      
        boxCollider = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
		bluePortalCollider = bluePortal.GetComponent<BoxCollider2D>();
		orangePortalCollider = orangePortal.GetComponent<BoxCollider2D>();
		
		rigidbody.isKinematic = true;
        startButton.GetComponent<Button>().onClick.AddListener(StartWalking);
        restartButton.GetComponent<Button>().onClick.AddListener(Restart);
		
		initXSpeed = xSpeed;
		initYSpeed = ySpeed;
		initXPos = transform.position.x;
		initYPos = transform.position.y;
		
		MainThemeOrchestra.Play();
		MainThemePiano.Play();
		MainThemeOrchestra.volume = 0f;
		MainThemePiano.volume = 1f;
		

    }

    private void Update()
    {
		if(!walking && !falling)
		{
			return;
		}
		else if(!walking)
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
			animator.SetFloat("xSpeed", xSpeed);
			animator.SetFloat("ySpeed", ySpeed);
			angularWall.SendMessage("ResetTurned");
		}
		
		if(col.gameObject == orangePortal)
		{
			
			Vector2 bluePortalPosition = new Vector2(0,0);
			bluePortalPosition = bluePortal.transform.position;
			transform.position = (bluePortalCollider.offset + bluePortalPosition) ;
			Vector3 relativePortalPosition = new Vector3(0,0,0);
			relativePortalPosition = (bluePortal.transform.rotation.eulerAngles + orangePortal.transform.rotation.eulerAngles);
				if (relativePortalPosition.z >=360f)
				{
						relativePortalPosition.z = (relativePortalPosition.z - 360f);
				}	
			
			
			if(relativePortalPosition != defaultRotation)
			{
				if(relativePortalPosition == quarterRotationClockwise)
				{
					float newVerticalMovement = (-1f*xSpeed);
					float newHorizontalMovement =  ySpeed;
					xSpeed = newHorizontalMovement;
					ySpeed = newVerticalMovement;
					animator.SetFloat("xSpeed", newHorizontalMovement);
					animator.SetFloat("ySpeed", newVerticalMovement);
				}
				if(relativePortalPosition == halfRotation)
				{
					xSpeed *= -1f;
					ySpeed *= -1f;
					animator.SetFloat("xSpeed", xSpeed);
					animator.SetFloat("ySpeed", ySpeed);
				}
				if(relativePortalPosition == threeQuarterRotationClockwise)
				{
					float newVerticalMovement = (xSpeed);
					float newHorizontalMovement =  (-1f*ySpeed);
					xSpeed = newHorizontalMovement;
					ySpeed = newVerticalMovement;
					animator.SetFloat("xSpeed", newHorizontalMovement);
					animator.SetFloat("ySpeed", newVerticalMovement);
				}
				
			}
		}
		if(col.gameObject == bluePortal)
		{
			
			Vector2 orangePortalPosition = new Vector2(0,0);
			orangePortalPosition = orangePortal.transform.position;
			transform.position = (orangePortalCollider.offset + orangePortalPosition);
			
			Vector3 relativePortalPosition = new Vector3(0,0,0);
			relativePortalPosition = (bluePortal.transform.rotation.eulerAngles + orangePortal.transform.rotation.eulerAngles);
				if (relativePortalPosition.z >=360f)
				{
						relativePortalPosition.z = (relativePortalPosition.z - 360f);
				}	
			
			if(relativePortalPosition != defaultRotation)
			{
				if(relativePortalPosition == quarterRotationClockwise)
				{
					float newVerticalMovement = (xSpeed);
					float newHorizontalMovement =  (-1f*ySpeed);
					xSpeed = newHorizontalMovement;
					ySpeed = newVerticalMovement;
					animator.SetFloat("xSpeed", newHorizontalMovement);
					animator.SetFloat("ySpeed", newVerticalMovement);
				}
				if(relativePortalPosition == halfRotation)
				{
					xSpeed *= -1f;
					ySpeed *= -1f;
					animator.SetFloat("xSpeed", xSpeed);
					animator.SetFloat("ySpeed", ySpeed);
				}
				if(relativePortalPosition == threeQuarterRotationClockwise)
				{
					float newVerticalMovement = (-1f*xSpeed);
					float newHorizontalMovement =  ySpeed;
					xSpeed = newHorizontalMovement;
					ySpeed = newVerticalMovement;
					animator.SetFloat("xSpeed", newHorizontalMovement);
					animator.SetFloat("ySpeed", newVerticalMovement);
				}
				
			}
		}
	}
	
	void StartWalking()
	{
		walking = true;
		startButton.GetComponent<Button>().interactable = false;
		MainThemePiano.volume = 0;
		MainThemeOrchestra.volume = 0.75f;
		
		
	}
	
	void Restart()
	{
		walking = false;
		falling = false;
		xSpeed = initXSpeed;
		ySpeed = initYSpeed;
		bridgeCounter = 0;
		startButton.GetComponent<Button>().interactable = true;
		restartButton.SetActive(false);
		transform.position = new Vector2(initXPos, initYPos);
		velocity = new Vector2(0f, 0f);
		MainThemeOrchestra.Play();
		MainThemeOrchestra.volume = 0;
		MainThemePiano.Play();
		MainThemePiano.volume = 1f;
	}
	
	public void FallDown()
	{
		if(bridgeCounter > 0)
		{
			return;
		}
		
		walking = false;
		falling = true;
		velocity = new Vector2(0f, -1f);
		restartButton.SetActive(true);
		MainThemePiano.Stop();
		MainThemeOrchestra.Stop();
		DeathTheme.Play();
		DeathTheme.volume = 0.3f;
	}
	
	public void ChangeBridgeCounter(int value)
	{
		bridgeCounter += value;
	}
	
	public void Turn(float value)
	{
		float tempx = xSpeed;
		xSpeed = ySpeed * value;
		ySpeed = tempx * value;
		animator.SetFloat("xSpeed", xSpeed);
		animator.SetFloat("ySpeed", ySpeed);
	}
}
