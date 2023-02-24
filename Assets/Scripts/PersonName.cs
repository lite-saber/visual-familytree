using GeneGenie.Gedcom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class PersonName
{
    // name pieces
    public string prefix;
    public string given; // not same as firstname, includes middle etc.
    public string surnamePrefix;

    // already got surname
    public string suffix;
    public string nick;

    // cached surname / firstname split, this is expensive
    // when trying to filter a list of individuals, so do it
    // upon setting the name
    public string surname;

    public PersonName(GedcomName inname)
    {
        given = inname.Given;
        prefix = inname.Prefix;
        suffix = inname.Suffix;
        surname = inname.Surname;
        surnamePrefix = inname.SurnamePrefix;
        nick = inname.Nick;

    }
};