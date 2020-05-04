using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.Reflection;
using System;

#if UNITY_EDITOR

[ExecuteInEditMode]
#endif



public class ResizeQuadEffectController : MonoBehaviour
{
    public bool FlipTexture = false;
    public int sorting = 0;


    public static void RebuildTextures(int flipTex = -1)
    {

        //flipTex -1 dont update
        //flipTex 0 dont flip
        //flipTex 1  flip


        if (instance == null)
        {

            ResizeQuadEffectController[] aux = FindObjectsOfType<ResizeQuadEffectController>();
            if (aux.Length > 1)
            {
                for (int i = 1; i < aux.Length; i++)
                {
                    DestroyImmediate(aux[i].gameObject);
                }
            }
                
            instance = aux[0].GetComponent<ResizeQuadEffectController>();
           
        }

        if (flipTex == 0)
            instance.FlipTexture = false;
        if (flipTex == 1)
            instance.FlipTexture = true;

        instance.RebuildRenderTexturesAll();
    }

    public static ResizeQuadEffectController instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }



    Texture effectTex2d;
    Camera effectCam;
    RenderTexture backTex2d;



    GameObject effectCamera;
    GameObject backgroundCamera;
    

    private void Start()

    {

        RebuildRenderTexturesAll();

        gameObject.transform.hideFlags = HideFlags.HideInInspector;
        
    }


    void Update()
    {
        if (effectCamera.GetComponent<Camera>().targetTexture == null)
        {
            RebuildRenderTexturesAll();
        }


    }

   

    public void AboutToRebuildAll()
    {// called when playing/stop is coming
        effectCamera.GetComponent<Camera>().targetTexture = null;
    }

    public void RebuildRenderTexturesAll()
    {

        int width, height;

        width = Camera.main.pixelWidth;
        height = Camera.main.pixelHeight;

        effectCamera = gameObject.transform.parent.gameObject;

        if (gameObject.transform.parent.parent.Find("0-BGCamera"))
            backgroundCamera = gameObject.transform.parent.parent.Find("0-BGCamera").gameObject;
        
        // Only for toon style
        if (backgroundCamera != null)
        {
            RenderTexture BackgroundRT = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
            GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_BackgroundTex", BackgroundRT);
            backgroundCamera.GetComponent<Camera>().forceIntoRenderTexture = true;
            backgroundCamera.GetComponent<Camera>().targetTexture = BackgroundRT;
        }


        //CREATING AND ADDING RT
        RenderTexture EffectRT = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
       
        //For Regular shader
        GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_MainTex", EffectRT);

        //For Toon Shader
        GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_EffectTex", EffectRT);

        effectCamera.GetComponent<Camera>().forceIntoRenderTexture = true;
        effectCamera.GetComponent<Camera>().targetTexture = EffectRT;

#if UNITY_EDITOR && UNITY_2019_2_OR_NEWER
        try
        {
            UnityEditor.SceneVisibilityManager.instance.Hide(gameObject, true);
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
#endif


        //#if UNITY_EDITOR
        /*
        if ((Application.platform == RuntimePlatform.WindowsEditor) || (Application.platform == RuntimePlatform.WindowsPlayer))
        {
            if (GraphicsSettings.renderPipelineAsset != null)
            {
                GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_FlipTex", 0f);
            }
            else {
                GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_FlipTex", 1f);
            }
        }
        */


        /*
        if (FlipTexture && (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor))
      
             GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_FlipTex", 1.0f);

        else
            GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_FlipTex", 0.0f);


*/


        //#endif





#if UNITY_EDITOR
        FlipTexture = UnityEditor.EditorPrefs.GetBool("_flipTexEditor");
        //print(FlipTexture);

        if (FlipTexture && (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor))

            GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_FlipTex", 1.0f);

        else
            GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_FlipTex", 0.0f);

#endif

    }

    public void SetSorting(int id = 0)
    {
        GetComponent<MeshRenderer>().sortingOrder = id;
    }


}
