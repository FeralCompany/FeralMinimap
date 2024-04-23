using FeralMinimap.Behaviors;
using HarmonyLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FeralMinimap.Patches;

[HarmonyPatch(typeof(HUDManager))]
public static class HUDManagerPatch
{
    private static GameObject _minimapViewGameObject = null!;

    public static RectTransform Tooltips { get; private set; } = null!;
    public static Vector2 TooltipsOrigin { get; private set; }

    [HarmonyPostfix]
    [HarmonyPatch("Awake")]
    private static void PostFix_Awake(HUDManager __instance, ref HUDElement ___Tooltips)
    {
        Tooltips = ___Tooltips.canvasGroup.gameObject.GetComponent<RectTransform>();
        TooltipsOrigin = Tooltips.anchoredPosition;

        if (_minimapViewGameObject) Object.Destroy(_minimapViewGameObject);
        _minimapViewGameObject = new GameObject("FeralMinimapView");
        _minimapViewGameObject.transform.SetParent(__instance.playerScreenTexture.transform, false);

        _minimapViewGameObject.AddComponent<MinimapView>();
    }
}
