using System.Collections.Generic;
using FeralMinimap.Util;
using GameNetcodeStuff;
using UnityEngine;

namespace FeralMinimap.Behaviors;

public class MinimapTarget : MonoBehaviour
{
    private int _index;
    private PlayerControllerB? _player;
    private RadarBoosterItem? _radar;

    private TransformAndName? _target;
    public static MinimapTarget Instance { get; private set; } = null!;
    public string Name { get; private set; } = string.Empty;
    public Vector3 Position { get; private set; } = Vector3.zero;
    public float EulerY { get; private set; } = MagicNumbers.DefaultMinimapEulerY;
    public float NearClipPlane { get; private set; } = MagicNumbers.DefaultNearClipPlane;
    public bool RequiresLighting { get; private set; }

    private static List<TransformAndName> PotentialTargets => StartOfRound.Instance.mapScreen.radarTargets;

    private void Awake()
    {
        Instance = this;
        Buttons.SwitchTarget.OnPressed(SwitchNextTarget);
    }

    private void Update()
    {
        InvalidateCache();

        if (ValidateIndex())
            FetchData();
    }

    private void OnDestroy()
    {
        if (Instance) Destroy(Instance);
    }

    private bool ValidateIndex()
    {
        if (PotentialTargets.Count == 0)
            return false;

        if (_index < 0) _index = 0;
        if (_index >= PotentialTargets.Count) _index = PotentialTargets.Count - 1;

        var target = PotentialTargets[_index];

        var startIndex = _index;
        while (!IsTargetValid(target, out _player, out _radar))
        {
            _index++;
            if (_index >= PotentialTargets.Count)
                _index = 0;

            if (_index == startIndex)
                return false;

            target = PotentialTargets[_index];
        }

        _target = target;
        return true;
    }

    private void InvalidateCache()
    {
        _target = null;
        _player = null;
        _radar = null;
    }

    private static bool IsTargetValid(TransformAndName target, out PlayerControllerB? player, out RadarBoosterItem? radar)
    {
        player = null;
        radar = null;
        if (target.isNonPlayer)
        {
            radar = target.transform.gameObject.GetComponent<RadarBoosterItem>();
            return true;
        }

        player = target.transform.gameObject.GetComponent<PlayerControllerB>();
        return player && (player.isPlayerDead || player.isPlayerControlled);
    }

    internal void SwitchNextTarget()
    {
        if (_index < 0 || _index + 1 >= PotentialTargets.Count)
        {
            _index = 0;
            return;
        }

        _index++;
    }

    private void FetchData()
    {
        Position = _target!.transform.position + MagicNumbers.TargetPosOffset;
        EulerY = _target.transform.eulerAngles.y;

        NearClipPlane = MagicNumbers.DefaultNearClipPlane;
        RequiresLighting = false;

        if (_player)
        {
            Name = $"ThisIsPlayerHowAboutThat: {_target.name}";
            if (_player!.isInHangarShipRoom) NearClipPlane = MagicNumbers.HangerShipNearClipPlane;

            if (_player.isInsideFactory) RequiresLighting = true;
        }
        else if (_radar)
        {
            if (_radar!.isInFactory) RequiresLighting = true;
            Name = $"Radar: {_target.name}";
            EulerY = MagicNumbers.DefaultMinimapEulerY;
        }
        else
        {
            Name = $"Unknown: {_target.name}";
            RequiresLighting = false;
        }
    }
}
