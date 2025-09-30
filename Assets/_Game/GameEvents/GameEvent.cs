using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent")]
public class GameEvent : ScriptableObject
{
    public List<GameEventListener> listeners = new List<GameEventListener>();

    // raise event through different methods signatures 

    // Delegate definition for code-based event subscription
    public delegate void GameEventHandler(Component sender, object data);

    // Event that can be subscribed to via code
    public event GameEventHandler OnEventRaised;

    public void Raise(Component sender, object data)
    {
        // Notify all component-based listeners
        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].OnEventRaised(sender, data);
        }

        // Notify all code-based listeners
        OnEventRaised?.Invoke(sender, data);
    }


    public void RegisterListerer(GameEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnRegisterListerer(GameEventListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }

}