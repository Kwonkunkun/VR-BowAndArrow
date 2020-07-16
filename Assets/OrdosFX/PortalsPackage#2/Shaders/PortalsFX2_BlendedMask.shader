Shader "PortalsFX2/MaskBlended"
{
	Properties
	{
		[HDR]_TintColor("Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		[Space]
		[Space]
		[Space]
		[Toggle(USE_CUTOUT)] _UseCutout("Use Cutout", Int) = 0
		_MaskTex ("MaskTexture", 2D) = "white" {}
		_MaskCutout("MaskCutout", Range(0,1)) = .5
		[Space]
		[Space]
		[Space]
		_Thickness("Border Thickness", Range(0,.3)) = .05
		[HDR]_ThicknessColor("TBorder Color", Color) = (1,1,1,1)
		[Space]
		[Space]
		[Space]
		_AlphaPow("Alpha Pow", Float) = 1
		_AlphaMul("Alpha Mul", Float) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature USE_CUTOUT// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			sampler2D _MaskTex;
			float _MaskCutout;
			float4 _MainTex_ST;
			float4 _MaskTex_ST;
			float4 _TintColor;
			float _Thickness;
			float4 _ThicknessColor;
			float _AlphaPow;
			float _AlphaMul;
			
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 maskUV: TEXCOORD1;
				UNITY_FOG_COORDS(2)
				float4 vertex : SV_POSITION;
				float4 color : COLOR0;
			};


			v2f vert (appdata vertexInput)
			{
				v2f pixelInput;
				pixelInput.vertex = UnityObjectToClipPos(vertexInput.vertex);
				pixelInput.uv = TRANSFORM_TEX(vertexInput.uv, _MainTex);
				pixelInput.maskUV = TRANSFORM_TEX(vertexInput.uv, _MaskTex);
				UNITY_TRANSFER_FOG(pixelInput, pixelInput.vertex);
				pixelInput.color = vertexInput.color;
				return pixelInput;
			}
			
			float4 frag (v2f pixelInput) : SV_Target
			{
				// sample the texture
				float mask = 1;
			float3 border = 0;

#ifdef USE_CUTOUT
				_MaskCutout = 1 - _MaskCutout;
				float maskCol = tex2D(_MaskTex, pixelInput.uv).r;
				
				if (maskCol < saturate(_MaskCutout + _Thickness) && maskCol > _MaskCutout)
				{
					border = _ThicknessColor;
				}
				else {
					border = 0;
				}
				
				if (maskCol > _MaskCutout)
				{
					mask = 1;
				}
				else {
					mask = 0;
				}
#endif
				//return float4 (mask,border.r,0,1);

				float4 col = tex2D(_MainTex, pixelInput.uv)*_TintColor*pixelInput.color;
				col.rgb += border;
				col.a = saturate(pow(col.a, _AlphaPow) * _AlphaMul * mask);
				// apply fog
				UNITY_APPLY_FOG(pixelInput.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
