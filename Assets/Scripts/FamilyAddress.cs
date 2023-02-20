using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class FamilyAddress
{
    public string Line1;
    public string Line2;
    public string Line3;
    public string City;
    public string Country;
    public string State;
    public string ZipCode;
    public FamilyAddress(string l1, string l2, string l3, string c, string co, string st, string z)
    {
        Line1 = l1;
        Line2 = l2;
        Line3 = l3;
        City = c;
        Country = co;
        State = st;
        ZipCode = z;

    }
}
