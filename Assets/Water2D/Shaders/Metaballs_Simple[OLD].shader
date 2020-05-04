

Shader "Water2D/Metaballs_Simple[OLD]" {
Properties {    
    _MainTex ("Texture", 2D) = "white" { }    
    _Color ("Main color", Color) = (1,1,1,1)
    _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5

	_Stroke ("Stroke alpha", Range(0,1)) = 0.1
	_StrokeColor ("Stroke color", Color) = (1,1,1,1)

	//_Color2 ("color2", Color) = (1,1,1,1)
	 

}
/// <summary>
/// Multiple metaball shader.

/// </summary>
SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	//GrabPass{}
    Pass {
    Blend SrcAlpha OneMinusSrcAlpha     
  // Blend One One // Additive
  // Blend One OneMinusSrcAlpha

  

	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag	
	#pragma shader_feature SCHEME1
	#include "UnityCG.cginc"
	sampler2D _MainTex;	
	float2 _screenPos;
	
	//scheme 1
	float4 _Color;
	fixed _Cutoff;
	fixed _Stroke;
	half4 _StrokeColor;
	
	

	float4 _colorArray[12]; // store fill(pair) and strokeColor(unpair) 
	float _cutOffStrokeArray[12]; // cutoff(pair) and strokeAmount(unpair) 

	float4 _CameraDepthTexture_TexelSize;


	struct v2f {
	    float4  pos : SV_POSITION;
	    float2  uv : TEXCOORD0;
	};	

	float4 _MainTex_ST;		
	v2f vert (appdata_base v){
	    v2f o;
	    o.pos = UnityObjectToClipPos (v.vertex);
	    o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
	    return o;
	};


	

	half4 frag (v2f i) : COLOR{		
		half4 _colorTex= tex2D (_MainTex, i.uv); 
		
		//scheme1 r=1 g=0 b=0  ** arrayID's: 0-1 
		//scheme2 r=0 g=1 b=0  ** arrayID's: 2-3
		//scheme3 r=0 g=0 b=1  ** arrayID's: 4-5
		//scheme4 r=1 g=1 b=0  ** arrayID's: 6-7
		//scheme5 r=0 g=1 b=1  ** arrayID's: 8-9
		//scheme6 r=1 g=1 b=1  ** arrayID's: 10-11
		
		int idFill;
		int idStroke;

		//SCHEME1 
		if(_colorTex.r>0 && _colorTex.g==0 && _colorTex.b==0){
			idFill = 0;
			idStroke = 1;
		}
		//SCHEME2 
		if(_colorTex.r==0 && _colorTex.g>0 && _colorTex.b==0){
			idFill = 2;
			idStroke = 3;
		}
		//SCHEME3 
		if(_colorTex.r==0 && _colorTex.g == 0 && _colorTex.b>0){
			idFill = 4;
			idStroke = 5;
		}
		//SCHEME4 
		if(_colorTex.r>0 && _colorTex.g > 0 && _colorTex.b==0){
			idFill = 6;
			idStroke = 7;
		}
		//SCHEME5 
		if(_colorTex.r==0 && _colorTex.g > 0 && _colorTex.b>0){
			idFill = 8;
			idStroke = 9;
		}
		//SCHEME6 
		if(_colorTex.r>0 && _colorTex.g > 0 && _colorTex.b>0){
			idFill = 10;
			idStroke = 11;
		}


		// PERFORM CUTOFF by SCHEMES
			clip(_colorTex.a - _cutOffStrokeArray[idFill]);

			if (_colorTex.a < _cutOffStrokeArray[idStroke]) {
				_colorTex = _colorArray[idStroke];
			} else {
				_colorTex = _colorArray[idFill];
			}

			return _colorTex;
			/*

		 // PERFORM CUTOFF
			clip(_colorTex.a - _Cutoff);

			if (_colorTex.a < _Stroke) {
				_colorTex = _colorArray[idStroke];
			} else {
				_colorTex = _colorArray[idFill];
			}

			return _colorTex;

			*/
	}




	ENDCG

    }
}
Fallback "VertexLit"
} 