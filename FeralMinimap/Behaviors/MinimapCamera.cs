using System;
using FeralCommon.Utils;
using FeralMinimap.Util;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace FeralMinimap.Behaviors;

public class MinimapCamera : MonoBehaviour
{
    public static MinimapCamera Instance = null!;

    public Camera Camera { get; private set; } = null!;
    public HDAdditionalCameraData CameraData { get; private set; } = null!;
    public Light Light { get; private set; } = null!;
    public MinimapTarget Target { get; private set; } = null!;

    private void Awake()
    {
        if (Instance) Destroy(Instance);
        Instance = this;

        Camera = gameObject.GetComponent<Camera>();
        Camera.name = "FeralMinimapCamera";
        Camera.cullingMask += Mask.Unused1;
        Camera.aspect = Config.Minimap.AspectRatio;
        Camera.orthographic = true;
        Camera.orthographicSize = Config.Minimap.Zoom;

        CameraData = gameObject.GetComponent<HDAdditionalCameraData>();

        Light = gameObject.AddComponent<Light>();
        Light.type = LightType.Directional;
        Light.range = 100;
        Light.color = Color.white;
        Light.colorTemperature = 6500;
        Light.useColorTemperature = true;
        Light.cullingMask = Mask.Unused1;
        Light.intensity = Config.Minimap.InsideBrightness;
        Light.enabled = false;

        Target = gameObject.AddComponent<MinimapTarget>();
    }

    private void Start()
    {
        Config.Minimap.InsideBrightness.OnValueChanged(newValue => Light.intensity = newValue);

        Config.Minimap.Zoom.OnValueChanged(_ => UpdateCameraSize());
        Config.Minimap.AspectRatio.OnValueChanged(_ => UpdateCameraSize());
        Config.Minimap.Size.OnValueChanged(_ => UpdateCameraSize());

        Camera.targetTexture = MinimapView.Instance.Feed;

        MinimapView.Instance.Text.font = StartOfRound.Instance.mapScreenPlayerName.font;
        MinimapView.Instance.Text.material = StartOfRound.Instance.mapScreenPlayerName.material;
        MinimapView.Instance.Text.color = StartOfRound.Instance.mapScreenPlayerName.color;
    }

    private void Update()
    {
        Camera.transform.eulerAngles = new Vector3(90F, GetEulerY(), 0F);
        Camera.transform.position = Target.Position;
        Camera.nearClipPlane = Target.NearClipPlane;

        Light.intensity = Config.Minimap.InsideBrightness;
        Light.enabled = Target.RequiresLighting;
    }

    private void UpdateCameraSize()
    {
        Camera.orthographicSize = Config.Minimap.Zoom;
        Camera.aspect = Config.Minimap.AspectRatio;
        Camera.pixelRect = new Rect(0, 0, Config.Minimap.Width, Config.Minimap.Height);
        Camera.rect = new Rect(0, 0, 1, 1);
    }

    private float GetEulerY()
    {
        var strategy = Config.Minimap.Rotation.Value;
        return strategy switch
        {
            RotationStrategy.Default => 315F,
            RotationStrategy.Target => Target.EulerY,
            RotationStrategy.North => 90F,
            RotationStrategy.East => 180F,
            RotationStrategy.South => 270F,
            RotationStrategy.West => 0F,
            _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, $"Invalid rotation strategy: {strategy}")
        };
    }
}
