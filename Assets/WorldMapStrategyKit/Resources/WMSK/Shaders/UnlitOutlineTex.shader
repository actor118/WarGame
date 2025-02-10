Shader "WMSK/Unlit Outline Textured" {
 
Properties {
	_MainTex ("Texture", 2D) = "white" {}
    _Color ("Color", Color) = (1,1,1,1)
    _AnimationSpeed("Animation Speed", Float) = 0.0
    _AnimationStartTime("Animation Start Time", Float) = 0.0
    _AnimationAcumOffset("Animation Acum Offset", Float) = 0.0
}
 
SubShader {
    Tags {
       "Queue"="Geometry+301"
       "RenderType"="Opaque"
  	}
  	ZWrite Off
/*	Stencil { // Uncomment to avoid outline overlap causing artifacts if color is transparent but this will prevent neighbour outlines to display properly
		Ref 4
		Comp NotEqual
		Pass Replace
		Fail Keep
    }
*/	
  	Blend SrcAlpha OneMinusSrcAlpha
    Pass {
    	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag
		#pragma enable_cbuffer

		#include "UnityCG.cginc"

		sampler2D _MainTex;

		CBUFFER_START(UnityPerMaterial)
		fixed4 _Color;
		float4 _MainTex_ST;
		float _AnimationSpeed, _AnimationStartTime, _AnimationAcumOffset;
		CBUFFER_END

		struct AppData {
			float4 vertex : POSITION;
			float2 uv: TEXCOORD0;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct v2f {
			float4 pos : POSITION;	
			float2 uv: TEXCOORD0;	
			UNITY_VERTEX_OUTPUT_STEREO
		};

		v2f vert(AppData v) {
				v2f o;						
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.pos = UnityObjectToClipPos(v.vertex);
			#if UNITY_REVERSED_Z
				o.pos.z += 0.001;
			#else
				o.pos.z -= 0.001;
			#endif	
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			o.uv.x += _AnimationAcumOffset + _AnimationSpeed * (_Time.y - _AnimationStartTime);
			return o;
		}
		
		fixed4 frag(v2f i) : SV_Target {
			fixed4 pix = tex2D(_MainTex, i.uv);
			return _Color * pix;					
		}
			
		ENDCG
    }

}
}
