// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

sampler2D input;

float4 PixelShaderPrototype(float2 uv: TEXCOORD0) : COLOR0
{
	uv.x += sin(uv.y * 10) * 0.05f;
	uv.y += cos(uv.x * 10) * 0.05f;
	float4 color = tex2D(input, uv);  
	return color;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderPrototype();
    }
}