Shader "Custom/Line"
{
	Properties
	{
		_Color ("Colour", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Width ("Width", Range(0,1)) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Transparent"  "DisableBatching" = "True" }
		LOD 100
			Blend One OneMinusSrcAlpha
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
				float4 color    : COLOR;
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
			half4 _Color;
			float _Width;
			
			v2f vert (appdata v)
			{
				v2f o;
				v.vertex.x = v.vertex.x *_Width;
				o.vertex = UnityObjectToClipPos(v.vertex); 
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color =  v.color * _Color;
				
				//UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv) * _Color;
				col.rgb *= col.a;
				//UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}

		Fallback "Standard"
}
