using UnityEngine;

public class InteractableBlock : MonoBehaviour, IInteractable
{

    public void Interact()
    {
       // Debug.Log("Interacted with the block!");

        GetComponent<Renderer>().material.color = Random.ColorHSV();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
}
