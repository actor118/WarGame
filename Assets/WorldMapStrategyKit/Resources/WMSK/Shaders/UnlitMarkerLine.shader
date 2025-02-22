﻿Shader "WMSK/Unlit Marker Line" {
 
Properties {
    _Color ("Color", Color) = (1,1,1,0.5)
    _MainTex("Texture (RGBA)", 2D) = "white" {}
    _StencilComp("Stencil Comp", Int) = 8
	_ZOffset("Z Offset", Float) = 1
}
 
SubShader {
         Tags {"Queue"="Geometry+301" "IgnoreProjector"="True" "RenderType"="Transparent"} 
         ZWrite Off
         Blend SrcAlpha OneMinusSrcAlpha 
         ColorMask RGB
         Stencil {
			Ref 1
			ReadMask 1
			Comp [_StencilComp]
			Pass replace
         }
	Pass {
    	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag
		#pragma multi_compile_local _ _VIEWPORT_CROP

		#include "UnityCG.cginc"

		fixed4 _Color;
		sampler2D _MainTex;
		float4 _MainTex_ST;
		float4x4 _ViewportInvProj;
		float _ZOffset;

		struct AppData {
			float4 vertex : POSITION;
			float2 uv: TEXCOORD0;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};
		
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv: TEXCOORD0;
			#if _VIEWPORT_CROP
				float3 wpos: TEXCOORD1;
			#endif
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f vert(AppData v) {
				v2f o;						
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.pos = UnityObjectToClipPos(v.vertex);	
				o.uv = v.uv;
				#if _VIEWPORT_CROP
					o.wpos = mul(unity_ObjectToWorld, v.vertex);
				#endif
				#if UNITY_REVERSED_Z
					o.pos.z+= 0.00001 * _ZOffset;
				#else
					o.pos.z-=0.00001 * _ZOffset;
				#endif				
				return o;								
			}
		
		fixed4 frag(v2f i) : SV_Target {
				#if _VIEWPORT_CROP
					float3 localPos = mul(_ViewportInvProj, float4(i.wpos, 1.0)).xyz;
					if (localPos.x < -0.5 || localPos.x > 0.5 || localPos.y < - 0.5 || localPos.y > 0.5 || localPos.z > 0) discard;
				#endif
				fixed4 pix = tex2D(_MainTex, i.uv);
				return _Color * pix;					
		}
			
		ENDCG
    }
 }

}
 