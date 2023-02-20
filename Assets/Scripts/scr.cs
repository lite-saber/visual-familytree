using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr : MonoBehaviour
{
    // Start is called before the first frame update
    public Text textBox;
    void Start()
    {
        GedcomMain gm = new GedcomMain();
        gm.getgedcom();
        UnityEngine.Debug.Log("Created the gedcom");
        string name = gm.getCount();
        textBox.text = name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
