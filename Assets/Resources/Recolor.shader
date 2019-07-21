// Recolor from Kino post processing effect suite

Shader "Hidden/Cb3/Recolor"
{
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        // 4 keys, fixed gradient
        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            #define RECOLOR_EDGE_COLOR
            #include "Recolor.hlsl"
            ENDHLSL
        }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            #define RECOLOR_EDGE_DEPTH
            #include "Recolor.hlsl"
            ENDHLSL
        }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            #define RECOLOR_EDGE_NORMAL
            #include "Recolor.hlsl"
            ENDHLSL
        }

        // 8 keys, fixed gradient
        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            #define RECOLOR_EDGE_COLOR
            #define RECOLOR_GRADIENT_EXT
            #include "Recolor.hlsl"
            ENDHLSL
        }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            #define RECOLOR_EDGE_DEPTH
            #define RECOLOR_GRADIENT_EXT
            #include "Recolor.hlsl"
            ENDHLSL
        }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            #define RECOLOR_EDGE_NORMAL
            #define RECOLOR_GRADIENT_EXT
            #include "Recolor.hlsl"
            ENDHLSL
        }

        // 4 keys, blend gradient
        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            #define RECOLOR_EDGE_COLOR
            #define RECOLOR_GRADIENT_LERP
            #include "Recolor.hlsl"
            ENDHLSL
        }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            #define RECOLOR_EDGE_DEPTH
            #define RECOLOR_GRADIENT_LERP
            #include "Recolor.hlsl"
            ENDHLSL
        }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            #define RECOLOR_EDGE_NORMAL
            #define RECOLOR_GRADIENT_LERP
            #include "Recolor.hlsl"
            ENDHLSL
        }

        // 8 keys, blend gradient
        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            #define RECOLOR_EDGE_COLOR
            #define RECOLOR_GRADIENT_EXT
            #define RECOLOR_GRADIENT_LERP
            #include "Recolor.hlsl"
            ENDHLSL
        }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            #define RECOLOR_EDGE_DEPTH
            #define RECOLOR_GRADIENT_EXT
            #define RECOLOR_GRADIENT_LERP
            #include "Recolor.hlsl"
            ENDHLSL
        }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            #define RECOLOR_EDGE_NORMAL
            #define RECOLOR_GRADIENT_EXT
            #define RECOLOR_GRADIENT_LERP
            #include "Recolor.hlsl"
            ENDHLSL
        }
    }
}
