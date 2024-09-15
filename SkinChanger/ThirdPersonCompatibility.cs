﻿using UnityEngine;
using UnityEngine.Events;

namespace SkinChanger;

public class ThirdPersonCompatibility : MonoBehaviour
{
	public interface ICommonCameraAPI
	{
		UnityEvent<bool> HeadVisibilityChanged();
	}

	private bool _isHeadVisible = false;

	public const string INHABITANT_HEAD_PATH = "player_mesh_noSuit:Traveller_HEA_Player/Ghostbird_Skin_01:Ghostbird_Rig_V01:Base/Ghostbird_Skin_01:Ghostbird_Rig_V01:Root/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine03/Ghostbird_Skin_01:Ghostbird_Rig_V01:Spine04/Ghostbird_Skin_01:Ghostbird_Rig_V01:Neck01/Ghostbird_Skin_01:Ghostbird_Rig_V01:Neck02/Ghostbird_Skin_01:Ghostbird_Rig_V01:Head/player_mesh_noSuit:Traveller_HEA_Player (1)";
	public const string NOMAI_HEAD_PATH = "Nomai_ANIM_SkyWatching_Idle/Nomai_Mesh:Mesh/Nomai_Mesh:Props_NOM_Mask_GearNew/Nomai_Mesh:Props_NOM_Mask_GearNew_Geo";

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
			"Inhabitant" => SkinChanger.instance.CurrentCharacter.GameObject.transform.Find(INHABITANT_HEAD_PATH),
			"Nomai" => SkinChanger.instance.CurrentCharacter.GameObject.transform.Find(NOMAI_HEAD_PATH),
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
