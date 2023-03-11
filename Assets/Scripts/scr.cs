using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class scr : MonoBehaviour
{
    // Start is called before the first frame update
    public Text textBox;
    public GameObject prefab;
    public GameObject startPos;
    private int loc;
    public float spacingY;
    private Vector3 coreLocation;
    void Start()
    {
        createCoreLocation();
        loc = 0;
        GedcomMain gm = new GedcomMain();
        gm.getgedcom();
        UnityEngine.Debug.Log("Created the gedcom");
        string name = gm.getCount();
        textBox.text = name;

        //Create the first individual
        FamilyPerson fp = gm.getRoot();
        createFamily(fp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Summary: Determine the next location to place the individual
    // Return: Vector3 of the X,Y,Z position to place the individual
    private void createCoreLocation()
    {
        float newX = startPos.transform.localPosition.x;
        float newY = startPos.transform.localPosition.y;
        coreLocation = new Vector3(newX,newY,0.0f);
    }

    private GuiFamilyData createFamily(FamilyPerson fp)
    {
        GameObject hit = (GameObject)Instantiate(prefab);//, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);

        GuiFamilyData gfd = new GuiFamilyData(hit, fp, coreLocation);
        //Pick the first marraige
        if(fp.marriages.Count > 0)
        {
            MarriageData md = fp.marriages[0];
            FamilyPerson spouse = null;
            if(md.wife == fp)
            {
                //they are the wife
                //so add husband
                spouse = md.husband;
            }
            else
            {
                //they are the husband
                //so add wife
                spouse = md.wife;
            }
            //Verify we aren't doing things wrong
            Assert.AreNotEqual(null,spouse);
            gfd.addSpouse(createFamily(spouse));
            //ADD CHILDREN
            foreach(FamilyPerson child in md.children)
            {
                gfd.addDecendent(createFamily(child));
            }

        }
        return gfd;
    }

}
