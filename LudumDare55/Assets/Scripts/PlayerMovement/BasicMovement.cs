using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Class to manage the player's input and movement.
/// </summary>
public class BasicMovement : MonoBehaviour
{
    // TODO maybe put stat manager here?

    protected Rigidbody rb = null; // Rigidbody component through which we apply force
    private bool canMove = true; // Flag to check if the player can move

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.Log("No Rigidbody component found on " + this.gameObject.name);
        }
    }

    protected void Move(Vector3 movementVector)
    {
        if (!canMove || rb == null)
        {
            return;
        }

        // TODO
    }

    public void setCanMove(bool value)
    {
        this.canMove = !value;
    }

}
