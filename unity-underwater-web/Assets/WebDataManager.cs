using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class WebDataManager : MonoBehaviour
{
    [Header("API Settings")]
    public string apiURL = "http://localhost:3000/api/creatures";
    public float pollInterval = 5f;

    private CreatureSpawner spawner;
    private HashSet<string> spawnedIDs = new HashSet<string>();

    void Start()
    {
        spawner = GetComponent<CreatureSpawner>();
        StartCoroutine(PollRoutine());
    }

    IEnumerator PollRoutine()
    {
        while (true)
        {
            yield return FetchData();
            yield return new WaitForSeconds(pollInterval);
        }
    }

    IEnumerator FetchData()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiURL);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("API Error: " + request.error);
            yield break;
        }

        string json = request.downloadHandler.text;

        CreatureData[] dataArray = JsonHelper.FromJson<CreatureData>(json);

        foreach (var data in dataArray)
        {
            if (!spawnedIDs.Contains(data.id))
            {
                spawner.SpawnCreature(data);
                spawnedIDs.Add(data.id);
            }
        }
    }
}