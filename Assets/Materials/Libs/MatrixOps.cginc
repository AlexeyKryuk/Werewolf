#ifndef MATRIXOPS_INCLUDED
#define MATRIXOPS_INCLUDED

half2x2 scale(half2 _scale){
    return half2x2(_scale.x, 0.0,
                    0.0, _scale.y);
}

half2x2 rotate2d(float _angle){
        return half2x2(cos(_angle),-sin(_angle),
                        sin(_angle),cos(_angle));
}

#endif