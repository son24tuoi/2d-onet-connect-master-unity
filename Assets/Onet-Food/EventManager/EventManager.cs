using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    #region Fields

    public static EventManager Instance { get; private set; }

    private readonly Dictionary<EventID, List<IEventHandler>> eventsWithoutData = new Dictionary<EventID, List<IEventHandler>>();
    private readonly Dictionary<EventID, List<IEventHandlerWithData>> eventsWithData = new Dictionary<EventID, List<IEventHandlerWithData>>();

    #endregion Fields

    // -------------------------------------------------------------------------------------------------

    #region Properties



    #endregion Properties

    // -------------------------------------------------------------------------------------------------

    #region Unity Lifecycle Methods

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion Unity Lifecycle Methods

    // -------------------------------------------------------------------------------------------------

    private void Init()
    {

    }

    // Đăng ký sự kiện không có dữ liệu
    public void Subcribe(EventID eventID, IEventHandler eventHandler)
    {
        if (eventsWithoutData.ContainsKey(eventID))
        {
            if (!eventsWithoutData[eventID].Contains(eventHandler))
            {
                eventsWithoutData[eventID].Add(eventHandler);
            }
        }
        else
        {
            eventsWithoutData[eventID] = new List<IEventHandler>
            {
                eventHandler
            };
        }
    }

    // Đăng ký sự kiện có dữ liệu
    public void Subcribe(EventID eventID, IEventHandlerWithData eventHandlerWithData)
    {
        if (eventsWithData.ContainsKey(eventID))
        {
            if (!eventsWithData[eventID].Contains(eventHandlerWithData))
            {
                eventsWithData[eventID].Add(eventHandlerWithData);
            }
        }
        else
        {
            eventsWithData[eventID] = new List<IEventHandlerWithData>
            {
                eventHandlerWithData
            };
        }
    }

    // Hủy đăng ký sự kiện không có dữ liệu
    public void Unsubcribe(EventID eventID, IEventHandler eventHandler)
    {
        if (eventsWithoutData.ContainsKey(eventID))
        {
            if (eventsWithoutData[eventID].Contains(eventHandler))
            {
                int index = eventsWithoutData[eventID].IndexOf(eventHandler);
                if (index != -1)
                {
                    eventsWithoutData[eventID].RemoveAt(index);
                }

                if (eventsWithoutData[eventID].Count == 0)
                {
                    eventsWithoutData.Remove(eventID);
                }
            }
        }
    }

    // Hủy đăng ký sự kiện có dữ liệu 
    public void Unsubcribe(EventID eventID, IEventHandlerWithData eventHandlerWithData)
    {
        if (eventsWithData.ContainsKey(eventID))
        {
            if (eventsWithData[eventID].Contains(eventHandlerWithData))
            {
                int index = eventsWithData[eventID].IndexOf(eventHandlerWithData);
                if (index != -1)
                {
                    eventsWithData[eventID].RemoveAt(index);
                }

                if (eventsWithData[eventID].Count == 0)
                {
                    eventsWithData.Remove(eventID);
                }
            }
        }
    }

    // Kích hoạt sự kiện không có dữ liệu
    public void Trigger(EventID eventID)
    {
        if (eventsWithoutData.ContainsKey(eventID))
        {
            List<IEventHandler> eventHandlerList = eventsWithoutData[eventID];
            int length = eventHandlerList.Count;
            for (int i = 0; i < length; i++)
            {
                eventHandlerList[i].EventHandler(eventID);
            }
        }
    }

    // Kích hoạt sự kiện có dữ liệu
    public void Trigger<T>(EventData<T> eventData)
    {
        if (eventsWithData.ContainsKey(eventData.eventID))
        {
            List<IEventHandlerWithData> eventHandlerList = eventsWithData[eventData.eventID];
            int length = eventHandlerList.Count;
            for (int i = 0; i < length; i++)
            {
                eventHandlerList[i].EventHandler(eventData);
            }
        }
    }


    // -------------------------------------------------------------------------------------------------

    #region Test

    [ContextMenu("Amount Event Without Data")]
    public void AmountEventWithoutData()
    {
        string log = $"Số lượng phần tử trong Dictionary Event Without Data: {eventsWithoutData.Count}\n";

        foreach (var kvp in eventsWithoutData)
        {
            log += $"Key: {kvp.Key}, Số lượng phần tử trong List: {kvp.Value.Count}\n";
        }

        Debug.Log(log);
    }

    [ContextMenu("Amount Event With Data")]
    public void AmountEventWithData()
    {
        string log = $"Số lượng phần tử trong Dictionary Event With Data: {eventsWithData.Count}\n";

        foreach (var kvp in eventsWithData)
        {
            log += $"Key: {kvp.Key}, Số lượng phần tử trong List: {kvp.Value.Count}\n";
        }

        Debug.Log(log);
    }

    #endregion Test
}
