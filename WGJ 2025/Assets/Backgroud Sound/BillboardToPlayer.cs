using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardToPlayer : MonoBehaviour
{
    [Tooltip("Drag your player here. If left empty, will try tag 'Player'.")]
    public Transform player;

    [Tooltip("Keep the sprite upright (rotate only around Y).")]
    public bool onlyRotateAroundY = true;

    [Tooltip("Enable if your sprite looks backwards (faces -Z).")]
    public bool invertForward = false;

    [Tooltip("Extra rotation to fix art alignment, if needed.")]
    public Vector3 additionalEulerOffset = Vector3.zero;

    void LateUpdate()
    {
        // Find target if not assigned
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
            else if (Camera.main) player = Camera.main.transform; // fallback: face camera
            else return;
        }

        // Direction from this sprite to the player
        Vector3 dir = player.position - transform.position;

        if (onlyRotateAroundY)
        {
            dir.y = 0f; // stay upright
            if (dir.sqrMagnitude < 1e-6f) return;
        }

        if (invertForward) dir = -dir;

        // Face the target
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        if (additionalEulerOffset != Vector3.zero)
            transform.rotation *= Quaternion.Euler(additionalEulerOffset);
    }
}