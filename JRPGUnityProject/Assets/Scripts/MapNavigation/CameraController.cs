using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject followTarget;
	public float moveSpeed;

	private Vector3 targetPos;

	// Use this for initialization
	void Start () {
		// To prevent awkward camera jumps across maps with multiple spawn points
		transform.position = new Vector3 (followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		targetPos = new Vector3 (followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);
		transform.position = Vector3.Lerp (transform.position, targetPos, moveSpeed * Time.deltaTime);
	}
}
