// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

float4x4 World;
float4x4 View;
float4x4 Projection;

sampler s0;

float4 PixelShaderFunction(float2 coords: TEXCOORD) : COLOR0
{
	float4 color = tex2D(s0, coords);  
	color.gb = color.r;
	return color; 
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}