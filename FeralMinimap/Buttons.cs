using FeralCommon.Input;
using FeralMinimap.Behaviors;

namespace FeralMinimap;

public static class Buttons
{
    public static readonly ButtonToggle MinimapEnabled = new("MinimapEnabled", "Minimap Enabled", "<keyboard>/m", true);
    public static readonly ButtonPress SwitchTarget = new("SwitchTarget", "Switch Target", "<keyboard>/l");

    internal static void InitController()
    {
        MinimapEnabled.OnToggle(newValue =>
        {
            if (MinimapView.Instance)
            {
                MinimapView.Instance.Image.enabled = newValue;
                MinimapView.Instance.Text.enabled = newValue;
                MinimapView.AdjustTooltips();
            }
        });

        SwitchTarget.OnPressed(() => MinimapTarget.Instance.SwitchNextTarget());
    }
}
