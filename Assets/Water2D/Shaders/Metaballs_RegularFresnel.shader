

Shader "Water2D/Metaballs_RegularFresnel" 
{
Properties 
{ 
    _MainTex ("Texture", 2D) = "white" { } 
         
    _botmcut ("Cutoff", Range(0,0.5)) = 0.1   

    _constant ("Multiplier", Range(0,6)) = 1  

	//[Toggle(FLIP_TEXTURE)]_FlipTex ("Flip Texture", Float) = 0
}
SubShader 
{
	Tags {"Queue" = "Transparent" }
    Pass {
    Blend SrcAlpha OneMinusSrcAlpha     
	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag	
	#pragma shader_feature FLIP_TEXTURE
	
	#include "UnityCG.cginc"	
	

	sampler2D _MainTex;	
	float _botmcut,_constant;
	float _FlipTex;

	struct v2f {
	    float4  pos : SV_POSITION;
	    float2  uv : TEXCOORD0;
	};	
	float4 _MainTex_ST;	
	
	v2f vert (appdata_base v)
	{
	    v2f o;
		
        o.pos = UnityObjectToClipPos (v.vertex);
	    o.uv.xy = ComputeGrabScreenPos(o.pos);
	    return o;
	}	
	
	half4 frag (v2f i) : COLOR
	{		

		#ifdef FLIP_TEXTURE
			//i.uv.y = 1 - i.uv.y;
		#endif

		//(_FlipTex == 1.0)
        #if UNITY_UV_STARTS_AT_TOP
		
			i.uv.y = 1 - i.uv.y;
		#endif
        
    
        if(_FlipTex == 1.0)
            i.uv.y = 1 - i.uv.y;


		half4 texcol,finalColor;
	    finalColor = tex2D (_MainTex, i.uv); 		

		if(finalColor.a < _botmcut)
		{
			finalColor.a = 0;  
		}
		else
		{
			finalColor.a *= _constant;  
		}
								
	    return finalColor;
	}
	ENDCG

    }
}
Fallback "VertexLit"
} 