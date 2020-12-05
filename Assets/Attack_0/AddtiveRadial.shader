Shader "Solutena/AddtiveRadial"
{
	Properties
	{
		_MainTex ("_MainTex", 2D) = "white" {}
		_MaskTex  ("_MaskTex", 2D) = "white" {}
		_Intensity  ("_Intensity  ", Float) = 0
		_Color ("_Color",Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags
		{
			"RenderType"="Transparent" 
			"Queue" = "Transparent"
            "IgnoreProjector" = "True"
		}
		
        Cull Off
        Lighting Off
        ZWrite Off
		Blend One One

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

            static const float PI = 3.141592f;
            static const float PI2 = 6.283185f;

			sampler2D _MainTex;
			sampler2D _MaskTex;
			float4 _MainTex_ST;
			float4 _MaskTex_ST;
			float4 _Color;
			float _Intensity;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
                float2 uv;
                float2 center = i.uv - 0.5f;
                uv.x = 1-(length(center)*2);
                uv.y = ((atan2(center.x,center.y)+PI)/PI2);
                fixed4 c = tex2D(_MainTex,i.uv);
				fixed4 m =tex2D(_MaskTex,uv+_MaskTex_ST.zw);
                c *= m;
				c *= _Color;
                return c;
			}
			ENDCG
		}
	}
}
