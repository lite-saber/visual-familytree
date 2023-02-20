using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class MarriageData
{
    public string Location { get; }
    public FamilyDate eventDate;
    public FamilyPerson wife;
    public FamilyPerson husband;
    public List<FamilyPerson> children;
    public MarriageData(string loc, DateTime? edate1, DateTime? edate2)
    {
        Location = loc;
        eventDate = new FamilyDate(edate1, edate2);
        wife = null;
        husband = null;
        children = new List<FamilyPerson>();
    }

    /* Add Husband to the family
     */
    public void addHusband(FamilyPerson husband_in)
    {
        husband = husband_in;
    }

    /* Add Wife to the family
     */
    public void addWife(FamilyPerson wife_in)
    {
        wife = wife_in;
    }

    /* Add child to family
     */
    public void setChildren(List<FamilyPerson> children_in)
    {
        children = new List<FamilyPerson>(children_in);
    }


}
