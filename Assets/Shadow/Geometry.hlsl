float _Progress;

[maxvertexcount(3)]
void Geometry(
    uint pid : SV_PrimitiveID,
    triangle Attributes input[3],
    inout TriangleStream<PackedVaryingsType> outStream
)
{
    AttributesMesh v0 = ConvertToAttributesMesh(input[0]);
    AttributesMesh v1 = ConvertToAttributesMesh(input[1]);
    AttributesMesh v2 = ConvertToAttributesMesh(input[2]);

    float3 p0 = v0.positionOS;
    float3 p1 = v1.positionOS;
    float3 p2 = v2.positionOS;

    float3 c = (p0 + p1 + p2) / 3;
    p0 = lerp(p0, c, _Progress);
    p1 = lerp(p1, c, _Progress);
    p2 = lerp(p2, c, _Progress);

    outStream.Append(PackVertexData(v0, p0));
    outStream.Append(PackVertexData(v1, p1));
    outStream.Append(PackVertexData(v2, p2));
    outStream.RestartStrip();

    return;
}
