using FeralCommon.Utils;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace FeralMinimap.Patches;

[HarmonyPatch(typeof(ManualCameraRenderer))]
public static class ManualCameraRendererPatch
{
    [HarmonyPostfix]
    [HarmonyPatch("Update")]
    private static void Postfix_Update(ref PlayerControllerB ___targetedPlayer, ref Light ___mapCameraLight)
    {
        if (___mapCameraLight && ___targetedPlayer)
        {
            ___mapCameraLight.enabled = ___targetedPlayer.isInsideFactory;
            if (Player.LocalPlayerNullable()) ___mapCameraLight.cullingMask = ~Player.LocalPlayer().gameplayCamera.cullingMask;
        }
    }
}
