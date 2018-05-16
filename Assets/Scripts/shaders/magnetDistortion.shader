// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "magnetDistortion"
{
	Properties{
		//_DisplaceTex("displacement texture",2D) = "white"{}
	}
		SubShader
	{
		Tags{ "Queue" = "Transparent" }

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		struct v2f
	{
		float4 disuv : TEXCOORD1;
		float4 pos : SV_POSITION;
		float4 pos2 : TEXCOORD0;
	};

	v2f vert(appdata_full v) {
		v2f o;
		o.disuv = v.texcoord;
		o.pos2 = v.vertex;
		o.pos = UnityObjectToClipPos(v.vertex);
		return o;
	}

	half4 frag(v2f i) : COLOR
	{
		float dis = i.pos.x*i.pos.x + i.pos.y*i.pos.y;
	    return  half4(i.pos2.x,0,0, 1.0);
	}
		ENDCG
	}

	}
}