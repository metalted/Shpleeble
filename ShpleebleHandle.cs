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

        public void SetData(ShpleebleData data)
        {
            controller.SetShpleebleData(data);
        }

        public void SetMode(CharacterMode mode) => controller.SetMode(mode);

        public void MoveTo(Vector3 position, bool instant = false) => controller.MoveTo(position, instant);

        public void LookAt(Vector3 euler, bool instant = false) => controller.LookAt(euler, instant);

        public void LookUpperBody(float angle, bool instant = false) => controller.LookUpperBody(angle, instant);

        public void Activate() => controller.Activate();
        public void Deactivate() => controller.Deactivate();
    }
}
