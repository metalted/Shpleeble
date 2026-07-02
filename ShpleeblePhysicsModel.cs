using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Shpleeble
{
    public class ShpleeblePhysicsModel : MonoBehaviour
    {
        public void SetCosmetics(ShpleebleData data)
        {
            SetupModelCar smc = GetComponentInChildren<SetupModelCar>();
            ClearHatAndGlasses(smc);
            smc.DoCarSetup(data.ToCosmeticsV16(), true, false, true);
        }

        public void SetInternalScale(Vector3 fullScale)
        {
            Vector3 invertedScale = InvertScale(fullScale);
            SetupModelCar smc = GetComponentInChildren<SetupModelCar>();
            Transform suspension = null;

            foreach(Transform t in smc.transform)
            {
                if(t.gameObject.name == "Wheel Physics")
                {
                    foreach(Transform c in t)
                    {
                        if(c.gameObject.name == "Suspension")
                        {
                            suspension = c;
                            break;
                        }
                    }
                    break;
                }
            }

            suspension.transform.localScale = invertedScale;
        }

        private Vector3 InvertScale(Vector3 scale)
        {
            return new Vector3(
                InvertScaleAxis(scale.x),
                InvertScaleAxis(scale.y),
                InvertScaleAxis(scale.z)
            );
        }

        private float InvertScaleAxis(float value)
        {
            if (value == 0f)
            {
                return 0f;
            }

            float sign = Mathf.Sign(value);
            float inverse = 1f / Mathf.Abs(value);

            inverse = Mathf.Clamp01(inverse);

            return inverse * sign;
        }

        private void ClearHatAndGlasses(SetupModelCar setup)
        {
            if (setup == null)
                return;

            if (setup.hatParent != null)
            {
                for (int i = setup.hatParent.childCount - 1; i >= 0; i--)
                {
                    Transform child = setup.hatParent.GetChild(i);

                    if (child == null)
                        continue;

                    HatValues hatValues = child.GetComponent<HatValues>();

                    if (hatValues == null)
                        continue;

                    child.gameObject.SetActive(false);
                    GameObject.Destroy(child.gameObject);
                }
            }

            setup.theHat = null;
            setup.theGlasses = null;
        }
    }
}
