Shader "Custom/SpriteColorSwap" {
    Properties{
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _SwapColor("Swap Color", Color) = (1,1,1,1)
        _SwapColor2("Swap Color 2", Color) = (1,1,1,1)
        _LineColor("Line Color", Color) = (1,1,1,1)
        _BlinkColor("Blink Color", Color) = (1,1,1,1)
        _ShadowColor("Shadow Color", Color) = (0,0,0,1)
        _NewColor("New Color", Color) = (1,0,0,1)
    }
    SubShader{
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
        Pass {
            Cull Off
            Lighting Off
            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGB

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _SwapColor;
            float4 _SwapColor2;
            float4 _LineColor;
            float4 _BlinkColor;
            float4 _ShadowColor;
            float4 _NewColor;

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 tmp;
                tmp.a = col.a;
                tmp.rgb = _SwapColor / max(0.01, (1.0 - _NewColor * _NewColor.a));
                if ((col.r == _SwapColor.r && col.g == _SwapColor.g && col.b == _SwapColor.b) || 
                    (col.r == _ShadowColor.r && col.g == _ShadowColor.g && col.b == _ShadowColor.b) ||
                    (col.r == _LineColor.r && col.g == _LineColor.g && col.b == _LineColor.b) || 
                    (col.r == _BlinkColor.r && col.g == _BlinkColor.g && col.b == _BlinkColor.b) ||
                    (col.r == _SwapColor2.r && col.g == _SwapColor2.g && col.b == _SwapColor2.b)) {
                    if (col.r == _ShadowColor.r && col.g == _ShadowColor.g && col.b == _ShadowColor.b) {
                        tmp.rgb = lerp(tmp, tmp*_ShadowColor, _ShadowColor.a);
                    }
                    return tmp;
                }
                return col;
            }
            ENDCG
        }
    }
        FallBack "Sprites/Default"
}