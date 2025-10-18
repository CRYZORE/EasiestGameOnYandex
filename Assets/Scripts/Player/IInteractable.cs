using UnityEngine;

public interface IInteractable
{
    string GetDescription();
    void Interact(); 
    Transform InteractionPanelPosition(); 
}
