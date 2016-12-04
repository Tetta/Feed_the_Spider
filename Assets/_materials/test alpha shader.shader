Shader "Sprites/Alpha Test Sprite"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
	_Color("Tint", Color) = (1,1,1,1)
		_Clip("Alpha Clip", Range(0,.9)) = .5
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

		SubShader
	{
		Tags
	{
		"Queue" = "AlphaTest"
		"IgnoreProjector" = "True"
		"RenderType" = "TransparentCutout"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}

		Cull Off
		Lighting Off
		ZWrite Off
		AlphaTest Greater[_Clip]

		CGPROGRAM
#pragma surface surf Lambert vertex:vert nofog keepalpha alphatest:_Cutoff
#pragma multi_compile _ PIXELSNAP_ON

		sampler2D _MainTex;
	fixed4 _Color;
	float _Clip;

	struct Input
	{
		float2 uv_MainTex;
		fixed4 color;
		float alpha_clip;
	};

	void vert(inout appdata_full v, out Input o)
	{
#if defined(PIXELSNAP_ON)
		v.vertex = UnityPixelSnap(v.vertex);
#endif

		UNITY_INITIALIZE_OUTPUT(Input, o);
		o.color = v.color * _Color;
	}

	void surf(Input IN, inout SurfaceOutput o)
	{
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
		o.Albedo = c.rgb * c.a;
		o.Alpha = c.a - _Clip;

	}
	ENDCG
	}

		Fallback "Transparent/VertexLit"
}

