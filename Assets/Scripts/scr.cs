using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class scr : MonoBehaviour
{
    // Start is called before the first frame update
    public Text textBox;
    public GameObject prefab;
    public GameObject startPos;
    private int loc;
    public float spacingY;
    void Start()
    {
        loc = 0;
        GedcomMain gm = new GedcomMain();
        gm.getgedcom();
        UnityEngine.Debug.Log("Created the gedcom");
        string name = gm.getCount();
        textBox.text = name;

        //Create the first individual
        FamilyPerson fp = gm.getRoot();
        string birth_str = "UNKNOWN";
        if(fp.Birth != null)
        {
            birth_str = fp.Birth.DateString;
        }
        string death_str = "UNKNOWN";
        if(fp.Death != null)
        {
            death_str = fp.Death.DateString;
        }
        createIndividual(fp.Name, birth_str + " - " + death_str);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Summary: Determine the next location to place the individual
    // Return: Vector3 of the X,Y,Z position to place the individual
    Vector3 determineLocation()
    {
        float newX = startPos.transform.localPosition.x;
        float newY = startPos.transform.localPosition.y + spacingY*loc;
        Vector3 tf = new Vector3(newX,newY,0.0f);
        loc++;
        return tf;

    }

    /// <summary>
    /// Create the individual paper
    /// </summary>
    /// <param name="name"></param>
    /// <param name="life"></param>
    void createIndividual(string name, string life)
    {
        Vector3 pos = determineLocation();
        GameObject canvas = GameObject.Find("Canvas");
        GameObject hit = (GameObject)Instantiate(prefab, pos, Quaternion.identity);
        hit.transform.SetParent(canvas.transform, false);
        //Set the offset based on the parent to match the offset from Canvas of the init_location
        hit.transform.localPosition = pos;
        //for(int i = 0; i< hit.transform.childCount; i++)
        //{
        //    GameObject child = hit.transform.GetChild(i).;
        //    if (child.tag == "name")
        //    {

        //    }
        //}
        TextMeshProUGUI[] texts = hit.GetComponentsInChildren<TextMeshProUGUI>();
        foreach(TextMeshProUGUI tm in texts)
        {
            if(tm.text == "name")
            {
                tm.text = name;
            }
            if (tm.text == "life")
            {
                tm.text = life;
            }
        }
    }
}
