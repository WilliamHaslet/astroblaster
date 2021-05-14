Shader "Custom/CutCircle"
{

    Properties
    {

        _MainTex("Main Texture", 2D) = "white" {}
        _Width("Width", Float) = 0.2
        _WedgeAngleTop("Wedge Angle Top", Float) = 90
        _WedgeAngleBottom("Wedge Angle Bottom", Float) = 90

    }

    SubShader
    {

        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

        ZWrite Off

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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

            fragInput vert(vertInput v)
            {
                fragInput o;
                o.vertex = UnityObjectToClipPos(v.vertex.xyz);
                o.uv = v.uv;
                return o;
            }

            float _Width;
            float _WedgeAngleTop;
            float _WedgeAngleBottom;

            float Circle(float2 uv, float2 center)
            {

                float2 outerRadiusSquared = 0.5 * 0.5;

                float2 innerRadiusSquared = (0.5 - _Width) * (0.5 - _Width);
                
                float2 distanceSquared = (uv - center) * (uv - center);

                return step(innerRadiusSquared, distanceSquared.x + distanceSquared.y) * step(distanceSquared.x + distanceSquared.y, outerRadiusSquared);
                
            }

            float WedgeMaskTop(float angle)
            {

                return step(angle, 90 - (_WedgeAngleTop / 2)) + step(90 + (_WedgeAngleTop / 2), angle);

            }

            float WedgeMaskBottom(float angle)
            {

                return step(-angle, 90 - (_WedgeAngleBottom / 2)) + step(90 + (_WedgeAngleBottom / 2), -angle);

            }

            float4 frag(fragInput i) : SV_Target
            {
                
                float2 center = float2(0.5, 0.5);

                float angle = degrees(atan2(i.uv.y - center.y, i.uv.x - center.x));

                return float4(1, 1, 1, Circle(i.uv, center) * WedgeMaskTop(angle) * WedgeMaskBottom(angle));

            }

            ENDHLSL

        }

    }

}
