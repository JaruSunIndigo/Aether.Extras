#include "Macros.fxh"

DECLARE_TEXTURE(colorMap, 0) = sampler_state
{
    Texture = (colorMap);
    AddressU = CLAMP;
    AddressV = CLAMP;
    MagFilter = LINEAR;
    MinFilter = LINEAR;
    Mipfilter = LINEAR;
};

DECLARE_TEXTURE(lightMap, 1) = sampler_state
{
    Texture = (lightMap);
    AddressU = CLAMP;
    AddressV = CLAMP;
    MagFilter = LINEAR;
    MinFilter = LINEAR;
    Mipfilter = LINEAR;
};


struct VertexShaderInput
{
    float3 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
};

float2 halfPixel;
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    output.Position = float4(input.Position,1);
    output.TexCoord = input.TexCoord - halfPixel;
    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float3 diffuseColor = SAMPLE_TEXTURE(colorMap,input.TexCoord).rgb;
    float4 light = SAMPLE_TEXTURE(lightMap,input.TexCoord);
    float3 diffuseLight = light.rgb;
    float specularLight = light.a;
	return float4((diffuseColor * diffuseLight + specularLight),1);
}

TECHNIQUE( Standard, VertexShaderFunction, PixelShaderFunction );
