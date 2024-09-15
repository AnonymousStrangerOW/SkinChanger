using UnityEngine;

namespace SkinChanger;

public interface ISkinChanger
{
	void RegisterCustomSkin(string name, string assetName, string bundlePath, Vector3 cameraOffset, float colliderRadius, float colliderHeight, Vector3 colliderCenter);
}
