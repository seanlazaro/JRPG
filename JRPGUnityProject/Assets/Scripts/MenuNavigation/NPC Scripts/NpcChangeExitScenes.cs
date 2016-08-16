using UnityEngine;
using System.Collections;

[RequireComponent(typeof (DialogueController))]
public class NpcChangeExitScenes : NpcCodeHandler {

	public GameObject[] exitTiles;
	public string[] destination;

	public override IEnumerator runCode()
	{
		Debug.Log ("Code ran");
		for (int i = 0; i < exitTiles.Length; i++)
		{
			exitTiles [i].GetComponent<ChangeScene> ().sceneToLoad = destination [i];
		}
		yield break;
	}
}
