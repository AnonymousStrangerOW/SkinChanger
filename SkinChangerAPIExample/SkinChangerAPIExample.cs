using OWML.ModHelper;
using System.IO;
using UnityEngine;

namespace SkinChangerAPIExample;

public class SkinChangerAPIExample : ModBehaviour
{
    public interface ISkinChanger
    {
        void RegisterCustomSkin(string name, string assetName, string bundlePath, Vector3 cameraOffset, float colliderRadius, float colliderHeight, Vector3 colliderCenter);
    }

    public void Start()
    {
        var api = ModHelper.Interaction.TryGetModApi<ISkinChanger>("pikpik_carrot.SkinChanger");

        var ernestoPath = Path.Combine(ModHelper.Manifest.ModFolderPath, "Assets/anglerfish_skin");
        ModHelper.Console.WriteLine(ernestoPath);
        api.RegisterCustomSkin("Ernesto", "Anglerfish_Skin", ernestoPath, new Vector3(0, 2.2f, 0.27f), 0.5f, 3.6f, new Vector3(0f, 0.875f, 0f));
        
        // Testing that the error fallback for broken skins works
        api.RegisterCustomSkin("Error", "nothing", Path.Combine(ModHelper.Manifest.ModFolderPath, "Assets/invalid_path"), Vector3.zero, 0f, 0f, Vector3.zero);
    }
}