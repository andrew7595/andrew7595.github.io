using UnityEngine;
using System.Collections.Generic;

public class CreatureSpawner : MonoBehaviour
{
    [Header("Creature Prefabs")]
    public List<GameObject> creaturePrefabs;

    [Header("Spawn Area")]
    public Vector3 spawnCenter = Vector3.zero;
    public Vector3 spawnRange = new Vector3(10, 3, 10);

    private Dictionary<string, GameObject> prefabDict;
    private CameraFocusController cameraFocus;

    void Awake()
    {
        prefabDict = new Dictionary<string, GameObject>();
        foreach (var prefab in creaturePrefabs)
        {
            prefabDict[prefab.name] = prefab;
        }

        cameraFocus = Camera.main.GetComponent<CameraFocusController>();
    }

    void Start()
    {
        // 假資料測試（之後可接後端）
        SpawnCreature(new CreatureData
        {
            id = System.Guid.NewGuid().ToString(),
            username = "小明",
            creatureType = creaturePrefabs[0].name,
            textureUrl = ""
        });
    }

    public void SpawnCreature(CreatureData data)
    {
        if (!prefabDict.ContainsKey(data.creatureType))
        {
            Debug.LogWarning("No prefab for creature type: " + data.creatureType);
            return;
        }

        Vector3 randomPos = spawnCenter + new Vector3(
            Random.Range(-spawnRange.x, spawnRange.x),
            Random.Range(-spawnRange.y, spawnRange.y),
            Random.Range(-spawnRange.z, spawnRange.z)
        );

        GameObject creature = Instantiate(
            prefabDict[data.creatureType],
            randomPos,
            Quaternion.identity,
            transform
        );

        TextureLoader loader = creature.GetComponent<TextureLoader>();
        if (loader != null)
            loader.LoadTextureFromURL(data.textureUrl);

        creature.name = data.creatureType + "_" + data.id;

        // 顯示名字
        NameBillboard name = creature.GetComponentInChildren<NameBillboard>();
        if (name != null)
            name.SetName(data.username);

        // 攝影機聚焦
        if (cameraFocus != null)
            cameraFocus.FocusOn(creature.transform);
    }
}