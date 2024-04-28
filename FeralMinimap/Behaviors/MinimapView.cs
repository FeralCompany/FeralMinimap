using FeralMinimap.Patches;
using FeralMinimap.Util;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

namespace FeralMinimap.Behaviors;

public class MinimapView : MonoBehaviour
{
    public static MinimapView Instance = null!;

    public RawImage Image { get; private set; } = null!;
    public RenderTexture Feed { get; private set; } = null!;
    public TextMeshProUGUI Text { get; private set; } = null!;

    private void Awake()
    {
        if (Instance) Destroy(Instance);
        Instance = this;

        Image = gameObject.AddComponent<RawImage>();
        Image.rectTransform.pivot = new Vector2(1f, 1f);
        Image.rectTransform.anchorMin = new Vector2(1, 1);
        Image.rectTransform.anchorMax = new Vector2(1, 1);
        Image.rectTransform.sizeDelta = new Vector2(Config.Minimap.Width, Config.Minimap.Height);
        Image.rectTransform.anchoredPosition =
            new Vector2(Config.Minimap.XPosOffset * -1, Config.Minimap.YPosOffset * -1 + MagicNumbers.MinimapYPosPadding);

        Feed = new RenderTexture(Config.Minimap.Width, Config.Minimap.Height, (int)Config.Minimap.RenderTextureDepth.Value,
            GraphicsFormat.R8G8B8A8_UNorm)
        {
            isPowerOfTwo = true,
            filterMode = FilterMode.Point,
            anisoLevel = 0
        };

        Image.texture = Feed;

        Text = new GameObject("FeralMinimapText").AddComponent<TextMeshProUGUI>();
        Text.transform.SetParent(Image.rectTransform, false);
        Text.rectTransform.pivot = new Vector2(0, 1);
        Text.rectTransform.anchorMin = new Vector2(0, 1);
        Text.rectTransform.anchorMax = new Vector2(0, 1);
        Text.rectTransform.anchoredPosition = new Vector2(5, -5);

        Text.text = "Initializing FeralMinimap...";
        Text.fontSizeMax = 50F;
        Text.fontSizeMin = 5F;
        Text.enableWordWrapping = false;
        Text.enableAutoSizing = true;

        AdjustTooltips();
        Buttons.MinimapEnabled.OnToggle(newValue =>
        {
            Image.enabled = newValue;
            Text.enabled = newValue;
            AdjustTooltips();
        });

        Config.Minimap.Size.OnValueChanged(_ => UpdateViewSize());
        Config.Minimap.AspectRatio.OnValueChanged(_ => UpdateViewSize());

        Config.Minimap.RenderTextureDepth.OnValueChanged(newValue => Feed.depth = (int)newValue);

        Config.Minimap.XPosOffset.OnValueChanged(newValue =>
        {
            var current = Image.rectTransform.anchoredPosition;
            Image.rectTransform.anchoredPosition = new Vector2(newValue * -1, current.y);
            AdjustTooltips();
        });

        Config.Minimap.YPosOffset.OnValueChanged(newValue =>
        {
            var current = Image.rectTransform.anchoredPosition;
            Image.rectTransform.anchoredPosition = new Vector2(current.x, newValue * -1 + MagicNumbers.MinimapYPosPadding);
            AdjustTooltips();
        });
    }

    private void Update()
    {
        Text.text = MinimapCamera.Instance.Target.Name;
    }

    private void UpdateViewSize()
    {
        Image.rectTransform.sizeDelta = new Vector2(Config.Minimap.Width, Config.Minimap.Height);
        Feed.height = Config.Minimap.Height;
        Feed.width = Config.Minimap.Width;
        AdjustTooltips();
    }

    private static void AdjustTooltips()
    {
        if (Buttons.MinimapEnabled)
        {
            var offset = new Vector2(0, Config.Minimap.Height + Config.Minimap.YPosOffset);
            HUDManagerPatch.Tooltips.anchoredPosition = HUDManagerPatch.TooltipsOrigin - offset;
        }
        else
        {
            HUDManagerPatch.Tooltips.anchoredPosition = HUDManagerPatch.TooltipsOrigin;
        }
    }
}
