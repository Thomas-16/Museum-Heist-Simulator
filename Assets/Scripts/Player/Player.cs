using Cinemachine;
using ECM2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [Tooltip("The first person camera parent.")]
    public GameObject cameraParent;

    private float _cameraPitch;

    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private HumanAnimationsController playerAnimationController;
    [SerializeField] private Transform eyeTransform;
    [Header("Settings")]
    [SerializeField] private float maxSprintSpeed = 8f;
    [SerializeField] private float maxWalkingSpeed = 5f;
    [SerializeField] private float walkingFOV = 80f;
    [SerializeField] private float sprintingFOV = 85f;
    [SerializeField] private float fovTransitionSpeed = .5f;
    [SerializeField] private Vector3 eyeNormalPos;
    [SerializeField] private Vector3 eyeRunningPos;
    [SerializeField] private float eyeTransitionSpeed = 5f;
    [SerializeField] private float cameraNoiseTransitionSpeed = 10f;

    private PlayerEmoteController playerEmoteController;


    private new void Awake() {
        base.Awake();

        playerEmoteController = GetComponent<PlayerEmoteController>();
    }

    private void Update() {
        HandleRunning();
        HandleMovementAnimations();

        if(playerEmoteController.IsEmoting() && playerEmoteController.GetTimeSinceEmoteStarted() >= .5f && InputManager.Instance.IsControllingPlayer()) {
            playerEmoteController.StopEmoting();
        }
    }
    private void HandleRunning() {
        bool isRunning = InputManager.Instance.IsCrouchPressed();
        if (isRunning) {
            maxWalkSpeed = maxSprintSpeed;
        }
        else {
            maxWalkSpeed = maxWalkingSpeed;
        }
        float targetFOV = isRunning && characterMovement.velocity.sqrMagnitude > 0.01f ? sprintingFOV : walkingFOV;
        virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, targetFOV, fovTransitionSpeed * Time.deltaTime);

        Vector3 targetEyePos = isRunning && characterMovement.velocity.sqrMagnitude > 0.01f && characterMovement.forwardSpeed > 0.01f ? eyeRunningPos : eyeNormalPos;
        eyeTransform.localPosition = Vector3.Lerp(eyeTransform.localPosition, targetEyePos, eyeTransitionSpeed * Time.deltaTime);

        float targetAmplitude = 0f, targetFrequency = 0f;
        if(characterMovement.velocity.sqrMagnitude < 0.01f) {
            targetAmplitude = 0.4f;
            targetFrequency = 0.25f;
        } else if(!isRunning && characterMovement.velocity.sqrMagnitude > 0.01f) {
            targetAmplitude = 0.3f;
            targetFrequency = 0.77f;
        } else if(isRunning && characterMovement.velocity.sqrMagnitude > 0.01f) {
            targetAmplitude = 0.3f;
            targetFrequency = 1.1f;
        }
        virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain =
            Mathf.Lerp(virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain, targetAmplitude, cameraNoiseTransitionSpeed * Time.deltaTime);
        virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain =
            Mathf.Lerp(virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain, targetFrequency, cameraNoiseTransitionSpeed * Time.deltaTime);
    }
    private void HandleMovementAnimations() {
        playerAnimationController.SetIsRunning(characterMovement.velocity.sqrMagnitude > 0.01f && InputManager.Instance.IsCrouchPressed());
        playerAnimationController.SetSpeed(characterMovement.forwardSpeed / maxWalkingSpeed / 1.5f);
        playerAnimationController.SetAbsSpeed(new Vector2(characterMovement.forwardSpeed, characterMovement.sidewaysSpeed).magnitude / maxWalkingSpeed / 1.5f);
        playerAnimationController.SetDirection(characterMovement.sidewaysSpeed / (InputManager.Instance.IsCrouchPressed() ? maxSprintSpeed : maxWalkingSpeed));
    }


    /// <summary>
    /// Add input (affecting Yaw).
    /// This is applied to the Character's rotation.
    /// </summary>

    public virtual void AddControlYawInput(float value) {
        if (value != 0.0f)
            AddYawInput(value);
    }

    /// <summary>
    /// Add input (affecting Pitch).
    /// This is applied to the cameraParent's local rotation.
    /// </summary>

    public virtual void AddControlPitchInput(float value, float minPitch = -80.0f, float maxPitch = 80.0f) {
        if (value != 0.0f)
            _cameraPitch = MathLib.ClampAngle(_cameraPitch + value, minPitch, maxPitch);
    }

    /// <summary>
    /// Update cameraParent local rotation applying current _cameraPitch value.
    /// </summary>

    protected virtual void UpdateCameraParentRotation() {
        cameraParent.transform.localRotation = Quaternion.Euler(_cameraPitch, 0.0f, 0.0f);
    }

    /// <summary>
    /// If overriden, base method MUST be called.
    /// </summary>

    protected virtual void LateUpdate() {
        UpdateCameraParentRotation();
    }

    /// <summary>
    /// If overriden, base method MUST be called.
    /// </summary>

    protected override void Reset() {
        // Call base method implementation

        base.Reset();

        // Disable character's rotation,
        // it is handled by the AddControlYawInput method 

        SetRotationMode(RotationMode.None);
    }
}
