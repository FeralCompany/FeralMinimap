using HarmonyLib;

namespace FeralMinimap.Patches;

[HarmonyPatch(typeof(RadarBoosterItem))]
public static class RadarBoosterPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(RadarBoosterItem.Update))]
    private static void PostFix_Update(RadarBoosterItem __instance)
    {
        if (Config.Fixes.AlwaysShowRadarBoosters) __instance.radarDot.SetActive(true);
    }
}
