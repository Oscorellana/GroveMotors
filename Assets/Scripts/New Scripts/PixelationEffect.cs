using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PixelationEffect : MonoBehaviour
{
    [Range(64, 1024)] public int pixelWidth = 320;
    [Range(64, 1024)] public int pixelHeight = 180;

    private Material pixelMat;

    private void Start()
    {
        Shader shader = Shader.Find("Hidden/PixelationShader");
        if (shader == null)
        {
            Debug.LogError("Shader not found! Make sure the shader name is Hidden/PixelationShader");
            enabled = false;
            return;
        }

        pixelMat = new Material(shader);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (pixelMat == null)
        {
            Graphics.Blit(src, dest);
            return;
        }

        pixelMat.SetFloat("_PixelWidth", pixelWidth);
        pixelMat.SetFloat("_PixelHeight", pixelHeight);

        Graphics.Blit(src, dest, pixelMat);
    }
}

