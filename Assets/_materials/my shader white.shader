// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "my shader white"
{
	Properties
	{
		_MainTex("Base (RGB), Alpha (A)", 2D) = "white" {}
		_Intensity("Intensity", Float) = 0.6
		_Color("Color", Color) = (0.5, 0.5, 0.5, 1)
	}
	
	SubShader
	{
		LOD 200
		
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;

	float4 _MainTex_ST;
			
			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			v2f o;
			float _Intensity;
			float4 _Color;

			v2f vert (appdata_t v)
			{
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				

				return o;
			}
				
			fixed4 frag (v2f IN) : COLOR
			{

				half4 tex	= tex2D(_MainTex, IN.texcoord) * IN.color;
				//half4 tex = tex2D(_MainTex, IN.texcoord);
				//half4 col = 1;
				/*
				if (factor >= 3) {
					return tex;
            		//col.rgb = factor * 3;
            	} else {
					//
					//return tex  * IN.color;
					tex.rgb = (tex.rgb/(IN.color * IN.color)) - IN.color;//(0.8, 0.8, 0.8);
					
					//col.rgb = 0;
				}
				*/
				//tex.rgb = (tex.rgb / (IN.color * IN.color)) + IN.color; 
				//tex.rgb = (tex2D(_MainTex, IN.texcoord) * IN.color +IN.color) * 0.8;
				//tex.rgb = tex2D(_MainTex, IN.texcoord) * IN.color;
				//tex.rgb = tex.rgb - (0, 0, 0, 0.5);
				//tex.rgb = tex.rgb * (200, 0, 0, 1.5);
				//tex.rgb = 0.299 * tex.r + 0.587 * tex.g + 0.184 * tex.b;
				//tex.rgb = tex.rgb * _Color + _Intensity;
				//tex.rgb = tex.rgb / _Color + _Intensity;
				//tex.r = max(tex.r - _Intensity, 0) ;
				//tex.g = max(tex.g - _Intensity, 0);
				//tex.b = max(tex.b - _Intensity, 0);
				
				tex.r = tex.r + (0.9 - tex.r*0.9) * 0.7;
				tex.g = tex.g + (0.9 - tex.g*0.9) * 0.7;
				tex.b = tex.b + (0.9 - tex.b*0.9) * 0.7;

				//tex.r = 0.3;
				//tex.g = 0.7;
				//tex.b = 0.7;

				//tex.rgb = tex.rgb + _Intensity;
				//gl_FragColor = texture2D (gm_BaseTexture, v_vTexcoord);

				
				tex.a = tex.a;
				//tex.a = 1;
				return tex;
				//return tex2D(_MainTex, IN.texcoord) * IN.color;

			}
			ENDCG
		}
	}


}
