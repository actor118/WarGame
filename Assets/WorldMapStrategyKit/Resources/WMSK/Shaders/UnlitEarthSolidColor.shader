Shader "WMSK/Unlit Earth Solid Color" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
    }
   SubShader {
       Tags { "Queue"="Geometry" "RenderType"="Opaque" }
	   ZClip Off	   
       Pass {
       	CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag	
		#include "UnityCG.cginc"

          fixed4 _Color;


				struct AppData {
					float4 vertex : POSITION;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f {
					float4 pos : SV_POSITION;
					UNITY_VERTEX_OUTPUT_STEREO
				};

			v2f vert(AppData v) {
				v2f o;						
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.pos = UnityObjectToClipPos(v.vertex);
					// Push back
					#if UNITY_REVERSED_Z
						o.pos.z -= 0.0005;
					#else
						o.pos.z += 0.0005;
					#endif
					return o;
				}

				fixed4 frag (v2f i) : SV_Target {
					return _Color;
				}

		ENDCG
        }
    } 
    FallBack Off
}