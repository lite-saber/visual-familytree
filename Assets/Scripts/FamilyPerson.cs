using GeneGenie.Gedcom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class FamilyPerson
{
    public string XRefID { get; }
    public string Name { get; }
    public FamilyDate Birth { get; }
    public FamilyAddress Birth_Location { get; }
    public FamilyAddress Death_Location { get; }
    public FamilyDate Death { get; }
    public enum ENUM_GENDER { MALE, FEMALE, UNKNOWN }
    public ENUM_GENDER gender { get;  }

    public List<MarriageData> marriages { get; }
    public FamilyPerson(GedcomIndividualRecord ir)
    {
        //Get Reference ID
        XRefID = ir.XRefID;

        //Get Gender
        switch (ir.Sex)
        {
            case GeneGenie.Gedcom.Enums.GedcomSex.Male:
                gender = ENUM_GENDER.MALE;
                break;
            case GeneGenie.Gedcom.Enums.GedcomSex.Female:
                gender = ENUM_GENDER.FEMALE;
                break;
            case GeneGenie.Gedcom.Enums.GedcomSex.NotSet:
            case GeneGenie.Gedcom.Enums.GedcomSex.Undetermined:
            case GeneGenie.Gedcom.Enums.GedcomSex.Both:
                gender = ENUM_GENDER.UNKNOWN;
                break;
        }

        //Get Birth
        Birth = new FamilyDate(ir.Birth.Date.DateTime1, ir.Birth.Date.DateTime2);
        GedcomAddress b_addr = ir.Birth.Address;
        Birth_Location = new FamilyAddress(b_addr.AddressLine1, b_addr.AddressLine2, b_addr.AddressLine3, b_addr.City, b_addr.Country, b_addr.State, b_addr.PostCode);

        //get Death
        b_addr = ir.Death.Address;
        Death_Location = new FamilyAddress(b_addr.AddressLine1, b_addr.AddressLine2, b_addr.AddressLine3, b_addr.City, b_addr.Country, b_addr.State, b_addr.PostCode);
        Death = new FamilyDate(ir.Death.Date.DateTime1, ir.Death.Date.DateTime2);

        marriages = new List<MarriageData>();
    }
    public void addMarraige(GedcomFamilyRecord fr, FamilyPerson husband_in, FamilyPerson wife_in, List<FamilyPerson> children)
    {
        GedcomFamilyEvent marriage_info = fr.Marriage;
        GedcomPlace marraige_place = marriage_info.Place;
        string marriage_location = marraige_place.Name;
        GedcomDate marriage_date = marriage_info.Date;
        MarriageData md = new MarriageData(marriage_location, marriage_date.DateTime1,marriage_date.DateTime2);
        md.setChildren(children);
        md.addWife(wife_in);
        md.addHusband(husband_in);
        marriages.Add(md);
    }


}
