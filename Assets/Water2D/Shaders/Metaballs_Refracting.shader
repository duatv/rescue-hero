

Shader "Water2D/Metaballs_Refracting" {
Properties {    

   // _TintColor ("Tint color", Color) = (1,1,1,1)
	_AmountOfTintColor ("Intensity", Range(0,1)) = 0.3

    _Mag ("Distortion", Range(0,3)) = 0.05
    _Speed ("Speed", Range(0,5)) = 1.0

	 _MainTex ("Texture", 2D) = "white" { }
	 _BackgroundTex ("BackgroundTexture", 2D) = "white" { }
	_botmcut ("Cutoff", Range(0,0.5)) = 0.1   
	_constant ("Multiplier", Range(0,6)) = 1  

	//[Toggle(FLIP_TEXTURE)]_FlipTex ("Flip Texture", Float) = 0
    
}
/// <summary>
/// Multiple metaball shader.

/// </summary>
SubShader {
    Tags {"Queue"="Transparent"}
	
    
    
    
    Pass 
    {
		
        Blend SrcAlpha OneMinusSrcAlpha     
      // Blend One One // Additive
      // Blend One OneMinusSrcAlpha
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag   
		#pragma shader_feature FLIP_TEXTURE

        #include "UnityCG.cginc" 
        
       
		sampler2D _MainTex;	
		sampler2D _BackgroundTex;
       // half4 _TintColor;
		float _AmountOfTintColor;
        float _Mag;
        float _Speed;
		float _botmcut,_constant;
		float _FlipTex;


        float4 _CameraDepthTexture_TexelSize;


        struct v2f {
            float4  pos : SV_POSITION;
            float4  uv : TEXCOORD0;
        };  

        float4 _MainTex_ST;  
   
        v2f vert (appdata_base v){
            v2f o = (v2f)0;
            o.pos = UnityObjectToClipPos (v.vertex);
            o.uv.xy = ComputeGrabScreenPos(o.pos);
            return o;
        };

        float random (float2 uv)
        {
            return frac(sin(dot(uv,float2(12.9898,78.233)))*43758.5453123);
        }
        

        float noise(float2 coord){
            float2 i = floor(coord);
            float2 f = frac(coord);

            // 4 corners of a rectangle surrounding our point
            float a = random(i);
            float b = random(i + float2(1.0, 0.0));
            float c = random(i + float2(0.0, 1.0));
            float d = random(i + float2(1.0, 1.0));

            float2 cubic = f * f * (3.0 - 2.0 * f);

            return lerp(a, b, cubic.x) + (c - a) * cubic.y * (1.0 - cubic.x) + (d - b) * cubic.x * cubic.y;
        }


        
        sampler2D _GrabTexture;
        float sprite_scale = 8.0;
        float scale_x = 0.81;
        
        half4 frag (v2f i) : COLOR{     
        
			#if UNITY_UV_STARTS_AT_TOP
                i.uv.y = 1 - i.uv.y;
            #endif
            
            if(_FlipTex == 1.0)
                i.uv.y = 1 - i.uv.y;

            float time = _Time.y;
            float2 noisecoord1 = i.uv * 8.0 * (_Mag);
            float2 noisecoord2 = i.uv * 8.0 * (_Mag) + 4.0;
            
            float2 motion1 = float2(time * 0.3, time * -0.4) * _Speed;
            float2 motion2 = float2(time * 0.1, time * 0.5) * _Speed;
            
           
            
            float2 distort1 = float2(noise(noisecoord1 + motion1), noise(noisecoord2 + motion1)) - float2(0.5,0.5);
            float2 distort2 = float2(noise(noisecoord1 + motion2), noise(noisecoord2 + motion2)) - float2(0.5,0.5);
            float2 distort_sum = (distort1 + distort2) / 60.0;
            
          
            
            
           half4 finalColor, backgroundColor;
			
			finalColor = tex2D (_MainTex, i.uv); 		

			if(finalColor.a < _botmcut)
			{
				finalColor.a = 0; 
				//finalColor.a = 1;
			}
			else
			{
				#ifdef FLIP_TEXTURE
					i.uv.y = 1 - i.uv.y;
				#endif
				i.uv.xy = i.uv.xy + distort_sum;
				backgroundColor = tex2D (_BackgroundTex, i.uv);

				finalColor = lerp(backgroundColor,finalColor , _AmountOfTintColor);
				finalColor = lerp(fixed(0.5), finalColor, 1.4);
				finalColor.a = 1.0;
			}

            
           
            
       
                
            return finalColor;
            
           
        }




        ENDCG

    }
}
Fallback "VertexLit"
} 