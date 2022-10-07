Shader "Unlit/Mode7Scroll"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Center ("Center Screen", float) = 1.0
        _Scale ("Scale", float) = 1.0
        _Fov("Fov", float) = 1.0
        _Horizon("Horiozon", float) = 1.0
        _FogColor("color", Color) = (1.0, 1.0, 1.0, 1.0)

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                half3 tangent : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Center;
            float _Scale;
            float _Fov;
            float _Horizon;
            float4 _FogColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.tangent = UnityObjectToWorldNormal(v.tangent);
                return o;
            }


            fixed4 frag (v2f i) : SV_Target
            {
                //SCENE ATTRIBUTES
                float CENTER_SCREEN= _Center;
                
                half3 worldTangent = UnityObjectToWorldDir(i.tangent);
                half3 viewTangent = mul((float3x3) UNITY_MATRIX_V, worldTangent);
                float2 camPos= float2(viewTangent.z * _WorldSpaceCameraPos.z ,  viewTangent.x * _WorldSpaceCameraPos.x);
                float camRot = atan2(viewTangent.x, viewTangent.z);
                //camRot =  normalize(camRot)* 500;
                //PROJECTION 
	            float2 uv = i.uv ;
                float distance = pow(uv.y, 10.0);
                uv -= CENTER_SCREEN;
                //if(uv.y> _Horizon)
                  //  camPos=-camPos;
	            float3 projection = float3(uv.x, uv.y - _Horizon - _Fov, uv.y - _Horizon); 
                float2 scene = projection.xy / projection.z; 
                //CAMERA TRANSFORMATION
                scene *= _Scale;
                //float2x2 rotationMatrix = float2x2(cos(i.vertex.x), - sin(i.vertex.y), sin(i.vertex.y), cos(i.vertex.x));
                float2x2 rotationMatrix = float2x2(cos(camRot), + sin(camRot), -sin(camRot), cos(camRot));
                scene = mul(scene, rotationMatrix);
                scene-=camPos;//Note: Always update camera position AFTER the rotation!
                //COLOR
                //float3 texel = texture(iChannel0, scene).xyz;
                float3 texel = tex2D(_MainTex, scene.xy);
                //texel *= abs(horizon-uv.y);//(Darkening based on distance)
                float3 pixelColour = lerp(texel, _FogColor, distance);
                return  float4(pixelColour,1.0);
            }

            ENDCG
        }
    }
}
