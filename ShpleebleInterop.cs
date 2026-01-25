using BepInEx;
using System;

public static class ShpleebleInterop
{
    private const string ShpleeblePluginGuid = "com.metalted.zeepkist.shpleeble";

    public static object CreateShpleeble()
    {
        if (!BepInEx.Bootstrap.Chainloader.PluginInfos.TryGetValue(
                ShpleeblePluginGuid,
                out var pluginInfo))
        {
            return null;
        }

        var pluginInstance = pluginInfo.Instance;
        if (pluginInstance == null)
            return null;

        var pluginType = pluginInstance.GetType();

        // Check IsReady (static)
        var isReadyProp = pluginType.GetProperty(
            "IsReady",
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.Static);

        if (isReadyProp == null || !(bool)isReadyProp.GetValue(null))
            return null;

        // Call Create()
        var createMethod = pluginType.GetMethod(
            "Create",
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.Static);

        if (createMethod == null)
            return null;

        try
        {
            return createMethod.Invoke(null, null);
        }
        catch
        {
            return null;
        }
    }
}
