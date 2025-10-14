Shader "Hidden/PixelationShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PixelWidth ("Pixel Width", Float) = 320
        _PixelHeight ("Pixel Height", Float) = 180
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            float _PixelWidth;
            float _PixelHeight;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Get screen pixel coordinates
                float2 uv = i.uv;

                // Calculate pixel size steps based on the target resolution
                float2 pixelSize = float2(1.0 / _PixelWidth, 1.0 / _PixelHeight);

                // Snap UVs to nearest pixel
                uv = floor(uv / pixelSize) * pixelSize + pixelSize * 0.5;

                return tex2D(_MainTex, uv);
            }
            ENDCG
        }
    }
}
