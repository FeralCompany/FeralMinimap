using FeralCommon.Config;
using FeralMinimap.Behaviors;
using FeralMinimap.Util;
using UnityEngine;

namespace FeralMinimap;

public static class Config
{
    internal static void InitController()
    {
        Minimap.Size.OnValueChanged(_ =>
        {
            if (MinimapView.Instance) MinimapView.Instance.UpdateViewSize();
        });

        Minimap.AspectRatio.OnValueChanged(_ =>
        {
            if (MinimapView.Instance) MinimapView.Instance.UpdateViewSize();
        });

        Minimap.CameraFeedQuality.OnValueChanged(_ =>
        {
            if (MinimapView.Instance) MinimapView.Instance.UpdateFeedSize();
        });

        Minimap.XPosOffset.OnValueChanged(newValue =>
        {
            if (MinimapView.Instance)
            {
                var current = MinimapView.Instance.Image.rectTransform.anchoredPosition;
                MinimapView.Instance.Image.rectTransform.anchoredPosition = new Vector2(newValue * -1, current.y);
                MinimapView.AdjustTooltips();
            }
        });

        Minimap.YPosOffset.OnValueChanged(newValue =>
        {
            if (MinimapView.Instance)
            {
                var current = MinimapView.Instance.Image.rectTransform.anchoredPosition;
                MinimapView.Instance.Image.rectTransform.anchoredPosition = new Vector2(current.x, newValue * -1 + MagicNumbers.MinimapYPosPadding);
                MinimapView.AdjustTooltips();
            }
        });

        Minimap.InsideBrightness.OnValueChanged(newValue => MinimapCamera.Instance.Light.intensity = newValue);

        Minimap.Zoom.OnValueChanged(_ =>
        {
            if (MinimapCamera.Instance) MinimapCamera.Instance.UpdateCameraSize();
        });

        Minimap.AspectRatio.OnValueChanged(_ =>
        {
            if (MinimapCamera.Instance) MinimapCamera.Instance.UpdateCameraSize();
        });

        Minimap.Size.OnValueChanged(_ =>
        {
            if (MinimapCamera.Instance) MinimapCamera.Instance.UpdateCameraSize();
        });
    }

    public static class Minimap
    {
        public static readonly IntConfig InsideBrightness = new IntConfig("Minimap", "Facility Brightness")
            .WithDescription("Sets the brightness of the minimap when the current view is within the facility.")
            .WithMin(0)
            .WithMax(100)
            .WithDefaultValue(15)
            .NoSlider();

        public static readonly EnumConfig<RotationStrategy> Rotation = new EnumConfig<RotationStrategy>("Minimap", "Rotation")
            .WithDescription("""
                             Determines in which orientation the minimap is rotated.
                             Default is a north-west direction at 315 degrees.
                             Target will rotate the minimap to match the target's current orientation.
                             The cardinal directions (NESW) will rotate the minimap to face the corresponding direction.
                             """
            )
            .WithDefaultValue(RotationStrategy.Default);

        public static readonly IntConfig Size = new IntConfig("Minimap", "Size")
            .WithDescription("The size (in px) of the minimap.")
            .WithMin(100)
            .WithMax(500)
            .WithDefaultValue(200)
            .NoSlider();

        public static readonly FloatConfig AspectRatio = new FloatConfig("Minimap", "Aspect Ratio")
            .WithDescription("""
                             The aspect ratio of the minimap. This is the ratio of width to height.
                             '4:3' is the default aspect ratio, which equates to 4/3=1.3333
                             '16:9' is a common widescreen aspect ratio, which equates to 16/9=1.7777
                             """
            )
            .WithMin(0)
            .WithMax(float.MaxValue)
            .WithDefaultValue(4F / 3F)
            .NoSlider();

        public static readonly FloatConfig Zoom = new FloatConfig("Minimap", "Zoom")
            .WithDescription("""
                             Determines the 'orthographic size' of the minimap camera. Otherwise known as the zoom level.
                             Larger values will zoom the map out, showing more of the facility or surrounding area.
                             """
            )
            .WithMin(1F)
            .WithMax(100F)
            .WithDefaultValue(19.7F)
            .WithStep(0.1F);

        public static readonly FloatConfig CameraFeedQuality = new FloatConfig("Minimap", "Camera Feed Quality")
            .WithDescription(
                "Multiplier for the size of the camera feed. Increasing this value will increase the size of the camera feed, which will increase the quality of the minimap.")
            .WithMin(0.25F)
            .WithMax(25F)
            .WithDefaultValue(1F)
            .WithStep(0.1F);

        public static readonly IntConfig XPosOffset = new IntConfig("Minimap", "X Position Offset")
            .WithDescription("""
                             The horizontal offset of the minimap from the right side of the screen.
                             0 represents being flush with the right side of the screen.
                             Positive values move the minimap to the left.
                             """
            );

        public static readonly IntConfig YPosOffset = new IntConfig("Minimap", "Y Position Offset")
            .WithDescription("""
                             The vertical offset of the minimap from the top of the screen.
                             0 represents being flush with the top of the screen.
                             Positive values move the minimap down.
                             """
            );

        public static int Height => Size;
        public static int Width => (int)(Size * AspectRatio);
    }

    public static class Fixes
    {
        public static readonly BoolConfig AlwaysShowRadarBoosters = new BoolConfig("Fixes", "Always Show Radar Boosters")
            .WithDescription("""
                             Always show radar boosters on the minimap, even when they're not active.
                             The default behavior is to only show radar boosters on the minimap when they're active or when they have never been activated (because they're visible by default).
                             Enabling this option will make radar boosters always visible on the minimap, regardless of their state.
                             """)
            .WithDefaultValue(true);
    }
}
