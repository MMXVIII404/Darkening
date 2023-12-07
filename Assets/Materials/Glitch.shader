Shader "Custom/Glitch"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GlitchIntensity ("Glitch Intensity", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _GlitchIntensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // 随机扭曲
                float glitchOffset = sin(_Time.y * _GlitchIntensity) * 0.05;
                i.uv.x += glitchOffset;

                // 颜色偏移
                float3 color = tex2D(_MainTex, i.uv).rgb;
                float3 colorOffset = tex2D(_MainTex, i.uv + float2(glitchOffset, 0)).rgb;
                color.r = colorOffset.r;

                return float4(color, 1.0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
