using UnityEngine;

public class InteractionTest : MonoBehaviour, IInteractable
{
    [Header("TestSettings")]
    [SerializeField] private string interactionText; 
    [SerializeField] private string interactionPrint; 
    [SerializeField] private Transform interactionPanelPivot; 
    
    public string GetDescription() {
        return interactionText; 
    }
    public void Interact() {
        Debug.Log(interactionPrint); 
    }
    public Transform InteractionPanelPosition() {
        return interactionPanelPivot; 
    }
}
