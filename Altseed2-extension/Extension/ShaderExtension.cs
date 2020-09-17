using Altseed2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Extension
{
    public class ShaderExtension
    {
        const string nineSliceCode = @"

struct PS_INPUT
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 UV1 : UV0;
    float2 UV2 : UV1;
};

cbuffer Consts : register(b1)
{
    float4 corner;
    float4 size;
};

Texture2D mainTex : register(t0);
SamplerState mainSamp : register(s0);

float4 main(PS_INPUT input) : SV_TARGET
{
    float2 uv = float2(0, 0);

    if (input.UV1.x < size.x / 2.0)
    {
        uv.x = (2.0 * corner.x / size.x) * input.UV1.x;
    }
    else if (input.UV1.x < 1.0 - size.x / 2.0)
    {
        uv.x = ((1.0 - 2.0 * corner.x) / (1.0 - size.x)) * input.UV1.x + (2.0 * corner.x - size.x) / (2.0 * (1.0 - size.x));
    }
    else 
    {
        uv.x = (2.0 * corner.x / size.x) * input.UV1.x + 1.0 - 2.0 * corner.x / size.x;
    }

    if (input.UV1.y < size.y / 2.0)
    {
        uv.y = (2.0 * corner.y / size.y) * input.UV1.y;
    }
    else if (input.UV1.y < 1.0 - size.y / 2.0)
    {
        uv.y = ((1.0 - 2.0 * corner.y) / (1.0 - size.y)) * input.UV1.y + (2.0 * corner.y - size.y) / (2.0 * (1.0 - size.y));
    }
    else 
    {
        uv.y = (2.0 * corner.y / size.y) * input.UV1.y + 1.0 - 2.0 * corner.y / size.y;
    }

    return mainTex.Sample(mainSamp, uv);
}

        ";

        public static Shader NineSliceShader => Shader.Create("nineSliceShader", nineSliceCode, ShaderStage.Pixel);
    }
}
