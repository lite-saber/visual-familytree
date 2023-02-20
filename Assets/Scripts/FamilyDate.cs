using GeneGenie.Gedcom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class FamilyDate
{
    public DateTime? start;
    public DateTime? end;
    public FamilyDate(DateTime? gd1, DateTime? gd2)
    {
        start = gd1;
        end = gd2;
    }
}
