// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

sampler s0;

float4 PixelShaderPrototype(float2 coords: TEXCOORD0) : COLOR0
{
	coords.x += sin(coords.x * 10) * 0.05f;
	coords.y += cos(coords.y * 10) * 0.05f;
	float4 color = tex2D(s0, coords);  
	return color;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderPrototype();
    }
}