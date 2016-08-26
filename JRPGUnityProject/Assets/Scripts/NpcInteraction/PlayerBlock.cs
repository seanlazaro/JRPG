using UnityEngine;
using System.Collections;

public class PlayerBlock : MonoBehaviour {

	GameObject player;
	Rigidbody2D playerRigidBody;
	public float verticalDisplacement;
	public float horizontalDisplacement;
	bool triggered;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		playerRigidBody = player.GetComponent<Rigidbody2D> ();
		triggered = false;
	}

	IEnumerator OnTriggerEnter2D(Collider2D collider)
	{ 
		if (collider.gameObject.tag == "Player" && !triggered)
		{
			player.GetComponent<PlayerSpriteController> ().EnableMovement (false);
			playerRigidBody.velocity = new Vector2 (
				horizontalDisplacement * player.GetComponent<PlayerSpriteController>().moveSpeed,
				verticalDisplacement * player.GetComponent<PlayerSpriteController>().moveSpeed);
			yield return new WaitForSeconds (1 / player.GetComponent<PlayerSpriteController> ().moveSpeed);
			playerRigidBody.velocity = new Vector2 (0, 0);
			this.gameObject.GetComponent<DialogueController> ().SpacePress = true;
			StartCoroutine(this.gameObject.GetComponent<DialogueController>().StartDialogue());
		}
		yield break;
	}
}
