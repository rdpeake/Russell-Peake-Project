float4 color;
float4x4 xCamerasViewProjection;
float4x4 xLightsViewProjection;
float4x4 xWorld;
float3 xLightPos;
float xLightPower;
float xAmbient;

Texture xShadowMap;
sampler ShadowMapSampler = sampler_state { texture = <xShadowMap>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = clamp; AddressV = clamp; };
Texture texture0;
sampler texture0Sampler = sampler_state { texture = <texture0>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

struct VertexToPixel
{
	float4 Position     : POSITION;
	float3 Normal		: TEXCOORD1;
	float3 Position3D    : TEXCOORD2;
};

struct PixelToFrame
{
	float4 Color        : COLOR0;
};

float DotProduct(float3 lightPos, float3 pos3D, float3 normal)
{
	float3 lightDir = normalize(pos3D - lightPos);
		return dot(-lightDir, normal);
}


VertexToPixel SimplestVertexShader(float4 inPos : POSITION, float3 inNormal : NORMAL0)
{
	VertexToPixel Output = (VertexToPixel)0;

	float4x4 preWorldViewProjection = mul(xWorld, xCamerasViewProjection);
	Output.Position = mul(inPos, preWorldViewProjection);
	Output.Normal = normalize(mul(inNormal, (float3x3)xWorld));
	Output.Position3D = mul(inPos, xWorld);

	return Output;
}


PixelToFrame OurFirstPixelShader(VertexToPixel PSIn)
{
	PixelToFrame Output = (PixelToFrame)0;

	float diffuseLightingFactor = DotProduct(xLightPos, PSIn.Position3D, PSIn.Normal);
	diffuseLightingFactor = saturate(diffuseLightingFactor);
	diffuseLightingFactor *= xLightPower;

	Output.Color = color*(diffuseLightingFactor + xAmbient);
	Output.Color.a = 1;
	return Output;
}

technique Simplest
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 SimplestVertexShader();
		PixelShader = compile ps_2_0 OurFirstPixelShader();
	}
}

struct VertexToPixelTextured
{
	float4 Position     : POSITION;
	float3 UV			: TEXCOORD0;
	float3 Normal		: TEXCOORD1;
	float3 Position3D    : TEXCOORD2;
};

VertexToPixelTextured SimplestVertexShaderTextured(float4 inPos : POSITION, float3 inNormal : NORMAL0, float3 inText: TEXCOORD0)
{
	VertexToPixelTextured Output = (VertexToPixelTextured)0;

	float4x4 preWorldViewProjection = mul(xWorld, xCamerasViewProjection);
	Output.Position = mul(inPos, preWorldViewProjection);
	Output.Normal = normalize(mul(inNormal, (float3x3)xWorld));
	Output.Position3D = mul(inPos, xWorld);
	Output.UV = inText;

	return Output;
}


PixelToFrame OurFirstPixelShaderTextured(VertexToPixelTextured PSIn)
{
	PixelToFrame Output = (PixelToFrame)0;

	float diffuseLightingFactor = DotProduct(xLightPos, PSIn.Position3D, PSIn.Normal);
	diffuseLightingFactor = saturate(diffuseLightingFactor);
	diffuseLightingFactor *= xLightPower;

	float4 textColor = tex2D(texture0Sampler, PSIn.UV);
	Output.Color = textColor*(diffuseLightingFactor + xAmbient);
	Output.Color.a = 1;
	return Output;
}


technique SimplestTextured
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 SimplestVertexShaderTextured();
		PixelShader = compile ps_2_0 OurFirstPixelShaderTextured();
	}
}

struct SMapVertexToPixel
{
	float4 Position     : POSITION;
	float4 Position2D    : TEXCOORD0;
};

struct SMapPixelToFrame
{
	float4 Color : COLOR0;
};


SMapVertexToPixel ShadowMapVertexShader(float4 inPos : POSITION)
{
	SMapVertexToPixel Output = (SMapVertexToPixel)0;

	float4x4 preLightsWorldViewProjection = mul(xWorld, xLightsViewProjection);

	Output.Position = mul(inPos, preLightsWorldViewProjection);
	Output.Position2D = Output.Position;

	return Output;
}

SMapPixelToFrame ShadowMapPixelShader(SMapVertexToPixel PSIn)
{
	SMapPixelToFrame Output = (SMapPixelToFrame)0;

	Output.Color = PSIn.Position2D.z / PSIn.Position2D.w;
	Output.Color.a = 1;
	return Output;
}


technique ShadowMap
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 ShadowMapVertexShader();
		PixelShader = compile ps_2_0 ShadowMapPixelShader();
	}
}


struct SSceneVertexToPixel
{
	float4 Position             : POSITION;
	float4 Pos2DAsSeenByLight    : TEXCOORD0;
	float3 Normal                : TEXCOORD2;
	float4 Position3D            : TEXCOORD3;
};

struct SScenePixelToFrame
{
	float4 Color : COLOR0;
};

SSceneVertexToPixel ShadowedSceneVertexShader(float4 inPos : POSITION, float3 inNormal : NORMAL)
{
	SSceneVertexToPixel Output = (SSceneVertexToPixel)0;

	float4x4 preWorldViewProjection = mul(xWorld, xCamerasViewProjection);
	float4x4 preLightsWorldViewProjection = mul(xWorld, xLightsViewProjection);

	Output.Position = mul(inPos, preWorldViewProjection);
	Output.Pos2DAsSeenByLight = mul(inPos, preLightsWorldViewProjection);
	Output.Normal = normalize(mul(inNormal, (float3x3)xWorld));
	Output.Position3D = mul(inPos, xWorld);

	return Output;
}

SScenePixelToFrame ShadowedScenePixelShader(SSceneVertexToPixel PSIn)
{
	SScenePixelToFrame Output = (SScenePixelToFrame)0;

	float2 ProjectedTexCoords;
	ProjectedTexCoords[0] = PSIn.Pos2DAsSeenByLight.x / PSIn.Pos2DAsSeenByLight.w / 2.0f + 0.5f;
	ProjectedTexCoords[1] = -PSIn.Pos2DAsSeenByLight.y / PSIn.Pos2DAsSeenByLight.w / 2.0f + 0.5f;

	float diffuseLightingFactor = 0;
	bool isAmbient = true;
	if ((saturate(ProjectedTexCoords).x == ProjectedTexCoords.x) && (saturate(ProjectedTexCoords).y == ProjectedTexCoords.y)
		&& PSIn.Pos2DAsSeenByLight.z > 0)
	{
		//if in range of spotlight, remove ambient
		isAmbient = false;
		float depthStoredInShadowMap = tex2D(ShadowMapSampler, ProjectedTexCoords);
		float realDistance = PSIn.Pos2DAsSeenByLight.z / PSIn.Pos2DAsSeenByLight.w;
		if ((realDistance - 1.0f / 300.0f) <= depthStoredInShadowMap)
		{
			diffuseLightingFactor = DotProduct(xLightPos, PSIn.Position3D, PSIn.Normal);
			diffuseLightingFactor = saturate(diffuseLightingFactor);
			diffuseLightingFactor *= xLightPower;

			diffuseLightingFactor *= 1 / realDistance*realDistance;

			//float lightTextureFactor = tex2D(CarLightSampler, ProjectedTexCoords).r;
			//diffuseLightingFactor *= lightTextureFactor;
		}
	}

	if (isAmbient) {
		Output.Color = color*xAmbient;
	}
	else {
		Output.Color = color*diffuseLightingFactor;
		//Output.Color.x = PSIn.Pos2DAsSeenByLight.z;
		//Output.Color.xyz = float3(ProjectedTexCoords, 1);
	}

	//Output.Color = color*(diffuseLightingFactor + 0.2/*ambientFactor*/);
	//Output.Color = xAmbient;
	//Output.Color.y = diffuseLightingFactor;
	//Output.Color.z = 0;
	//Output.Color = diffuseLightingFactor;
	//Output.Color = ambientFactor;
	//Output.Color = diffuseLightingFactor + ambientFactor;
	Output.Color.a = 1;

	return Output;
}

technique ShadowedScene
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 ShadowedSceneVertexShader();
		PixelShader = compile ps_2_0 ShadowedScenePixelShader();
	}
}

struct SSceneVertexToPixelTextured
{
	float4 Position             : POSITION;
	float4 Pos2DAsSeenByLight    : TEXCOORD0;
	float3 UV					 : TEXCOORD1;
	float3 Normal                : TEXCOORD2;
	float4 Position3D            : TEXCOORD3;
};

SSceneVertexToPixelTextured ShadowedSceneVertexShaderTextured(float4 inPos : POSITION, float3 inNormal : NORMAL, float3 inUV : TEXCOORD0)
{
	SSceneVertexToPixelTextured Output = (SSceneVertexToPixelTextured)0;

	float4x4 preWorldViewProjection = mul(xWorld, xCamerasViewProjection);
		float4x4 preLightsWorldViewProjection = mul(xWorld, xLightsViewProjection);

		Output.Position = mul(inPos, preWorldViewProjection);
	Output.Pos2DAsSeenByLight = mul(inPos, preLightsWorldViewProjection);
	Output.Normal = normalize(mul(inNormal, (float3x3)xWorld));
	Output.Position3D = mul(inPos, xWorld);
	Output.UV = inUV;

	return Output;
}

SScenePixelToFrame ShadowedScenePixelShaderTextured(SSceneVertexToPixelTextured PSIn)
{
	SScenePixelToFrame Output = (SScenePixelToFrame)0;

	float2 ProjectedTexCoords;
	ProjectedTexCoords[0] = PSIn.Pos2DAsSeenByLight.x / PSIn.Pos2DAsSeenByLight.w / 2.0f + 0.5f;
	ProjectedTexCoords[1] = -PSIn.Pos2DAsSeenByLight.y / PSIn.Pos2DAsSeenByLight.w / 2.0f + 0.5f;

	float diffuseLightingFactor = 0;
	float ambientFactor = xAmbient;
	if ((saturate(ProjectedTexCoords).x == ProjectedTexCoords.x) && (saturate(ProjectedTexCoords).y == ProjectedTexCoords.y)
		&& PSIn.Pos2DAsSeenByLight.z > 0)
	{
		//if in range of spotlight, remove ambient
		ambientFactor = 0;
		float depthStoredInShadowMap = tex2D(ShadowMapSampler, ProjectedTexCoords);
		float realDistance = PSIn.Pos2DAsSeenByLight.z / PSIn.Pos2DAsSeenByLight.w;
		if ((realDistance - 1.0f / 300.0f) <= depthStoredInShadowMap)
		{
			diffuseLightingFactor = DotProduct(xLightPos, PSIn.Position3D, PSIn.Normal);
			diffuseLightingFactor = saturate(diffuseLightingFactor);
			diffuseLightingFactor *= xLightPower;

			diffuseLightingFactor *= 1 / realDistance*realDistance;

			//float lightTextureFactor = tex2D(CarLightSampler, ProjectedTexCoords).r;
			//diffuseLightingFactor *= lightTextureFactor;
		}
	}

	float4 textColor = tex2D(texture0Sampler, PSIn.UV);
	Output.Color = textColor*(diffuseLightingFactor + xAmbient);
	Output.Color.a = 1;

	//Output.Color = diffuseLightingFactor;
	//Output.Color.a = 1;

	return Output;
}

technique ShadowedSceneTextured
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 ShadowedSceneVertexShaderTextured();
		PixelShader = compile ps_2_0 ShadowedScenePixelShaderTextured();
	}
}


VertexToPixelTextured lightSourceVertex(float4 inPos : POSITION, float3 inNormal : NORMAL0, float3 inUV : texcoord0)
{
	VertexToPixelTextured Output = (VertexToPixelTextured)0;

	float4x4 preWorldViewProjection = mul(xWorld, xCamerasViewProjection);
	Output.Position = mul(inPos, preWorldViewProjection);
	Output.Normal = normalize(mul(inNormal, (float3x3)xWorld));
	Output.Position3D = mul(inPos, xWorld);
	Output.UV = inUV;

	return Output;
}


PixelToFrame lightSourcePixel(VertexToPixel PSIn)
{
	PixelToFrame Output = (PixelToFrame)0;
	
	Output.Color = float4(.5,.49,0,1);
	return Output;
}


technique LightSource
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 lightSourceVertex();
		PixelShader = compile ps_2_0 lightSourcePixel();
	}
}