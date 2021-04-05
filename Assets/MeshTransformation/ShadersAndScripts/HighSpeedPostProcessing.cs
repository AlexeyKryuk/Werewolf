﻿using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[ExecuteInEditMode]
public class HighSpeedPostProcessing : MonoBehaviour
{
    [Header("ToneMapping")]
    [Range(0, 1)]
    public float PostExposure = 2.2f;
    public float Contrast = 1.5f;
    [Range(0, 1)]
    public float Disaturate = 0.42f;
    [Range(-1, 1)]
    public float Min = 0.03f;
    [Range(0.5f, 1.0f)]
    public float Max = 1f;
    [Range(0, 10)]
    public float Saturation = 1.5f;
    [Header("Depth Of Field")]
    public bool DepthOfField = false;
    [Range(0, 3)]
    public float BlurAmount = 0;
    public float BlurDistance = 200;
    [Range(0.5f, 15)]
    public float BlurRange = 11;
    [Header("Vignette")]
    [Range(0, 2)]
    public float VignetteIntensity = 0f;

    public Material material;

    Camera Cam;
#if UNITY_EDITOR
    public SceneView SceneView;
     Camera SceneCamera;
#endif
    private void Start()
    {

        Cam = GetComponent<Camera>();
        if (Cam.depthTextureMode == DepthTextureMode.None)
            Cam.depthTextureMode = DepthTextureMode.Depth;
#if UNITY_EDITOR
        material = (Material)AssetDatabase.LoadAssetAtPath("Assets/MeshTransformation/ShadersAndScripts/ToneMap.mat", typeof(Material));
#endif
    }

    void OnEnable()
    {
#if UNITY_EDITOR
        material = (Material)AssetDatabase.LoadAssetAtPath("Assets/MeshTransformation/ShadersAndScripts/ToneMap.mat", typeof(Material));
#endif
        Cam = GetComponent<Camera>();
        Set();

      //  UpdateComponents();

    }
#if UNITY_EDITOR

    private Camera GetCamera()
    {
        // SceneView = EditorWindow.GetWindow<SceneView>();
        SceneView = SceneView.lastActiveSceneView;
        return SceneView.camera;
    }
#endif
    void Set()
    {
        //    if (GetComponent<Camera>().depthTextureMode == DepthTextureMode.None)
        Cam.depthTextureMode = DepthTextureMode.Depth;

          }

        //    private Material BlurMaterial;

        // Creates a private material used to the effect
        // void Awake()
        //  {
        //      material = new Material(Shader.Find("Hidden/ToneMap"));
        //  }

        // Postprocess the image
        void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //    if (intensity == 0)
        //     {
        //     Graphics.Blit(source, destination);
        //     return;
        //}

        material.SetFloat("PostExposure", PostExposure);
        material.SetFloat("Contrast", Contrast);
        material.SetFloat("_Disaturate", Disaturate);
        material.SetFloat("Saturation", Saturation);
        material.SetFloat("_Min", Min);
        material.SetFloat("_Max", Max);
        material.SetFloat("BlurDistance", BlurDistance); 
        material.SetFloat("BlurRange", 14.99f - BlurRange);
        material.SetInt("FepthOfField", System.Convert.ToInt32(DepthOfField)); 
        material.SetFloat("VignetteIntensity", VignetteIntensity);
        if (DepthOfField && BlurAmount < 2.0f) {
            material.SetFloat("_BlurAmount", BlurAmount + 0.8f);
            RenderTexture BlumTex = null;
            BlumTex = RenderTexture.GetTemporary(Screen.width / 2, Screen.height / 2, 0, source.format);
            var temp1 = RenderTexture.GetTemporary(Screen.width / 2, Screen.height / 2, 0, source.format);
            Graphics.Blit(source, temp1, material, 1);
            Graphics.Blit(temp1, BlumTex, material, 1);
            RenderTexture.ReleaseTemporary(temp1);
            material.SetTexture("_BlurTex", BlumTex);
            RenderTexture.ReleaseTemporary(BlumTex);
        }
        else if (DepthOfField)
        {
            material.SetFloat("_BlurAmount", BlurAmount + -0.5f);
            RenderTexture BlumTex;
            BlumTex = RenderTexture.GetTemporary(Screen.width / 3, Screen.height / 3, 0, source.format);
            var temp1 = RenderTexture.GetTemporary(Screen.width / 4, Screen.height / 4, 0, source.format);
            //   var temp2 = RenderTexture.GetTemporary(Screen.width / 4, Screen.height / 4, 0, source.format);
            Graphics.Blit(source, temp1, material, 1);
            Graphics.Blit(temp1, BlumTex, material, 1);
          //  Graphics.Blit(temp2, BlumTex, material, 1);

            material.SetTexture("_BlurTex", BlumTex);
            RenderTexture.ReleaseTemporary(BlumTex);
            RenderTexture.ReleaseTemporary(temp1);
            //RenderTexture.ReleaseTemporary(temp2);
        }



        Graphics.Blit(source, destination, material,0);
    }

    void Update()
    {
     //   UpdateComponents();
        if (material == null)
        {
            this.enabled = false;
        }
    }

    public void UpdateComponents()
    {

#if UNITY_EDITOR
        if (SceneCamera == null)
        {
            SceneCamera = GetCamera();
        }

        if (SceneCamera == null) // This shouldn't happen, but it does
            return;


        GameObject cameraGo = SceneCamera.gameObject;

        var cType = this.GetType();

        var existing = cameraGo.GetComponent(cType) ?? cameraGo.AddComponent(cType);
        EditorUtility.CopySerialized(this, existing);
#endif

    }


}