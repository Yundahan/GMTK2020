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

	public Button button;
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

    private void Awake()
    {      
        boxCollider = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
		
		rigidbody.isKinematic = true;
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
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
	
	void TaskOnClick()
	{
		walking = true;
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
	}
	
	public void ChangeBridgeCounter(int value)
	{
		bridgeCounter += value;
	}
}
