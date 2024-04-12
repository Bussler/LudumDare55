using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Player movement script.
/// Gets input from MainControls system and relays it to BasicMovement in FixedUpdate.
/// </summary>
public class PlayerMovement : BasicMovement
{
    private Vector3 movementVector = Vector3.zero; // Movement input
    private MainControls input = null; // Input system

    private void Awake()
    {
        input = new MainControls();
    }

    protected override void Start()
    {
        base.Start();
    }

    // Subscribe to events of input system
    private void OnEnable()
    {
        input.Enable();
        input.movement.basic.performed += OnBasicActionPerformed;
    }

    private void OnDisable()
    {
        input.Disable();
        input.movement.basic.performed -= OnBasicActionPerformed;
    }

    private void FixedUpdate()
    {
        Move(movementVector);
    }

    private void OnBasicActionPerformed(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            Debug.Log("Basic action performed");
            // TODO
        }
    }
}
