using Epic.OnlineServices;
using OWML.Common;
using OWML.ModHelper;
using OWML.ModHelper.Events;
using System.Collections.Generic;
using UnityEngine;


namespace SkinChanger
{
    public class SkinChanger : ModBehaviour
    {
        List<PlayableCharacter> characters;

        public GameObject playerNude;
        public GameObject playerClothed;

        public PlayerCameraController CamOffset;
        public CapsuleCollider PlayerCollider;
        public bool IsPlayerSuited;
        const string Hatchling = "Hatchling";

        public static SkinChanger instance;

    class PlayableCharacter
        {
            public GameObject gameObject;
            public string SettingName;
            public Vector3 CameraOffset;

            public float ColliderRadius;
            public float ColliderHeight;
            public Vector3 ColliderCenter;

            public PlayableCharacter(string name, string settingName, Vector3 camOffset, float collRadius, float collHeight, Vector3 collCenter)
            {
                gameObject = GameObject.Find(name);
                if (gameObject == null)
                {
                    instance.ModHelper.Console.WriteLine(name + " JKLLLLLLLLLLLLLLLLLLLLLLL,DSDK/.CGBJKHG,BUHASO,L.BFGN,LUSNV,CCCCCCBVGLRFEWIHSD");
                }
                SettingName = settingName;
                CameraOffset = camOffset;
                ColliderRadius = collRadius;
                ColliderHeight = collHeight;
                ColliderCenter = collCenter;
            }
        }

        private void Start()
        {
            instance = this;

            // Starting here, you'll have access to OWML's mod helper.
            ModHelper.Console.WriteLine("Other Playable Characters is loaded!", MessageType.Success);

            // Get the New Horizons API and load configs
            var newHorizons = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
            newHorizons.LoadConfigs(this);

            {
                GlobalMessenger.AddListener("SuitUp", () =>
                {
                    IsPlayerSuited = true;
                    ModHelper.Console.WriteLine(IsPlayerSuited);
                });

                GlobalMessenger.AddListener("RemoveSuit", () =>
                {
                    IsPlayerSuited = false;
                    ModHelper.Console.WriteLine(IsPlayerSuited);
                });
            }

            // Example of accessing game code.
            LoadManager.OnCompleteSceneLoad += (scene, loadScene) =>
            {
                if (loadScene != OWScene.SolarSystem) return;
                ModHelper.Console.WriteLine("Loaded into solar system!", MessageType.Success);
            };

            newHorizons.GetStarSystemLoadedEvent().AddListener((system) =>
            {
                var Cam = GameObject.Find("PlayerCamera");
                CamOffset = Cam.GetComponent<PlayerCameraController>();

                playerNude = GameObject.Find("Traveller_HEA_Player_v0");
                playerClothed = GameObject.Find("Traveller_HEA_Player_v1");
                PlayerCollider = GameObject.Find("Player_Body").GetComponent<CapsuleCollider>();

                characters = new List<PlayableCharacter>()
                {                          //Unity Object name      //Setting Name  //Camera Offset                     //Collider Radius, Height, Center
                    new PlayableCharacter("Traveller_HEA_Player_v0", Hatchling,     new Vector3(0, 0.8496093f, 0.15f),  0.5f, 2f,   Vector3.zero),
                    new PlayableCharacter("Traveller_HEA_Player_v1", Hatchling,     new Vector3(0, 0.8496093f, 0.15f),  0.5f, 2f,   Vector3.zero),
                    new PlayableCharacter("Traveller_HEA_Player_v3", "Inhabitant",  new Vector3(0, 2.2f, 0.27f),        0.5f, 3.6f, new Vector3(0f, 0.875f, 0f)),
                    new PlayableCharacter("Traveller_HEA_Player_v4", "Nomai",       new Vector3(0, 1.1f, 0.3f),         0.5f, 2.5f, new Vector3(0f, 0.3f, 0f)),
                    new PlayableCharacter("Traveller_HEA_Player_v5", "Chert",       new Vector3(0, 0.3f, 0.2f),         0.5f, 1.5f, new Vector3(0f, -0.2f, 0f)),
                    new PlayableCharacter("Traveller_HEA_Player_v6", "Esker",       new Vector3(0, 0.9f, 0.2f),         0.5f, 2f,   Vector3.zero),
                    new PlayableCharacter("Traveller_HEA_Player_v7", "Riebeck",     new Vector3(0, 1.1f, 0.3f),         0.5f, 2.5f, new Vector3(0f, 0.2f, 0f)),
                    new PlayableCharacter("Traveller_HEA_Player_v8", "Gabbro",      new Vector3(0, 1.1f, 0.2f),         0.5f, 2.5f, new Vector3(0f, 0.2f, 0)),
                    new PlayableCharacter("Traveller_HEA_Player_v9", "Feldspar",    new Vector3(0f, 0.4f, 0.2f),        0.5f, 2f,   Vector3.zero),
                    new PlayableCharacter("Traveller_HEA_Player_v10","Slate",       new Vector3(0, 1.2f, 0.2f),         0.5f, 2.5f, new Vector3(0f, 0.2f, 0)),
                    new PlayableCharacter("Traveller_HEA_Player_v11","Hal",         new Vector3(0, 0.8496093f, 0.15f),  0.5f, 2f,   Vector3.zero),
                    new PlayableCharacter("Traveller_HEA_Player_v12","Hornfels",    new Vector3(0, 1.2f, 0.2f),         0.5f, 2.5f, new Vector3(0f, 0.2f, 0)),
                    new PlayableCharacter("Traveller_HEA_Player_v13","Gossan",      new Vector3(0f, 0.4f, 0.2f),        0.5f, 2f,   Vector3.zero),
                    new PlayableCharacter("Traveller_HEA_Player_v14","Mica",        new Vector3(0, 0.3f, 0.1f),        0.5f, 1.5f, new Vector3(0f, -0.2f, 0)),
                    new PlayableCharacter("Traveller_HEA_Player_v15","Arkose",      new Vector3(0, 0.3f, 0.1f),        0.5f, 1.5f, new Vector3(0f, -0.2f, 0)),
                    new PlayableCharacter("Traveller_HEA_Player_v16","Tephra",      new Vector3(0, 0.3f, 0.1f),        0.5f, 1.5f, new Vector3(0f, -0.2f, 0)),
                    new PlayableCharacter("Traveller_HEA_Player_v17","Galena",      new Vector3(0, 0.3f, 0.1f),        0.5f, 1.5f, new Vector3(0f, -0.2f, 0)),
                    new PlayableCharacter("Traveller_HEA_Player_v18","Spinel",      new Vector3(0, 0.7f, 0.2f),         0.5f, 2f,   Vector3.zero),
                    new PlayableCharacter("Traveller_HEA_Player_v19","Porphy",      new Vector3(0, 1.3f, 0.2f),         0.5f, 2.5f, new Vector3(0f, 0.25f, 0)),
                    new PlayableCharacter("Traveller_HEA_Player_v20","Rutile",      new Vector3(0, 0.8496093f, 0.15f),  0.5f, 2f,   Vector3.zero),
                    new PlayableCharacter("Traveller_HEA_Player_v21","Marl",        new Vector3(0, 1.3f, 0.2f),         0.5f, 2.5f, new Vector3(0f, 0.25f, 0)),
                    new PlayableCharacter("Traveller_HEA_Player_v22","Gneiss",      new Vector3(0f, 0.4f, 0.2f),        0.5f, 2f,   Vector3.zero),
                    new PlayableCharacter("Traveller_HEA_Player_v23","Moraine",     new Vector3(0, 0.3f, 0.1f),        0.5f, 1.5f, new Vector3(0f, -0.2f, 0)),
                    new PlayableCharacter("Traveller_HEA_Player_v24","Tuff",        new Vector3(0, 0.8496093f, 0.15f),  0.5f, 2f,   Vector3.zero),
                    new PlayableCharacter("Traveller_HEA_Player_v25","Tektite",     new Vector3(0, 1.2f, 0.2f),         0.5f, 2.5f, new Vector3(0f, 0.25f, 0))

                };

                ChangeSkin();
            });
        }
        public override void Configure(IModConfig config)
        {
            if (LoadManager.s_currentScene != OWScene.SolarSystem && LoadManager.s_currentScene != OWScene.EyeOfTheUniverse) return;
            ChangeSkin();
        }

        const string ModelSetting = "Model";
        const string ChangeCollider = "Change Collider";
        void ChangeSkin()
        {
            var modelSetting = ModHelper.Config.GetSettingsValue<string>(ModelSetting);
            var useCollider = ModHelper.Config.GetSettingsValue<bool>(ChangeCollider);

            if (!useCollider)
            {
                PlayerCollider.radius = characters[0].ColliderRadius;
                PlayerCollider.height = characters[0].ColliderHeight;
                PlayerCollider.center = characters[0].ColliderCenter;
            }

            foreach (var character in characters)
            {
                if (modelSetting == character.SettingName)
                {
                    character.gameObject.SetActive(true);
                    CamOffset._origLocalPosition = character.CameraOffset;

                    if (useCollider) {
                        PlayerCollider.radius = character.ColliderRadius;
                        PlayerCollider.height = character.ColliderHeight;
                        PlayerCollider.center = character.ColliderCenter;
                    }
                }
                else {
                    character.gameObject.SetActive(false);
                }
            }
        }
        void Update() //Force player active
        {
            var modelSetting = ModHelper.Config.GetSettingsValue<string>(ModelSetting);
            if (modelSetting == Hatchling)
            {
                playerNude.SetActive(!IsPlayerSuited);
                playerClothed.SetActive(IsPlayerSuited);
            }
        }
    }
}
