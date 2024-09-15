using QSB.PlayerBodySetup.Remote;
using System.Reflection;
using UnityEngine;

namespace SkinChangerQSB;

/// <summary>
/// Accesses QSB values/methods using reflection
/// Taken from QSB Skins mod
/// </summary>
public static class QSBHelper
{
    public static GameObject GetPlayerPrefab()
    {
        return typeof(RemotePlayerCreation).GetMethod("GetPrefab", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null) as GameObject;
    }

    private static IQSBAPI _api;
    public static IQSBAPI API => _api ??= SkinChanger.SkinChanger.instance.ModHelper.Interaction.TryGetModApi<IQSBAPI>("Raicuparta.QuantumSpaceBuddies");
}