// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32716,y:32678,varname:node_4795,prsc:2|emission-161-OUT,alpha-23-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32327,y:32759,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:True,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:8223b6fd93ecee54abc70982e1ce78e8,ntxv:2,isnm:False|UVIN-6962-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:1634,x:32149,y:33005,varname:node_1634,prsc:2,tex:0c67151324cdf384fb532a237831207c,ntxv:0,isnm:False|UVIN-4884-OUT,TEX-3760-TEX;n:type:ShaderForge.SFN_Multiply,id:7526,x:32327,y:33005,varname:node_7526,prsc:2|A-1634-RGB,B-1634-A;n:type:ShaderForge.SFN_Add,id:161,x:32513,y:32759,varname:node_161,prsc:2|A-6074-RGB,B-7526-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:3760,x:31924,y:32988,ptovrint:False,ptlb:SweepTex,ptin:_SweepTex,varname:node_3760,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0c67151324cdf384fb532a237831207c,ntxv:0,isnm:False;n:type:ShaderForge.SFN_TexCoord,id:6962,x:31667,y:32770,varname:node_6962,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:4884,x:31924,y:32833,varname:node_4884,prsc:2|A-6962-UVOUT,B-787-OUT;n:type:ShaderForge.SFN_Slider,id:1925,x:31161,y:32989,ptovrint:False,ptlb:Progress,ptin:_Progress,varname:node_1925,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:787,x:31667,y:32989,varname:node_787,prsc:2|A-9457-OUT,B-7090-OUT;n:type:ShaderForge.SFN_Vector2,id:9457,x:31492,y:32861,varname:node_9457,prsc:2,v1:1,v2:0;n:type:ShaderForge.SFN_RemapRange,id:7090,x:31492,y:32989,varname:node_7090,prsc:2,frmn:0,frmx:1,tomn:1,tomx:-1|IN-1925-OUT;n:type:ShaderForge.SFN_Slider,id:8707,x:32048,y:33231,ptovrint:False,ptlb:Image Opacity,ptin:_ImageOpacity,varname:node_8707,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Multiply,id:7715,x:32531,y:33190,varname:node_7715,prsc:2|A-6074-A,B-8707-OUT;n:type:ShaderForge.SFN_Add,id:23,x:32740,y:33268,varname:node_23,prsc:2|A-7715-OUT,B-8873-OUT;n:type:ShaderForge.SFN_Multiply,id:8873,x:32531,y:33342,varname:node_8873,prsc:2|A-6074-A,B-1634-A;proporder:6074-3760-1925-8707;pass:END;sub:END;*/

Shader "VFX/Sweep" {
    Properties {
        [HideInInspector]_MainTex ("MainTex", 2D) = "black" {}
        _SweepTex ("SweepTex", 2D) = "white" {}
        _Progress ("Progress", Range(0, 1)) = 0
        _ImageOpacity ("Image Opacity", Range(0, 1)) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            //#define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            //#pragma multi_compile_fwdbase
            //#pragma only_renderers d3d9 d3d11 glcore gles 
            //#pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _SweepTex; uniform float4 _SweepTex_ST;
            uniform float _Progress;
            uniform float _ImageOpacity;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float2 node_4884 = (i.uv0+(float2(1,0)*(_Progress*-2.0+1.0)));
                float4 node_1634 = tex2D(_SweepTex,TRANSFORM_TEX(node_4884, _SweepTex));
                float3 emissive = (_MainTex_var.rgb+(node_1634.rgb*node_1634.a));
                float3 finalColor = emissive;
                return fixed4(finalColor,((_MainTex_var.a*_ImageOpacity)+(_MainTex_var.a*node_1634.a)));
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
