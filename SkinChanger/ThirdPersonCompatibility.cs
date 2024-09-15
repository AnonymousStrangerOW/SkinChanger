using UnityEngine;
using UnityEngine.Events;

namespace SkinChanger;

public class ThirdPersonCompatibility : MonoBehaviour
{
	public interface ICommonCameraAPI
	{
		UnityEvent<bool> HeadVisibilityChanged();
	}

	private bool _isHeadVisible = false;

	public void Start()
	{
		var commonCameraAPI = SkinChanger.instance.ModHelper.Interaction.TryGetModApi<ICommonCameraAPI>("xen.CommonCameraUtility");

		commonCameraAPI.HeadVisibilityChanged().AddListener(UpdateHeadVisibility);

		SkinChanger.instance.skinChanged += (_) => UpdateHeadVisibility(_isHeadVisible);
	}

	private void UpdateHeadVisibility(bool visible)
	{
		_isHeadVisible = visible;

		var head = SkinChanger.instance.CurrentCharacter.SettingName switch
		{
			"Inhabitant" => SkinChanger.instance.CurrentCharacter.GameObject.transform.Find("player_mesh_noSuit:Traveller_HEA_Player (1)"),
			"Nomai" => SkinChanger.instance.CurrentCharacter.GameObject.transform.Find("Nomai_ANIM_SkyWatching_Idle/Nomai_Mesh:Mesh/Nomai_Mesh:Props_NOM_Mask_GearNew/Nomai_Mesh:Props_NOM_Mask_GearNew_Geo"),
			_ => null,
		};
		
		if (head != null)
		{
			foreach (var t in head.GetComponentsInChildren<Transform>())
			{
				t.gameObject.layer = visible ? LayerMask.NameToLayer("Default") : LayerMask.NameToLayer("VisibleToProbe");
			}
		}
	}
}
