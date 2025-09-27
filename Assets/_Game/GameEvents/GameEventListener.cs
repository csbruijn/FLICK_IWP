using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CostumGameEvent : UnityEvent<Component, object> { }

public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;
    public CostumGameEvent response;

    private void OnEnable()
    {
        gameEvent.RegisterListerer(this);
    }

    private void OnDisable()
    {
        gameEvent.UnRegisterListerer(this);
    }

    public void OnEventRaised(Component sender, object data)
    {
        //Debug.Log($"recieved event from {sender} to select: {data}");

        response.Invoke(sender, data);
    }
}