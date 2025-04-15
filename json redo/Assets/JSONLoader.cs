using System.Collections;
using SimpleJSON;
using UnityEngine;

public class JSONLoader : MonoBehaviour
{
    [SerializeField]
    string url="https://pokeapi.co/api/v2/pokemon/";
    [SerializeField]
    int pokemonID=1;
    public delegate void JSONRefreshed(JSONNode json);
    public JSONRefreshed jsonRefreshed;
    public JSONNode currentJSON;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartRefreshJSON();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartRefreshJSON()
    {
        StartCoroutine(RefreshJSON());
    }
    IEnumerator RefreshJSON()
    {
        string urlFinal=url+pokemonID+"/";
        WWW www=new WWW(urlFinal);
        yield return www;
        if(www.error == null)
        {
            currentJSON = JSON.Parse(www.text);
            if (jsonRefreshed != null)
            {
                jsonRefreshed.Invoke(currentJSON);
                Debug.Log(www.text);
            }
        }
        else
        {
            Debug.LogError("Error loading JSON: " + www.error);
        }
        StopCoroutine(RefreshJSON());
    }
}
