using UnityEngine;

public class MonoBehaviourCustom : MonoBehaviour
{
    protected bool AreThereNullReferences(params object[] references)
    {
        if(references == null || references.Length <= 0)
        {
            return true;
        }
        foreach (object reference in references)
        {
            if(reference == null)
            {
                Debug.LogError($"There are null references in {this.gameObject}");
                return true;
            }
        }
        return false;
    }
}