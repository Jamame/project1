using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using TMPro;


public class timeTeller : MonoBehaviour
{
    public GameObject timeTextObject;
    string url = "https://worldtimeapi.org/api/timezone/America/Chicago";

    // Start is called before the first frame update
    void Start()
    {
    //InvokeRepeating("UpdateTime", 0f, 10f);   
    InvokeRepeating("GetDataFromWeb", 0f, 1f); 
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
            	int startTime = webRequest.downloadHandler.text.IndexOf("T", 50);
            	int endTime = webRequest.downloadHandler.text.IndexOf(".",startTime);
				string tempTime = webRequest.downloadHandler.text.Substring(startTime+1, (endTime-startTime-4));
                int hours = Int32.Parse(webRequest.downloadHandler.text.Substring(startTime+1, (endTime-startTime-7)));
                //Debug.Log("the time is " + hours);

                //Debug.Log ("hours is " + hours);
                // Debug.Log ("startTime is " + startTime);
                // Debug.Log ("endTime is " + endTime);

                if(timeTextObject.GetComponent<TextMeshPro>().name == "TimeAMPMText") {
                    if (hours == 0) {
                        hours = hours + 12;
                        int Place = tempTime.IndexOf("00");
                        tempTime = tempTime.Remove(Place, 2).Insert(Place, hours.ToString());
                        tempTime = tempTime + " AM";
                    } else if ((hours > 1) && (hours < 12)) {
                        tempTime = tempTime + " AM";
                    } else {
                        int prevHours = hours;
                        hours = hours - 12;
                        int Place = tempTime.IndexOf(prevHours.ToString());
                        tempTime = tempTime.Remove(Place, 2).Insert(Place, hours.ToString());
                        tempTime = tempTime + " PM";
                    }
                    Debug.Log ("tempTime is " + tempTime);
                }
                timeTextObject.GetComponent<TextMeshPro>().text = "" + tempTime + "\n";
            }
        }
    }
}