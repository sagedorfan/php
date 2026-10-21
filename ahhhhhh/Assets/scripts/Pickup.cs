using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp : MonoBehaviour
{
    public Transform holdPoint;
    public float pickUpRange = 3f;
    public LayerMask pickupLayer;

    private GameObject heldObject;
    private Rigidbody heldRb;

    private bool originalGravity;
    private bool originalKinematic;


    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (heldObject == null)
            {
                PickUpObject();
            }
            else
            {
                DropObject();
            }
        }
    }


    void PickUpObject()
    {
        RaycastHit hit;

        if (Physics.Raycast(
            transform.position,
            transform.forward,
            out hit,
            pickUpRange,
            pickupLayer))
        {
            Rigidbody rb = hit.collider.attachedRigidbody;

            if (rb != null)
            {
                heldObject = rb.gameObject;
                heldRb = rb;

                // remember how the object was set up
                originalGravity = heldRb.useGravity;
                originalKinematic = heldRb.isKinematic;

                // stop physics while holding it
                heldRb.useGravity = false;
                heldRb.isKinematic = true;

                // move it to the hold point
                heldObject.transform.SetParent(holdPoint);
                heldObject.transform.localPosition = Vector3.zero;
                heldObject.transform.localRotation = Quaternion.identity;

                Debug.Log("Picked up: " + heldObject.name);
            }
        }
    }


    void DropObject()
    {
        heldObject.transform.SetParent(null);

        // restore physics
        heldRb.useGravity = originalGravity;
        heldRb.isKinematic = originalKinematic;

        Debug.Log("Dropped: " + heldObject.name);

        heldObject = null;
        heldRb = null;
    }
}