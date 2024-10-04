using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleSubcriber : MonoBehaviour, IEventHandler, IEventHandlerWithData
{
    #region Fields

    [SerializeField] private Material material;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color defaultColor1 = Color.black;

    #endregion Fields

    // -------------------------------------------------------------------------------------------------

    #region Properties



    #endregion Properties

    // -------------------------------------------------------------------------------------------------

    #region Unity Lifecycle Methods

    // private void Start()
    // {
    //     EventManager.Instance.Subcribe(EventID.Template, this as IEventHandler);
    //     EventManager.Instance.Subcribe(EventID.Template1, this as IEventHandler);
    //     EventManager.Instance.Subcribe(EventID.Template, this as IEventHandlerWithData);
    // }

    // private void OnDestroy()
    // {
    //     EventManager.Instance.Unsubcribe(EventID.Template, this as IEventHandler);
    //     EventManager.Instance.Unsubcribe(EventID.Template1, this as IEventHandler);
    //     EventManager.Instance.Unsubcribe(EventID.Template, this as IEventHandlerWithData);
    // }


    #endregion Unity Lifecycle Methods

    // -------------------------------------------------------------------------------------------------




    public void EventHandler(EventID eventID)
    {
        // switch (eventID)
        // {
        //     case EventID.Template:
        //         ChangeColorMaterial(defaultColor);
        //         break;
        //     case EventID.Template1:
        //         ChangeColorMaterial(defaultColor1);
        //         break;
        //     default:
        //         Debug.Log("Unknow EventID");
        //         break;
        // }
    }

    public void EventHandler<T>(EventData<T> eventData)
    {
        switch (eventData.eventID)
        {
            case EventID.Template:
                if (eventData.data is Color targetColor)
                {
                    ChangeColorMaterial(targetColor);
                }
                else if (eventData.data is SampleEventData sampleEventData)
                {
                    ChangeColorMaterial(sampleEventData.color);
                }
                break;

            default:
                Debug.Log("Unknown EventID");
                break;
        }
    }

    public void ChangeColorMaterial(Color color)
    {
        material.color = color;
    }
}
