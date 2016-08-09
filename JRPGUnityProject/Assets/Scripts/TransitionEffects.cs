using UnityEngine;
using System.Collections;

public class TransitionEffects : Singleton<TransitionEffects> {

    // Prevent use of constructor
    protected TransitionEffects() { }

    bool fading = false;

    public IEnumerator Fade(float fadeTime, bool fadeOut)
    {
        if (fading) yield break;
        fading = true;

        GameObject fadeObj = new GameObject();
        GUITexture fadeTexture = fadeObj.AddComponent<GUITexture>();
        fadeTexture.texture = (Texture)Resources.Load("FadeTexture");
        fadeTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);

        Color fadeStartColor;
        Color fadeNewColor;
        if (fadeOut)
        {
            fadeStartColor = Color.clear;
            fadeNewColor = Color.black;
        }
        else
        {
            fadeStartColor = Color.black;
            fadeNewColor = Color.clear;
		}

        float fadeLerpVar = 0f;

		//If statement added to avoid errors when testing without the audio manager.
		//This happens when one does not pass through the title screen.
		if(GameObject.Find("Audio Manager") != null)
			StartCoroutine (AudioManager.Instance.AudioFade (fadeTime, fadeOut));
        
		while (fadeLerpVar < 1f)
        {
            fadeLerpVar += Time.deltaTime / fadeTime;
            fadeTexture.color = Color.Lerp(fadeStartColor, fadeNewColor, fadeLerpVar);
            yield return new WaitForEndOfFrame();
        }
        fading = false;
    }
}
