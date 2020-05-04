

Shader "Water2D/Metaballs_Simple_FOR_URP" {
Properties {    
    //_MainTex ("Main Texture", 2D) = "white" { }    
    _BackgroundTex ("Background Texture", 2D) = "white" { }    
    _EffectTex ("Effect Texture", 2D) = "white" { }    
    _Color ("Main color", Color) = (1,1,1,1)
    _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5

    _Stroke ("Stroke alpha", Range(0,1)) = 0.1
    _StrokeColor ("Stroke color", Color) = (1,1,1,1)

	 //[Toggle(FLIP_TEXTURE)]_FlipTex ("Flip Texture", Float) = 0

   
     

}
/// <summary>
/// Multiple metaball shader.

/// </summary>
SubShader {
    Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="Transparent"}
    //GrabPass{"_GrabTexture"}
    Pass {
    Blend SrcAlpha OneMinusSrcAlpha     
  // Blend One One // Additive
  // Blend One OneMinusSrcAlpha

  

    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag   
    #pragma shader_feature FLIP_TEXTURE

    #include "UnityCG.cginc"
    
    //sampler2D _MainTex; 
    
    sampler2D _BackgroundTex; 
    sampler2D _EffectTex; 
	sampler2D _GrabTexture; 
	Float _FlipTex;
    
    float2 _screenPos;
    
    //scheme 1
    float4 _Color;
    fixed _Cutoff;
    fixed _Stroke;
    half4 _StrokeColor;
    
    

    float4 _colorArray[12]; // store fill(pair) and strokeColor(unpair) 
    float _cutOffStrokeArray[12]; // cutoff(pair) and strokeAmount(unpair) 

    float4 ColorOfRenderCamera = float4(0.0,0.0,0.0,0.0);


    struct v2f {
        float4  pos : SV_POSITION;
        float2  uv : TEXCOORD0;
    };  
    
    
    float4 _EffectTex_ST;     
    v2f vert (appdata_base v){
        v2f o;
        //o.pos = UnityObjectToClipPos (v.vertex);
        //o.uv = TRANSFORM_TEX (v.texcoord, _EffectTex);
        o.pos = UnityObjectToClipPos (v.vertex);
        //o.uv = float4( v.texcoord.xy, 0, 0 );
         o.uv.xy = ComputeGrabScreenPos(o.pos);
        // o.uv.xy = o.pos;
        return o;
    };


    

    half4 frag (v2f i) : COLOR{     

		//#ifdef FLIP_TEXTURE
		//	i.uv.y = 1 - i.uv.y;
		//#endif

		if(_FlipTex)
			i.uv.y = 1 - i.uv.y;
        
		half4 _colorTex= tex2D (_EffectTex, i.uv); 
        
        //scheme1 r=1 g=0 b=0  ** arrayID's: 0-1 
        //scheme2 r=0 g=1 b=0  ** arrayID's: 2-3
        //scheme3 r=0 g=0 b=1  ** arrayID's: 4-5
        //scheme4 r=1 g=1 b=0  ** arrayID's: 6-7
        //scheme5 r=0 g=1 b=1  ** arrayID's: 8-9
        //scheme6 r=1 g=1 b=1  ** arrayID's: 10-11
        
        int idFill = 2;
        int idStroke = 3;

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

        
        // Now I need read and compare the effect tex against the default effect camera color
        // if color = default(black) then load the color of _BackgroundTex
        // if not, do perform the cutoff
		
		
		//screen_uv.y = 1 - screen_uv.y

			half4 _colorTexBackground = tex2D (_BackgroundTex, i.uv); 
            
      
        
      
         
           float ColorToCompare = 0.0;
           if(idFill == 0 )
           {
                ColorToCompare = _colorTex.r;
           }
            if(idFill == 2 )
           {
                ColorToCompare = _colorTex.g;
           }
            if(idFill == 4 )
           {
                ColorToCompare = _colorTex.b;
           }
           
           //the rest
           if(idFill > 4 )
                ColorToCompare = _colorTex.r;
           
           
           clip(ColorToCompare - _cutOffStrokeArray[idFill]);
		  
		  
		  //if((ColorToCompare - _cutOffStrokeArray[idFill] > .2f) && (_colorTex.r != ColorOfRenderCamera.r && _colorTex.g != ColorOfRenderCamera.g && _colorTex.b != ColorOfRenderCamera.b))
		  // {
		   //_colorTex = lerp(_colorArray[idFill], _colorTexBackground, 1.0 -_colorArray[idFill].a);
		  // }else{
		  /// _colorTex = float4(0,0,0,0);
		  // }

		   

            if (ColorToCompare < _cutOffStrokeArray[idStroke]) {
               
				_colorTex = lerp(_colorArray[idStroke], _colorTexBackground, 1.0 -_colorArray[idStroke].a);
            } else {
                
			   _colorTex = lerp(_colorArray[idFill], _colorTexBackground, 1.0 -_colorArray[idFill].a);
            }
           
		 
		  
		  

            //clip(_colorTex.a - _cutOffStrokeArray[idFill]);

            //if (_colorTex.a < _cutOffStrokeArray[idStroke]) {
            //    _colorTex = _colorArray[idStroke];
            //} else {
            //    _colorTex = _colorArray[idFill];
            //}
            
           
            
            
        //}
        

        return _colorTex;
           
    }




    ENDCG

    }
}
Fallback "VertexLit"
} 