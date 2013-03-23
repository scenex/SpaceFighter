// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

sampler2D input;

float2 circleCenter = { 0.5, 0.5 }; 
float circleRadius = 2; 

float4 PixelShaderFunction(float4 color : COLOR0, float2 uv : TEXCOORD0) : COLOR0 
{ 
    float distanceFromCenter = length(circleCenter - uv); 
    float distanceFromCircle = abs(circleRadius - distanceFromCenter); 

	if (distanceFromCircle > 1.5)
	{
		uv.x *= 2;
		uv.y *= 2;
	}

	color = tex2D(input, uv);
	return color;
}

technique Technique1 
{ 
    pass Pass1 
    { 
        PixelShader = compile ps_2_0 PixelShaderFunction(); 
    } 
} 
