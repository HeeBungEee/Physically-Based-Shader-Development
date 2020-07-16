using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Camera))]
[ExecuteInEditMode]
public class PostEffects : MonoBehaviour
{
    public Shader curShader;
    private Material curMaterial;
    Material material
    {
        get
        {
            if (curMaterial == null)
            {
                curMaterial = new Material(curShader);
                curMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return curMaterial;
        }
    }
    
    void Start ()
    {
        curShader = Shader.Find("Hidden/PostEffects");
        GetComponent<Camera>().allowHDR = true;
    
        if (!curShader && !curShader.isSupported)
        {
            enabled = false;
            Debug.Log("Shader Not Supported");
            return;
        }
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }

    void Update()
    {
        if (!GetComponent<Camera>().enabled)
            return;
    }

    void OnDisable()
    {
        if (curMaterial)
        {
            DestroyImmediate(curMaterial);
        }
    }

    public bool InvertEffect;
    public bool DepthEffect;
    public bool LinearInvertEffect;
    public bool ToneMappingEffect;
    
    [Range(1.0f, 10.0f)]
    public float ToneMapperExposure = 2.0f;

    void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture) 
    {
        if (curShader != null)
        {
            if (InvertEffect)
            {
                Graphics.Blit(sourceTexture, destTexture, material, 0);
            }
            else if (DepthEffect)
            {
                Graphics.Blit(sourceTexture, destTexture, material, 1);
            }
            else if (LinearInvertEffect)
            {
                Graphics.Blit(sourceTexture, destTexture, material, 2);
            }
            else if (ToneMappingEffect)
            {
                material.SetFloat("_ToneMapperExposure", ToneMapperExposure);
                Graphics.Blit(sourceTexture, destTexture, material, 3);
            }
            else
            {
                Graphics.Blit(sourceTexture, destTexture);
            }
        }
    }
}
