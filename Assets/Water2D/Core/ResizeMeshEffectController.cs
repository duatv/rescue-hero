using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

[ExecuteInEditMode]
#endif



public class ResizeMeshEffectController : MonoBehaviour
{

   

    Texture effectTex2d;
    RenderTexture backTex2d;
    Camera effectCam;
    Camera backgroundCam;



    GameObject effectCamera;
    GameObject backgroundCamera;
  
    private void Start()

    {
       
        RebuildRenderTexturesAll();

    }


    void Update()
    {
        if (effectCamera.GetComponent<Camera>().targetTexture == null) {
            RebuildRenderTexturesAll();
        }


    }

    public void RebuildRenderTexturesAll()
    {
        effectCamera = gameObject.transform.parent.gameObject;
        backgroundCamera = gameObject.transform.parent.parent.Find("0-BGCamera").gameObject;



        effectCamera.GetComponent<Camera>().backgroundColor = new Color(0f, 0f, 0f, 0f);


        //CREATING AND ADDING RT
        RenderTexture EffectRT = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
        RenderTexture BackgroundRT = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);


        GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_EffectTex", EffectRT);
        GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_BackgroundTex", BackgroundRT);

        effectCamera.GetComponent<Camera>().targetTexture = EffectRT;
        backgroundCamera.GetComponent<Camera>().targetTexture = BackgroundRT;

        //Debug.Log(EffectRT.height);

    }
    

}
