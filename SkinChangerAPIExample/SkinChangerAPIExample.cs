using OWML.ModHelper;
using System.IO;
using UnityEngine;

namespace SkinChangerAPIExample;

public class SkinChangerAPIExample : ModBehaviour
{
    public interface ISkinChanger
    {
        void RegisterCustomSkin(ModBehaviour mod, string name, string assetName, string bundlePath, Vector3 cameraOffset, float colliderRadius, float colliderHeight, Vector3 colliderCenter);
    }

    public void Start()
    {
        var api = ModHelper.Interaction.TryGetModApi<ISkinChanger>("pikpik_carrot.SkinChanger");

        api.RegisterCustomSkin(this, "Ernesto", "Anglerfish_Skin", "Assets/anglerfish_skin", new Vector3(0, 2.2f, 0.27f), 0.5f, 3.6f, new Vector3(0f, 0.875f, 0f));
        
        // Testing that the error fallback for broken skins works
        api.RegisterCustomSkin(this, "Missing Skin", string.Empty, string.Empty, Vector3.zero, 0f, 0f, Vector3.zero);
    }
}