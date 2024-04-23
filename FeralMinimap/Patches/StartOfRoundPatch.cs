using FeralMinimap.Behaviors;
using HarmonyLib;
using UnityEngine;

namespace FeralMinimap.Patches;

[HarmonyPatch(typeof(StartOfRound))]
public static class StartOfRoundPatch
{
    private static GameObject _minimapCamera = null!;

    [HarmonyPostfix]
    [HarmonyPatch("Awake")]
    public static void PostFix_Awake(StartOfRound __instance)
    {
        if (_minimapCamera) Object.Destroy(_minimapCamera);

        _minimapCamera = Object.Instantiate(__instance.mapScreen.mapCamera.gameObject);
        _minimapCamera.name = "FeralMinimapCamera";
        _minimapCamera.AddComponent<MinimapCamera>();
    }
}
