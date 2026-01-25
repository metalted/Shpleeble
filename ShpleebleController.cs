using UnityEngine;

namespace Shpleeble
{
    public class ShpleebleController : MonoBehaviour
    {
        public ShpleebleData Data { get; private set; }
        public CharacterMode Mode { get; private set; }

        private ShpleebleView view;
        private bool active;

        // -----------------
        // Targets
        // -----------------
        private Vector3 targetPosition;

        // Race / rigid rotation
        private Quaternion targetRaceRotation;

        // Build / character rotations
        private Quaternion targetBodyRotation;
        private Quaternion targetArmatureRotation;

        // -----------------
        // Tuning
        // -----------------
        private float maxMoveDuration = 0.3f;
        private float maxRotateDuration = 0.2f;

        // -----------------
        // Setup
        // -----------------
        public void Initialize(ShpleebleView view)
        {
            this.view = view;

            // Initialize rotation targets to current state
            targetPosition = transform.position;
            targetRaceRotation = transform.rotation;
            targetBodyRotation = transform.rotation;

            if (view.TryGetUpperBodyRotation(out Quaternion upper))
                targetArmatureRotation = upper;
        }

        // -----------------
        // Data
        // -----------------
        public void SetShpleebleData(ShpleebleData data)
        {
            Data = data;

            view.SetName(data.name);
            view.ApplyCosmetics(data.ToCosmeticsV16());

            SetMode((CharacterMode)data.state);
        }

        public void SetMode(CharacterMode mode)
        {
            if (Mode == mode)
                return;

            Mode = mode;
            view.ApplyMode(mode);
        }

        public void SetHorn(bool active)
        {
            view.SetHorn(active);
        }

        // -----------------
        // Movement
        // -----------------
        public void MoveTo(Vector3 pos, bool instant)
        {
            targetPosition = pos;

            if (instant)
                transform.position = pos;
        }

        // -----------------
        // Rotation intents
        // -----------------

        // Race-like modes (single rigid body)
        public void SetRaceRotation(Quaternion rotation, bool instant)
        {
            targetRaceRotation = rotation;

            if (instant)
                transform.rotation = rotation;
        }

        // Build / character modes (full body)
        public void SetBodyRotation(float yawDegrees, bool instant)
        {
            targetBodyRotation = Quaternion.Euler(0f, yawDegrees, 0f);

            if (instant)
                transform.rotation = targetBodyRotation;
        }

        // Build / character modes (upper body)
        public void SetUpperBodyRotation(float pitchDegrees, bool instant)
        {
            targetArmatureRotation = Quaternion.Euler(0f, 270f, 180f - pitchDegrees);

            if (instant)
                view.SetUpperBodyRotation(targetArmatureRotation);
        }

        // -----------------
        // Update loop
        // -----------------
        private void Update()
        {
            if (!active || view == null)
                return;

            view.FaceCamera();

            // -------- Movement --------
            if (targetPosition != transform.position)
            {
                float distance = Vector3.Distance(transform.position, targetPosition);
                float moveSpeed = distance / maxMoveDuration;

                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    moveSpeed * Time.deltaTime
                );
            }

            // -------- Rotation --------
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

        // -----------------
        // Rotation handlers
        // -----------------
        private void HandleBuildModeRotation()
        {
            // Upper body
            if (view.TryGetUpperBodyRotation(out Quaternion currentUpper))
            {
                if (currentUpper != targetArmatureRotation)
                {
                    float angle = Quaternion.Angle(currentUpper, targetArmatureRotation);
                    float rotateSpeed = angle / maxRotateDuration;

                    Quaternion next = Quaternion.RotateTowards(
                        currentUpper,
                        targetArmatureRotation,
                        rotateSpeed * Time.deltaTime
                    );

                    view.SetUpperBodyRotation(next);
                }
            }

            // Full body
            if (transform.rotation != targetBodyRotation)
            {
                float angle = Quaternion.Angle(transform.rotation, targetBodyRotation);
                float rotateSpeed = angle / maxRotateDuration;

                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetBodyRotation,
                    rotateSpeed * Time.deltaTime
                );
            }
        }

        private void HandleRaceModeRotation()
        {
            if (transform.rotation != targetRaceRotation)
            {
                float angle = Quaternion.Angle(transform.rotation, targetRaceRotation);
                float rotateSpeed = angle / maxRotateDuration;

                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRaceRotation,
                    rotateSpeed * Time.deltaTime
                );
            }
        }

        // -----------------
        // Lifecycle
        // -----------------
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
