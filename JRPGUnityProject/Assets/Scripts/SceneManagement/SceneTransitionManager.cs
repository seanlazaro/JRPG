using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneTransitionManager : Singleton<SceneTransitionManager> {

    // Prevent use of constructor
    protected SceneTransitionManager() { }
    

    const float fadeTime = 1f;
    const float battleFadeTime = 1f;
    

    string destinationTile;
    public string DestinationTile
    {
        get { return destinationTile; }
    }

    string previousScene;
    Vector3 previousPosition;

    GameObject engagedEnemySprite;
    List<GameObject> enemySpritesInScene = new List<GameObject>();

    public IEnumerator LoadScene(string sceneToLoad, string destinationTile)
    {
        GameObject player = GameObject.FindWithTag("Player");
        PlayerSpriteController psc = player.GetComponent<PlayerSpriteController>();
        psc.EnableMovement(false);
        
        StartCoroutine(TransitionEffects.Instance.Fade(fadeTime, true));
        yield return new WaitForSeconds(fadeTime);


        this.destinationTile = destinationTile;

        previousScene = SceneManager.GetActiveScene().name;

        for (int i = 0; i < enemySpritesInScene.Count; i++)
        {
            Destroy(enemySpritesInScene[i]);
        }
        enemySpritesInScene.Clear();


        SceneManager.LoadScene(sceneToLoad);
    }

    public IEnumerator SpawnPlayer(Vector3 spawnPosition, Vector2 directionToFace)
    {
        GameObject player = GameObject.FindWithTag("Player");     
        player.transform.position = spawnPosition;

        PlayerSpriteController psc = player.GetComponent<PlayerSpriteController>();
        psc.OnSpawnPlayer(directionToFace);

        StartCoroutine(TransitionEffects.Instance.Fade(fadeTime, false));
        yield return new WaitForSeconds(fadeTime);

        psc.EnableMovement(true);
    }

    public void SpawnEnemySprite(GameObject[] possibleEnemies, Vector3 spawnPosition)
    {
        // Don't spawn enemies if returning from battle
        if (previousScene == "Battle") return;

        System.Random r = new System.Random();
        int i = r.Next(possibleEnemies.Length);

        GameObject enemy = Instantiate(possibleEnemies[i]);
        enemy.transform.position = spawnPosition;
    }

    public IEnumerator EnterBattle(GameObject engagedEnemySprite)
    {
        GameObject player = GameObject.FindWithTag("Player");
        PlayerSpriteController psc = player.GetComponent<PlayerSpriteController>();
        psc.EnableMovement(false);
        
        StartCoroutine(TransitionEffects.Instance.Fade(battleFadeTime, true));
        yield return new WaitForSeconds(battleFadeTime);


        destinationTile = null;

        this.engagedEnemySprite = engagedEnemySprite;

        previousScene = SceneManager.GetActiveScene().name;
        previousPosition = player.transform.position;


        GameObject[] enemySpritesArray = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemySpritesArray.Length; i++)
        {
            DontDestroyOnLoad(enemySpritesArray[i]);
            enemySpritesArray[i].SetActive(false);
            enemySpritesInScene.Add(enemySpritesArray[i]);
        }


        SceneManager.LoadScene("Battle");
    }

    public IEnumerator SpawnEnemyInBattle(Vector2 spawnPosition)
    {
        EnterBattle battleData = engagedEnemySprite.GetComponent<EnterBattle>();

        GameObject enemy = Instantiate(battleData.enemyInBattle);
        enemy.transform.position = spawnPosition;
        enemy.GetComponent<Battler>().battleState = battleData.enemyBattleState;

        StartCoroutine(TransitionEffects.Instance.Fade(battleFadeTime, false));
        yield return null;
    }

	public IEnumerator ExitBattle()
	{
		StartCoroutine(TransitionEffects.Instance.Fade(fadeTime, true));
		// 2f is added because the win message takes 3 seconds.
		yield return new WaitForSeconds(fadeTime + 2f);

		string temp = previousScene;

		previousScene = "Battle";

		SceneManager.LoadScene(temp);


		for (int i = 0; i < enemySpritesInScene.Count; i++)
		{
			enemySpritesInScene[i].SetActive(true);
		}

		enemySpritesInScene.Remove(engagedEnemySprite);
		Destroy(engagedEnemySprite);
	}

	public IEnumerator ExitLostBattle()
	{
		StartCoroutine(TransitionEffects.Instance.Fade(fadeTime, true));
		// 2f is added because the win message takes 3 seconds.
		yield return new WaitForSeconds(fadeTime + 2f);

		string temp = "Game Over";

		previousScene = "Battle";

		SceneManager.LoadScene(temp);


		for (int i = 0; i < enemySpritesInScene.Count; i++)
		{
			enemySpritesInScene[i].SetActive(true);
		}

		enemySpritesInScene.Remove(engagedEnemySprite);
		Destroy(engagedEnemySprite);
	}

    void OnLevelWasLoaded()
    {
		if (previousScene == "Battle" && SceneManager.GetActiveScene().name != "Game Over")
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.transform.position = previousPosition;

            PlayerSpriteController psc = player.GetComponent<PlayerSpriteController>();
            psc.EnableMovement(true);
        }
    }
}
