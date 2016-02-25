using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class testBattleScene : MonoBehaviour {

    public GameObject[] testEnemies;

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
    
    }

	IEnumerator OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
			float fadeTime = GameObject.Find ("Player").GetComponent<Fading>().BeginFade(1);
			yield return new WaitForSeconds (fadeTime);
            GameStateController.possibleEnemies = testEnemies;
            SceneManager.LoadScene("Battle");
		}
    }
}
