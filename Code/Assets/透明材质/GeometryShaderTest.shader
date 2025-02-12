Shader "Unlit/GeometryShaderTest"
{
    Properties
    {
        _MainTex("Texture", 2D) = "green" {}
        [HDR]_Color("Color",Color) = (0,1,0,1)
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma geometry geom_02
                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    float3 normal:NORMAL;
                };

                struct v2g
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                    float3 normal:NORMAL;
                };

                struct g2f
                {
                    float2 uv:TEXCOORD0;
                    float4 vertex : SV_POSITION;
                    float3 worldNormal:TEXCOORD1;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                half4 _Color;
                v2g vert(appdata v)
                {
                    v2g o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.normal = v.normal;
                    return o;
                }

                [maxvertexcount(9)]
                void geom(triangle v2g input[3],inout TriangleStream<g2f> outStream)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        g2f o = (g2f)0;
                        o.vertex = input[i].vertex;
                        o.uv = input[i].uv;
                        o.worldNormal = UnityObjectToWorldNormal(input[i].normal);
                        outStream.Append(o);
                    }
                    outStream.RestartStrip();
                }

                [maxvertexcount(9)]
                void geom_02(triangle v2g input[3], inout LineStream<g2f> outStream)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        g2f o = (g2f)0;
                        o.vertex = input[i].vertex;
                        o.uv = input[i].uv;
                        o.worldNormal = UnityObjectToWorldNormal(input[i].normal);
                        outStream.Append(o);
                    }
                    outStream.RestartStrip();
                }

                half4 frag(g2f i) : SV_Target
                {
                    half3 worldNormal = normalize(i.worldNormal);
                    half4 col = tex2D(_MainTex, i.uv) * _Color;
                    return col;
                }
                ENDCG
            }
        }
}