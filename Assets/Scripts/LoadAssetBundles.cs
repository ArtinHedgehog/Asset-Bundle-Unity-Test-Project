
// Based On Tutorial From El Profesor Kudo
// Link : https://www.youtube.com/watch?v=sDL8AbEKIsY&t=322s

using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LoadAssetBundles : MonoBehaviour
{
    [Header("Asset Bundle Settings")]
    public string assetBundleName;
    public string AssetName;

    [Header("Download Settings")]
    public bool DownloadFromServer = true;
    public string URL;

    private void Start()
    {
        if (DownloadFromServer)
        {
            StartCoroutine(InstantiateObject());
        }
        else
        {
            LoadLocally();
        }
    }

    private void LoadLocally()
    {
        string path = Path.Combine(Application.streamingAssetsPath, assetBundleName);
        AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile(path);
        if (myLoadedAssetBundle == null)
        {
            Debug.LogError("Failed to load AssetBundle!");
            return;
        }
        GameObject prefab = myLoadedAssetBundle.LoadAsset<GameObject>(AssetName);
        Instantiate(prefab);
    }

    private IEnumerator InstantiateObject()
    {
        using (UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(URL))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
                GameObject cube = bundle.LoadAsset<GameObject>(bundle.GetAllAssetNames()[0]);
                Instantiate(cube);
            }
            else
            {
                Debug.LogError(request.error);
            }
        }
    }
}
