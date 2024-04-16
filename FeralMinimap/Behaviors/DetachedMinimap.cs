using System.Collections;
using System.Collections.Generic;
using FeralCommon.Utils;
using GameNetcodeStuff;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

namespace FeralMinimap.Behaviors;

public class DetachedMinimap : MonoBehaviour
{
    private const float MaxFontSize = 100F;
    private const float MinFontSize = 1F;

    private Camera _camera = null!;
    private Light _cameraLight = null!;

    private RawImage _cameraView = null!;
    private TextMeshProUGUI _cameraViewText = null!;

    private int _targetIndex;
    private PlayerControllerB? _targetPlayer;
    private TransformAndName? _targetTransform;
    private Vector2? _tooltipFixedPos;
    private Vector2? _tooltipOriginalPos;

    private RectTransform? _tooltipRectFix;

    private static List<TransformAndName>? PotentialTargets => StartOfRound.Instance?.mapScreen?.radarTargets;

    private void Awake()
    {
        StartCoroutine(InitCamera());
    }

    private void Update()
    {
        if (!_camera) return;

        RepositionComponents();

        if (!Buttons.MinimapEnabled)
        {
            _cameraView.enabled = false;
            _cameraViewText.enabled = false;
            return;
        }

        ValidateCurrentTarget();
        if (_targetTransform == null)
        {
            _cameraView.enabled = false;
            _cameraViewText.enabled = false;
            return;
        }

        _cameraView.enabled = true;
        _cameraViewText.enabled = true;

        if (_cameraLight.cullingMask == 0 && Player.LocalPlayer()) _cameraLight.cullingMask = ~Player.LocalPlayer().gameplayCamera.cullingMask;
        _cameraLight.intensity = Config.Minimap.Brightness;

        _cameraViewText.text = _targetPlayer
            ? $"Player: {_targetTransform.name}"
            : $"Radar: {_targetTransform.name}";

        if (!_targetPlayer)
        {
            _camera.nearClipPlane = -2.47F;
            MoveCameraTo(_targetTransform.transform.position);
            return;
        }

        if (_targetPlayer!.isInHangarShipRoom)
            _camera.nearClipPlane = -0.96F;
        else
            _camera.nearClipPlane = -2.47F;

        if (_targetPlayer.isPlayerDead)
            if (_targetPlayer.redirectToEnemy)
                MoveCameraTo(_targetPlayer.redirectToEnemy.transform.position);
            else if (_targetPlayer.deadBody)
                MoveCameraTo(_targetPlayer.deadBody.transform.position);
            else
                MoveCameraTo(_targetPlayer.placeOfDeath);
        else
            MoveCameraTo(_targetTransform.transform.position);
    }

    private IEnumerator InitCamera()
    {
        yield return new WaitUntil(() => StartOfRound.Instance
                                         && StartOfRound.Instance.mapScreen
                                         && StartOfRound.Instance.mapScreen.mapCamera);

        _camera = Instantiate(StartOfRound.Instance.mapScreen.mapCamera, gameObject.transform);
        _camera.targetTexture = new RenderTexture(455, 315, 32, GraphicsFormat.R8G8B8A8_UNorm)
        {
            isPowerOfTwo = false,
            filterMode = FilterMode.Point,
            anisoLevel = 0
        };
        _camera.enabled = true;

        _cameraLight = UnityTool.CreateRootGameObject("MinimapLight").AddComponent<Light>();
        _cameraLight.transform.position = new Vector3(0f, 100f, 0f);
        _cameraLight.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        _cameraLight.type = LightType.Directional;
        _cameraLight.range = 100;
        _cameraLight.color = Color.white;
        _cameraLight.colorTemperature = 6500;
        _cameraLight.intensity = Config.Minimap.Brightness;
        _cameraLight.cullingMask = 0;

        _cameraView = UnityTool.CreateRootGameObject("MinimapView").AddComponent<RawImage>();
        _cameraView.transform.SetParent(HUDManager.Instance.playerScreenTexture.transform, false);

        _cameraView.rectTransform.pivot = new Vector2(1f, 1f);
        _cameraView.rectTransform.anchorMin = new Vector2(1, 1);
        _cameraView.rectTransform.anchorMax = new Vector2(1, 1);
        _cameraView.rectTransform.sizeDelta = new Vector2(Config.Minimap.Size - 10, Config.Minimap.Size - 10);

        _cameraView.texture = _camera.targetTexture;

        _cameraViewText = UnityTool.CreateRootGameObject("MinimapViewText").AddComponent<TextMeshProUGUI>();
        _cameraViewText.rectTransform.SetParent(_cameraView.rectTransform, false);

        _cameraViewText.font = StartOfRound.Instance.mapScreenPlayerName.font;
        _cameraViewText.color = StartOfRound.Instance.mapScreenPlayerName.color;
        _cameraViewText.text = "Initializing Minimap...";

        _cameraViewText.fontSizeMax = MaxFontSize;
        _cameraViewText.fontSizeMin = MinFontSize;
        _cameraViewText.enableWordWrapping = false;
        _cameraViewText.enableAutoSizing = true;

        _cameraViewText.rectTransform.pivot = new Vector2(0, 1);
        _cameraViewText.rectTransform.anchorMin = new Vector2(0, 1);
        _cameraViewText.rectTransform.anchorMax = new Vector2(0, 1);

        Buttons.SwitchTarget.OnPressed(SwitchNextTarget);
    }

    private void RepositionComponents()
    {
        RepositionTooltip();
        RepositionCameraView();
    }

    private void RepositionTooltip()
    {
        if (!_tooltipRectFix)
        {
            _tooltipRectFix = HUDManager.Instance?.Tooltips?.canvasGroup?.gameObject.GetComponent<RectTransform>();
            if (!_tooltipRectFix) return;
        }

        _tooltipOriginalPos ??= _tooltipRectFix!.anchoredPosition;
        _tooltipFixedPos = _tooltipOriginalPos - new Vector2(0, Config.Minimap.Size);

        if (_camera && _camera.enabled)
            _tooltipRectFix!.anchoredPosition = _tooltipFixedPos.Value;
        else
            _tooltipRectFix!.anchoredPosition = _tooltipOriginalPos.Value;
    }

    private void RepositionCameraView()
    {
        _cameraView.rectTransform.sizeDelta = new Vector2(Config.Minimap.Size, Config.Minimap.Size);
        _cameraView.rectTransform.anchoredPosition = new Vector2(0, -5F);
    }

    private void ValidateCurrentTarget()
    {
        var targets = PotentialTargets;
        if (targets == null || targets.Count == 0)
        {
            _targetTransform = null;
            _targetPlayer = null;
            return;
        }

        if (_targetIndex >= targets.Count)
            _targetIndex = targets.Count - 1;

        _targetTransform = targets[_targetIndex];
        _targetPlayer = _targetTransform.transform.GetComponent<PlayerControllerB>();
        if (!_targetPlayer)
        {
            _targetPlayer = null;
            return;
        }

        if (_targetPlayer.isPlayerControlled) return;

        if (_targetIndex + 1 >= targets.Count)
            _targetIndex = 0;
        else
            _targetIndex++;
        ValidateCurrentTarget();
    }

    private void SwitchNextTarget()
    {
        var targets = PotentialTargets;
        if (targets == null || targets.Count == 0)
            return;
        if (_targetIndex + 1 >= targets.Count)
            _targetIndex = 0;
        else
            _targetIndex++;
    }

    private void MoveCameraTo(Vector3 pos)
    {
        _camera.transform.position = new Vector3(pos.x, pos.y + 3.636f, pos.z);
    }
}
