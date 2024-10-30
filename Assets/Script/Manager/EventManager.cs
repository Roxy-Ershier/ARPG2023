using GGG.Tool.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EventManager:SingletonNonMono<EventManager>
{
    private interface IEventHelp
    {

    }

    private class EventHelp:IEventHelp
    {
        private event Action _action;

        public EventHelp(Action action)
        {
            _action = action;
        }

        public void Call()
        {
            _action?.Invoke();
        }

        public void AddCall(Action action)
        {
            _action += action;
        }

        public void RemoveCall(Action action)
        {
            _action -= action;
        }

    }

    private class EventHelp<T> : IEventHelp
    {
        private event Action<T> _action;

        public EventHelp(Action<T> action)
        {
            _action = action;
        }

        public void Call(T value)
        {
            _action?.Invoke(value);
        }

        public void AddCall(Action<T> action)
        {
            _action += action;
        }

        public void RemoveCall(Action<T> action)
        {
            _action -= action;
        }

    }

    private class EventHelp<T1,T2> : IEventHelp
    {
        private event Action<T1, T2> _action;

        public EventHelp(Action<T1, T2> action)
        {
            _action = action;
        }

        public void Call(T1 value,T2 value2)
        {
            _action?.Invoke(value,value2);
        }

        public void AddCall(Action<T1, T2> action)
        {
            _action += action;
        }

        public void RemoveCall(Action<T1, T2> action)
        {
            _action -= action;
        }
    }

    private class EventHelp<T1, T2, T3> : IEventHelp
    {
        private event Action<T1, T2, T3> _action;

        public EventHelp(Action<T1, T2, T3> action)
        {
            _action = action;
        }

        public void Call(T1 value, T2 value2, T3 value3)
        {
            _action?.Invoke(value, value2, value3);
        }

        public void AddCall(Action<T1, T2, T3> action)
        {
            _action += action;
        }

        public void RemoveCall(Action<T1, T2, T3> action)
        {
            _action -= action;
        }
    }

    private class EventHelp<T1, T2, T3, T4> : IEventHelp
    {
        private event Action<T1, T2, T3, T4> _action;

        public EventHelp(Action<T1, T2, T3, T4> action)
        {
            _action = action;
        }

        public void Call(T1 value, T2 value2, T3 value3, T4 value4)
        {
            _action?.Invoke(value, value2, value3, value4);
        }

        public void AddCall(Action<T1, T2, T3, T4> action)
        {
            _action += action;
        }

        public void RemoveCall(Action<T1, T2, T3, T4> action)
        {
            _action -= action;
        }
    }


    private class EventHelp<T1,T2,T3,T4,T5> : IEventHelp
    {
        private event Action<T1, T2, T3, T4, T5> _action;

        public EventHelp(Action<T1, T2, T3, T4, T5> action)
        {
            _action = action;
        }

        public void Call(T1 value, T2 value2,T3 value3,T4 value4,T5 value5)
        {
            _action?.Invoke(value, value2, value3, value4, value5);
        }

        public void AddCall(Action<T1, T2, T3, T4, T5> action)
        {
            _action += action;
        }

        public void RemoveCall(Action<T1, T2, T3, T4, T5> action)
        {
            _action -= action;
        }
    }


    private Dictionary<string, IEventHelp> _eventCentre = new Dictionary<string, IEventHelp>();
    public void AddEventListening(string eventName, Action action)
    {
        if(_eventCentre.TryGetValue(eventName,out var e))
        {
            (e as EventHelp).AddCall(action);
        }
        else
        {
            _eventCentre.Add(eventName, new EventHelp(action));
        }
    }
    public void AddEventListening<T>(string eventName, Action<T> action)
    {
        if (_eventCentre.TryGetValue(eventName, out var e))
        {
            (e as EventHelp<T>).AddCall(action);
        }
        else
        {
            _eventCentre.Add(eventName, new EventHelp<T>(action));
        }
    }

    public void AddEventListening<T1,T2>(string eventName, Action<T1, T2> action)
    {
        if (_eventCentre.TryGetValue(eventName, out var e))
        {
            (e as EventHelp<T1, T2>).AddCall(action);
        }
        else
        {
            _eventCentre.Add(eventName, new EventHelp<T1, T2>(action));
        }
    }

    public void AddEventListening<T1, T2, T3>(string eventName, Action<T1, T2, T3> action)
    {
        if (_eventCentre.TryGetValue(eventName, out var e))
        {
            (e as EventHelp<T1, T2, T3>).AddCall(action);
        }
        else
        {
            _eventCentre.Add(eventName, new EventHelp<T1, T2, T3>(action));
        }
    }

    public void AddEventListening<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> action)
    {
        if (_eventCentre.TryGetValue(eventName, out var e))
        {
            (e as EventHelp<T1, T2, T3, T4>).AddCall(action);
        }
        else
        {
            _eventCentre.Add(eventName, new EventHelp<T1, T2, T3, T4>(action));
        }
    }

    public void AddEventListening<T1, T2,T3,T4,T5>(string eventName, Action<T1, T2, T3, T4, T5> action)
    {
        if (_eventCentre.TryGetValue(eventName, out var e))
        {
            (e as EventHelp<T1, T2, T3, T4, T5>).AddCall(action);
        }
        else
        {
            _eventCentre.Add(eventName, new EventHelp<T1, T2, T3, T4, T5>(action));
        }
    }
    public void CallEvent(string eventName)
    {
        if (_eventCentre.TryGetValue(eventName, out var e))
        {
            (e as EventHelp).Call();
        }
        else
        {
            Debug.LogError($"没有注册[{eventName}]事件！");
        }
    }
    public void CallEvent<T>(string eventName,T value)
    {
        if (_eventCentre.TryGetValue(eventName, out var e))
        {
            (e as EventHelp<T>).Call(value);
        }
        else
        {
            Debug.LogError($"没有注册[{eventName}]事件！");
        }
    }
    public void CallEvent<T1,T2>(string eventName, T1 value,T2 value2)
    {
        if (_eventCentre.TryGetValue(eventName, out var e))
        {
            (e as EventHelp<T1, T2>).Call(value, value2);
        }
        else
        {
            Debug.LogError($"没有注册[{eventName}]事件！");
        }
    }

    public void CallEvent<T1, T2, T3>(string eventName, T1 value, T2 value2, T3 value3)
    {
        if (_eventCentre.TryGetValue(eventName, out var e))
        {
            (e as EventHelp<T1, T2, T3>).Call(value, value2, value3);
        }
        else
        {
            Debug.LogError($"没有注册[{eventName}]事件！");
        }
    }

    public void CallEvent<T1, T2, T3, T4>(string eventName, T1 value, T2 value2, T3 value3, T4 value4)
    {
        if (_eventCentre.TryGetValue(eventName, out var e))
        {
            (e as EventHelp<T1, T2, T3, T4>).Call(value, value2, value3, value4);
        }
        else
        {
            Debug.LogError($"没有注册[{eventName}]事件！");
        }
    }

    public void CallEvent<T1, T2, T3, T4, T5>(string eventName, T1 value, T2 value2,T3 value3,T4 value4,T5 value5)
    {
        if (_eventCentre.TryGetValue(eventName, out var e))
        {
            (e as EventHelp<T1, T2, T3, T4, T5>).Call(value, value2, value3, value4, value5);
        }
        else
        {
            Debug.LogError($"没有注册[{eventName}]事件！");
        }
    }

    public void RemoveEvent(string eventName,Action action)
    {
        if (_eventCentre.TryGetValue(eventName, out var e))
        {
            (e as EventHelp).RemoveCall(action);
        }
        else
        {
            Debug.LogError($"没有注册[{eventName}]事件！");
        }
    }
    public void RemoveEvent<T>(string eventName, Action<T> action)
    {
        if (_eventCentre.TryGetValue(eventName, out var e))
        {
            (e as EventHelp<T>).RemoveCall(action);
        }
        else
        {
            Debug.LogError($"没有注册[{eventName}]事件！");
        }
    }
    public void RemoveEvent<T1,T2>(string eventName, Action<T1, T2> action)
    {
        if (_eventCentre.TryGetValue(eventName, out var e))
        {
            (e as EventHelp<T1, T2>).RemoveCall(action);
        }
        else
        {
            Debug.LogError($"没有注册[{eventName}]事件！");
        }
    }

    public void RemoveEvent<T1, T2, T3>(string eventName, Action<T1, T2, T3> action)
    {
        if (_eventCentre.TryGetValue(eventName, out var e))
        {
            (e as EventHelp<T1, T2, T3>).RemoveCall(action);
        }
        else
        {
            Debug.LogError($"没有注册[{eventName}]事件！");
        }
    }

    public void RemoveEvent<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> action)
    {
        if (_eventCentre.TryGetValue(eventName, out var e))
        {
            (e as EventHelp<T1, T2, T3, T4>).RemoveCall(action);
        }
        else
        {
            Debug.LogError($"没有注册[{eventName}]事件！");
        }
    }

    public void RemoveEvent<T1, T2, T3, T4, T5>(string eventName, Action<T1, T2, T3, T4, T5> action)
    {
        if (_eventCentre.TryGetValue(eventName, out var e))
        {
            (e as EventHelp<T1, T2, T3, T4, T5>).RemoveCall(action);
        }
        else
        {
            Debug.LogError($"没有注册[{eventName}]事件！");
        }
    }
}
