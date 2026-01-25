using TMPro;
using UnityEngine;

namespace Shpleeble
{

    /// <summary>
    /// Defines the modes a player character can be in.
    /// </summary>
    public enum CharacterMode
    {
        Build = 0,
        Race = 1,
        Paraglider = 2,
        Offroad = 3,
        Paint = 4,
        Treegun = 5,
        Soap = 7,
        Bulldozer = 8,
        None = 9
    }

    public class ShpleebleView
    {
        private SetupModelCar soapbox;
        private SetupModelCar cameraMan;
        private TextMeshPro displayName;
        private GameObject horn;
        private GameObject paraglider;
        private Transform armatureTop;
        private Transform[] wheels;
        private Transform[] offroadWheels;
        private Transform[] soapWheels;
        private GameObject bulldozer;
        private GameObject treegun;
        private GameObject roadBlock;
        private GameObject brush;

        public ShpleebleView(
            SetupModelCar soapbox,
            SetupModelCar cameraMan,
            TextMeshPro displayName,
            GameObject horn,
            GameObject paraglider,
            Transform armatureTop,
            Transform[] wheels,
            Transform[] offroadWheels,
            Transform[] soapWheels,
            GameObject bulldozer,
            GameObject treegun,
            GameObject roadBlock,
            GameObject brush)
        {
            this.soapbox = soapbox;
            this.cameraMan = cameraMan;
            this.displayName = displayName;
            this.horn = horn;
            this.paraglider = paraglider;
            this.armatureTop = armatureTop;
            this.wheels = wheels;
            this.offroadWheels = offroadWheels;
            this.soapWheels = soapWheels;
            this.bulldozer = bulldozer;
            this.treegun = treegun;
            this.roadBlock = roadBlock;
            this.brush = brush;
        }

        public void SetName(string name)
        {
            displayName.text = name;
        }

        public void ApplyCosmetics(CosmeticsV16 cosmetics)
        {
            soapbox.DoCarSetup(cosmetics, false, false, true);
            cameraMan.DoCarSetup(cosmetics, false, false, true);
        }

        private void SetActive(GameObject go, bool active)
        {
            if (go != null)
                go.SetActive(active);
        }

        private void SetActive(Transform[] group, bool active)
        {
            if (group == null) return;

            foreach (var t in group)
                t.gameObject.SetActive(active);
        }

        public void ApplyMode(CharacterMode mode)
        {
            // Defaults (safe baseline)
            SetActive(cameraMan.gameObject, false);
            SetActive(soapbox.gameObject, true);
            SetActive(paraglider.gameObject, false);
            SetActive(bulldozer.gameObject, false);

            SetActive(wheels, false);
            SetActive(soapWheels, false);
            SetActive(offroadWheels, false);

            switch (mode)
            {
                // =====================
                // BUILD-LIKE MODES
                // =====================
                case CharacterMode.Build:
                    SetActive(soapbox.gameObject, false);
                    SetActive(cameraMan.gameObject, true);

                    SetActive(roadBlock, true);
                    SetActive(treegun, false);
                    SetActive(brush, false);
                    break;
                case CharacterMode.Paint:
                    SetActive(soapbox.gameObject, false);
                    SetActive(cameraMan.gameObject, true);

                    SetActive(roadBlock, false);
                    SetActive(treegun, false);
                    SetActive(brush, true);
                    break;
                case CharacterMode.Treegun:
                    SetActive(soapbox.gameObject, false);
                    SetActive(cameraMan.gameObject, true);

                    SetActive(roadBlock, false);
                    SetActive(treegun, true);
                    SetActive(brush, false);
                    break;

                // =====================
                // RACE
                // =====================
                case CharacterMode.Race:
                    SetActive(wheels, true);
                    break;

                // =====================
                // OFFROAD
                // =====================
                case CharacterMode.Offroad:
                    SetActive(offroadWheels, true);
                    break;

                // =====================
                // SOAP
                // =====================
                case CharacterMode.Soap:
                    SetActive(soapWheels, true);
                    break;

                // =====================
                // BULLDOZER
                // =====================
                case CharacterMode.Bulldozer:
                    SetActive(bulldozer.gameObject, true);
                    SetActive(wheels, true);
                    break;

                // =====================
                // PARAGLIDER
                // =====================
                case CharacterMode.Paraglider:
                    SetActive(paraglider.gameObject, true);
                    SetActive(bulldozer.gameObject, false);

                    foreach (Transform t in paraglider.transform)
                    {
                        t.gameObject.SetActive(true);
                    }

                    SetActive(wheels, true);
                    break;
            }
        }

        public void SetUpperBodyRotation(Quaternion q)
        {
            armatureTop.localRotation = q;
        }

        public void FaceCamera()
        {
            if (Camera.main != null)
            {
                displayName.transform.LookAt(Camera.main.transform.position);
            }
        }

        public bool TryGetUpperBodyRotation(out Quaternion rotation)
        {
            if (armatureTop == null)
            {
                rotation = default;
                return false;
            }

            rotation = armatureTop.localRotation;
            return true;
        }

        public static ShpleebleView FromRoot(Transform root)
        {
            SetupModelCar soapbox = root.GetChild(0).GetComponent<SetupModelCar>();
            SetupModelCar cameraMan = root.GetChild(1).GetComponent<SetupModelCar>();
            TextMeshPro displayName = root.GetChild(2).GetComponent<TextMeshPro>();

            GameObject hornModel = soapbox.transform.Find("Visible Horn").gameObject;
            GameObject paragliderModel = soapbox.transform.Find("Glider").gameObject;
            GameObject bullDozerModel = soapbox.transform.Find("Bulldozer").gameObject;

            Transform armatureTop = cameraMan.transform.Find("Character/Armature/Top");

            Transform LFWheel = soapbox.transform.Find("New v16 Wheels/Left Front/WheelModel LF animator/WheelHolder LF/Wheel LF");
            Transform LFOffroad = soapbox.transform.Find("New v16 Wheels/Left Front/WheelModel LF animator/WheelHolder LF/Offroad Wheel LF");
            Transform LFSoap = soapbox.transform.Find("New v16 Wheels/Left Front/WheelModel LF animator/WheelHolder LF/Soapwheel LF");

            Transform RFWheel = soapbox.transform.Find("New v16 Wheels/Right Front/WheelModel RF animator/WheelHolder RF/Wheel RF");
            Transform RFOffroad = soapbox.transform.Find("New v16 Wheels/Right Front/WheelModel RF animator/WheelHolder RF/Offroad Wheel RF");
            Transform RFSoap = soapbox.transform.Find("New v16 Wheels/Right Front/WheelModel RF animator/WheelHolder RF/Soapwheel RF");

            Transform RRWheel = soapbox.transform.Find("New v16 Wheels/Right Rear/WheelModel RR animator/WheelHolder RR/Wheel RR");
            Transform RROffroad = soapbox.transform.Find("New v16 Wheels/Right Rear/WheelModel RR animator/WheelHolder RR/Offroad Wheel RR");
            Transform RRSoap = soapbox.transform.Find("New v16 Wheels/Right Rear/WheelModel RR animator/WheelHolder RR/Soapwheel RR");

            Transform LRWheel = soapbox.transform.Find("New v16 Wheels/Left Rear/WheelModel LR animator/WheelHolder LR/Wheel LR");
            Transform LROffroad = soapbox.transform.Find("New v16 Wheels/Left Rear/WheelModel LR animator/WheelHolder LR/Offroad Wheel LR");
            Transform LRSoap = soapbox.transform.Find("New v16 Wheels/Left Rear/WheelModel LR animator/WheelHolder LR/Soapwheel LR");

            Transform[] wheels = new Transform[] { LFWheel, RFWheel, RRWheel, LRWheel };
            Transform[] offroads = new Transform[] { LFOffroad, RFOffroad, RROffroad, LROffroad };
            Transform[] soaps = new Transform[] { LFSoap, RFSoap, RRSoap, LRSoap };

            Transform rightArm = cameraMan.transform.Find("Character/Armature/Top/Right Arm");
            GameObject treegun = rightArm.Find("TreeGun").gameObject;
            GameObject roadBlock = rightArm.Find("RoadBlock").gameObject;
            GameObject brush = rightArm.Find("Brush").gameObject;

            return new ShpleebleView(
                soapbox,
                cameraMan,
                displayName,
                hornModel,
                paragliderModel,
                armatureTop,
                wheels,
                offroads,
                soaps,
                bullDozerModel,
                treegun,
                roadBlock,
                brush
            );
        }
    }

}
