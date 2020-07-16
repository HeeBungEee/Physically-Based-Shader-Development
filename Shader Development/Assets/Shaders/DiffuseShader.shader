Shader "Custom/DiffuseColor"
{
	Properties
	{
		_Color("Color", Color) = (1, 0, 0, 1)
		_DiffuseTex("Texture", 2D) = "white" {}
		_Ambient("Ambient", Range(0, 1)) = 0.25
	}

		SubShader
	{
		Tags { "LightMode" = "ForwardBase" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"

			float4 _Color;
			float _Ambient;
			sampler2D _DiffuseTex;
			float4 _DiffuseTex_ST;

			struct appdata
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				float3 worldNormal = UnityObjectToWorldNormal(v.normal); // 월드 노멀 벡터 계산
				o.worldNormal = worldNormal; // 출력 데이터 구조체에 할당
				o.uv = TRANSFORM_TEX(v.uv, _DiffuseTex);
				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				float3 normalDirection = normalize(i.worldNormal);

				float4 tex = tex2D(_DiffuseTex, i.uv);

				float nl = max(_Ambient, dot(normalDirection, _WorldSpaceLightPos0.xyz));
				float4 diffuseTerm = nl * _Color * tex * _LightColor0;

				return diffuseTerm;
			}
			ENDCG
		}
	}
}