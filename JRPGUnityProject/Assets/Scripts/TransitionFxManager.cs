using UnityEngine;
using System.Collections;

public class TransitionFxManager : Singleton<TransitionFxManager> {

    // Prevent use of constructor
    protected TransitionFxManager() { }

    bool fading = false;
    GUITexture fadeTexture;
    float fadeLerpVar;
    float fadeTime;
    Color fadeStartColor;
    Color fadeNewColor;

    public static void Fade(float fadeTime, bool fadeOut) 
    {
        if (Instance.fading) return;
        Instance.fading = true;

        GameObject fadeObj = new GameObject();
        Instance.fadeTexture = fadeObj.AddComponent<GUITexture>();
        Instance.fadeTexture.texture = (Texture)Resources.Load("FadeTexture");
        Instance.fadeTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);

        Instance.fadeLerpVar = 0f;
        Instance.fadeTime = fadeTime;
        if (fadeOut)
        {

            Instance.fadeStartColor = Color.clear;
            Instance.fadeNewColor = Color.black;
        }
        else
        {
            Instance.fadeStartColor = Color.black;
            Instance.fadeNewColor = Color.clear;
        }
    }

    void Update()
    {
        if (fading)
        {
            fadeLerpVar += Time.deltaTime / Instance.fadeTime;
            Instance.fadeTexture.color = Color.Lerp(fadeStartColor, fadeNewColor, fadeLerpVar);
            if (fadeLerpVar > 1f) fading = false;
        }
    }
}
