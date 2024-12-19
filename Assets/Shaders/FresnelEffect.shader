Shader "Custom/FresnelEffect"
{
    Properties
    {
        _FresnelColor ("Fresnel Color", Color) = (1, 1, 1, 1)
        _Power ("Fresnel Power", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 worldViewDir : TEXCOORD1;
            };

            float4 _FresnelColor;
            float _Power;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                // Transform normal to world space
                o.worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));

                // Calculate view direction in world space
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldViewDir = normalize(_WorldSpaceCameraPos - worldPos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float fresnel = pow(1.0 - dot(normalize(i.worldNormal), normalize(i.worldViewDir)), _Power);
                return fresnel * _FresnelColor;
            }
            ENDCG
        }
    }
}
