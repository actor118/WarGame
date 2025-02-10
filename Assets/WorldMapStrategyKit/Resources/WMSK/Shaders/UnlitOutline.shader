Shader "WMSK/Unlit Outline" {
 
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
    _Color ("Color", Color) = (1,1,1,1)
}
 
SubShader {
    Tags {
       "Queue"="Geometry+301"
       "RenderType"="Opaque"
  	}
  	ZWrite Off
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
			#if UNITY_REVERSED_Z
			o.pos.z += 0.001;
			#else
			o.pos.z -= 0.001;
			#endif
			return o;
		}
		
		fixed4 frag(v2f i) : SV_Target {
				return _Color;					
		}
			
		ENDCG
    }
    
   // SECOND STROKE ***********
 
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
			float4 pos : POSITION;	
			float4 rpos: TEXCOORD0;	
			UNITY_VERTEX_OUTPUT_STEREO
		};
		
		v2f vert(AppData v) {
				v2f o;						
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.pos = UnityObjectToClipPos(v.vertex);
				o.pos.x += 2 * (o.pos.w/_ScreenParams.x);
				#if UNITY_REVERSED_Z
					o.pos.z += 0.001;
				#else
					o.pos.z -= 0.001;
				#endif

				o.rpos = o.pos;
			return o;									
		}
		
		fixed4 frag(v2f i) : SV_Target {
			return _Color;
		}
			
		ENDCG
    }
    
      // THIRD STROKE ***********
 
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
			float4 pos : POSITION;	
			float4 rpos: TEXCOORD0;	
			UNITY_VERTEX_OUTPUT_STEREO
		};
		
		v2f vert(AppData v) {
				v2f o;						
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.pos = UnityObjectToClipPos(v.vertex);
			o.pos.y += 2 * (o.pos.w/_ScreenParams.y);
			#if UNITY_REVERSED_Z
			o.pos.z += 0.001;
			#else
			o.pos.z -= 0.001;
			#endif
			o.rpos = o.pos;
			return o;									
		}
		
		fixed4 frag(v2f i) : SV_Target {
			return _Color;
		}
			
		ENDCG
    }
    
       
      // FOURTH STROKE ***********
 
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
			float4 pos : POSITION;	
			float4 rpos: TEXCOORD0;	
			UNITY_VERTEX_OUTPUT_STEREO
		};
		
		v2f vert(AppData v) {
				v2f o;						
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.pos = UnityObjectToClipPos(v.vertex);
			o.pos.x -= 2 * (o.pos.w/_ScreenParams.x);
			#if UNITY_REVERSED_Z
			o.pos.z += 0.001;
			#else
			o.pos.z -= 0.001;
			#endif
			o.rpos = o.pos;
			return o;									
		}
		
		fixed4 frag(v2f i) : SV_Target {
			return _Color;
		}
			
		ENDCG
    }
    
    // FIFTH STROKE ***********
 
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
			float4 pos : POSITION;	
			float4 rpos: TEXCOORD0;	
			UNITY_VERTEX_OUTPUT_STEREO
		};
		
		v2f vert(AppData v) {
				v2f o;						
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.pos = UnityObjectToClipPos(v.vertex);
			o.pos.y -= 2 * (o.pos.w/_ScreenParams.y);
			#if UNITY_REVERSED_Z
			o.pos.z += 0.001;
			#else
			o.pos.z -= 0.001;
			#endif
			o.rpos = o.pos;
			return o;									
		}
		
		fixed4 frag(v2f i) : SV_Target {
				return _Color;
		}
			
		ENDCG
    }
    
}
}
