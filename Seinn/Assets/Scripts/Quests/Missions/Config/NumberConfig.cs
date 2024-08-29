using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mehroz;

public class NumberConfig : MonoBehaviour
{
    private MisionData config;
    private List<Utils.NumberType> numConfig;
    private List<int> numDivMult;
    private List<int> maxDigits;
    public List<bool> isEquation;

    public static NumberConfig current;

    private void Awake()
    {
        current = this;
        config = SaveSystem.LoadConfigData();
        LoadConfig(config);
        //SetConfig(null, null, null);
    }

    public void LoadConfig(MisionData config)
    {
        SetConfig(config.numConfig, config.maxDigits, config.numDivMult, config.isEquation);
    }

    public void SetConfig(List<Utils.NumberType>nC, List<int>mD, List<int> nDM, List<bool> iE)
    {
        numConfig = nC;
        maxDigits = mD;
        numDivMult = nDM;
        isEquation = iE;
    }

    public Utils.NumberType GetNumberType(int questOrder)
    {
        return numConfig[questOrder - 1];
    }

    public bool GetEquationType(int questOrder)
    {
        return isEquation[questOrder - 1];
    }

    public CustomNumber GetNewNumber(int questOrder)
    {
        int ind = questOrder - 1;
        return new CustomNumber(numConfig[ind], maxDigits[ind], numDivMult[ind],questOrder);
    }

}
