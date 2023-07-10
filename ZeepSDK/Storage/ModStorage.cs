﻿using System;
using System.IO;
using BepInEx;
using Newtonsoft.Json;

namespace ZeepSDK.Storage;

internal class ModStorage : IModStorage
{
    private readonly BaseUnityPlugin plugin;
    private readonly JsonSerializerSettings settings;

    public ModStorage(BaseUnityPlugin plugin)
    {
        this.plugin = plugin;
        settings = new JsonSerializerSettings();
    }

    private string CreatePath(string name, string extension = ".json")
    {
        string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        return Path.Combine(appData, plugin.Info.Metadata.GUID, name) + extension;
    }

    public void AddConverter(JsonConverter converter)
    {
        settings.Converters.Add(converter);
    }

    public void RemoveConverter(JsonConverter converter)
    {
        settings.Converters.Remove(converter);
    }

    public void SaveToJson(string name, object data)
    {
        string json = JsonConvert.SerializeObject(data);
        File.WriteAllText(CreatePath(name), json);
    }

    public object LoadFromJson(string name)
    {
        string json = File.ReadAllText(CreatePath(name));
        return JsonConvert.DeserializeObject(json);
    }

    public object LoadFromJson(string name, Type type)
    {
        string json = File.ReadAllText(CreatePath(name));
        return JsonConvert.DeserializeObject(json, type);
    }

    public TData LoadFromJson<TData>(string name)
    {
        string json = File.ReadAllText(CreatePath(name));
        return JsonConvert.DeserializeObject<TData>(json);
    }
}
