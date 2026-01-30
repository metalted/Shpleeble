using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Shpleeble
{
    public class ShpleebleHandle
    {
        private readonly ShpleebleController controller;

        internal ShpleebleHandle(ShpleebleController controller)
        {
            this.controller = controller;
        }

        public ShpleebleData Data => controller.Data;
        public CharacterMode Mode => controller.Mode;

        // Data / state
        public void SetData(ShpleebleData data)
        {
            controller.SetShpleebleData(data);
        }

        public void SetMode(CharacterMode mode)
        {
            controller.SetMode(mode);
        }

        // Movement
        public void MoveTo(Vector3 position, bool instant = false)
        {
            controller.MoveTo(position, instant);
        }

        // Rotation intents

        // For race-like modes (single rigid body)
        public void SetRaceRotation(Quaternion rotation, bool instant = false)
        {
            controller.SetRaceRotation(rotation, instant);
        }

        // For build / character modes (full body yaw)
        public void SetBodyRotation(float yawDegrees, bool instant = false)
        {
            controller.SetBodyRotation(yawDegrees, instant);
        }

        // For build / character modes (upper body / "head")
        public void SetUpperBodyRotation(float pitchDegrees, bool instant = false)
        {
            controller.SetUpperBodyRotation(pitchDegrees, instant);
        }

        // Misc
        public void SetHorn(bool active)
        {
            controller.SetHorn(active);
        }

        public void Activate()
        {
            controller.Activate();
        }

        public void Deactivate()
        {
            controller.Deactivate();
        }

        //Snapshot
        public void ApplySnapshot(Vector3 position, Vector3 eulerRotation, CharacterMode mode, bool instant = false)
        {
            // 1. Mode first (important for controller behavior)
            controller.SetMode(mode);

            // 2. Position is always authoritative
            controller.MoveTo(position, instant);

            // 3. Rotation depends on mode
            switch (mode)
            {
                // Vehicle / race-like modes
                case CharacterMode.Race:
                case CharacterMode.Offroad:
                case CharacterMode.Soap:
                case CharacterMode.Paraglider:
                case CharacterMode.Bulldozer:
                    Quaternion fullRotation = Quaternion.Euler(eulerRotation);
                    controller.SetRaceRotation(fullRotation, instant);
                    break;

                // Character / build modes
                case CharacterMode.Build:
                case CharacterMode.Paint:
                case CharacterMode.Treegun:
                    float bodyYaw = eulerRotation.y;
                    float bodyPitch = eulerRotation.x;

                    controller.SetBodyRotation(bodyYaw, instant);
                    controller.SetUpperBodyRotation(bodyPitch, instant);
                    break;                    
            }
        }
    }
}
