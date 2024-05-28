Shader "Custom/UnlitReflectiveMetal"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _GlowColor ("Glow Color", Color) = (1, 0.5, 0, 1)
        _BaseColor ("Base Color", Color) = (0.1, 0, 0, 1)
        _GlowIntensity ("Glow Intensity", Range(0.0, 5.0)) = 2.0
        _Speed ("Speed", Range(0.0, 5.0)) = 1.0
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _GlowColor;
            float4 _BaseColor;
            float _GlowIntensity;
            float _Speed;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 baseColor = tex2D(_MainTex, i.texcoord) * _BaseColor;

                // Calculate time-based offset for the noise texture
                float2 noiseUV = i.texcoord + _Time.y * _Speed * float2(0.1, 0.1);
                half4 noise = tex2D(_NoiseTex, noiseUV);

                // Combine base color and noise to create a glowing effect
                float glowFactor = noise.r * _GlowIntensity;
                half4 finalColor = lerp(baseColor, _GlowColor, glowFactor);

                finalColor.a = baseColor.a; // Preserve the original alpha
                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Unlit/Transparent"
}
