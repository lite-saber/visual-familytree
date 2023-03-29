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
    public int depth_step;
    public int offset_step;
    public int maxDepth;
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
        GuiFamilyData gfd = createFamily(fp,0,0);
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

    private Vector3 calculatePos(int depth, int offset)
    {
        Vector3 pos = new Vector3(coreLocation.x + depth*depth_step, coreLocation.y + offset*offset_step,0.0f);
        return pos;
    }

    private GuiFamilyData createFamily(FamilyPerson fp, int depth, int offset)
    {
        GameObject hit = (GameObject)Instantiate(prefab);

        GuiFamilyData gfd = new GuiFamilyData(hit, fp, calculatePos(depth,offset));
        //Pick the first marraige
        if(fp.marriages.Count > 0)
        {
            MarriageData md = fp.marriages[0];
            FamilyPerson spouse = null;
            int offset_move = 0;
            if(md.wife == fp)
            {
                //they are the wife
                //so add husband
                spouse = md.husband;
                offset_move = -1;
            }
            else if(md.husband == fp)
            {
                //they are the husband
                //so add wife
                spouse = md.wife;
                offset_move = 1;
            }
            //Add Spouse: There could be no spouse known
            if (spouse != null)
            {
                gfd.addSpouse(createFamily(spouse,depth,offset+offset_move));
            }
            //ADD CHILDREN
            int child_offset = 0;
            foreach(FamilyPerson child in md.children)
            {
                gfd.addDecendent(createFamily(child,depth-1,offset+child_offset*2));
                child_offset++;
            }

        }
        return gfd;
    }

}
