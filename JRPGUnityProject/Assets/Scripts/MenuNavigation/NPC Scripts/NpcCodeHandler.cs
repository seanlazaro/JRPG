using UnityEngine;
using System.Collections;

[RequireComponent(typeof (DialogueController))]
public abstract class NpcCodeHandler : MonoBehaviour {

	GameObject parent;

	// Use this for initialization
	public void Start () {
		parent = this.gameObject;
	}

	// Update is called once per frame
	public void Update () {
		if (parent.GetComponent<DialogueController> ().triggerOtherScripts) {
			StartCoroutine(runCode());
			parent.GetComponent<DialogueController> ().triggerOtherScripts = false;
			Debug.Log ("Run Code");
		}
		else
		Debug.Log ("Fail!");
	}
	public abstract IEnumerator runCode ();
}
