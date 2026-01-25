using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace Shpleeble
{
    public class ShpleeblePrefabCache
    {
        public bool IsReady => _prefabController != null;

        private ShpleebleController _prefabController;

        public void TryCaptureFromMainMenu()
        {
            if (_prefabController != null)
                return;

            NetworkedGhostSpawner spawner = GameObject.FindObjectOfType<NetworkedGhostSpawner>(true);

            if (spawner == null)
            {
                return;
            }

            _prefabController = BuildPrefab(spawner);

            GameObject prefabRoot = _prefabController.gameObject;
            GameObject.DontDestroyOnLoad(prefabRoot);
            prefabRoot.SetActive(false);
        }

        public ShpleebleController GetPrefab()
        {
            return _prefabController;
        }

        private ShpleebleController BuildPrefab(NetworkedGhostSpawner spawner)
        {
            // =========================
            // ROOT
            // =========================
            GameObject root = new GameObject("Shpleeble_BasePrefab");
            ShpleebleController controller = root.AddComponent<ShpleebleController>();

            // =========================
            // SOAPBOX
            // =========================
            SetupModelCar soapbox = GameObject
                .Instantiate(
                    spawner.zeepkistGhostPrefab.ghostModel.transform,
                    root.transform
                )
                .GetComponent<SetupModelCar>();

            // Remove ghost wheel animation scripts
            Ghost_AnimateWheel[] ghostWheels =
                soapbox.GetComponentsInChildren<Ghost_AnimateWheel>(true);

            foreach (var w in ghostWheels)
                GameObject.Destroy(w);

            // Remove ghost wheel animation scripts
            Ghost_AnimateWheel_v16[] ghostWheels16 = soapbox.GetComponentsInChildren<Ghost_AnimateWheel_v16>(true);

            foreach (var w in ghostWheels16)
            {
                GameObject.Destroy(w);
            }

            // Re-parent arms to armature top
            Transform soapboxArmatureTop = soapbox.transform.Find("Character/Armature/Top");
            Transform soapboxLeftArm = soapbox.transform.Find("Character/Left Arm");
            Transform soapboxRightArm = soapbox.transform.Find("Character/Right Arm");

            soapboxLeftArm.SetParent(soapboxArmatureTop);
            soapboxLeftArm.localPosition = new Vector3(-0.25f, 0f, 1.25f);
            soapboxLeftArm.localEulerAngles = new Vector3(0f, 240f, 0f);

            soapboxRightArm.SetParent(soapboxArmatureTop);
            soapboxRightArm.localPosition = new Vector3(-0.25f, 0f, -1.25f);
            soapboxRightArm.localEulerAngles = new Vector3(0f, 120f, 0f);

            // =========================
            // CAMERA MAN
            // =========================
            SetupModelCar cameraMan = GameObject
                .Instantiate(
                    spawner.zeepkistGhostPrefab.cameraManModel.transform,
                    root.transform
                )
                .GetComponent<SetupModelCar>();

            GameObject camera = cameraMan
                .transform
                .Find("Character/Right Arm/Camera")
                .gameObject;

            camera.SetActive(false);

            Transform cameraArmatureTop = cameraMan.transform.Find("Character/Armature/Top");
            Transform cameraLeftArm = cameraMan.transform.Find("Character/Left Arm");
            Transform cameraRightArm = cameraMan.transform.Find("Character/Right Arm");

            cameraLeftArm.SetParent(cameraArmatureTop);
            cameraLeftArm.localPosition = new Vector3(-0.25f, 0f, 1.25f);
            cameraLeftArm.localEulerAngles = new Vector3(0f, 240f, 0f);

            cameraRightArm.SetParent(cameraArmatureTop);
            cameraRightArm.localPosition = new Vector3(-0.25f, 0f, -1.25f);
            cameraRightArm.localEulerAngles = new Vector3(0f, 120f, 0f);

            cameraMan.transform.localPosition = new Vector3(0, 0.62f, 0);

            // =========================
            // DISPLAY NAME
            // =========================
            TextMeshPro displayName = GameObject
                .Instantiate(
                    spawner.zeepkistGhostPrefab.nameDisplay.transform,
                    root.transform
                )
                .GetComponent<TextMeshPro>();

            DisplayPlayerName nameScript = displayName.GetComponent<DisplayPlayerName>();
            if (nameScript != null)
                GameObject.Destroy(nameScript);

            Transform hoethouder = displayName.transform.Find("hoethouder");
            if (hoethouder != null)
                GameObject.Destroy(hoethouder.gameObject);

            displayName.transform.localScale = new Vector3(-1f, 1f, 1f);

            // =========================
            // OTHER OBJECTS
            // =========================
            GameObject hornModel = soapbox.transform.Find("Visible Horn").gameObject;
            hornModel.SetActive(false);

            GameObject paragliderModel = soapbox.transform.Find("Glider").gameObject;
            foreach (Transform t in paragliderModel.transform)
                t.gameObject.SetActive(true);

            paragliderModel.SetActive(false);

            // =========================
            // MODE OBJECTS
            // =========================

            GameObject house = GameObject.Find("House");
            GameObject treegun = house.transform.Find("TreeGun").gameObject;
            GameObject roadBlock = house.transform.Find("MainMenu_Block/Road_Step_Up_Medium (1)").gameObject;
            GameObject scenery = GameObject.Find("Scenery");
            GameObject sakura = scenery.transform.Find("Normal Tree Sakura/IgnoreEdgeBugCheck (1)").gameObject;

            GameObject treegunCopy = GameObject.Instantiate(treegun);
            treegunCopy.name = "TreeGun";
            GameObject roadBlockCopy = GameObject.Instantiate(roadBlock);
            roadBlockCopy.name = "RoadBlock";
            GameObject sakuraCopy = GameObject.Instantiate(sakura);
            sakuraCopy.name = "Brush";

            treegunCopy.transform.parent = cameraRightArm;
            roadBlockCopy.transform.parent = cameraRightArm;
            sakuraCopy.transform.parent = cameraRightArm;

            treegunCopy.transform.localPosition = new Vector3(-0.5f, 0.59f, 1f);
            treegunCopy.transform.localEulerAngles = new Vector3(9f, 336f, 8f);
            treegunCopy.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            roadBlockCopy.transform.localPosition = new Vector3(0.25f, 0.16f, 0.93f);
            roadBlockCopy.transform.localEulerAngles = new Vector3(0f, 330f, 9f);
            roadBlockCopy.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);

            sakuraCopy.transform.localPosition = new Vector3(0, 0.1f, 1f);
            sakuraCopy.transform.localEulerAngles = new Vector3(0, 0, 0);
            sakuraCopy.transform.localScale = new Vector3(0.03f, 0.03f, 0.06f);

            treegunCopy.SetActive(true);
            roadBlockCopy.SetActive(true);
            sakuraCopy.SetActive(true);

            root.SetActive(false);
            GameObject.DontDestroyOnLoad(root);

            return controller;
        }
    }
}