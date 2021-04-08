#ifndef NOISES_INCLUDED
#define NOISES_INCLUDED

half3 mod289(half3 x)
{
    return x - floor(x * (1.0 / 289.0)) * 289.0;
}
            
half2 mod289(half2 x)
{
    return x - floor(x * (1.0 / 289.0)) * 289.0;
}
            
half3 permute(half3 x)
{
    return mod289((x * 34.0 + 1.0) * x);
}
            
half3 taylorInvSqrt(half3 r)
{
    return 1.79284291400159 - 0.85373472095314 * r;
}
            
float snoise(half2 v)
{
    const half4 C = half4( 0.211324865405187,  // (3.0-sqrt(3.0))/6.0
                            0.366025403784439,  // 0.5*(sqrt(3.0)-1.0)
                        -0.577350269189626,  // -1.0 + 2.0 * C.x
                            0.024390243902439); // 1.0 / 41.0
            
    // First corner
    half2 i  = floor(v + dot(v, C.yy));
    half2 x0 = v -   i + dot(i, C.xx);
            
    // Other corners
    half2 i1;
    i1.x = step(x0.y, x0.x);
    i1.y = 1.0 - i1.x;
            
    // x1 = x0 - i1  + 1.0 * C.xx;
    // x2 = x0 - 1.0 + 2.0 * C.xx;
    half2 x1 = x0 + C.xx - i1;
    half2 x2 = x0 + C.zz;
            
    // Permutations
    i = mod289(i); // Avoid truncation effects in permutation
    half3 p =
        permute(permute(i.y + half3(0.0, i1.y, 1.0))
                    + i.x + half3(0.0, i1.x, 1.0));
            
    half3 m = max(0.5 - half3(dot(x0, x0), dot(x1, x1), dot(x2, x2)), 0.0);
    m = m * m;
    m = m * m;
            
    // Gradients: 41 points uniformly over a line, mapped onto a diamond.
    // The ring size 17*17 = 289 is close to a multiple of 41 (41*7 = 287)
    half3 x = 2.0 * frac(p * C.www) - 1.0;
    half3 h = abs(x) - 0.5;
    half3 ox = floor(x + 0.5);
    half3 a0 = x - ox;
            
    // Normalise gradients implicitly by scaling m
    m *= taylorInvSqrt(a0 * a0 + h * h);
            
    // Compute final noise value at P
    half3 g;
    g.x = a0.x * x0.x + h.x * x0.y;
    g.y = a0.y * x1.x + h.y * x1.y;
    g.z = a0.z * x2.x + h.z * x2.y;
    return 130.0 * dot(m, g);
}

#endif