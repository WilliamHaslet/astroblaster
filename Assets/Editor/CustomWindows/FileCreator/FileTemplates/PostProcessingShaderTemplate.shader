Shader "CustomPostProcessing/Template"
{

    Properties
    {

        [HideInInspector] _MainTex("Screen Texture", 2D) = "" {}
    
    }

    SubShader
    {

        Cull Off 
        ZWrite Off 
        ZTest Always

        Pass
        {

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"

            struct vertexInput
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct fragmentInput
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fragmentInput vert(vertexInput v)
            {
                fragmentInput o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            float3 frag(fragmentInput i) : SV_Target
            {

                float3 screenColor = tex2D(_MainTex, i.uv).xyz;

                return 1 - screenColor;

            }

            ENDHLSL

        }

    }

}
