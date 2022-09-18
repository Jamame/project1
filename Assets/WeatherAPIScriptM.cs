using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class WeatherAPIScriptM : MonoBehaviour
{
    public GameObject weatherTextObject;
        // add your personal API key after APPID= and before &units=
       string url = "http://api.openweathermap.org/data/2.5/weather?lat=41.88&lon=-87.6&APPID=dbfbf4bc04a3853e3ac6f266bf842b7d&units=metric";

   
    void Start()
    {

    // wait a couple seconds to start and then refresh every 900 seconds

       InvokeRepeating("GetDataFromWeb", 2f, 900f);
   }

   void GetDataFromWeb()
   {

       StartCoroutine(GetRequest(url));
   }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();


            if (webRequest.result ==  UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                // print out the weather data to make sure it makes sense
                Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);

                // this code will NOT fail gracefully, so make sure you have
                // your API key before running or you will get an error

            	// grab the current temperature and simplify it if needed
            	int startTemp = webRequest.downloadHandler.text.IndexOf("temp",0);
            	int endTemp = webRequest.downloadHandler.text.IndexOf(",",startTemp);
				double tempC = float.Parse(webRequest.downloadHandler.text.Substring(startTemp+6, (endTemp-startTemp-6)));
				int easyTempC = Mathf.RoundToInt((float)tempC);
                //Debug.Log ("integer temperature is " + easyTempF.ToString());
                int startConditions = webRequest.downloadHandler.text.IndexOf("main",0);
                int endConditions = webRequest.downloadHandler.text.IndexOf(",",startConditions);
                string conditions = webRequest.downloadHandler.text.Substring(startConditions+7, (endConditions-startConditions-8));
                //Debug.Log(conditions);

                weatherTextObject.GetComponent<TextMeshPro>().text = "" + easyTempC.ToString() + "°C\n" + conditions;
            }
        }
    }
}

