﻿Shader "Custom/CountShader" {
	Properties {
		_MainTex ("Font Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
	}

	SubShader {

		Tags {
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
			"PreviewType"="Plane"
		}
		Lighting Off Cull Off ZTest Always ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform fixed4 _Color;


			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;// * _Color;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				return o;
			}



			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.texcoord).rgba;// i.color;
				col.rgb = _Color;//fixed3(1,0,0);
				//col.a *= tex2D(_MainTex, i.texcoord).a;
				return col;
			}
			ENDCG
		}
	}
}
