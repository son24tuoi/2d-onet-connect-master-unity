using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventData<T>
{
    public EventID eventID;

    public T data;

    public EventData(EventID eventID, T data)
    {
        this.eventID = eventID;
        this.data = data;
    }
}

public class EventData
{
    public EventID eventID;

    public EventData(EventID eventID)
    {
        this.eventID = eventID;
    }
}
