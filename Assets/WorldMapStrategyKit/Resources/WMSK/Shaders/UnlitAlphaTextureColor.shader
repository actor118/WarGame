// Used by cities material

Shader "WMSK/Unlit Alpha Texture Color" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white"
    }

   	SubShader {
       Tags {
       	"Queue"="Geometry+302" // Draw over highlight
//       	"Queue"="Transparent" // Draw over highlight
       }

 	ZWrite Off
 	Blend SrcAlpha OneMinusSrcAlpha
    Pass {
    	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag				
		#include "UnityCG.cginc"

		fixed4 _Color;
		sampler2D _MainTex;

		struct AppData {
			float4 vertex : POSITION;
			float2 texcoord: TEXCOORD0;
            UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct v2f {
			float4 pos : SV_POSITION;
			float2 texcoord: TEXCOORD0;
			UNITY_VERTEX_OUTPUT_STEREO
		};


		v2f vert(AppData v) {
			v2f o;						
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.pos = UnityObjectToClipPos(v.vertex);	

			#if UNITY_REVERSED_Z
				o.pos.z += 0.0005;
			#else
				o.pos.z -= 0.0005;
			#endif

			o.texcoord = v.texcoord;

			return o;
		}
		
		fixed4 frag(v2f i) : SV_Target {
			return tex2D(_MainTex, i.texcoord) * _Color;					
		}
			
		ENDCG
    }
    }
    
}