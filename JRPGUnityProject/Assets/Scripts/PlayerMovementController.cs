// From gamesplusjames' Unity RPG Tutorial on Youtube

using UnityEngine;
using System.Collections;

public class PlayerMovementController : MonoBehaviour {

    float moveSpeed;

    Animator anim;
    bool playerMoving;
    Vector2 lastMove;

    // Use this for initialization
    void Start () {
        moveSpeed = 5f;
        anim = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    void Update () {        
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        playerMoving = false;

        if (xInput > 0.5f || xInput < -0.5f)
        {
            transform.Translate(new Vector3 (xInput * moveSpeed * Time.deltaTime, 0f, 0f));
            playerMoving = true;
            lastMove = new Vector2(xInput, 0f);
        }
        if (yInput > 0.5f || yInput < -0.5f)
        {
            transform.Translate(new Vector3(0f, yInput * moveSpeed * Time.deltaTime, 0f));
            playerMoving = true;
            lastMove = new Vector2(0f, yInput);
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
