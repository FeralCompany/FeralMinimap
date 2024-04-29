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
        Instance = this;

        Image = gameObject.AddComponent<RawImage>();
        Image.rectTransform.pivot = new Vector2(1f, 1f);
        Image.rectTransform.anchorMin = new Vector2(1, 1);
        Image.rectTransform.anchorMax = new Vector2(1, 1);
        Image.rectTransform.sizeDelta = new Vector2(Config.Minimap.Width, Config.Minimap.Height);
        Image.rectTransform.anchoredPosition =
            new Vector2(Config.Minimap.XPosOffset * -1, Config.Minimap.YPosOffset * -1 + MagicNumbers.MinimapYPosPadding);

        UpdateFeedSize();

        Text = new GameObject("FeralMinimapText").AddComponent<TextMeshProUGUI>();
        Text.transform.SetParent(Image.rectTransform, false);
        Text.rectTransform.pivot = new Vector2(0, 1);
        Text.rectTransform.anchorMin = new Vector2(0, 1);
        Text.rectTransform.anchorMax = new Vector2(0, 1);
        Text.rectTransform.sizeDelta = new Vector2(Config.Minimap.Width, Config.Minimap.Height);
        Text.rectTransform.anchoredPosition = new Vector2(0, 0);

        Text.text = "Initializing FeralMinimap...";
        Text.fontSizeMax = 20F;
        Text.fontSizeMin = 1F;
        Text.enableWordWrapping = false;
        Text.enableAutoSizing = true;

        AdjustTooltips();
    }

    private void Update()
    {
        Text.text = MinimapCamera.Instance.Target.Name;
    }

    private void OnDestroy()
    {
        if (Image) Destroy(Image);
        if (Feed) Destroy(Feed);
        if (Text) Destroy(Text);
        if (Instance) Destroy(Instance);
    }

    internal void UpdateViewSize()
    {
        Image.rectTransform.sizeDelta = new Vector2(Config.Minimap.Width, Config.Minimap.Height);
        Text.rectTransform.sizeDelta = new Vector2(Config.Minimap.Width, Config.Minimap.Height);

        UpdateFeedSize();

        AdjustTooltips();
    }

    internal void UpdateFeedSize()
    {
        var oldFeed = Feed;
        Feed = new RenderTexture((int)(Config.Minimap.Width * Config.Minimap.CameraFeedQuality),
            (int)(Config.Minimap.Height * Config.Minimap.CameraFeedQuality), 32, GraphicsFormat.R8G8B8A8_UNorm)
        {
            isPowerOfTwo = true,
            filterMode = FilterMode.Point,
            anisoLevel = 0
        };

        Image.texture = Feed;
        if (MinimapCamera.Instance && MinimapCamera.Instance.Camera)
            MinimapCamera.Instance.Camera.targetTexture = Feed;

        Destroy(oldFeed);
    }

    internal static void AdjustTooltips()
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
