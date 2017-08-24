Shader "Sprites/SuperSprites" {
	Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_BumpMap ("NormalMap", 2D) = "bump" {}
		_BumpScale ("NormalStrength", Range(0,5)) = 1
        _Color ("Tint", Color) = (1,1,1,1)
		_Emission ("Emission", Color) = (0,0,0)
		_Smoothness ("Smoothness", Range(0,1)) = 0.8
		_Metallic ("Metallness", Range(0,1)) = 0
		_Cutoff ("Shadow Cutoff", Range(0,1)) = 0.85
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }
	SubShader {
		/*Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }*/
		Tags
        {
            "Queue"="Geometry"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

Pass {
   Name "Caster"
   Tags { "LightMode" = "ShadowCaster" }

CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 2.0
#pragma multi_compile_shadowcaster
#pragma multi_compile_instancing
#include "UnityCG.cginc"

uniform float4 _MainTex_ST;
uniform sampler2D _MainTex;
uniform fixed _Cutoff;
uniform fixed4 _Color;

struct v2f {
    V2F_SHADOW_CASTER;
    float2  uv : TEXCOORD1;
    UNITY_VERTEX_OUTPUT_STEREO
};

v2f vert( appdata_base v )
{
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
    o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
    return o;
}

float4 frag( v2f i ) : SV_Target
{
    fixed4 texcol = tex2D( _MainTex, i.uv );
    clip( texcol.a*_Color.a - (_Cutoff) );
    SHADOW_CASTER_FRAGMENT(i)
}
ENDCG
}
		
		Cull Off
        Lighting On
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows nolightmap nodynlightmap keepalpha noinstancing addshadow alphatest:_Cutoff vertex:vert 
		//#pragma surface surf Standard addshadow alphatest:_Cutoff
		#pragma multi_compile _ PIXELSNAP_ON
        #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
		#include "UnitySprites.cginc"
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _BumpMap;
		fixed _BumpScale;
		fixed _Smoothness;
		fixed _Metallic;
		fixed3 _Emission;
		struct Input
        {
            float2 uv_MainTex;
			float2 uv_BumpMap;
            fixed4 color;
        };

		UNITY_INSTANCING_CBUFFER_START(Props)
		UNITY_INSTANCING_CBUFFER_END

		void vert (inout appdata_full v, out Input o)
        {
            v.vertex.xy *= _Flip.xy;

            #if defined(PIXELSNAP_ON)
            v.vertex = UnityPixelSnap (v.vertex);
            #endif

            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.color = v.color * _Color * _RendererColor;
        }

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = SampleSpriteTexture (IN.uv_MainTex) * IN.color;
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
			o.Normal.xy *= _BumpScale;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Emission= _Emission;
			o.Alpha = c.a*IN.color.a;
		}
		ENDCG

	}

	FallBack "Legacy Shaders/Transparent/Cutout/Bumped Diffuse"
}