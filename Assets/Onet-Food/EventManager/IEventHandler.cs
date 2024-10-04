using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Không có dữ liệu
public interface IEventHandler
{
    public void EventHandler(EventID eventID);
}

// Có dữ liệu
public interface IEventHandlerWithData
{
    public void EventHandler<T>(EventData<T> eventData);
}
