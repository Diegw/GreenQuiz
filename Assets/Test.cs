using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    // Start is called before the first frame update

    void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
    }

    void Update()
    {
        //Check if the left Mouse button is clicked
        if (Input.GetKey(KeyCode.Mouse0))
        {
            m_PointerEventData = new PointerEventData(InputManager.EventSystem);
            m_PointerEventData.position = new Vector3(Screen.width/2, Screen.height/2, 0);
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);
            foreach (RaycastResult result in results)
            {
                Debug.Log(result.gameObject.name);
            }
        }
    }
}
