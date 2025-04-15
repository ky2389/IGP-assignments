using UnityEngine;
using SimpleJSON;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class GameManager : MonoBehaviour
{
    JSONLoader jsonLoader;
    [SerializeField]
    TextMeshProUGUI tX_Name;
    [SerializeField]
    Image img;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jsonLoader = GetComponent<JSONLoader>();
        if (jsonLoader != null)
        {
            jsonLoader.jsonRefreshed += ReadJSON;
        }
        else
        {
            Debug.LogError("JSONLoader component not found on GameManager.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ReadJSON(JSONNode json)
    {
        tX_Name.text= json["name"];
        string imageURL= json["sprites"]["other"]["home"]["front_default"];
        StartCoroutine(DownloadImage(imageURL));
    }
    public void OnDestroy()
    {
        jsonLoader.jsonRefreshed -= ReadJSON;
    }
    IEnumerator DownloadImage(string imageURL){
        UnityWebRequest request=UnityWebRequestTexture.GetTexture(imageURL);
        yield return request.SendWebRequest();
        if(request.result==UnityWebRequest.Result.ConnectionError || request.result==UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error downloading image: " + request.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            img.sprite=Sprite.Create(texture,new Rect(0,0,texture.width,texture.height),new Vector2(0,0));
        }
    }
}
