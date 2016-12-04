Shader "my shader"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}
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

			v2f vert (appdata_t v)
			{
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				return o;
			}
				
			fixed4 frag (v2f IN) : COLOR
			{

				half4 tex	= tex2D(_MainTex, IN.texcoord);
				half4 col = 1;
				float factor = tex.rgb.x + tex.rgb.y + tex.rgb.z;
            	if (factor >= 3) {
					return tex;
            		//col.rgb = factor * 3;
            	} else {
					//
					//return tex  * IN.color;
					tex.rgb = (tex.rgb/(IN.color * IN.color)) - IN.color;//(0.8, 0.8, 0.8);
					return tex;
					//col.rgb = 0;
				}

								
			}
			ENDCG
		}
	}


}
