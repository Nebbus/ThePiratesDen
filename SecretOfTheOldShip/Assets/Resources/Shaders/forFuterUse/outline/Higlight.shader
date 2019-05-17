Shader "Custom/Higlight"
{

	Properties
	{
		_MainTex("Main Texture (RGB)", 2D)	= "white"{}
		_Color("Color", Color)				= (1,1,1,1)
	_Space1("", Range(0,0)) = 0
		_DistrotTex("   Higlight Distor Texture (RGB)", 2D)	 = "whit"{}
		_BumpMap("   Higlight Normal Map", 2D)				 = "bump" {}
		_DistortColor("   Higlight Color ", Color)			 = (1, 1, 1, 1)	
	_Space2("", Range(0,0)) = 0
		_OutlineWidth("   Higlight Edge width ",Range(1.1,10.0))		= 1.1
		_BlurRadius("   Higlight Blur Raidus ",Range(0.0,20.0))			= 1.0
		_BlurIntensity("   Higlight Blur Intensity ",Range(0.0, 1.0))	= 0.01
		_BumpAmt("   Higlight Distortion", Range(0,128))		        = 10
		_Glow("   Higlight Color Intensity", Range(0,10))				= 1
		




	}

	SubShader
	{

		Tags
		{
			"Queue" = "Transparent"
		}


		GrabPass{}
	    UsePass "Custom/OutlineDistort/OUTLINEDISTORT"
		GrabPass{}
		UsePass "Custom/OutlineBlur/OUTLINEHORIZONTALBLUR"
		GrabPass{}
		UsePass "Custom/OutlineBlur/OUTLINEVERTICALBLUR"

		UsePass "Custom/Outline/OBJECT"
	}
}
