using OWML.ModHelper;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SkinChanger;

public class SkinChangerAPI	: ISkinChanger
{
	public class CustomSkin
	{
		public string Name { get; private set; }
		public string BundlePath { get; private set; }
		public string PrefabID { get; private set; }
		public Vector3 CameraOffset { get; private set; }
		public float ColliderRadius { get; private set; }
		public float ColliderHeight { get; private set; }
		public Vector3 ColliderCenter { get; private set; }

		public CustomSkin(string name, string assetName, string bundlePath, Vector3 cameraOffset, float colliderRadius, float colliderHeight, Vector3 colliderCenter)
		{
			Name = name;
			BundlePath = bundlePath;
			PrefabID = assetName;
			CameraOffset = cameraOffset;
			ColliderRadius = colliderRadius;
			ColliderHeight = colliderHeight;
			ColliderCenter = colliderCenter;
		}
	}

	public void RegisterCustomSkin(ModBehaviour mod, string name, string assetName, string bundlePath, Vector3 cameraOffset, float colliderRadius, float colliderHeight, Vector3 colliderCenter)
	{
		if (SkinChanger.instance.Initialized)
		{
			SkinChanger.instance.ModHelper.Console.WriteLine($"Custom skin initialization already ran! You must call {nameof(RegisterCustomSkin)} on ModBehaviour Start", OWML.Common.MessageType.Error);
		}
		else
		{
			SkinChanger.instance.CustomSkins.Add(new CustomSkin(name, assetName, Path.Combine(mod.ModHelper.Manifest.ModFolderPath, bundlePath), cameraOffset, colliderRadius, colliderHeight, colliderCenter));
			SkinChanger.instance.ModHelper.Console.WriteLine($"Registered custom skin {name} at {bundlePath}");
		}
	}
}
