// From gamesplusjames' Unity RPG Tutorial on Youtube

using UnityEngine;
using System.Collections;

public class PlayerSpriteController : MonoBehaviour {

	enum DirectionFacing {North=1, East, South, West};

    public float moveSpeed;

    Animator anim;
    Rigidbody2D playerRigidBody;
    
    bool playerMoving;
    Vector2 lastMove;

    bool movementEnabled = true;

	bool npcInRange;

	// Used in interaction.

	public GameObject interactionBox;
	GameObject interactingNPC;
	RectTransform rt;

	float width;
	float height;
	GameObject[] npcs;

	// Use this for initialization
    void Start () {
        anim = GetComponent<Animator> ();
        playerRigidBody = GetComponent<Rigidbody2D> ();
		npcs = GameObject.FindGameObjectsWithTag("NPC");
		interactionBox = GameObject.Find("Interaction Box");
		rt = new RectTransform ();
	}
    
    // Update is called once per frame
    void Update () {
        if (!movementEnabled) return;
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
		int directionFacing;
		// Following code changes postition of the "interaction dot".
		if (lastMove.x > 0.5f)
			directionFacing = (int)DirectionFacing.East;
		else if (lastMove.x < -0.5f)
			directionFacing = (int)DirectionFacing.West;
		else if (lastMove.y > 0.5f)
			directionFacing = (int)DirectionFacing.South;
		else if (lastMove.y < -0.5f)
			directionFacing = (int)DirectionFacing.North;
		else
			directionFacing = (int)DirectionFacing.East;
		switch (directionFacing) {
		case 1: //North
			interactionBox.transform.position = interactionBox.transform.parent.TransformPoint (0, -1, 0);
			break;
		case 3: //South
			interactionBox.transform.position = interactionBox.transform.parent.TransformPoint(0,1,0);
			break;
		case 2: //East
			interactionBox.transform.position = interactionBox.transform.parent.TransformPoint(1,0,0);
			break;
		case 4: //West
			interactionBox.transform.position = interactionBox.transform.parent.TransformPoint(-1,0,0);
			break;
		default:
			break;
		}
			
		// GetKeyUp is used here, and GetKeyDown is used in the dialogue controller script, to prevent conflict.
		if (Input.GetKeyUp (KeyCode.Space)) {
			if(CheckForNPC(interactionBox, ref interactingNPC))
			{
				StartCoroutine(interactingNPC.GetComponent<DialogueController> ().StartDialogue ());
			}
		}

	}
    
	public bool CheckForNPC(GameObject interactionBox, ref GameObject npcInteracting)
	{
		bool npcInRange = false; 
		foreach (GameObject npc in npcs) {
			// Finds the dimensions of the npc object.
			rt = (RectTransform)npc.transform;
			width = rt.rect.width;
			height = rt.rect.height;

			// The range for the interaction box to be detected is slightly increased for convenience.
			if (interactionBox.transform.position.x > npc.transform.position.x - 0.2 && interactionBox.transform.position.x < npc.transform.position.x + width) {
				if (interactionBox.transform.position.y > npc.transform.position.y && interactionBox.transform.position.y < npc.transform.position.y + height +0.2) {
					npcInteracting = npc;
					npcInRange = true;
					break;
				}
			}
		}

		return npcInRange;
	}

    public void OnSpawnPlayer(Vector2 directionToFace)
    {
        lastMove = directionToFace;
    }

    public void EnableMovement(bool enable)
    {
        movementEnabled = enable;

        if(!enable)
        {
            playerMoving = false;
            playerRigidBody.velocity = new Vector2(0f, 0f);

            anim.SetFloat("MoveX", 0f);
            anim.SetFloat("MoveY", 0f);
            anim.SetBool("PlayerMoving", playerMoving);
        }
    }
}
