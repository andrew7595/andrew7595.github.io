using UnityEngine;

public class ClearAllCreatures : MonoBehaviour
{
    public Transform spawnedRoot;

    public void ClearAll()
    {
        for (int i = spawnedRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(spawnedRoot.GetChild(i).gameObject);
        }
    }
}