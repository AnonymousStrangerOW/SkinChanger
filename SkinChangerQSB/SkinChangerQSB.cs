using QSB;
using QSB.Player;
using QSB.Utility;
using QSB.WorldSync;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SkinChangerQSB;

/// <summary>
/// This was largely copied from https://github.com/xen-42/ow-qsb-skins but changing the method of applying skins to use SkinChanger
/// </summary>
public class SkinChangerQSB : MonoBehaviour
{
    public static SkinChangerQSB Instance { get; private set; }

    public string LocalSkin { get; private set; }

    // Links QSB playerID to current skin name and the game object they have on representing it
    private readonly Dictionary<uint, (string skinName, GameObject currentMesh)> _skins = new();

    public static string ChangeSkinMessage => nameof(ChangeSkinMessage);

    public void Start()
    {
        SkinChanger.SkinChanger.instance.skinChanged += OnSkinChanged;

        Instance = this;

        LoadManager.OnCompleteSceneLoad += OnCompleteSceneLoad;
        QSBPlayerManager.OnAddPlayer += OnPlayerAdded;

        QSBHelper.API.RegisterHandler<string>(ChangeSkinMessage, OnReceiveChangeSkinMessage);
        QSBCore.RegisterNotRequiredForAllPlayers(SkinChanger.SkinChanger.instance);
    }

    private void OnCompleteSceneLoad(OWScene originalScene, OWScene loadScene)
    {
        if (loadScene == OWScene.SolarSystem || loadScene == OWScene.EyeOfTheUniverse)
        {
            // Wait for QSB to finish connecting and syncing first
            Delay.RunWhen(
                () => QSBWorldSync.AllObjectsReady,
                () => ChangePlayerSkin(QSBPlayerManager.LocalPlayer, LocalSkin)
            );
        }
    }

    public void OnSkinChanged(string skin)
    {
        LocalSkin = skin;
        if (QSBWorldSync.AllObjectsReady)
        {
            ChangePlayerSkin(QSBPlayerManager.LocalPlayer, skin);
        }
    }

    private void OnPlayerAdded(PlayerInfo player)
    {
        // Send them info about our skin
        // Make sure they've finished loading in first
        Delay.RunWhen(
            () => player.Body != null,
            () => SendChangeSkinMessage(LocalSkin, to: player.PlayerId)
        );
    }

    public void OnReceiveChangeSkinMessage(uint From, string Data)
    {
        Delay.RunWhen(
           () => QSBPlayerManager.GetPlayer(From).Body != null,
           () => Instance.ChangePlayerSkin(QSBPlayerManager.GetPlayer(From), Data)
        );
    }

    public static void SendChangeSkinMessage(string skin, uint to = uint.MaxValue)
    {
        QSBHelper.API.SendMessage(ChangeSkinMessage, skin, to: to);
    }

    public void ChangePlayerSkin(PlayerInfo player, string skinName)
    {
        DebugLogger.Write($"Changing skin on {player.PlayerId} to {skinName}");

        if (player.IsLocalPlayer)
        {
            // Immediately tell all other clients to alter our skin
            SendChangeSkinMessage(skinName);
            // SkinChanger base will handle changing our skin for us
        }
        else
        {
            if (_skins.TryGetValue(player.PlayerId, out var skin))
            {
                if (skin.skinName == skinName)
                {
                    // Already has that skin
                    return;
                }
                else if (skin.currentMesh != null)
                {
                    GameObject.Destroy(skin.currentMesh);
                }
            }

            // Replace skin
            var isDefaultSkin = skinName.ToUpper() == "HATCHLING";

            player.Body.transform.Find("REMOTE_Traveller_HEA_Player_v2").gameObject.SetActive(isDefaultSkin);

            if (isDefaultSkin)
            {
                _skins[player.PlayerId] = (skinName, null);
            }
            else
            {
                DebugLogger.Write("Creating new skin object");
                // TODO: The copied gameobject still uses the animations from the original player
                var prefab = SkinChanger.SkinChanger.instance.characters.First(x => x.SettingName == skinName).GameObject;
                var mesh = prefab.InstantiateInactive();
                mesh.transform.parent = player.Body.transform;
                mesh.transform.localPosition = new Vector3(0, -1.03f, -0.2f);
                mesh.transform.localScale = Vector3.one * .1f;
                mesh.transform.localRotation = Quaternion.identity;
                mesh.SetActive(true);

                _skins[player.PlayerId] = (skinName, mesh);
            }
        }
    }
}