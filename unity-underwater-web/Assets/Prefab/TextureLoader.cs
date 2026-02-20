using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TextureLoader : MonoBehaviour
{
    private Renderer targetRenderer;

    void Awake()
    {
        targetRenderer = GetComponentInChildren<Renderer>();
    }

    public void LoadTextureFromURL(string url)
    {
        if (!string.IsNullOrEmpty(url))
            StartCoroutine(DownloadTexture(url));
    }

    IEnumerator DownloadTexture(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("Texture download failed: " + request.error);
            yield break;
        }

        Texture2D texture = DownloadHandlerTexture.GetContent(request);

        // 建立材質實例（避免共用材質污染）
        Material newMat = new Material(targetRenderer.material);
        newMat.mainTexture = texture;
        targetRenderer.material = newMat;
    }
}