using System;
using System.Reflection;

namespace MinishootRandomizer;

public static class ReflectionHelper
{
    public static void InvokeStaticAction(Type type, string actionName)
    {
        ILogger logger = Plugin.ServiceContainer.Get<ILogger>();
        Delegate eventDelegate = GetEventDelegate(type, actionName);

        for (int i = 0; i < eventDelegate.GetInvocationList().Length; i++)
        {
            try
            {
                eventDelegate.GetInvocationList()[i].DynamicInvoke();
            }
            catch (Exception e)
            {
                logger.LogError($"Error invoking action '{i}' on event '{actionName}' on type '{type.FullName}': {e.Message}");
            }
        }
    }

    public static T GetPrivateFieldValue<T>(object instance, string fieldName)
    {
        Type type = instance.GetType();
        FieldInfo field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);

        if (field == null)
        {
            throw new ReflectionException($"Field '{fieldName}' not found on type '{type.FullName}'.");
        }

        return (T)field.GetValue(instance);
    }

    public static void ClearActionInvocationList(Type type, string actionName)
    {
        // Get the event info
        EventInfo eventInfo = type.GetEvent(actionName, BindingFlags.Static | BindingFlags.Public);
        if (eventInfo == null)
        {
            throw new ArgumentException($"Event '{actionName}' not found on type '{type.FullName}'.");
        }

        // Get the field that backs the event
        FieldInfo fieldInfo = type.GetField(actionName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
        if (fieldInfo == null)
        {
            throw new ArgumentException($"Field for event '{actionName}' not found on type '{type.FullName}'.");
        }

        // Set the field to null to clear the invocation list
        fieldInfo.SetValue(null, null);
    }

    private static Delegate GetEventDelegate(Type type, string actionName)
    {
        EventInfo eventInfo = type.GetEvent(actionName, BindingFlags.Static | BindingFlags.Public);

        if (eventInfo == null)
        {
            throw new ReflectionException($"Event '{actionName}' not found on type '{type.FullName}'.");
        }

        FieldInfo field = type.GetField(eventInfo.Name, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);

        if (field == null)
        {
            throw new ReflectionException($"Backing field for event '{actionName}' not found on type '{type.FullName}'.");
        }

        Delegate eventDelegate = (Delegate)field.GetValue(null);

        if (eventDelegate == null)
        {
            throw new ReflectionException($"Delegate for event '{actionName}' is null on type '{type.FullName}'.");
        }

        return eventDelegate;
    }
}
