Shader "WMSK/Unlit Country Frontiers Order 3" {
 
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
    _Color ("Color", Color) = (0,1,0,1)
    _OuterColor("Outer Color", Color) = (0,0.8,0,0.8)
}
 
SubShader {
	LOD 300
    Tags {
        "Queue"="Geometry+300"
        "RenderType"="Opaque"
    	}
 	Blend SrcAlpha OneMinusSrcAlpha


	CGINCLUDE
		#include "UnityCG.cginc"

		struct AppData {
			float4 vertex : POSITION;
            UNITY_VERTEX_INPUT_INSTANCE_ID
		};
		
		struct v2f {
			float4 pos : SV_POSITION;
			UNITY_VERTEX_INPUT_INSTANCE_ID
			UNITY_VERTEX_OUTPUT_STEREO
		};

		fixed4 _OuterColor, _Color;

	ENDCG

    Pass {
	   	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag				

		v2f vert(AppData v) {
			v2f o;						
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.pos = UnityObjectToClipPos(v.vertex);	

			#if UNITY_REVERSED_Z
				o.pos.z+= 0.00001;
			#else
				o.pos.z-=0.00001;
			#endif

			return o;								
		}
		
		fixed4 frag(v2f i) : SV_Target {
			return _Color;					
		}
		ENDCG
		
    }
}

SubShader {
	LOD 200
    Tags {
        "Queue"="Geometry+300"
        "RenderType"="Opaque"
    	}
    Blend SrcAlpha OneMinusSrcAlpha

    Pass { // (+1,0)
	   	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag				
		
		v2f vert(AppData v) {
			v2f o;						
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.pos = UnityObjectToClipPos(v.vertex);
			o.pos.x += 1.25 * (o.pos.w/_ScreenParams.x);
			return o;
		}
		
		fixed4 frag(v2f i) : SV_Target {
			return _Color * 0.5 + _OuterColor * 0.5;					
		}
		ENDCG
		
    }
   Pass { // (-1,0)
	   	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag				

		v2f vert(AppData v) {
			v2f o;						
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

			o.pos = UnityObjectToClipPos(v.vertex);
			o.pos.x -= 1.25 * (o.pos.w/_ScreenParams.x);
			return o;
		}
		
		fixed4 frag(v2f i) : SV_Target {
			return _Color * 0.5 + _OuterColor * 0.5;					
		}
		ENDCG
		
    }    
  Pass { // (0,-1)
	   	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag				

		v2f vert(AppData v) {
			v2f o;						
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.pos = UnityObjectToClipPos(v.vertex);
			o.pos.y -= 1.25 * (o.pos.w/_ScreenParams.y);
			return o;
		}
		
		fixed4 frag(v2f i) : SV_Target {
			return _Color * 0.5 + _OuterColor * 0.5;					
		}
		ENDCG
    } 
           
    Pass { // (0, +1)
	   	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag				
		
		v2f vert(AppData v) {
			v2f o;						
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.pos = UnityObjectToClipPos(v.vertex);
			o.pos.y += 1.25 * (o.pos.w/_ScreenParams.y);
			return o;
		}
		
		fixed4 frag(v2f i) : SV_Target {
			return _Color * 0.5 + _OuterColor * 0.5;					
		}
		ENDCG
    }    
    
     Pass {
	   	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag				

		v2f vert(AppData v) {
			v2f o;						
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

			o.pos = UnityObjectToClipPos(v.vertex);
			#if UNITY_REVERSED_Z
				o.pos.z += 0.00001;
			#else
				o.pos.z -= 0.00001;
			#endif
			return o;
		}
		
		fixed4 frag(v2f i) : SV_Target {
			return _Color;					
		}
		ENDCG
		
    }  
}

SubShader {
	LOD 100
    Tags {
        "Queue"="Geometry+300"
        "RenderType"="Opaque"
    	}
    Blend SrcAlpha OneMinusSrcAlpha

	// right wing ---------------------------------
  
     Pass { // (+3,0)
	   	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag				
		
		v2f vert(AppData v) {
			v2f o;						
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.pos = UnityObjectToClipPos(v.vertex);
			o.pos.x += 4.25 * (o.pos.w/_ScreenParams.x);
			return o;
		}
		
		fixed4 frag(v2f i) : SV_Target {
			return _OuterColor;					
		}
		ENDCG
    }
            
    Pass { // (+2,0)
	   	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag				

		v2f vert(AppData v) {
			v2f o;						
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

			o.pos = UnityObjectToClipPos(v.vertex);
			o.pos.x += 2.75 * (o.pos.w/_ScreenParams.x);
			return o;
		}
		
		fixed4 frag(v2f i) : SV_Target {
			return _OuterColor;					
		}
		ENDCG
    }
    
    // left wing ---------------------------------
    
 
     Pass { // (-3,0)
	   	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag				

		v2f vert(AppData v) {
			v2f o;						
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

			o.pos = UnityObjectToClipPos(v.vertex);
			o.pos.x -= 4.25 * (o.pos.w/_ScreenParams.x);
			return o;
		}
		
		fixed4 frag(v2f i) : SV_Target {
			return _OuterColor;					
		}
		ENDCG
    }
    
    
    Pass { // (-2,0)
	   	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag				
		
		v2f vert(AppData v) {
			v2f o;						
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

			o.pos = UnityObjectToClipPos(v.vertex);
			o.pos.x -= 2.75 * (o.pos.w/_ScreenParams.x);
			return o;
		}
		
		fixed4 frag(v2f i) : SV_Target {
			return _OuterColor;					
		}
		ENDCG
		
    }     
    
    
    // top wing ---------------------------------------

                           
	Pass { // (0,+3)
	   	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag				
		
		v2f vert(AppData v) {
			v2f o;						
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

			o.pos = UnityObjectToClipPos(v.vertex);
			o.pos.y += 4.25 * (o.pos.w/_ScreenParams.y);
			return o;
		}
		
		fixed4 frag(v2f i) : SV_Target {
			return _OuterColor;					
		}
		ENDCG
    } 
                                                      
    Pass { // (0,+2)
	   	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag				
		
		v2f vert(AppData v) {
			v2f o;						
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

			o.pos = UnityObjectToClipPos(v.vertex);
			o.pos.y += 2.75 * (o.pos.w/_ScreenParams.y);
			return o;
		}
		
		fixed4 frag(v2f i) : SV_Target {
			return _OuterColor;					
		}
		ENDCG
		
    } 
    
    
    // bottom wing ---------------------------------------
                            
	Pass { // (0,-3)
	   	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag				
		
		v2f vert(AppData v) {
			v2f o;						
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.pos = UnityObjectToClipPos(v.vertex);
			o.pos.y -= 4.25 * (o.pos.w/_ScreenParams.y);
			return o;
		}
		
		fixed4 frag(v2f i) : SV_Target {
			return _OuterColor;					
		}
		ENDCG
    } 
        
  Pass { // (0,-2)
	   	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag				
		
		v2f vert(AppData v) {
			v2f o;						
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.pos = UnityObjectToClipPos(v.vertex);
			o.pos.y -= 2.75 * (o.pos.w/_ScreenParams.y);
			return o;
		}
		
		fixed4 frag(v2f i) : SV_Target {
			return _OuterColor;					
		}
		ENDCG
    }            
       
      // central kernel ---------------------------------------
        
    Pass { // (+1,0)
	   	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag				
		
		v2f vert(AppData v) {
			v2f o;						
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.pos = UnityObjectToClipPos(v.vertex);
			o.pos.x += 1.25 * (o.pos.w/_ScreenParams.x);
			return o;
		}
		
		fixed4 frag(v2f i) : SV_Target {
			return _Color;					
		}
		ENDCG
		
    }
    
    
   Pass { // (-1,0)
	   	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag				
		
		v2f vert(AppData v) {
			v2f o;						
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.pos = UnityObjectToClipPos(v.vertex);
			o.pos.x -= 1.25 * (o.pos.w/_ScreenParams.x);
			return o;
		}
		
		fixed4 frag(v2f i) : SV_Target {
			return _Color;			
		}
		ENDCG
		
    }   
       
  Pass { // (0,-1)
	   	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag				
		
		v2f vert(AppData v) {
			v2f o;						
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.pos = UnityObjectToClipPos(v.vertex);
 			o.pos.y -= 1.25 * (o.pos.w/_ScreenParams.y);
			 return o;
		}
		
		fixed4 frag(v2f i) : SV_Target {
			return _Color;					
		}
		ENDCG
    } 
 
                                
     Pass { // (0,+1)
	   	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag				
		
		v2f vert(AppData v) {
			v2f o;						
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.pos = UnityObjectToClipPos(v.vertex);
			o.pos.y += 1.25 * (o.pos.w/_ScreenParams.y);		
			return o;
		}
		
		fixed4 frag(v2f i) : SV_Target {
			return _Color;					
		}
		ENDCG
    }    

    
    Pass {
	   	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag				
		
		v2f vert(AppData v) {
			v2f o;						
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.pos = UnityObjectToClipPos(v.vertex);
			#if UNITY_REVERSED_Z
				o.pos.z+= 0.00001;
			#else
				o.pos.z-=0.00001; 
			#endif
			return o;
		}
		
		fixed4 frag(v2f i) : SV_Target {
			return _Color;					
		}
		ENDCG
		
    }  
    
}
 
}
