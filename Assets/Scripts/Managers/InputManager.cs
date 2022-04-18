using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IManager
{
    public static EventSystem EventSystem => _eventSystem;

    private static EventSystem _eventSystem;

    public void Contruct()
    {
        if(EventSystem.current == null)
        {
            Debug.LogError("Event System is null");
            return;
        }
        _eventSystem = EventSystem.current;
    }

    public void Activate()
    {
    }

    public void Deactivate()
    {
    }
}