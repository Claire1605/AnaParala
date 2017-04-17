Shader "Custom/Line"
{
	Properties
	{
		_Colour ("Colour", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Width ("Width", Range(0,1)) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque"  "DisableBatching" = "True" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				//UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
				float3 normal : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			half4 _Colour;
			float _Width;
			
			v2f vert (appdata v)
			{
				v2f o;
				v.vertex.x = v.vertex.x *_Width;
				o.vertex = UnityObjectToClipPos(v.vertex); 
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = o.uv.x *  0.5 + 0.5;

				//UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv) * _Colour;
				//UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}

		Fallback "Standard"
}
