#ifndef FRACTAL_NOISE_INCLUDED
#define FRACTAL_NOISE_INCLUDED

// Original work (fbm simplex noise) Copyright (C) 2020 Sebastian Lague.
// License: Distributed under the MIT License. See LICENSE file.
// https://github.com/SebLague/Solar-System/blob/Development/Assets/Scripts/Celestial/Shaders/Includes/FractalNoise.cginc
// Adapted for Shadergraph, billow noise, domain warping and uber cloudy noise added by Emil Tonev.
#include "SimplexNoise.hlsl"

// Returns values in the range [-1, 1]
float fbmSimplexNoise(float3 pos, float4 params[3]) {
	// Extract parameters for readability
	float3 offset = params[0].xyz;
	int numLayers = int(params[0].w);
	float persistence = params[1].x;
	float lacunarity = params[1].y;
	float scale = params[1].z;
	float multiplier = params[1].w;
    float verticalShift = params[2].x;

	// Sum up noise layers
    float noiseSum = 0;
    float amplitude = 1;
	float amplitudeSum = 1e-6; // Avoid division by zero
    float frequency = scale;
    for (int i = 0; i < numLayers; i++) {
        noiseSum += snoise(pos * frequency + offset) * amplitude;
    	amplitudeSum += amplitude;
        amplitude *= persistence;
        frequency *= lacunarity;
    }
	return (noiseSum * multiplier + verticalShift) / amplitudeSum;
}

// Shadergraph interface functions
void fbmSimplexNoise3D_float(
	float3 pos,
	float3 offset,
	int numLayers,
	float persistence,
	float lacunarity,
	float scale,
	out float noisePattern)
{
	float4 noiseParams[3];
	noiseParams[0].xyz = offset;
	noiseParams[0].w = numLayers;
	noiseParams[1] =  float4(persistence,lacunarity,scale,1);
	noiseParams[2] = float4(0,0,0,0);
					
	noisePattern = fbmSimplexNoise(pos,noiseParams); // Returns values in the range [-1, 1]
}

void fbmBillowNoise3D_float(
	float3 pos,
	float3 offset,
	int numLayers,
	float persistence,
	float lacunarity,
	float scale,
	out float noisePattern)
{
	float4 noiseParams[3];
	noiseParams[0].xyz = offset;
	noiseParams[0].w = numLayers;
	noiseParams[1] =  float4(persistence,lacunarity,scale,1);
	noiseParams[2] = float4(0,0,0,0);
					
	noisePattern = abs(fbmSimplexNoise(pos,noiseParams));
}

void domainWarp_float(
	float3 pos,
	float3 offset,
	int numLayers,
	float persistence,
	float lacunarity,
	float scale,
	float warpAmount,
	out float noise)
{
	float4 noiseParams[3];
	noiseParams[0].xyz = offset;
	noiseParams[0].w = numLayers;
	noiseParams[1] =  float4(persistence,lacunarity,scale,1);
	noiseParams[2] = float4(0,0,0,0);

	float3 q = float3( 
					fbmSimplexNoise(pos + float3(0.0, 0.0, 0.0), noiseParams), 
					fbmSimplexNoise(pos + float3(5.2, 1.3, 2.1), noiseParams), 
					fbmSimplexNoise(pos + float3(2.8, 4.7, 0.6), noiseParams) 
					);

	float3 r = float3( 
		fbmSimplexNoise(pos + warpAmount * q + float3(1.7, 9.2, 3.3), noiseParams), 
		fbmSimplexNoise(pos + warpAmount * q + float3(8.3, 2.8, 7.5), noiseParams), 
		fbmSimplexNoise(pos + warpAmount * q + float3(3.1, 6.7, 1.2), noiseParams)
	);
					
	noise = fbmSimplexNoise(pos+warpAmount*r,noiseParams);
}

void domainWarpOffsetOutputs_float(
	float3 pos,
	float3 offset,
	int numLayers,
	float persistence,
	float lacunarity,
	float scale,
	float warpAmount,
	out float noise,
	out float3 q,
	out float3 r)
{
	float4 noiseParams[3];
	noiseParams[0].xyz = offset;
	noiseParams[0].w = numLayers;
	noiseParams[1] =  float4(persistence,lacunarity,scale,1);
	noiseParams[2] = float4(0,0,0,0);

	q = float3( 
					fbmSimplexNoise(pos + float3(0.0, 0.0, 0.0), noiseParams), 
					fbmSimplexNoise(pos + float3(5.2, 1.3, 2.1), noiseParams), 
					fbmSimplexNoise(pos + float3(2.8, 4.7, 0.6), noiseParams) 
					);

	r = float3( 
		fbmSimplexNoise(pos + warpAmount * q + float3(1.7, 9.2, 3.3), noiseParams), 
		fbmSimplexNoise(pos + warpAmount * q + float3(8.3, 2.8, 7.5), noiseParams), 
		fbmSimplexNoise(pos + warpAmount * q + float3(3.1, 6.7, 1.2), noiseParams)
	);
					
	noise = fbmSimplexNoise(pos+warpAmount*r,noiseParams);
}

void domainWarpOffsetOutputsFiltered_float(
float3 pos,
float3 offset,
int numLayers,
float persistence,
float lacunarity,
float scale,
float warpAmount,
float detailLevel,
out float noise,
out float3 q,
out float3 r)
{
	const int MaxSamples = 10;

	float3 uvw = pos * scale;

	float3 ddx_uvw = uvw + ddx(uvw);
	float3 ddy_uvw = uvw + ddy(uvw);

	int sx = 1 + int( clamp( detailLevel*length(ddx_uvw-uvw), 0.0, float(MaxSamples-1) ) );
	int sy = 1 + int( clamp( detailLevel*length(ddy_uvw-uvw), 0.0, float(MaxSamples-1) ) );
	
	for( int j=0; j<sy; j++ )
	{
		for( int i=0; i<sx; i++ )
		{
			float2 st = float2( float(i), float(j) ) / float2( float(sx),float(sy) );
			float3 samplePos = uvw + st.x * (ddx_uvw - uvw) + st.y * (ddy_uvw - uvw);
			float noiseSample;
			float3 qSample;
			float3 rSample;
			domainWarpOffsetOutputs_float(samplePos, offset, numLayers,
			                              persistence, lacunarity, 1, warpAmount,
			                              noiseSample, qSample, rSample);
			noise += noiseSample;
			q += qSample;
			r += rSample;
		}
	}

	noise = noise / float(sx*sy);
	q = q / float(sx*sy);
	r = r / float(sx*sy);

}

void uberCloudyNoise_float(float3 pos,
	float3 offset,
	int numLayers,
	float persistence,
	float lacunarity,
	float scale,
	float cloudiness,
	out float cloudyNoise)
{
	pos += offset;
	float noiseSum = 0.;
	float amplitude = 1.;
	float amplitudeSum = 1e-6; // Avoid division by zero
	float frequency = scale;
	for (int i = 0; i < numLayers; i ++) {
		float noiseData = snoise((pos * frequency)).x * amplitude;
		amplitudeSum += 2.*amplitude ;

        
		float billowNoise = abs(noiseData);        
        
		noiseSum += lerp(noiseData, billowNoise, max(0.0f, cloudiness));

		amplitude *= persistence;
		frequency *= lacunarity;
	}
	cloudyNoise = noiseSum/amplitudeSum;
}
#endif