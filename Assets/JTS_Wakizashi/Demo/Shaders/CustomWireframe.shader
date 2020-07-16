// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "JTS/Wireframe" 
{
	Properties 
	{
		_LineColor ("Line Color", Color) = (0,0,0,1)
		_BackTint ("BackTint", Color) = (0.5,0.5,0.5,0.5)
		_Thickness ("Line Thickness", Float) = 1
	}

	SubShader 
	{
		Pass
		{
			Tags { "RenderType"="Transparent" "Queue"="Overlay" }

			Blend SrcAlpha OneMinusSrcAlpha 
			ZWrite Off

			CGPROGRAM
			#pragma target 5.0
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma geometry geom

			float showWireframe = 1.0;
			float _Thickness = 1;
			float4 _LineColor = {1,1,1,1};
			float4 _BackTint = {0.6,0.6,0.6,0.25};

			struct v2f{
				float4	pos	: POSITION;
				float2  uv	: TEXCOORD0;
			};

			struct g2f{
				float4	pos	: POSITION;
				float2	uv	: TEXCOORD0;
				float3  dist: TEXCOORD1;
			};

			v2f vert(appdata_base v){
				v2f output;
				output.pos =  UnityObjectToClipPos(v.vertex);
				return output;
			}

			[maxvertexcount(3)]
			void geom(triangle v2f p[3], inout TriangleStream<g2f> tStream){
				float2 p0 = _ScreenParams.xy * p[0].pos.xy / p[0].pos.w;
				float2 p1 = _ScreenParams.xy * p[1].pos.xy / p[1].pos.w;
				float2 p2 = _ScreenParams.xy * p[2].pos.xy / p[2].pos.w;
				
				//edge vectors
				float2 v0 = p2 - p1;
				float2 v1 = p2 - p0;
				float2 v2 = p1 - p0;

				//area of the triangle
			 	float area = abs(v1.x*v2.y - v1.y * v2.x);

				g2f pt;
				
				//point 1
				pt.pos = p[0].pos;
				pt.uv = p[0].uv;
				pt.dist = float3(area/length(v0), 0, 0);
				tStream.Append(pt);

				//point 2
				pt.pos =  p[1].pos;
				pt.uv = p[1].uv;
				pt.dist = float3(0, area/length(v1), 0);
				tStream.Append(pt);
				
				//point 3
				pt.pos = p[2].pos;
				pt.uv = p[2].uv;
				pt.dist = float3(0, 0, area/length(v2));
				tStream.Append(pt);
			}


			float4 frag(g2f input) : COLOR{			

				//calculate lines
				float val = min( input.dist.x, min( input.dist.y, input.dist.z));
				val = exp2( -1/_Thickness * val * val );

				//calculate final color
				float4 rVal = lerp(_BackTint, (val * _LineColor), val * _LineColor.a);
				rVal.a = lerp(0.0, rVal.a, showWireframe);

				return rVal;
			}




			ENDCG
		}
	} 
}
