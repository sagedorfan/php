using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;
    public float pickUpRange = 5f;

    [Tooltip("Layer used for pickup objects")]
    public LayerMask interactableLayer;

    private GameObject heldObject;
    private bool canDrop = true;
    private int layerNumber;

    void Start()
    {
        layerNumber = LayerMask.NameToLayer("StableLayer");
    }

    void Update()
    {
        if (Mouse.current == null) return;

        // Left mouse button = Pick Up / Drop
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (heldObject == null)
            {
                TryPickUp();
            }
            else if (canDrop)
            {
                TryDrop();
            }
        }
    }

    private bool TryFindInteractable(out Collider result)
    {
        RaycastHit hit;

        if (Physics.Raycast(
            transform.position,
            transform.TransformDirection(Vector3.forward),
            out hit,
            pickUpRange,
            interactableLayer))
        {
            result = hit.collider;
            return true;
        }

        result = null;
        return false;
    }

    private void TryPickUp()
    {
        if (!TryFindInteractable(out Collider hitCollider))
            return;

        GameObject objectToPickUp = hitCollider.gameObject;

        PickUpObject(objectToPickUp);
    }

    private void TryDrop()
    {
        StopClipping();
        DropObject();
    }

    private void PickUpObject(GameObject objectToPickUp)
    {
        heldObject = objectToPickUp;

        // Move object to player's holding position
        heldObject.transform.SetParent(holdPos);
        heldObject.transform.position = holdPos.position;
        heldObject.transform.rotation = holdPos.rotation;

        // Stop physics while holding
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Put object on stable layer
        heldObject.layer = layerNumber;

        // Prevent object from colliding with player
        Collider objectCollider = heldObject.GetComponent<Collider>();
        Collider playerCollider = player.GetComponent<Collider>();

        if (objectCollider != null && playerCollider != null)
        {
            Physics.IgnoreCollision(objectCollider, playerCollider, true);
        }
    }

    private void DropObject()
    {
        RestorePhysicalState();

        heldObject.transform.SetParent(null);

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
        }

        heldObject = null;
    }

    private void RestorePhysicalState()
    {
        Collider objectCollider = heldObject.GetComponent<Collider>();
        Collider playerCollider = player.GetComponent<Collider>();

        if (objectCollider != null && playerCollider != null)
        {
            Physics.IgnoreCollision(objectCollider, playerCollider, false);
        }

        // Restore object's original layer
        heldObject.layer = 6;
    }

    private void StopClipping()
    {
        if (heldObject == null) return;

        float clipRange = Vector3.Distance(
            heldObject.transform.position,
            player.transform.position
        );

        RaycastHit[] hits;

        hits = Physics.RaycastAll(
            player.transform.position,
            player.transform.TransformDirection(Vector3.forward),
            clipRange
        );

        if (hits.Length > 1)
        {
            heldObject.transform.position =
                transform.position + new Vector3(0f, -0.5f, 0f);
        }
    }
}
