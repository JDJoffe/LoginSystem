Shader "Animated/AnimatedTexture"
{
    Properties
    {
	// use this to change transparency
		_Color("Tex Color",Color) = (1,1,1,1)
		// maintex
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		// maintex scroll speed
		_Speed ("Speed", Range(0, 10)) = 1
		// maintex scroll direction,direction is setup like a circle where 1 and 0 move the texture in the same direction, values inbetween move in different directions.
		_Direction ("Direction", Range(0, 1)) = 0
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
		// big shiny
        _Metallic ("Metallic", Range(0,1)) = 0.0
		// height map
        _ParallaxMap ("Height Map", 2D) = "black" {}
		// height map speed
		_ParallaxSpeed ("Parallax Speed", Range(0, 10)) = 1
		// height map direcition, 
		_ParallaxDirection ("Parallax Direction", Range(0, 1)) = 0
		_ParallaxStrength ("Parallax Strength", Range(0, 1)) = 1
		//_Cutoff("CutOff",Range(0,1)) = 0
		
    }
    SubShader
    {
	// render transparency
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		ZWrite Off
		
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
		// input uv
            float2 uv_MainTex;
			float2 uv_ParallaxMap;
        };

		// floats for all properties
        half _Glossiness;
        half _Metallic;
		float _Speed;
		float _Direction;
		float _Parallax;
		sampler2D _ParallaxMap;
		float _ParallaxSpeed;
		float _ParallaxDirection;
		float _ParallaxStrength;
		float4 _Color;
	//	float _Cutoff
        void surf (Input IN, inout SurfaceOutputStandard o) 
        {
		// change the direction the texture offset moves , use sin and cos to make it circular where 0 and 1 move to the right and the values inbetween determine different directions
		//				.25
		//				|
		//		  .5 --     -- 1 & 0
		//				|
		//				.75
			float2 direction = float2(cos(_Direction * UNITY_PI * 2), sin(_Direction * UNITY_PI * 2));
			float2 parallaxDirection = float2(cos(_ParallaxDirection * UNITY_PI * 2), sin(_ParallaxDirection * UNITY_PI * 2));
			// the maintex uv += the parallax map movemet and direction 
			//scroll the parallax and set it's speed, direction and strength because unity is dumb and sets the heightscale too low'
			IN.uv_MainTex += tex2D(_ParallaxMap, IN.uv_ParallaxMap + parallaxDirection * _Time.x * _ParallaxSpeed) * _ParallaxStrength;
			// c = the maintexture and uv 
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex + direction * _Time.x * _Speed )* _Color;
			// albedo rgb
            o.Albedo = c.rgb;
			// apply metallic
            o.Metallic = _Metallic;
			// apply glossiness
            o.Smoothness = _Glossiness;
			// apply alpha
            o.Alpha = c.a;
			//if (c.b > _Cutoff) c.a = 0;
			//c.rgb *= c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
