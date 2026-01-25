using UnityEngine;

namespace Shpleeble
{
    public class ShpleebleController : MonoBehaviour
    {
        public ShpleebleData Data { get; private set; }
        public CharacterMode Mode { get; private set; }

        private ShpleebleView view;
        private bool active;

        private Vector3 targetPosition;
        private Quaternion targetRotation;
        private Quaternion targetBodyRotation;
        private Quaternion targetArmatureRotation;
        private float maxMoveDuration = 0.3f;
        private float maxRotateDuration = 0.2f;

        public void Initialize(ShpleebleView view)
        {
            this.view = view;
        }

        public void SetShpleebleData(ShpleebleData data)
        {
            Data = data;
            view.SetName(data.name);
            view.ApplyCosmetics(data.ToCosmeticsV16());
            SetMode((CharacterMode)data.state);
        }

        public void SetMode(CharacterMode mode)
        {
            if (Mode == mode) return;
            Mode = mode;
            view.ApplyMode(mode);
        }

        public void MoveTo(Vector3 pos, bool instant)
        {
            targetPosition = pos;
            if (instant) transform.position = pos;
        }

        public void LookAt(Vector3 euler, bool instant)
        {
            targetRotation = Quaternion.Euler(euler);
            if (instant) transform.rotation = targetRotation;
        }

        public void LookUpperBody(float angle, bool instant)
        {
            targetArmatureRotation = Quaternion.Euler(0, 270f, 180f - angle);
            if (instant) view.SetUpperBodyRotation(targetArmatureRotation);
        }

        private void Update()
        {
            if (!active || view == null)
                return;

            view.FaceCamera();

            // -----------------
            // Movement
            // -----------------
            if (targetPosition != transform.position)
            {
                float distance = Vector3.Distance(transform.position, targetPosition);
                float moveDuration = distance / maxMoveDuration;

                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    moveDuration * Time.deltaTime
                );
            }

            // -----------------
            // Rotation by mode
            // -----------------
            switch (Mode)
            {
                case CharacterMode.Build:
                case CharacterMode.Paint:
                case CharacterMode.Treegun:
                    HandleBuildModeRotation();
                    break;

                case CharacterMode.Race:
                case CharacterMode.Paraglider:
                case CharacterMode.Offroad:
                case CharacterMode.Soap:
                case CharacterMode.Bulldozer:
                    HandleRaceModeRotation();
                    break;
            }
        }

        private void HandleBuildModeRotation()
        {
            // Upper body (armature)
            if (view.TryGetUpperBodyRotation(out Quaternion currentArmature))
            {
                if (targetArmatureRotation != currentArmature)
                {
                    float angle = Quaternion.Angle(currentArmature, targetArmatureRotation);
                    float rotateDuration = angle / maxRotateDuration;

                    Quaternion next = Quaternion.RotateTowards(
                        currentArmature,
                        targetArmatureRotation,
                        rotateDuration * Time.deltaTime
                    );

                    view.SetUpperBodyRotation(next);
                }
            }

            // Full body
            if (targetBodyRotation != transform.rotation)
            {
                float angle = Quaternion.Angle(transform.rotation, targetBodyRotation);
                float rotateDuration = angle / maxRotateDuration;

                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetBodyRotation,
                    rotateDuration * Time.deltaTime
                );
            }
        }

        private void HandleRaceModeRotation()
        {
            if (targetRotation != transform.rotation)
            {
                float angle = Quaternion.Angle(transform.rotation, targetRotation);
                float rotateDuration = angle / maxRotateDuration;

                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    rotateDuration * Time.deltaTime
                );
            }
        }

        public void Activate()
        {
            active = true;
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            active = false;
            gameObject.SetActive(false);
        }
    }
}
