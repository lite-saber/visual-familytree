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
    private string rootPerson;
    private Dictionary<string, FamilyPerson> allData;
    private Dictionary<string, bool> alreadySeen;
    enum marriage_person { HUSBAND, WIFE }

    public GedcomMain()
    {
        allData = new Dictionary<string, FamilyPerson>();
        alreadySeen = new Dictionary<string, bool>();
        rootPerson = null;
    }

    /// <summary>
    /// Return the Root person to get
    /// </summary>
    /// <returns>FamilyPerson who is the root;  or returns NULL if nothing loaded</returns>
    public FamilyPerson getRoot()
    {
        if (rootPerson == null)
        {
            return null;
        }
        return allData[rootPerson];
    }

    private void loadMarriage(FamilyPerson rootPerson, GedcomFamilyRecord fr, marriage_person mtype)
    {
        //Find Husband
        FamilyPerson husband = null;
        string spouse_name = "UNKNOWN";
        if (mtype == marriage_person.HUSBAND)
        {
            husband = rootPerson;
        }
        else
        {
            //lookup husband
            GedcomIndividualRecord spouse = gedcomReader.Database.Individuals.Find(ir => fr.Husband == ir.XRefID);
            if (spouse != null)
            {
                //recurse
                spouse_name = getGedcomName(spouse);
                UnityEngine.Debug.Log($" {rootPerson.Name} has husband {spouse_name}");
                husband = loadIndividual(spouse, fr.XRefID);//do not traverse to this marriage again - this will cause a loop
            }
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
            if (spouse != null)
            {
                //recurse
                spouse_name = getGedcomName(spouse);
                UnityEngine.Debug.Log($" {rootPerson.Name} has wife {spouse_name}");
                wife = loadIndividual(spouse, fr.XRefID);//do not traverse to this marriage again - this will cause a loop
            }
        }
        //Add children
        List<FamilyPerson> childrenRecords = new List<FamilyPerson>();
        foreach (string childID in fr.Children)
        {
            GedcomIndividualRecord childRecord = gedcomReader.Database.Individuals.Find(f => f.XRefID == childID);
            if (childRecord != null)
            {
                //this will recurse
                UnityEngine.Debug.Log($"  {rootPerson.Name} married to {spouse_name}, follow child {getGedcomName(childRecord)}");
                childrenRecords.Add(loadIndividual(childRecord));
            }
        }

        rootPerson.addMarraige(fr, husband, wife, childrenRecords);
        UnityEngine.Debug.Log($" {rootPerson.Name} finished marriage to {spouse_name}");
    }

    private string getGedcomName(GedcomIndividualRecord ir)
    {

        //Get only the preferred name
        GedcomName ir_name = ir.GetName();
        string Name = "UNKNOWN";
        if (ir_name != null)
        {
            //Get fully concatonated name
            Name = ir_name.Name;
        }
        return Name;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ir"></param> Individual record
    /// <param name="ignore_marriage"></param> if NULL then process all marriage; if set to a GedcomFamilyRecord.XRefID, then do not traverse to that marrriage again
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    private FamilyPerson loadIndividual(GedcomIndividualRecord ir, string ignore_marriage = null)
    {
        FamilyPerson fp;
        //Check if individual exists - this is a loop, and should never happen
        if(alreadySeen.ContainsKey(ir.XRefID))
        {

            //Do not need this anymore since the duplicate marriage is already protected against.
            //throw new System.Exception($"Saw the {ir.XRefID}, '{getGedcomName(ir)}', for the second time in unravel, so this is a loop");
        }
        //alreadySeen.Add(ir.XRefID, true);
        if (allData.ContainsKey(ir.XRefID))
        {
            fp = allData[ir.XRefID];
            UnityEngine.Debug.Log($"ReProcessed {fp.Name}");
        }
        else
        {
            fp = new FamilyPerson(ir);
            UnityEngine.Debug.Log($"Processed {fp.Name}");
            allData.Add(fp.XRefID, fp);

            //Find if person is a husband
            List<GedcomFamilyRecord> find_marriage = gedcomReader.Database.Families.FindAll(f => f.Husband == ir.XRefID);
            foreach (GedcomFamilyRecord marriage_one in find_marriage)
            {
                //Skip the ignore marriage to not create a cycle
                if (marriage_one.XRefID != ignore_marriage)
                {
                    UnityEngine.Debug.Log($"  {fp.Name} follow as husband in marriage");
                    loadMarriage(fp, marriage_one, marriage_person.HUSBAND);
                } else
                {
                    UnityEngine.Debug.Log($" {fp.Name} skipped marriage as husband");
                }
            }
            find_marriage = gedcomReader.Database.Families.FindAll(f => f.Wife == ir.XRefID);
            foreach (GedcomFamilyRecord marriage_one in find_marriage)
            {
                //Skip the ignore marriage to not create a cycle
                if (marriage_one.XRefID != ignore_marriage)
                {
                    UnityEngine.Debug.Log($"  {fp.Name} follow as wife in marriage");
                    loadMarriage(fp, marriage_one, marriage_person.WIFE);
                }
                else
                {
                    UnityEngine.Debug.Log($" {fp.Name} skipped marriage as wife");
                }
            }
        }
        UnityEngine.Debug.Log($" {fp.Name} finished Individual");
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
            //Set Root peron on the first person
            if (null == rootPerson)
            {
                rootPerson = ir.XRefID;
            }
            alreadySeen.Clear();//remove already seen to confirm no loops
            UnityEngine.Debug.Log($"Clear - next entry");

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
