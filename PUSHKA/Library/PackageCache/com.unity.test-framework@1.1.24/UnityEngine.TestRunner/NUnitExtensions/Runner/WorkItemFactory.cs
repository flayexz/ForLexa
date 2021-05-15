# Unity.Mathematics

A C# math library providing vector types and math functions with a shader like
syntax. Used by the Burst compiler to compile C#/IL to highly efficient
native code.

The main goal of this library is to provide a friendly Math API familiar to SIMD and graphic/shaders developers, using the well known `float4`, `float3` types...etc. with all intrinsics functions provided by a static class `math` that can be imported easily into your C# program with `using static Unity.Mathematics.math`.

In addition to this, the Burst compiler is able to recognize these types and provide the optimized SIMD type for the running CPU on all supported platforms (x64, ARMv7a...etc.)

NOTICE: The API is a work in progress and we may introduce breaking changes (API and underlying behavior)

## Usage

You can use this library in your Unity game by using the Package Manager and referen