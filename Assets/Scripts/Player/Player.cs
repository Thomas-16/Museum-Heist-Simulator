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
    [Header("Settings")]
    [SerializeField] private float maxSprintSpeed = 8f;
    [SerializeField] private float maxWalkingSpeed = 5f;
    [SerializeField] private float walkingFOV = 80f;
    [SerializeField] private float sprintingFOV = 85f;
    [SerializeField] private float fovTransitionSpeed = .5f;

    private new Rigidbody rigidbody;

    private new void Awake() {
        base.Awake();

        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update() {
        HandleRunning();
    }
    private void HandleRunning() {
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        if (Input.GetKey(KeyCode.LeftShift)) {
            maxWalkSpeed = maxSprintSpeed;
        }
        else {
            maxWalkSpeed = maxWalkingSpeed;
        }
        float targetFOV = isRunning && characterMovement.velocity.sqrMagnitude > 0.01f ? sprintingFOV : walkingFOV;
        virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, targetFOV, fovTransitionSpeed * Time.deltaTime);
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
