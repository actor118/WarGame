﻿Shader "WMSK/Unlit Surface Single Color Alpha" {
 
Properties {
    _Color ("Color", Color) = (1,1,1)
    _MainTex ("Main Tex", 2D) = "white" {}
    _WaterMaskTex ("Water Mask Tex", 2D) = "white" {}
    _WaterLevel ("Water Level", Float) = 0.1
    _AlphaOnWater ("Alpha On Water", Float) = 0.2
}
 
SubShader {
    Tags {
        "Queue"="Geometry+1"
        "RenderType"="Transparent"
    }
	ZWrite Off
    Offset 1, 1
    Blend SrcAlpha OneMinusSrcAlpha
	Pass {
 	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag
        #pragma multi_compile _ USE_MASK
		#include "UnityCG.cginc"

		sampler2D _WaterMaskTex;
		fixed4 _Color;
		fixed _WaterLevel, _AlphaOnWater;

		struct appdata {
			float4 vertex : POSITION;
			float2 uv  : TEXCOORD0;
            #if USE_MASK
			    float2 uv2  : TEXCOORD1;
            #endif
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct v2f {
			float4 pos : SV_POSITION;
			float2 uv  : TEXCOORD0;
            #if USE_MASK
			    float2 uv2  : TEXCOORD1;
            #endif
			UNITY_VERTEX_OUTPUT_STEREO
		};
		
		v2f vert(appdata v) {
				v2f o;						
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.pos = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;
            #if USE_MASK
    			o.uv2 = v.uv2;
            #endif
			return o;									
		}
		
		fixed4 frag(v2f i) : SV_Target {
			fixed4 color = _Color;

            #if USE_MASK
			    fixed4 mask = tex2D(_WaterMaskTex, i.uv2);
			    if (mask.a <= _WaterLevel) {
    				color.a *= _AlphaOnWater;
	    		}
            #endif
			return color;
		}
			
	ENDCG
    }
}
}
