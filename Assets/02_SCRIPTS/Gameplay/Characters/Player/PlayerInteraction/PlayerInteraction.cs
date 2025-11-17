using UnityEngine;
using TMPro; 

public class PlayerInteraction : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject interactionPanel; 
    [SerializeField] private TMP_Text interactionText; 

    [Header("InteractionSettings")]
    [SerializeField] private float interactionSphereRadius; 
    [SerializeField] private LayerMask interactableLayers; 
    
    [Header("Keybinds")]
    [SerializeField] KeyCode interactionKey = KeyCode.E; 

    // Update is called once per frame
    void Update()
    {
        InteractionSphere(); 
    }
    void InteractionSphere() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionSphereRadius, interactableLayers); 
        float minDistance = interactionSphereRadius; 
        GameObject closestObject = null;

        if(hitColliders.Length > 0) { 
            foreach (Collider hit in hitColliders)
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position); 
                if(distance < minDistance) {
                    closestObject = hit.gameObject;
                    minDistance = distance; 
                }
            }
            IInteractable interactable = null; 
            if(closestObject != null) interactable = closestObject.GetComponent<IInteractable>(); 
            if(interactable != null) {
                interactionPanel.SetActive(true); 
                interactionPanel.transform.position = interactable.InteractionPanelPosition().position; 
                interactionText.text = interactable.GetDescription(); 
                if(Input.GetKeyDown(interactionKey)) interactable.Interact();  
            }
        }
        else {
            closestObject = null; 
            minDistance = interactionSphereRadius; 
            interactionPanel.SetActive(false);
        }
    }
}
