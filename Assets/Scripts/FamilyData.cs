using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class FamilyDataOne
{
    public int id;
    public string main;
}
[Serializable]
public class FamilyDataInfo
{
    public int id;
    public string name;
    public List<FamilyDataOne> family;
}
