Shader "Unlit/StretchWidthAroundPointInMeshLocalYSpace"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Colour("Tint Colour", Color) = (1,1,1,1)

		_Width("Width",Range(0,8)) = 1
		_Position("Position", Range(-20,20)) = 0
		_Distance("FX Distance", Range(0,10)) = 0
	}
		SubShader
		{
			Tags{ "RenderType" = "Opaque"}
			LOD 100

			Pass
			{
				CGPROGRAM
				//declare this is a vertex and fragment shader by defining the names
				//of the vertex and fragment functions.
				#pragma vertex VertexFunction
				#pragma fragment FragmentFunction

				#include "UnityCG.cginc"
				//declare the 2 structs we want to pass in and out of the the vertex and fragment functions
				struct VertexDataWeWant
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
					//use the semantics NORMAL and TANGENT to 
					//bring through that information from the mesh with whatever name.
					//Note, this shader isn't actually using this information anywhere.
					float3 normalDirection : NORMAL;
					float3 tangentDirection : TANGENT;
				};

				struct UvAndOutputVertexData
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				half4 _Colour;
				half _Width;
				half _Position;
				half _Distance;

				UvAndOutputVertexData VertexFunction(VertexDataWeWant vertexInformation)
				{
					//declare the output struct
					UvAndOutputVertexData o;
					//if statement identifies a range of verticies on the mesh in local Y space by use the position & distance variables
					if (vertexInformation.vertex.y > _Position - _Distance && vertexInformation.vertex.y < _Position + _Distance)
					{
						//only make these verticies wider, I'm not sure if you want to do something with the normal or tangent direction here
						//because inside VertexDataWeWant I declared variables using the NORMAL and TANGENT semantics you could access those here if you want.
						vertexInformation.vertex.x = vertexInformation.vertex.x * _Width;
					}
					//Convert from local to screen space and maybe do some other built in unity stuff.
					o.vertex = UnityObjectToClipPos(vertexInformation.vertex);

					//just get the texture from the UVs, no manipulation.
					o.uv = TRANSFORM_TEX(vertexInformation.uv, _MainTex);

					//return the output, this is automatically passed into the fragment function because we've declared this as the vertex function.
					return o;
				}

				//Sets the final colour for each fragment (essentially each pixel on the mesh)
				fixed4 FragmentFunction(UvAndOutputVertexData i) : SV_Target
				{
					// sample the texture at the input UV position (i.uv) using the tex2D function
					fixed4 col = tex2D(_MainTex, i.uv);
					//multiply the texture colour by the tint colour to get the final output.
					return col *_Colour.rgba;
				}
			ENDCG
		}
	}
}
