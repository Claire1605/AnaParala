Shader "Unlit/ShaderTest01ExtrudeWidth"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	_Colour("Color", Color) = (1,1,1,1)
	_Width("Width",Range(0,8)) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "DisableBatching"="True"}
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
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			half4 _Colour;
			float _Width;
			
			v2f vert (appdata v)
			{
				v2f o;
				//Set the X position value to match the input mesh then multiply the x value of each vertex by the width variable thus increasing the width 
				//(this is done in mesh local space before the conversion).
				v.vertex.x = v.vertex.x * _Width;
				//basically convert from local to screen space but I'm not sure exactly what this function does other than that.
				o.vertex = UnityObjectToClipPos(v.vertex);
				//just get the texture from the UVs, no manipulation.
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//built in unity method for fog
				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
