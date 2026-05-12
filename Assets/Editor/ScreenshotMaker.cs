using UnityEngine;
using UnityEditor;
using System.IO;

public class ScreenshotMaker
{
    [MenuItem("Tools/Make Game Screenshot 4K")]
    static void CaptureGameView()
    {
        Camera cam = Camera.main;

        if (cam == null)
        {
            Debug.LogError("Main Camera not found!");
            return;
        }

        int width = 3840;
        int height = 2160;

        RenderTexture rt = new RenderTexture(width, height, 24);
        cam.targetTexture = rt;

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        cam.Render();

        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        cam.targetTexture = null;
        RenderTexture.active = null;

        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes("GameScreenshot.png", bytes);

        Object.DestroyImmediate(rt);
        Object.DestroyImmediate(tex);

        Debug.Log("Game screenshot saved in project root!");
    }
}
