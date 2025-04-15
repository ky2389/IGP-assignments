using UnityEngine;
using SimpleJSON;
using TMPro;
public class GameManager : MonoBehaviour
{
    JSONLoader jsonLoader;
    [SerializeField]
    TextMeshProUGUI tX_Name;
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
    }
    public void OnDestroy()
    {
        jsonLoader.jsonRefreshed -= ReadJSON;
    }
}
