using UnityEngine;

namespace FeralMinimap.Util;

public static class MagicNumbers
{
    public const float MinimapYPosPadding = -8F;
    public const float DefaultNearClipPlane = -2.47F;
    public const float HangerShipNearClipPlane = -0.96F;
    public const float DefaultMinimapEulerY = 315F;

    public static readonly Vector3 TargetPosOffset = new(0F, 3.636F, 0F);
}
