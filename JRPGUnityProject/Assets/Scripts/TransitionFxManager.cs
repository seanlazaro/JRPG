using UnityEngine;
using System.Collections;

public class TransitionFxManager : Singleton<TransitionFxManager> {

    bool fading = false;

    // Prevent use of constructor
    protected TransitionFxManager() { }

    // fadeTime in seconds
    // fadeOut false for fade in
    public static void Fade(float fadeTime, bool fadeOut)
    {
        if (Instance.fading) return;
        Instance.fading = true;

        Material material = (Material)Resources.Load("BlackDefaultSprite");
        Instance.DoFadeCoroutine(fadeTime, fadeOut, Color.black, material);
    }

    void DoFadeCoroutine(float fadeTime, bool fadeOut, Color color, Material material)
    {
        if (fadeOut) StartCoroutine(FadeOutCoroutine(fadeTime, color, material));
        else StartCoroutine(FadeInCoroutine(fadeTime, color, material));
    }

    IEnumerator FadeOutCoroutine(float fadeTime, Color color, Material material)
    {
        float alpha = 0f;
        while (alpha < 1f)
        {
            yield return new WaitForEndOfFrame();
            alpha = Mathf.Clamp01(alpha + Time.deltaTime / fadeTime);
            DrawQuadrilateral(alpha, color, material);
        }
        fading = false;
    }

    IEnumerator FadeInCoroutine(float fadeTime, Color color, Material material)
    {
        float alpha = 1f;
        while (alpha > 0f)
        {
            yield return new WaitForEndOfFrame();
            alpha = Mathf.Clamp01(alpha - Time.deltaTime / fadeTime);
            DrawQuadrilateral(alpha, color, material);
        }
        fading = false;
    }

    void DrawQuadrilateral(float alpha, Color color, Material material)
    {
        color.a = alpha;
        material.SetPass(0);
        GL.PushMatrix();
        GL.LoadOrtho();
        GL.Begin(GL.QUADS);
        GL.Color(color);
        GL.Vertex3(0f, 0f, -1f);
        GL.Vertex3(0f, 1f, -1f);
        GL.Vertex3(1f, 1f, -1f);
        GL.Vertex3(1f, 0f, -1f);
        GL.End();
        GL.PopMatrix();
    }
}
