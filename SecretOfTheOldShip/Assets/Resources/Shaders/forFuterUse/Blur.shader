Shader "Custom/Blur"
{


	//\============================================================================================
	//\ Shader that makes a object transparet, and blurs everything behind it
	//\ somthat the effect of frosted glas.
	//\============================================================================================


	Properties
	{
		_BlurRadius("Blur Raidus ",Range(0.0,20.0))		  = 1.0
		_BlurIntensity("Blur Intensity ",Range(0.0, 1.0)) = 0.01

	}




	SubShader
	{

		Tags
		{
			"Queue" = "Transparent"
		}

		GrabPass{} // gets the object and everything behinds it and sends it dow to this pas



		Pass
		{
			Name "HORIZONTALBLUR"


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
			struct v2f
			{
				float4 vertex	: SV_POSITION;
				float4 uvgrab	: TEXCOORD0;
			};


		//\============================================================================================
		//\ Imports - RE - import property from shader lab to nvida cg
		//\============================================================================================
			float		_BlurRadius;
			float		_BlurIntensity;
			sampler2D	_GrabTexture;
			float4		_GrabTexture_TexelSize;

		//\============================================================================================
		//\ Vertex Function - Builds the object 
		//\============================================================================================
			v2f vert(appdata_base IN)
			{
				v2f OUT;

				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				#if UNITY_UV_STARTS_AT_TOP
					float scale = -1;
				#else
					float scale = 1.0;
				#endif

				OUT.uvgrab.xy = (float2(OUT.vertex.x, OUT.vertex.y * scale) + OUT.vertex.w)*0.5; // makes sure to onlu grabe thats behindf the object and not the hole screen
				OUT.uvgrab.zw = OUT.vertex.zw;

				return OUT;
			}


		//\============================================================================================
		//\ Fragment Function - Color it in 
		//\============================================================================================
			half4 frag(v2f IN) : COLOR
			{
				half4 texcol = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(IN.uvgrab)); // grabb everyting behind and put it in teccol
				half4 texsum = half4(0, 0, 0, 0);

				#define GRABPIXEL(weight, kernelx) tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(IN.uvgrab.x + _GrabTexture_TexelSize.x * kernelx * _BlurRadius, IN.uvgrab.y, IN.uvgrab.z, IN.uvgrab.w)))*weight

				texsum += GRABPIXEL(0.05, -4.0);
				texsum += GRABPIXEL(0.09, -3.0);
				texsum += GRABPIXEL(0.12, -2.0);
				texsum += GRABPIXEL(0.15, -1.0);
				texsum += GRABPIXEL(0.18, -0.0);
				texsum += GRABPIXEL(0.15,  1.0);
				texsum += GRABPIXEL(0.12,  2.0);
				texsum += GRABPIXEL(0.09,  3.0);
				texsum += GRABPIXEL(0.05,  4.0);

				texcol = lerp(texcol, texsum, _BlurIntensity);
				return texcol;
			}

				ENDCG
		}

			GrabPass{} 

			Pass
			{

				NAME "VERTICALBLUR"
				CGPROGRAM 


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
				struct v2f
				{
					float4 vertex	: SV_POSITION;
					float4 uvgrab	: TEXCOORD0;
				};

			//\============================================================================================
			//\ Imports - RE - import property from shader lab to nvida cg
			//\============================================================================================
				float		_BlurRadius;
				float		_BlurIntensity;
				sampler2D	_GrabTexture;
				float4		_GrabTexture_TexelSize;

			//\============================================================================================
			//\ Vertex Function 
			//\============================================================================================
				v2f vert(appdata_base IN)
				{
					v2f OUT;

					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					#if UNITY_UV_STARTS_AT_TOP
						float scale = -1;
					#else
						float scale = 1.0;
					#endif

					OUT.uvgrab.xy = (float2(OUT.vertex.x, OUT.vertex.y * scale) + OUT.vertex.w)*0.5; // makes sure to onlu grabe thats behindf the object and not the hole screen
					OUT.uvgrab.zw = OUT.vertex.zw;

					return OUT;
				}

			//\============================================================================================
			//\ Fragment Function 
			//\============================================================================================
				half4 frag(v2f IN) : COLOR
				{
					half4 texcol = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(IN.uvgrab)); // grabb everyting behind and put it in teccol
					half4 texsum = half4(0, 0, 0, 0);
					
					#define GRABPIXEL(weight, kernely) tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(IN.uvgrab.x, IN.uvgrab.y + _GrabTexture_TexelSize.y * kernely * _BlurRadius, IN.uvgrab.z, IN.uvgrab.w)))*weight

					texsum += GRABPIXEL(0.05, -4.0);
					texsum += GRABPIXEL(0.09, -3.0);
					texsum += GRABPIXEL(0.12, -2.0);
					texsum += GRABPIXEL(0.15, -1.0);
					texsum += GRABPIXEL(0.18, -0.0);
					texsum += GRABPIXEL(0.15,  1.0);
					texsum += GRABPIXEL(0.12,  2.0);
					texsum += GRABPIXEL(0.09,  3.0);
					texsum += GRABPIXEL(0.05,  4.0);


					texcol = lerp(texcol, texsum, _BlurIntensity);
					return texcol;
				}

				ENDCG
			}

	}
}
