using System;
using System.IO;
using MVPTema2EnacheMihai.Models;
using Newtonsoft.Json;

public class WinsManager
{
    private const string WinsFilePath = "wins.json";
    private WinsData _winsData;

    public WinsManager()
    {
        LoadWins();
    }

    private void LoadWins()
    {
        if (File.Exists(WinsFilePath))
        {
            string json = File.ReadAllText(WinsFilePath);
            _winsData = JsonConvert.DeserializeObject<WinsData>(json);
        }
        else
        {
            _winsData = new WinsData();
            SaveWins();
        }
    }

    private void SaveWins()
    {
        string json = JsonConvert.SerializeObject(_winsData);
        File.WriteAllText(WinsFilePath, json);
    }

    public void IncrementPlayerOneWins()
    {
        _winsData.PlayerOneWins++;
        SaveWins();
    }

    public void IncrementPlayerTwoWins()
    {
        _winsData.PlayerTwoWins++;
        SaveWins();
    }

    public int GetPlayerOneWins()
    {
        return _winsData.PlayerOneWins;
    }

    public int GetPlayerTwoWins()
    {
        return _winsData.PlayerTwoWins;
    }

    private string SerializeToJson(object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    private T DeserializeFromJson<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
}
