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
    public GedcomDate Birth { get; }
    public FamilyAddress Birth_Location { get; }
    public FamilyAddress Death_Location { get; }
    public GedcomDate Death { get; }
    public enum ENUM_GENDER { MALE, FEMALE, UNKNOWN }
    public ENUM_GENDER gender { get;  }

    public List<MarriageData> marriages { get; }
    public FamilyPerson(GedcomIndividualRecord ir)
    {
        //Name
        //Get only the preferred name
        GedcomName ir_name = ir.GetName();
        Name = null;
        if(ir_name != null)
        {
            //Get fully concatonated name
            Name = ir_name.Name;
        }

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
        GedcomIndividualEvent ir_birth = ir.Birth;
        Birth = null;
        Birth_Location = null;
        if (ir_birth != null)
        {
            if (ir_birth.Date != null)
            {
                Birth = ir_birth.Date;
            }
            GedcomAddress b_addr = ir_birth.Address;
            if (b_addr != null)
            {
                Birth_Location = new FamilyAddress(b_addr.AddressLine1, b_addr.AddressLine2, b_addr.AddressLine3, b_addr.City, b_addr.Country, b_addr.State, b_addr.PostCode);
            }
        }

        //get Death
        GedcomIndividualEvent ir_death = ir.Death;
        Death_Location = null;
        Death = null;
        if (ir_death != null)
        {
            GedcomAddress b_addr = ir_death.Address;
            if (b_addr != null)
            {
                Death_Location = new FamilyAddress(b_addr.AddressLine1, b_addr.AddressLine2, b_addr.AddressLine3, b_addr.City, b_addr.Country, b_addr.State, b_addr.PostCode);
            }
            if (ir_death.Date != null)
            {
                Death = ir_death.Date;
            }
        }
        marriages = new List<MarriageData>();
    }
    public void addMarraige(GedcomFamilyRecord fr, FamilyPerson husband_in, FamilyPerson wife_in, List<FamilyPerson> children)
    {
        GedcomFamilyEvent marriage_info = fr.Marriage;
        string marriage_location = "";
        GedcomDate marriage_date = null;
        if (marriage_info != null)
        {
            GedcomPlace marraige_place = marriage_info.Place;
            if (marraige_place != null)
            {
                marriage_location = marraige_place.Name;
                marriage_date = marriage_info.Date;
            }
        }
        MarriageData md = new MarriageData(marriage_location, marriage_date);
        md.setChildren(children);
        md.addWife(wife_in);
        md.addHusband(husband_in);
        marriages.Add(md);
    }


}
