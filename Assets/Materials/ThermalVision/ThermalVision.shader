Shader "Custom/ThermalVision"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

		[PowerSlider(4)] _FresnelExponent ("Fresnel Exponent", Range(0.25, 4)) = 1
        _GradientTex ("Gradient Texture", 2D) = "white" {}
        _NoiseSpeed ("Noise Speed", Vector) = (1,1,0,0)
        _NoisePower ("Noise Power", Range(0,10)) = 0.1
        _NoiseScale ("Noise Scale", Range(0, 50)) = 10
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        #include "../Libs/NoiseSimplex.cginc"

        sampler2D _MainTex;
        sampler2D _GradientTex;

        struct Input {
			float2 uv_MainTex;
			float3 worldNormal;
			float3 viewDir;
			INTERNAL_DATA
		};

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

		float _FresnelExponent;
        float2 _NoiseSpeed;
        float _NoisePower;
        float _NoiseScale;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input i, inout SurfaceOutputStandard o) {
			//sample and tint albedo texture
			fixed4 col = tex2D(_MainTex, i.uv_MainTex);
			col *= _Color;
			o.Albedo = col.rgb;

			//just apply the values for metalness and smoothness
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;

			//get the dot product between the normal and the view direction
			float fresnel = dot(i.worldNormal, i.viewDir);
			//invert the fresnel so the big values are on the outside
			fresnel = saturate(1 - fresnel);
			//raise the fresnel value to the exponents power to be able to adjust it
			fresnel = pow(fresnel, _FresnelExponent);

            float noise = snoise((i.uv_MainTex * _NoiseScale) + float2(_Time.y, _Time.y) * _NoiseSpeed) * _NoisePower;
            fresnel += noise;
            fresnel = saturate(fresnel);
			//combine the fresnel value with a color
            fixed4 fresnelGradColor = tex2D(_GradientTex, float2(fresnel, 0.5));
			float3 fresnelColor = fresnelGradColor;
			//apply the fresnel value to the emission
			o.Emission = fresnelColor;
		}
        ENDCG
    }
    FallBack "Diffuse"
}
