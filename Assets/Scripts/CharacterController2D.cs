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
	
    private BoxCollider2D boxCollider;
	private Animator animator;
	private Rigidbody2D rigidbody;

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
	
	private void Awake()
    {      
        boxCollider = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
		
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
