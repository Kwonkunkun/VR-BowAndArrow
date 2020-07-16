Shader "JTS/CustomSword" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)

		_MainTex ("Diffuse", 2D) = "white" {}
		_NormTex ("Normal", 2D) = "white" {}
		_MaskTex ("Mask", 2D) = "white" {}

		_ClothHighlight ("Cloth Highlight", Color) = (1,1,1,1)
		_Cloth ("Cloth", Range(0,1)) = 0.0
		_Silk ("Cloth Silk", Range(0,1)) = 0.0
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#include "AutoLight.cginc"
		#include "UnityPBSLighting.cginc"
		#pragma surface surf Standard vertex:vert fullforwardshadows
		#pragma target 3.0
		#pragma glsl

		sampler2D _MainTex,_MaskTex,_NormTex;
		half _Cloth,_Silk;
		fixed4 _Color,_ClothHighlight;


		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
		    float3 lightDir;
		};


		void vert (inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
		    o.lightDir = WorldSpaceLightDir(v.vertex); 
		}


		void surf (Input IN, inout SurfaceOutputStandard o) {

			//---------------------
			//##  GET TEXTURES  ##
			//---------------------
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 m = tex2D (_MaskTex, IN.uv_MainTex);
			fixed4 n = tex2D (_NormTex, IN.uv_MainTex);

			//------------------------
			//##  SET BASE DIFFUSE  ##
			//------------------------
			o.Albedo = lerp(c.rgb, c.rgb * _Color.rgb, _Color.a);

			//------------------------
			//## CALCULATE NORMAL  ##
			//------------------------
			//note: texture must be set to 'texture' and also 'bypass rgb sampling'
			//o.Normal = n.rgb * 2.0 - 1.0;
			o.Normal = UnpackNormal(n);

			//-------------------------
			//##  SET PBR SETTINGS  ##
			//-------------------------
			o.Metallic = m.r;
			o.Smoothness = m.g;
			o.Alpha = c.a;


			//-------------------------------
			//##  LIGHT TERM CALCULATION  ##
			//-------------------------------
			half3 useNormal = o.Normal;//nS.rgb * 2.0 - 1.0;
		    half NdotV = max(0,dot(o.Normal,IN.viewDir));

			//------------------------------------
			//##  FRESNEL CALULATION (Schlick)  ##
			//------------------------------------
			half3 fresnel;
			half3 f0 = half3(0,0,0);
			fresnel = f0+(1.0-f0)*pow((dot(o.Normal,normalize(IN.lightDir+IN.viewDir))),5);
			fresnel = fresnel * (f0+(1.0-f0)*pow((1.0-NdotV),5));
			fresnel = saturate(max(fresnel,f0+(1.0-f0)*pow((1.0-NdotV),5)));
			half cSmooth = saturate(lerp(1.0, 0.0-_Cloth*2, NdotV)) * m.b;
			half cSmooth2 = saturate(lerp(2.0, 0.0-_Cloth, NdotV)) * m.b * _Silk;

			//set cloth shading
			o.Albedo = lerp(o.Albedo, 0.0, cSmooth2 - cSmooth * _ClothHighlight.a);
			o.Albedo = lerp(o.Albedo, _ClothHighlight.rgb, cSmooth * _ClothHighlight.a);

		}
		ENDCG
	} 
	FallBack "Diffuse"
}
