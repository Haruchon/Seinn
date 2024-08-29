using System;
using System.Collections.Generic;

[System.Serializable]
public class MisionData
{
    public List<Utils.NumberType> numConfig;
    public List<int> numDivMult;
    public List<int> maxDigits;
    public List<bool> isEquation;


    public MisionData()
    {
        this.numConfig = null;
        this.numDivMult = null;
        this.maxDigits = null;
        this.isEquation = null;
    }

    public MisionData(List<Utils.NumberType> nC, List<int> nDM, List<int> mD, List<bool>iE)
    {
        this.numConfig = nC;
        this.numDivMult = nDM;
        this.maxDigits = mD;
        this.isEquation = iE;
    }

    public override string ToString()
    {
        string ret = "Number types\n";
        int i = 1;
        foreach(Utils.NumberType type in numConfig)
        {
            ret += i.ToString() + " - " + type.ToString() + "\n";
            i++;
        }
        ret += "Max digits\n";
        i = 1;
        foreach (int j in maxDigits)
        {
            ret += i.ToString() + " - " + j.ToString() + "\n";
            i++;
        }
        ret += "Div o Mult\n";
        i = 1;
        foreach (int j in numDivMult)
        {
            ret += i.ToString() + " - " + j.ToString() + "\n";
            i++;
        }
        ret += "Eq o Ineq\n";
        i = 1;
        foreach (bool b in isEquation)
        {
            ret += i.ToString() + " - " + (b?"Ecuacion":"Inecuacion") + "\n";
            i++;
        }
        return ret;
    }

}
