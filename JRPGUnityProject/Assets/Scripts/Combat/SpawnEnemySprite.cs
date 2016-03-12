using UnityEngine;
using System.Collections;

public class SpawnEnemySprite : MonoBehaviour {

    public GameObject[] possibleEnemies;

	void Start () {
        SceneTransitionManager.Instance.SpawnEnemySprite(possibleEnemies, transform.position);
	}
}
