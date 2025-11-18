using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class NetworkSaveManager : MonoBehaviour
{
    private const string uploadUrl = "";

    public IEnumerator UploadJson(string jsonData)
    {
        UnityWebRequest request = new UnityWebRequest(uploadUrl, "POST");
        byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);

        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Upload successful!");
        }
        else
        {
            Debug.LogError("Upload failed: " + request.error);
        }
    }
}
