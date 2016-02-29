// From gamesplusjames' Unity RPG Tutorial on Youtube

using UnityEngine;
using System.Collections;

public class PlayerMovementController : MonoBehaviour {

    public float moveSpeed;

    Animator anim;
    Rigidbody2D playerRigidBody;
    
    bool playerMoving;
    Vector2 lastMove;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator> ();
        playerRigidBody = GetComponent<Rigidbody2D> ();
    }
    
    // Update is called once per frame
    void Update () {        
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        playerMoving = false;

        if (xInput > 0.5f || xInput < -0.5f) // Moving horizontally
        {
            playerRigidBody.velocity = new Vector2(xInput * moveSpeed, playerRigidBody.velocity.y);
            //anim.gameObject.transform.Translate(new Vector3(xInput * moveSpeed * Time.deltaTime, 0f));
            playerMoving = true;
            lastMove = new Vector2(xInput, 0f);
        }
        else
        {
            playerRigidBody.velocity = new Vector2( 0f, playerRigidBody.velocity.y);
        }

        if (yInput > 0.5f || yInput < -0.5f) // Moving vertically
        {
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, yInput * moveSpeed);
            //anim.gameObject.transform.Translate(new Vector3(0f, yInput * moveSpeed * Time.deltaTime));
            playerMoving = true;
            lastMove = new Vector2(0f, yInput);
        }
        else
        {
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0f);
        }

        anim.SetFloat("MoveX", xInput);
        anim.SetFloat("MoveY", yInput);
        anim.SetBool("PlayerMoving", playerMoving);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
    }
    
    public void OnSpawnPlayer(Vector2 directionToFace)
    {
        lastMove = directionToFace;
    }
}
