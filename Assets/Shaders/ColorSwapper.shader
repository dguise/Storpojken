Shader "Custom/ColorSwapper" {
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _ColorTint ("Tint", Color) = (1,1,1,1)
        _Color1in ("Color 1 In", Color) = (0,0.90980392156862745098039215686275,0.84705882352941176470588235294118,1)
        _Color1out ("Color 1 Out", Color) = (1,1,1,1)
        _Color2in ("Color 2 In", Color) = (0,0.43921568627450980392156862745098,0.92549019607843137254901960784314,1)
        _Color2out ("Color 2 Out", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    }
 
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
 
        Cull Off
        Lighting Off
        ZWrite Off
        Fog { Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag        
            #pragma multi_compile DUMMY PIXELSNAP_ON
            #include "UnityCG.cginc"
         
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };
            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                half2 texcoord  : TEXCOORD0;
            };
         
            fixed4 _ColorTint;
            fixed4 _Color1in;
            fixed4 _Color1out;
            fixed4 _Color2in;
            fixed4 _Color2out;
            
 
            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;      
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif
               
                return OUT;
            }
 
            sampler2D _MainTex;          
         
			fixed4 frag(v2f IN) : COLOR
            {
                float4 texColor = tex2D( _MainTex, IN.texcoord );
               

			   //texColor = _Color2in;
			   
				if(
					(texColor.r == _Color1in.r) &&
                    (texColor.g == _Color1in.g) &&
                    (texColor.b == _Color1in.b))
                { 
                    texColor = _Color1out;
					texColor.a = _Color1out.a;
                }
				
				if((texColor.r == _Color2in.r) &&
                    (texColor.g == _Color2in.g) &&
                    (texColor.b == _Color2in.b))
                { 
                    texColor = _Color2out;
					texColor.a = _Color2out.a;
                }
				
                return texColor * _ColorTint; //apply the tint
            }
        ENDCG
        }
    }
}