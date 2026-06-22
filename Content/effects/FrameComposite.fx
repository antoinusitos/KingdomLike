#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D BackgroundTexture;
sampler2D BackgroundTextureSampler = sampler_state
{
	Texture = <BackgroundTexture>;
};

Texture2D MainLayerTexture;
sampler2D MainLayerTextureSampler = sampler_state
{
	Texture = <MainLayerTexture>;
};

Texture2D ForegroundTexture;
sampler2D ForegroundTextureSampler = sampler_state
{
	Texture = <ForegroundTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 backgroundColor = tex2D(BackgroundTextureSampler, input.TextureCoordinates);
    float4 mainLayerColor = tex2D(MainLayerTextureSampler, input.TextureCoordinates);
    float4 foregroundColor = tex2D(ForegroundTextureSampler, input.TextureCoordinates);
    
    float4 firstPass = lerp(backgroundColor, mainLayerColor, mainLayerColor.a);
    float4 secondPass = lerp(firstPass, foregroundColor, foregroundColor.a);
	return secondPass * input.Color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};