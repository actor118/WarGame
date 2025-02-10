Shader "WMSK/Unlit Single Color Cursor" {
 
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
    _Color ("Color", Color) = (1,1,1)
    _Orientation ("Orientation", Float) = 0 // 0 = horizontal, 1 = vertical
}
 
SubShader {
    Color [_Color]
        Tags {
        "Queue"="Transparent"
        "RenderType"="Transparent"
    }
    ZWrite Off
    Pass {
    	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag				
		
		#include "UnityCG.cginc"
		
		fixed4 _Color;
		float _Orientation;

		struct AppData {
			float4 vertex : POSITION;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct v2f {
			float4 pos : POSITION;	
			float4 scrPos : TEXCOORD0;
			UNITY_VERTEX_OUTPUT_STEREO
		};


		v2f vert(AppData v) {
				v2f o;						
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.pos = UnityObjectToClipPos(v.vertex);
			o.scrPos = ComputeScreenPos(o.pos);
			return o;
		}
		
		fixed4 frag(v2f i) : SV_Target {
			float2 wcoord = (i.scrPos.xy/i.scrPos.w);
			wcoord.x *= _ScreenParams.x;
			wcoord.y *= _ScreenParams.y;
			float wc = _Orientation==0 ? wcoord.x: wcoord.y;
			if ( fmod((int)(wc/4),2) )
				discard;
			return _Color;					
		}
			
		ENDCG
    }

}
 
}
