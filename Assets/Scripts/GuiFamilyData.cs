using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class GuiFamilyData
{
    public GameObject obj;
    public FamilyPerson fp;
    public List<GuiFamilyData> decendents;
    public List<GuiFamilyData> ancestors;
    public GuiFamilyData spouse;
    public Vector3 centerPoint;

    public GuiFamilyData(GameObject in_obj, FamilyPerson in_fp, Vector3 in_centerPoint)
    {
        decendents = new List<GuiFamilyData>();
        ancestors = new List<GuiFamilyData>();
        obj = in_obj;
        fp = in_fp;
        spouse = null;
        centerPoint = in_centerPoint;
        setupData();
    }

    public void setupData()
    {
        //TODO: Make this done once and save off
        GameObject canvas = GameObject.Find("Canvas");
        obj.transform.SetParent(canvas.transform, false);

        Vector3 initPos = new Vector3(centerPoint.x,centerPoint.y,0.0f);
        //Set the offset based on the parent to match the offset from Canvas of the init_location

        obj.transform.localPosition = initPos;

        string birth_str = "UNKNOWN";
        if (fp.Birth != null)
        {
            birth_str = fp.Birth.DateString;
        }
        string death_str = "UNKNOWN";
        if (fp.Death != null)
        {
            death_str = fp.Death.DateString;
        }
        string name = fp.Name;
        string life = birth_str + " - " + death_str;
        TextMeshProUGUI[] texts = obj.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI tm in texts)
        {
            if (tm.text == "name")
            {
                tm.text = name;
            }
            if (tm.text == "life")
            {
                tm.text = life;
            }
        }

    }
    public void addDecendent(GuiFamilyData gfd)
    {
        decendents.Add(gfd);
    }
    public void addAncestor(GuiFamilyData gfd)
    {
        ancestors.Add(gfd);
    }

    public void addSpouse(GuiFamilyData gfd)
    {
        spouse = gfd;
    }
}
