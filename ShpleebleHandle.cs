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

        // -----------------
        // Data / state
        // -----------------
        public void SetData(ShpleebleData data)
        {
            controller.SetShpleebleData(data);
        }

        public void SetMode(CharacterMode mode)
        {
            controller.SetMode(mode);
        }

        // -----------------
        // Movement
        // -----------------
        public void MoveTo(Vector3 position, bool instant = false)
        {
            controller.MoveTo(position, instant);
        }

        // -----------------
        // Rotation intents
        // -----------------

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

        // -----------------
        // Misc
        // -----------------
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
    }
}
