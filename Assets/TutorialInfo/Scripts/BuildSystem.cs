using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
[RequireComponent(typeof(CharacterController))]

public class BuildSystem : MonoBehaviour
{
    public Transform shootingPoint;
    private CharacterController characterController;
    public GameObject blockObject;
    private InputAction attackInput;
    private InputAction interactInput;
    private InputAction rightclickInput;
    private InputAction scrollwheelInput;
    GameObject lastHighlightedBlock;

    public Color normalColor;
    public Color highlightedColor;

    [SerializeField] private float holdDuration = 2f;
    private float holdTimer = 0f;
    private bool actionTriggered = false;

    [SerializeField] private float buildDistance = 5f;
    [SerializeField] private float interactRange = 3f;

    public Block[] avalibleBuildingBlocks;
    int currentBlockIndex = 0;
    Block currentBlock;
    public TMP_Text blockNameText;


    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        attackInput = InputSystem.actions.FindAction("Attack");
        interactInput = InputSystem.actions.FindAction("Interact");
        rightclickInput = InputSystem.actions.FindAction("RightClick");
        scrollwheelInput = InputSystem.actions.FindAction("ScrollWheel");
        currentBlockIndex = 0;
        ChangeCurrentBlock();
    }

    private void Update()
    {
        if (attackInput.WasPressedThisFrame())
        {
            BuildBlock(currentBlock.BlockObject); 
        }
        if (interactInput.WasPressedThisFrame())
        {
            PerformInteractionCheck();
        }
        /*if (rightclickInput.WasPressedThisFrame())
        {
            DestroyBlock();
        } */
        if (rightclickInput.IsPressed())
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= holdDuration && !actionTriggered)
            {
                DestroyBlock();
                actionTriggered = true;
            }
        }
        else
        {
            holdTimer = 0f;
            actionTriggered = false;
        }
        HighlightBlock();
        HandleScrollWheel();
    }

    private void HandleScrollWheel()
    {
        if (scrollwheelInput == null)
            return;

        Vector2 scroll = scrollwheelInput.ReadValue<Vector2>();

        if (scroll.y > 0)
        {
            currentBlockIndex++;

            if (currentBlockIndex >= avalibleBuildingBlocks.Length)
            {
                currentBlockIndex = 0;
            }

            ChangeCurrentBlock();
        }
        else if (scroll.y < 0)
        {
            currentBlockIndex--;

            if (currentBlockIndex < 0)
            {
                currentBlockIndex = avalibleBuildingBlocks.Length - 1;
            }

            ChangeCurrentBlock();
        }
    }

    private void ChangeCurrentBlock()
    {
        if (avalibleBuildingBlocks == null || avalibleBuildingBlocks.Length == 0)
        {
            Debug.LogWarning("Building Blocks array is empty!");
            return;
        }

        if (currentBlockIndex < 0 || currentBlockIndex >= avalibleBuildingBlocks.Length)
        {
            currentBlockIndex = 0;
        }

        currentBlock = avalibleBuildingBlocks[currentBlockIndex];

        if (currentBlock == null)
        {
            Debug.LogWarning("The selected block is not assigned in the Inspector!");
            return;
        }

        SetText();

        Debug.Log("Selected block: " + currentBlock.BlockObject.name);
    }

    void SetText()
    {
        blockNameText.text = currentBlock.BlockName;
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

    void DestroyBlock()
    {
        if (Physics.Raycast(shootingPoint.position, shootingPoint.forward, out RaycastHit hitInfo, buildDistance))
        {
            if (hitInfo.transform.tag == "Block")
            {
                Destroy(hitInfo.transform.gameObject);
            }
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

    void HighlightBlock()
    {
        if (Physics.Raycast(shootingPoint.position, shootingPoint.forward, out RaycastHit hitInfo, buildDistance))
        {
            if (hitInfo.transform.tag == "Block")
            {
                if (lastHighlightedBlock == null)
                {
                    lastHighlightedBlock = hitInfo.transform.gameObject;
                    hitInfo.transform.gameObject.GetComponent<Renderer>().material.color = highlightedColor;
                }
                else if (lastHighlightedBlock != hitInfo.transform.gameObject)
                {
                    lastHighlightedBlock.GetComponent<Renderer>().material.color = normalColor;
                    hitInfo.transform.gameObject.GetComponent<Renderer>().material.color = highlightedColor;
                    lastHighlightedBlock = hitInfo.transform.gameObject;
                }
            }
            else if (lastHighlightedBlock != null)
            {
                lastHighlightedBlock.GetComponent<Renderer>().material.color = normalColor;
                lastHighlightedBlock = null;
            }
            
        }
    }
}
