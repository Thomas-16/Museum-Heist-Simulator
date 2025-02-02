using ECM2.Examples.FirstPerson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonPlayerInput : MonoBehaviour
{
    [Space(15.0f)]
    public bool invertLook = false;
    [Tooltip("Mouse look sensitivity")]
    public Vector2 mouseSensitivity = new Vector2(1.0f, 1.0f);

    [Space(15.0f)]
    [Tooltip("How far in degrees can you move the camera down.")]
    public float minPitch = -80.0f;
    [Tooltip("How far in degrees can you move the camera up.")]
    public float maxPitch = 80.0f;

    private Player _character;

    private void Awake() {
        _character = GetComponent<Player>();
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        Vector2 lookInput = new Vector2 {
            x = Input.GetAxisRaw("Mouse X"),
            y = Input.GetAxisRaw("Mouse Y")
        };

        lookInput *= mouseSensitivity;

        _character.AddControlYawInput(lookInput.x);
        _character.AddControlPitchInput(invertLook ? lookInput.y : -lookInput.y);


        // Movement input, relative to character's view direction
        Vector2 inputMove = new Vector2() {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetAxisRaw("Vertical")
        };

        Vector3 movementDirection = Vector3.zero;

        movementDirection += _character.GetRightVector() * inputMove.x;
        movementDirection += _character.GetForwardVector() * inputMove.y;

        _character.SetMovementDirection(movementDirection);

        // Crouch input

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
            _character.Crouch();
        else if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.C))
            _character.UnCrouch();

        // Jump input

        if (Input.GetButtonDown("Jump"))
            _character.Jump();
        else if (Input.GetButtonUp("Jump"))
            _character.StopJumping();
    }

}
