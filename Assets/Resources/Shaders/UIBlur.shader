// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "VFX/Blur"
{
    Properties
    {
        _MaxBlur ("Max Blur", Float) = 1.0
    }
    SubShader
    {
        Tags { "IgnoreProjector" = "True" "Queue" = "Transparent" "RenderType" = "Transparent" }
       
        GrabPass { }
       
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
 
            #include "UnityCG.cginc"
 
            sampler2D _GrabTexture;
            float4 _GrabTexture_TexelSize;
            float _MaxBlur;


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float4 color : COLOR;
            };
 
            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
				float4 color : COLOR;
            };

 
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = ComputeGrabScreenPos(o.pos);
				o.color = v.color;
                return o;
            }           
 
            half4 frag (v2f i) : SV_Target
            {
 
                half4 pixelCol = half4(0, 0, 0, 0);
 
                #define ADDPIXEL(weight,kernelX,factorX) tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(i.uv.x + _GrabTexture_TexelSize.x * kernelX * factorX, i.uv.y, i.uv.z, i.uv.w))) * weight
               
				float factor = _MaxBlur * i.color.a;

                pixelCol += ADDPIXEL(0.05, 4.0, factor);
                pixelCol += ADDPIXEL(0.09, 3.0, factor);
                pixelCol += ADDPIXEL(0.12, 2.0, factor);
                pixelCol += ADDPIXEL(0.15, 1.0, factor);
                pixelCol += ADDPIXEL(0.18, 0.0, factor);
                pixelCol += ADDPIXEL(0.15, -1.0, factor);
                pixelCol += ADDPIXEL(0.12, -2.0, factor);
                pixelCol += ADDPIXEL(0.09, -3.0, factor);
                pixelCol += ADDPIXEL(0.05, -4.0, factor);

				pixelCol *= i.color;

                return pixelCol;
            }
            ENDCG
        }
 
        GrabPass { }
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
 
            #include "UnityCG.cginc"
 
            sampler2D _GrabTexture;
            float4 _GrabTexture_TexelSize;
            float _MaxBlur;


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float4 color : COLOR;
            };
 
            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
				float4 color : COLOR;
            };

 
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = ComputeGrabScreenPos(o.pos);
				o.color = v.color;
                return o;
            }           
 
            fixed4 frag (v2f i) : SV_Target
            {
 
                fixed4 pixelCol = fixed4(0, 0, 0, 0);
 
                #define ADDPIXEL(weight,kernelY,factorY) tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(i.uv.x, i.uv.y + _GrabTexture_TexelSize.y * kernelY * factorY, i.uv.z, i.uv.w))) * weight
               
				float factor = _MaxBlur * i.color.a;
				
				pixelCol += ADDPIXEL(0.05, 4.0, factor);
				pixelCol += ADDPIXEL(0.09, 3.0, factor);
				pixelCol += ADDPIXEL(0.12, 2.0, factor);
				pixelCol += ADDPIXEL(0.15, 1.0, factor);
				pixelCol += ADDPIXEL(0.18, 0.0, factor);
				pixelCol += ADDPIXEL(0.15, -1.0, factor);
				pixelCol += ADDPIXEL(0.12, -2.0, factor);
				pixelCol += ADDPIXEL(0.09, -3.0, factor);
				pixelCol += ADDPIXEL(0.05, -4.0, factor);

                return pixelCol;
            }
            ENDCG
        }
    }
	FallBack "Diffuse"
}