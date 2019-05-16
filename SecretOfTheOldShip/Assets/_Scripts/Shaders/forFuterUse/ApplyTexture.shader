Shader "Custom/ApplyTexture"
{
	Properties
	{
		_MainTex(" Main Texture (RGB)", 2D) = "white"{}	//allows for a texture property
		_Color("Color", Color)				= (1,1,1)	//allows for a color property
	}

		SubShader
		{
			Pass
			{
				CGPROGRAM // Allows talk between diffrent lagues: shader lab and nvidia C for graphics

				
				//\============================================================================================
				//\ Function Defines -defines the name for the vertex and fragment shader
				//\============================================================================================
				
				#pragma vertex vert		// Define for the building function (Defined in Vertex Function)
				#pragma fragment frag	// Define for the colering function (Defined in Fragment Function)

				//\============================================================================================
				//\ Includes
				//\============================================================================================
				
				#include "UnityCG.cginc"// Built in shader funktion


				//\============================================================================================
				//\ Structures - Can get data like - vertices, normal, color, uv
				//\============================================================================================

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv     : TEXCOORD0;
				};
				struct v2f
				{
					float4 pos	: SV_POSITION;
					float2 uv	: TEXCOORD0;
				};

				//\============================================================================================
				//\ Imports - RE - import property from shader lab to nvida cg
				//\============================================================================================

				float4		_Color;
				sampler2D	_MainTex;

				//\============================================================================================
				//\ Vertex Function - Builds the object 
				//\============================================================================================
				v2f vert(appdata IN) 
				{
					v2f OUT;

					OUT.pos = UnityObjectToClipPos(IN.vertex);
					OUT.uv	= IN.uv;
					return OUT;
				}

				//\============================================================================================
				//\ Fragment Function - Color it in 
				//\============================================================================================
				
				fixed4 frag(v2f IN):sv_Target
				{
					float4 texColor = tex2D(_MainTex, IN.uv);
					return texColor * _Color;
				}

				ENDCG
			}


		}
}
