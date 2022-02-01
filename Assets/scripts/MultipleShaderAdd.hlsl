
#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED
void MultipleAdd(float4 a, float4 b, float4 c, float4 d, float4 f, float4 g, float4 i, out float4 img)
{
    img = (a + b + c + d + f + g + i);
}
#endif