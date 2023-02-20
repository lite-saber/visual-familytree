using GeneGenie.Gedcom;
using GeneGenie.Gedcom.Parser;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Using the GeneGenie Gedcom C# library.
// See https://github.com/TheGeneGenieProject/GeneGenie.Gedcom/tree/main
public class GedcomMain
{
    private GedcomRecordReader gedcomReader = null;
    private Dictionary<string, FamilyPerson> allData;
    private Dictionary<string, bool> alreadySeen;
    enum marriage_person { HUSBAND, WIFE }

    public GedcomMain()
    {
        allData = new Dictionary<string, FamilyPerson>();
        alreadySeen = new Dictionary<string, bool>();
    }
    private void loadMarriage(FamilyPerson rootPerson, GedcomFamilyRecord fr, marriage_person mtype)
    {
        //Add children
        List<FamilyPerson> childrenRecords = new List<FamilyPerson>();
        foreach (string childID in fr.Children)
        {
            GedcomIndividualRecord childRecord = gedcomReader.Database.Individuals.Find(f => f.XRefID == childID);
            if (childRecord != null)
            {
                childrenRecords.Add(new FamilyPerson(childRecord));
            }
        }
        //Find Husband
        FamilyPerson husband = null;
        if(mtype == marriage_person.HUSBAND)
        {
            husband = rootPerson;
        } else
        {
            //lookup husband
            GedcomIndividualRecord spouse = gedcomReader.Database.Individuals.Find(ir => fr.Husband == ir.XRefID);
            husband = new FamilyPerson(spouse);
        }
        //Find Wife
        FamilyPerson wife = null;
        if (mtype == marriage_person.WIFE)
        {
            wife = rootPerson;
        }
        else
        {
            //lookup husband
            GedcomIndividualRecord spouse = gedcomReader.Database.Individuals.Find(ir => fr.Wife == ir.XRefID);
            wife = new FamilyPerson(spouse);
        }

        rootPerson.addMarraige(fr, husband, wife, childrenRecords);
    }

    private FamilyPerson loadIndividual(GedcomIndividualRecord ir)
    {
        FamilyPerson fp;
        //Check if individual exists - this is a loop, and should never happen
        if(alreadySeen.ContainsKey(ir.XRefID))
        {
            throw new System.Exception($"Saw the {ir.XRefID} for the second time in unravel, so this is a loop");
        }
        alreadySeen.Add(ir.XRefID, true);
        if (allData.ContainsKey(ir.XRefID))
        {
            fp = allData[ir.XRefID];
        }
        else
        {
            fp = new FamilyPerson(ir);
            allData.Add(fp.XRefID, fp);

            //Find if person is a husband
            List<GedcomFamilyRecord> find_marriage = gedcomReader.Database.Families.FindAll(f => f.Husband == ir.XRefID);
            foreach (GedcomFamilyRecord marriage_one in find_marriage)
            {
                loadMarriage(fp, marriage_one, marriage_person.HUSBAND);
            }
            find_marriage = gedcomReader.Database.Families.FindAll(f => f.Wife == ir.XRefID);
            foreach (GedcomFamilyRecord marriage_one in find_marriage)
            {
                loadMarriage(fp, marriage_one, marriage_person.WIFE);
            }
        }
        return fp;

    }
    /* Load the gedcom file
     * Return "" if no error, else return parsing error
     */
    public string getgedcom()
    {
        gedcomReader = GedcomRecordReader.CreateReader(Application.dataPath + "\\Data\\Sabins.ged");
        //Error check the parsing
        if (gedcomReader.Parser.ErrorState != GeneGenie.Gedcom.Enums.GedcomErrorState.NoError)
        {
            return $"Could not read file, encountered error {gedcomReader.Parser.ErrorState}.";
        }

        //Convert everything to my format
        foreach (GedcomIndividualRecord ir in gedcomReader.Database.Individuals)
        {
            alreadySeen.Clear();//remove already seen to confirm no loops
            loadIndividual(ir);      
        }
        return "";
    }

    /* Get the number of people in the Database
     * Return String of the number
     * If not file open, then return null
     */
    public string getCount()
    {
        if (gedcomReader == null) return null;
        string myout = gedcomReader.Database.Count.ToString();
        return myout;
    }

    //public FamilyPerson getNext()
    //{
    //}
}
