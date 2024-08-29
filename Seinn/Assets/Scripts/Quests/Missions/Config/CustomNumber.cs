using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mehroz;

public class CustomNumber
{
    public int operation;
    public string color;
    public Fraction number;
    public Utils.NumberType numType;

    public CustomNumber()
    {
        operation = (int)Utils.OperationColors.Azul;
        color = ((Utils.OperationColors)operation).ToString();
        number = new Fraction();
        numType = Utils.NumberType.Natural;
    }

    public CustomNumber(int opera, double numb, Utils.NumberType nT)
    {
        this.operation = opera;
        this.color = ((Utils.OperationColors)this.operation).ToString();
        this.number = new Fraction(RoundDown(numb,2));
        this.numType = nT;
    }

    public CustomNumber(Utils.NumberType numType, int maxDigits, int numDivMult, int questOrder)
    {
        this.operation = GetOperation(questOrder);
        this.color = ((Utils.OperationColors)this.operation).ToString();
        this.numType = numType;
        bool done = false;
        while (!done)
        {
            try
            {
                double numb = RandomNumber(maxDigits, numDivMult, numType);
                //Debug.Log("number: " + numb.ToString());
                this.number = new Fraction(numb);
                //Debug.Log("fraction: " + number.ToString());
                done = true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }

    public override string ToString()
    {
        switch (numType)
        {
            case Utils.NumberType.Natural:
                return number.ToString();

            case Utils.NumberType.Decimal:
                return number.ToDouble().ToString();

            case Utils.NumberType.Fraccion:
                return number.ToString();

            default:
                return number.ToString();
        }
    }

    // Statics

    public static int GetOperation(int questOrder)
    {
        if (questOrder < 4)
            return Random.Range(0, questOrder);
        else
            return Random.Range(0, 4);
    }

    public static double RoundDown(double number, int decimalPlaces)
    {
        return Mathf.Floor((float)number * Mathf.Pow(10, decimalPlaces)) / Mathf.Pow(10, decimalPlaces);
    }

    public static double RandomNumber(int maxDigits, int numDivMult, Utils.NumberType numType)
    {
        float max = Mathf.Pow(10, maxDigits) - 1;
        if (numType.Equals(Utils.NumberType.Natural))
        {
            if (numDivMult != 0)
            {
                double n = Mathf.Round(Random.Range(0, (int)max)) * numDivMult;
                if (n < max)
                    return RoundDown(n, 2);
                else
                    return RoundDown(RandomNumber(maxDigits, numDivMult, numType), 2);
            }
            else
            {
                return RoundDown(Random.Range(0, (int)max), 2);
            }
        }   
        else if (numType.Equals(Utils.NumberType.Fraccion))
        {
            Fraction f = new Fraction(RoundDown(Random.Range(0, max), 2));
            if (f.Numerator < max)
                return RoundDown(f.ToDouble(), 2);
            else
                return RoundDown(RandomNumber(maxDigits, numDivMult, numType), 2);
        }
        else
            return RoundDown(Random.Range(0,max),2);
    }
}
