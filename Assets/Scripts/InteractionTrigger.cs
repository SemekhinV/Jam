using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class InteractionTrigger : MonoBehaviour
{
    public bool InTrigger = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        InTrigger = true;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        InTrigger = false;
    }
}