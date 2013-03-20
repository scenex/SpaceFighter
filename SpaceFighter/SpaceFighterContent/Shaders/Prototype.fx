// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

sampler s0;
float elapsed;

float4 PixelShaderPrototype(float2 coords: TEXCOORD0) : COLOR0
{
	coords.x += sin(coords.x + elapsed) * 0.05f;
	coords.y += cos(coords.y + elapsed) * 0.05f;
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