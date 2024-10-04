Shader "Custom/GrayScale"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
 
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
 
        CGPROGRAM
        #pragma surface surf Lambert
 
        sampler2D _MainTex;
 
        struct Input
        {
            float2 uv_MainTex;
        };
 
        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            float gray = dot(c.rgb, float3(0.299, 0.587, 0.114));
            o.Albedo = float3(gray, gray, gray);
        }
        ENDCG
    }
    FallBack "Diffuse"
}