using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using System;

public class WebData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    String familysearch_base = "https://ident.familysearch.org/cis-web/oauth2/v3/token";
    String CLIENT_ID = "client";
    String USERNAME = "username";
    String PASSWORD = "password";

    private FamilyDataInfo GetFamily()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("{0}?client_id={1}&grant_type=password&username={2}&password={3}", familysearch_base, CLIENT_ID, USERNAME, PASSWORD));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        FamilyDataInfo info = JsonUtility.FromJson<FamilyDataInfo>(jsonResponse);
        return info;
    }
}
