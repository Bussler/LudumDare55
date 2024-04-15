Shader "SineDiffuse" {
    Properties {
       _Color ("Diffuse Material Color", Color) = (1, 0, 0, 1)
       _MainTex ("Diffuse Texture", 2D) = "white" {}
       _Frequency ("Color Wave Frequency", float) = 1.0
       _ColorWaveOffset ("Diffuse Material Color", Color) = (0.1, 0, 0, 1.0)
       _Alpha ("Alpha", Range(0, 1)) = 1 // Property for controlling transparency
       _Frequency ("Freq", Range(0, 1000)) = 100 // Property for controlling transparency
    }
    SubShader {
        Tags { "Queue" = "Transparent" } // Set rendering queue to Transparent

        Pass {    
            Blend SrcAlpha OneMinusSrcAlpha // Enable alpha blending

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
            };

            uniform float4 _Color;
            uniform float4 _ColorWaveOffset;
            uniform sampler2D _MainTex;
            uniform float _Alpha;

            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.texcoord = v.texcoord;
                return o;
            }

            float4 frag(v2f i) : SV_Target {
                float4 texColor = tex2D(_MainTex, i.texcoord);
                float3 diffuseReflection = texColor.rgb * _Color.rgb;
                return float4((0.25 * sin(100 * _Time) + 0.5) * diffuseReflection, texColor.a * _Alpha); // Output with transparency
            }
            ENDCG
        }
    }
}
