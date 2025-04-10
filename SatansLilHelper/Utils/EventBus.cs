using System;
using System.Collections.Generic;
using System.Linq;

namespace SatansLilHelper.Utils;

public static class EventBus
{
    private static Dictionary<ChannelKey, object> Listeners { get; } = [];

    public static void Send<T>(object key, T arguments)
    {
        ChannelKey fullKey = new(key, typeof(T));
        if (!Listeners.TryGetValue(fullKey, out object? value))
            return;

        List<EventReaction<T>> actionList = (List<EventReaction<T>>)value;

        foreach (EventReaction<T> listener in actionList)
            listener.Reaction(arguments);
    }

    public static void Subscribe<T>(this object subscriber, object key, Action<T> listenerAction)
    {
        ChannelKey fullKey = new(key, typeof(T));
        if (!Listeners.ContainsKey(fullKey))
            Listeners[fullKey] = new List<EventReaction<T>>();

        List<EventReaction<T>> list = (List<EventReaction<T>>)Listeners[fullKey];
        if (list.Any(reaction => reaction.Subscriber.Equals(subscriber)))
            return;
        list.Add(new EventReaction<T>(subscriber, listenerAction));
    }

    public static void UnSubscribe<T>(this object subscriber, object key)
    {
        ChannelKey fullKey = new(key, typeof(T));
        if (!Listeners.TryGetValue(fullKey, out object? value))
            return;
        ((List<EventReaction<T>>)value).RemoveAll(listener =>
            listener.Subscriber.Equals(subscriber)
        );
    }

    private class ChannelKey(object key, Type? argumentType = null)
    {
        private Type? ArgumentType { get; } = argumentType;
        private object Key { get; } = key;

        public override bool Equals(object? @object)
        {
            return @object is ChannelKey anotherKey
                && anotherKey.Key.Equals(Key)
                && anotherKey.ArgumentType == ArgumentType;
        }

        public override int GetHashCode()
        {
            return (Key?.GetHashCode() ?? 0) + (ArgumentType?.GetHashCode() ?? 0);
        }
    }

    private class EventReaction<T>(object subscriber, Action<T> reaction)
    {
        public Action<T> Reaction { get; } = reaction;
        public object Subscriber { get; } = subscriber;
    }
}
