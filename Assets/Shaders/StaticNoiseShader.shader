Shader "Hidden/StaticNoiseShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Intensity ("Intensity", Range(0, 1)) = 0.0
        _Time2 ("Time", Float) = 0.0
    }

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

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
            float _Intensity;
            float _Time2;

            // Pseudo-random hash - fast and visually noisy like TV static
            float hash(float2 p)
            {
                p = frac(p * float2(443.897, 441.423));
                p += dot(p, p + 19.19);
                return frac(p.x * p.y);
            }

            // Layered noise for a richer static look
            float staticNoise(float2 uv, float time)
            {
                // Coarse grain - large scanline-style blocks
                float coarse = hash(float2(floor(uv.y * 120.0 + time * 37.0), floor(uv.x * 80.0 + time * 13.0)));
                // Fine grain - per-pixel randomness
                float fine = hash(uv * float2(1920.0, 1080.0) + time * 97.3);
                // Scanline bias: horizontal bands brighten periodically
                float scanline = step(0.97, frac(uv.y * 60.0 + time * 5.0));

                return lerp(fine, coarse, 0.4) + scanline * 0.25;
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                if (_Intensity <= 0.0)
                    return col;

                float noise = staticNoise(i.uv, _Time2);

                // Chromatic aberration: shift R and B channels sideways for extra grit
                float shift = _Intensity * 0.008;
                float r = tex2D(_MainTex, i.uv + float2( shift, 0.0)).r;
                float g = col.g;
                float b = tex2D(_MainTex, i.uv + float2(-shift, 0.0)).b;
                fixed4 aberrated = fixed4(r, g, b, col.a);

                // Blend scene with grey static
                fixed4 staticColor = fixed4(noise, noise, noise, 1.0);
                fixed4 result = lerp(aberrated, staticColor, _Intensity * noise);

                // Darken edges slightly (vignette) as intensity rises
                float2 centered = i.uv - 0.5;
                float vignette = 1.0 - dot(centered, centered) * 2.5 * _Intensity;
                result.rgb *= saturate(vignette);

                return result;
            }
            ENDCG
        }
    }
}
