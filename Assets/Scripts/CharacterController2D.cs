using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterController2D : MonoBehaviour
{
    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    float speed = 9;

    [SerializeField, Tooltip("Acceleration while grounded.")]
    float walkAcceleration = 75;

	public Button button;
	
    private BoxCollider2D boxCollider;

    private Vector2 velocity;
	
	private bool walking = false;
	private float xSpeed = 1f;
	private float ySpeed = 0f;

    private void Awake()
    {      
        boxCollider = GetComponent<BoxCollider2D>();
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    private void Update()
    {
		if(!walking)
		{
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
	
	void TaskOnClick()
	{
		walking = !walking;
	}
}
