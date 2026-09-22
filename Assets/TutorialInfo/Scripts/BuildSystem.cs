using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))]

public class BuildSystem : MonoBehaviour
{
    public Transform shootingPoint;
    private CharacterController characterController;
    public GameObject blockObject;
    private InputAction attackInput;
    private InputAction interactInput;
    [SerializeField] private float buildDistance = 5f;
    [SerializeField] private float interactRange = 3f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        attackInput = InputSystem.actions.FindAction("Attack");
        interactInput = InputSystem.actions.FindAction("Interact");
    }

    private void Update()
    {
        if (attackInput.WasPressedThisFrame())
        {
            BuildBlock(blockObject);
        }
        if (interactInput.WasPressedThisFrame())
        {
            PerformInteractionCheck();
        }

    }

    void BuildBlock(GameObject block)
    {
        if(Physics.Raycast(shootingPoint.position, shootingPoint.forward, out RaycastHit hitInfo, buildDistance))
        {
            if(hitInfo.transform.tag == "Block")
            {
                Vector3 spawnPosition = new Vector3(Mathf.RoundToInt(hitInfo.point.x + hitInfo.normal.x /2), Mathf.RoundToInt(hitInfo.point.y + hitInfo.normal.y /2), Mathf.RoundToInt(hitInfo.point.z + hitInfo.normal.z /2));
                Instantiate(block, spawnPosition, Quaternion.identity);
            }
            else
            {
                Vector3 spawnPosition = new Vector3(Mathf.RoundToInt(hitInfo.point.x), Mathf.RoundToInt(hitInfo.point.y), Mathf.RoundToInt(hitInfo.point.z));
                Instantiate(block, spawnPosition, Quaternion.identity);
            }
            
            // Debug.Log(hitInfo.transform.name);
        }
    }

    void PerformInteractionCheck()
    {
        if (Physics.Raycast(shootingPoint.position, shootingPoint.forward, out RaycastHit hitInfo, interactRange))
        {
            if (hitInfo.transform.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.Interact();
            }
        }
    }
}
