Shader "Custom/SimpleOutlineURP"
{
    Properties
    {
        _OutlineColor("Outline Color", Color) = (1,1,0,1)
        _OutlineWidth("Outline Width", Float) = 0.03
    }

    SubShader
    {
        Tags{
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Transparent+100"
        }

        Pass
        {
            Name "Outline"
            Tags{ "LightMode"="UniversalForward" }

            Cull Front
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _OutlineColor;
            float _OutlineWidth;

            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                // Expand vertex position along normal
                float3 pos = IN.positionOS + IN.normalOS * _OutlineWidth;

                // Correct transforms for Unity 6 URP
                float3 worldPos = TransformObjectToWorld(pos);
                OUT.positionHCS = TransformWorldToHClip(worldPos);

                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                return _OutlineColor;
            }

            ENDHLSL
        }
    }
}
