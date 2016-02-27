using UnityEngine;
using System.Collections;

public class TransitionFxManager : Singleton<TransitionFxManager> {

    bool fading = false;

    float alpha;
    int fadeDir; // +1 to fade out, -1 to fade in
    float fadeTime;
    Texture2D fadeTexture; 

    // Prevent use of constructor
    protected TransitionFxManager() { }

    // fadeTime in seconds
    // fadeOut false for fade in
    public static void Fade(float fadeTime, bool fadeOut)
    {
        if (Instance.fading) return;
        Instance.fading = true;

        if (fadeOut)
        {
            Instance.alpha = 0f;
            Instance.fadeDir = 1;
        }
        else
        {
            Instance.alpha = 1f;
            Instance.fadeDir = -1;
        }
        Instance.fadeTime = fadeTime;
        Instance.fadeTexture = (Texture2D) Resources.Load("FadeTexture");
    }
    
    //// fadeTime in seconds
    //// fadeOut false for fade in
    //public static void Fade(float fadeTime, bool fadeOut)
    //{
    //    if (Instance.fading) return;
    //    Instance.fading = true;

    //    Material material = (Material)Resources.Load("BlackDefaultSprite");
    //    Instance.DoFadeCoroutine(fadeTime, fadeOut, Color.black, material);
    //}

    //void DoFadeCoroutine(float fadeTime, bool fadeOut, Color color, Material material)
    //{
    //    if (fadeOut) StartCoroutine(FadeOutCoroutine(fadeTime, color, material));
    //    else StartCoroutine(FadeInCoroutine(fadeTime, color, material));
    //}

    //IEnumerator FadeOutCoroutine(float fadeTime, Color color, Material material)
    //{
    //    float alpha = 0f;
    //    while (alpha < 1f)
    //    {
    //        yield return new WaitForEndOfFrame();
    //        alpha = Mathf.Clamp01(alpha + Time.deltaTime / fadeTime);
    //        DrawQuadrilateral(alpha, color, material);
    //    }
    //    fading = false;
    //}

    //IEnumerator FadeInCoroutine(float fadeTime, Color color, Material material)
    //{
    //    float alpha = 1f;
    //    while (alpha > 0f)
    //    {
    //        yield return new WaitForEndOfFrame();
    //        alpha = Mathf.Clamp01(alpha - Time.deltaTime / fadeTime);
    //        DrawQuadrilateral(alpha, color, material);
    //    }
    //    fading = false;
    //}

    //void DrawQuadrilateral(float alpha, Color color, Material material)
    //{
    //    color.a = alpha;
    //    material.SetPass(0);
    //    GL.PushMatrix();
    //    GL.LoadOrtho();
    //    GL.Begin(GL.QUADS);
    //    GL.Color(color);
    //    GL.Vertex3(0f, 0f, -1f);
    //    GL.Vertex3(0f, 1f, -1f);
    //    GL.Vertex3(1f, 1f, -1f);
    //    GL.Vertex3(1f, 0f, -1f);
    //    GL.End();
    //    GL.PopMatrix();
    //}

    void OnGUI()
    {
        if(fading)
        {
            alpha += (Time.deltaTime / fadeTime) * fadeDir;

            if (alpha > 1f || alpha < 0f)
            {
                alpha = Mathf.Clamp01(alpha);
                fading = false;
            }
            
            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
            GUI.depth = -69; // Use very low depth to show fade over everything else
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
        }
    }
}
