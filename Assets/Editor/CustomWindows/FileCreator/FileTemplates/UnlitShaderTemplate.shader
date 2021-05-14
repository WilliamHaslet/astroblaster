Shader "Custom/Template"
{

    Properties
    {

        _MainTex("Main Texture", 2D) = "" {}

    }

    SubShader
    {

        Tags { "RenderType"="Opaque" }

        /*Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

        ZWrite Off

        Blend SrcAlpha OneMinusSrcAlpha*/

        Pass
        {

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"

            struct vertInput
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct fragInput
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;

            fragInput vert(vertInput v)
            {
                fragInput o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                return o;
            }

            float4 frag(fragInput i) : SV_Target
            {

                return tex2D(_MainTex, i.uv);

            }

            ENDHLSL

        }

    }

}
