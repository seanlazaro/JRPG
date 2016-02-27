using UnityEngine;
using System;
using System.Collections;

public class PlayerMovementController : MonoBehaviour {

    public float moveSpeed;

    Animator anim;
	Rigidbody2D playerRigidBody;
   	
	bool playerMoving;
    Vector2 lastMove;
    public Vector2 LastMove
    {
        get { return lastMove; }
        set { lastMove = value; }
    }

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator> ();
		playerRigidBody = GetComponent<Rigidbody2D> ();
	
	}
    
    // Update is called once per frame
    void Update () {

        playerMoving = false;

        if (Input.GetAxisRaw ("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f) {
            //transform.Translate (new Vector3 (Input.GetAxisRaw ("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
			playerRigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, playerRigidBody.velocity.y);
			playerMoving = true;
            lastMove = new Vector2 (Input.GetAxisRaw ("Horizontal"), 0f);
        } 

        if (Input.GetAxisRaw ("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f) {
            //transform.Translate (new Vector3 (0f, Input.GetAxisRaw ("Vertical") * moveSpeed * Time.deltaTime, 0f));
					playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, Input.GetAxisRaw("Vertical") * moveSpeed);            
			playerMoving = true;
            lastMove = new Vector2 (0f, Input.GetAxisRaw ("Vertical"));
        } 
		if(Input.GetAxisRaw ("Horizontal") < 0.5f && Input.GetAxisRaw("Horizontal") > -0.5f)
		{
							playerRigidBody.velocity = new Vector2( 0f, playerRigidBody.velocity.y);
		}
		if(Input.GetAxisRaw ("Vertical") < 0.5f && Input.GetAxisRaw("Vertical") > -0.5f)
		{
							playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0f);
		}
        anim.SetFloat ("MoveX", Input.GetAxisRaw ("Horizontal"));
        anim.SetFloat ("MoveY", Input.GetAxisRaw ("Vertical"));
        anim.SetBool ("PlayerMoving", playerMoving);
        anim.SetFloat ("LastMoveX", lastMove.x);
        anim.SetFloat ("LastMoveY", lastMove.y);
    }
}
