using OWML.Common;
using OWML.ModHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace SkinChanger
{
	public class SkinChanger : ModBehaviour
	{
		public static SkinChanger instance;

		// Action for QSB compat to listen to
		public Action<string> skinChanged;

		public PlayerCameraController PlayerCamera;
		// cant use array cuz one of them is a shape :((((
		public CapsuleCollider PlayerCollider1;
		public CapsuleCollider PlayerCollider2;
		public CapsuleShape PlayerCollider3;
		public GameObject Traveller_HEA_Player_v2;

		Dictionary<string, GameObject> prefabs = new();
		public List<PlayableCharacter> characters;

		public class PlayableCharacter
		{
			public GameObject GameObject;
			public string SettingName;
			public Vector3 CameraOffset;

			public float ColliderRadius;
			public float ColliderHeight;
			public Vector3 ColliderCenter;

			public PlayableCharacter(string prefabName, string settingName, Vector3 camOffset, float collRadius, float collHeight, Vector3 collCenter)
			{
				GameObject = prefabName != null ? Instantiate(instance.prefabs[prefabName]) : instance.Traveller_HEA_Player_v2;
				SettingName = settingName;
				CameraOffset = camOffset;

				ColliderRadius = collRadius;
				ColliderHeight = collHeight;
				ColliderCenter = collCenter;
			}
		}

		// stolen from qsb and nh LOLOLLOLOLOLO
		public static void ReplaceShaders(GameObject prefab)
		{
			foreach (var renderer in prefab.GetComponentsInChildren<Renderer>(true))
			{
				foreach (var material in renderer.sharedMaterials)
				{
					if (material == null) continue;

					var replacementShader = Shader.Find(material.shader.name);
					if (replacementShader == null) continue;

					// preserve override tag and render queue (for Standard shader)
					// keywords and properties are already preserved
					if (material.renderQueue != material.shader.renderQueue)
					{
						var renderType = material.GetTag("RenderType", false);
						var renderQueue = material.renderQueue;
						material.shader = replacementShader;
						material.SetOverrideTag("RenderType", renderType);
						material.renderQueue = renderQueue;
					}
					else
					{
						material.shader = replacementShader;
					}
				}
			}
		}

		private void Start()
		{
			instance = this;

			// Enable QSB compat if required
			if (ModHelper.Interaction.ModExists("Raicuparta.QuantumSpaceBuddies"))
			{
				// QSB compat has to be in a different DLL since it depends on the QSB DLL.
				// Adapted from QSB-NH compat in Quantum Space Buddies
				var skinChangerQSB = Assembly.LoadFrom(Path.Combine(ModHelper.Manifest.ModFolderPath, "SkinChangerQSB.dll"));
				gameObject.AddComponent(skinChangerQSB.GetType("SkinChangerQSB.SkinChangerQSB", true));
			}

			foreach (var path in Directory.EnumerateFiles(Path.Combine(ModHelper.Manifest.ModFolderPath, "Assets")))
			{
				if (Path.GetExtension(path) == ".manifest") continue; // ignore the non bundle files

				ModHelper.Console.WriteLine($"loading async {path}");
				var req1 = AssetBundle.LoadFromFileAsync(path);
				req1.completed += _ =>
				{
					var req2 = req1.assetBundle.LoadAllAssetsAsync<GameObject>();
					req2.completed += _ =>
					{
						var prefab = (GameObject)req2.asset;
						ReplaceShaders(prefab);
						prefabs.Add(prefab.name, prefab);
						ModHelper.Console.WriteLine($"loaded prefab {prefab}");
					};
				};
			}

			LoadManager.OnCompleteSceneLoad += (scene, loadScene) =>
			{
				if (loadScene is not (OWScene.SolarSystem or OWScene.EyeOfTheUniverse)) return;

				var player = GameObject.Find("Player_Body");
				PlayerCamera = player.transform.Find("PlayerCamera").GetComponent<PlayerCameraController>();

				PlayerCollider1 = player.GetComponent<CapsuleCollider>();
				PlayerCollider2 = player.transform.Find("PlayerDetector").GetComponent<CapsuleCollider>();
				PlayerCollider3 = player.transform.Find("PlayerDetector").GetComponent<CapsuleShape>();

				Traveller_HEA_Player_v2 = player.transform.Find("Traveller_HEA_Player_v2").gameObject;

				// make the instances of the prefabs
				characters = new List<PlayableCharacter>()
				{
					//Unity Object name                                         //Setting Name      //Camera Offset                     //Collider Radius, Height, Center
					new PlayableCharacter("Traveller_HEA_Player_v0", "N0", new Vector3(0, 0.8496093f, 0.15f), 0.5f, 2f, Vector3.zero),
					new PlayableCharacter("Traveller_HEA_Player_v1", "N1", new Vector3(0, 0.8496093f, 0.15f), 0.5f, 2f, Vector3.zero),
					new PlayableCharacter("Traveller_HEA_Player_v3", "Inhabitant", new Vector3(0, 2.2f, 0.27f), 0.5f, 3.6f, new Vector3(0f, 0.875f, 0f)),
					new PlayableCharacter("Traveller_HEA_Player_v4", "Nomai", new Vector3(0, 1.1f, 0.3f), 0.5f, 2.5f, new Vector3(0f, 0.3f, 0f)),
					new PlayableCharacter("Traveller_HEA_Player_v5", "Chert", new Vector3(0, 0.3f, 0.2f), 0.5f, 1.5f, new Vector3(0f, -0.2f, 0f)),
					new PlayableCharacter("Traveller_HEA_Player_v6", "Esker", new Vector3(0, 0.9f, 0.2f), 0.5f, 2f, Vector3.zero),
					new PlayableCharacter("Traveller_HEA_Player_v7", "Riebeck", new Vector3(0, 1.1f, 0.3f), 0.5f, 2.5f, new Vector3(0f, 0.2f, 0f)),
					new PlayableCharacter("Traveller_HEA_Player_v8", "Gabbro", new Vector3(0, 1.1f, 0.2f), 0.5f, 2.5f, new Vector3(0f, 0.2f, 0)),
					new PlayableCharacter("Traveller_HEA_Player_v9", "Feldspar", new Vector3(0f, 0.4f, 0.2f), 0.5f, 2f, Vector3.zero),
					new PlayableCharacter("Traveller_HEA_Player_v10", "Slate", new Vector3(0, 1.2f, 0.2f), 0.5f, 2.5f, new Vector3(0f, 0.2f, 0)),
					new PlayableCharacter("Traveller_HEA_Player_v11", "Hal", new Vector3(0, 0.8496093f, 0.15f), 0.5f, 2f, Vector3.zero),
					new PlayableCharacter("Traveller_HEA_Player_v12", "Hornfels", new Vector3(0, 1.2f, 0.2f), 0.5f, 2.5f, new Vector3(0f, 0.2f, 0)),
					new PlayableCharacter("Traveller_HEA_Player_v13", "Gossan", new Vector3(0f, 0.5f, 0.1f), 0.5f, 2f, Vector3.zero),
					new PlayableCharacter("Traveller_HEA_Player_v14", "Mica", new Vector3(0, 0.3f, 0.1f), 0.5f, 1.5f, new Vector3(0f, -0.2f, 0)),
					new PlayableCharacter("Traveller_HEA_Player_v15", "Arkose", new Vector3(0, 0.3f, 0.1f), 0.5f, 1.5f, new Vector3(0f, -0.2f, 0)),
					new PlayableCharacter("Traveller_HEA_Player_v16", "Tephra", new Vector3(0, 0.3f, 0.1f), 0.5f, 1.5f, new Vector3(0f, -0.2f, 0)),
					new PlayableCharacter("Traveller_HEA_Player_v17", "Galena", new Vector3(0, 0.3f, 0.1f), 0.5f, 1.5f, new Vector3(0f, -0.2f, 0)),
					new PlayableCharacter("Traveller_HEA_Player_v18", "Spinel", new Vector3(0, 0.7f, 0.2f), 0.5f, 2f, Vector3.zero),
					new PlayableCharacter("Traveller_HEA_Player_v19", "Porphy", new Vector3(0, 1.3f, 0.2f), 0.5f, 2.5f, new Vector3(0f, 0.25f, 0)),
					new PlayableCharacter("Traveller_HEA_Player_v20", "Rutile", new Vector3(0, 0.8496093f, 0.15f), 0.5f, 2f, Vector3.zero),
					new PlayableCharacter("Traveller_HEA_Player_v21", "Marl", new Vector3(0, 1.3f, 0.25f), 0.5f, 2.5f, new Vector3(0f, 0.25f, 0)),
					new PlayableCharacter("Traveller_HEA_Player_v22", "Gneiss", new Vector3(0f, 0.4f, 0.2f), 0.5f, 2f, Vector3.zero),
					new PlayableCharacter("Traveller_HEA_Player_v23", "Moraine", new Vector3(0, 0.3f, 0.1f), 0.5f, 1.5f, new Vector3(0f, -0.2f, 0)),
					new PlayableCharacter("Traveller_HEA_Player_v24", "Tuff", new Vector3(0, 0.8496093f, 0.15f), 0.5f, 2f, Vector3.zero),
					new PlayableCharacter("Traveller_HEA_Player_v25", "Tektite", new Vector3(0, 1.2f, 0.2f), 0.5f, 2.5f, new Vector3(0f, 0.25f, 0))
				};

				// parent them to the player
				foreach (var character in characters)
				{
					character.GameObject.transform.SetParent(player.transform, false);
					// this terrible position comes from vanilla player. cry about it
					character.GameObject.transform.localPosition = new Vector3(0, -1.03f, -0.2f);
					character.GameObject.transform.localScale = Vector3.one * .1f;
				}

				ChangeSkin();
			};
		}

		public override void Configure(IModConfig config)
		{
			if (LoadManager.s_currentScene is not (OWScene.SolarSystem or OWScene.EyeOfTheUniverse)) return;
			ChangeSkin();
		}

		const string ModelSetting = "Model";
		const string ChangeCamera = "Change Camera";
		const string ChangeCollider = "Change Collider";

		void ChangeSkin()
		{
			var modelSetting = ModHelper.Config.GetSettingsValue<string>(ModelSetting);
			var useCamera = ModHelper.Config.GetSettingsValue<bool>(ChangeCamera);
			var useCollider = ModHelper.Config.GetSettingsValue<bool>(ChangeCollider);

			if (!useCamera)
			{
				PlayerCamera._origLocalPosition = characters[0].CameraOffset;
			}

			if (!useCollider)
			{
				PlayerCollider1.radius = characters[0].ColliderRadius;
				PlayerCollider2.radius = characters[0].ColliderRadius;
				PlayerCollider3.radius = characters[0].ColliderRadius;
				PlayerCollider1.height = characters[0].ColliderHeight;
				PlayerCollider2.height = characters[0].ColliderHeight;
				PlayerCollider3.height = characters[0].ColliderHeight;
				PlayerCollider1.center = characters[0].ColliderCenter;
				PlayerCollider2.center = characters[0].ColliderCenter;
				PlayerCollider3.center = characters[0].ColliderCenter;
			}

			foreach (var character in characters)
			{
				if (modelSetting == character.SettingName)
				{
					character.GameObject.SetActive(true);

					if (useCamera)
					{
						PlayerCamera._origLocalPosition = character.CameraOffset;
					}

					if (useCollider)
					{
						PlayerCollider1.radius = character.ColliderRadius;
						PlayerCollider2.radius = character.ColliderRadius;
						PlayerCollider3.radius = character.ColliderRadius;
						PlayerCollider1.height = character.ColliderHeight;
						PlayerCollider2.height = character.ColliderHeight;
						PlayerCollider3.height = character.ColliderHeight;
						PlayerCollider1.center = character.ColliderCenter;
						PlayerCollider2.center = character.ColliderCenter;
						PlayerCollider3.center = character.ColliderCenter;
					}
				}
				else
				{
					character.GameObject.SetActive(false);
				}
			}

			skinChanged?.Invoke(modelSetting);
		}
	}
}
