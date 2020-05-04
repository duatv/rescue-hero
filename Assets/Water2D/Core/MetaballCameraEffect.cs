using System;
using UnityEngine;

namespace Water2D
{
    [ExecuteInEditMode]
    public class MetaballCameraEffect : MonoBehaviour
    {
       

        //public Shader blurShader = null;

        
        public Material cutOutMaterial;

        public Camera bgCamera;


        RenderTexture bgTargetTexture;

        public void Restart()
        {
            OnEnable();
        }

        private void OnEnable()
        {
			if (Screen.width > 0 && Screen.height > 0) {
				bgTargetTexture = new RenderTexture (Screen.width, Screen.height, 16);
				bgCamera.targetTexture = bgTargetTexture;
			}
        }

        protected void OnDisable()
        {
           
        }

        protected void Start()
        {


            // Disable if the shader can't run on the users graphics card
            if (!cutOutMaterial.shader.isSupported)
            {
                enabled = false;
                return;
            }

        }

     


        // Called by the camera to apply the image effect
		RenderTexture buffer;
		
        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
			//if(!Application.isPlaying)
			//	return;

            int rtW = source.width / 4;
            int rtH = source.height / 4;
            buffer = RenderTexture.GetTemporary(rtW, rtH, 0);
			
     
            Graphics.Blit(bgTargetTexture, destination); // background
            Graphics.Blit(source, destination, cutOutMaterial); // water
            RenderTexture.ReleaseTemporary(buffer);


        }
    }
}
