using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Utils
{
    public partial class Identifications
    {
        public static Dictionary<string, int> goalID = new Dictionary<string, int>(){
            {"KillLog",1},
            {"CutBadPlant",2},
            {"CollectGoodPlant",3},
            {"AnswerStatues",4},
            {"BoxInstructions",5},
            {"BarrelInstructions",6},
            {"DeactivateStatues",7},
            {"KillSmallMinotaur1",8},
            {"GetKey",9},
            {"KillSmallMinotaur2",10},
            {"WeirdBowls",11},
            {"KillSmallMinotaur3",12},
            {"GrowTrees",13},
            {"KillSmallMinotaur4",14},
            {"DeactivateStatues2",15},
            {"KillMinotaur",16},
        };
    }


    public static int numMissions = 10;
    public static float Precision = 0.0001f;
    public static bool CheckEquality(double x, double y)
    {
        return Mathf.Abs((float)(x - y)) < Precision;
    }
    public enum OperationColors {
        Azul = 0,
        Rojo = 1,
        Verde = 2,
        Negro = 3,
    }

    public enum OperationNames
    {
        Suma = 0,
        Resta = 1,
        Multiplicacion = 2,
        Division = 3,
    }

    public enum NumberType
    {
        Natural = 0,
        Decimal = 1,
        Fraccion = 2,
    }

    public static string RandomEquation(double answer, bool isEquation, int questOrder)
    {
        string equation;
        int number1 = Random.Range(0, 999);
        int number2 = Random.Range(0, 10);
        int operation = CustomNumber.GetOperation(questOrder);
        double kek;
        CustomNumber kek2;
        switch (operation)
        {
            case 0:
                kek = answer + number1;
                kek2 = new CustomNumber(operation, kek, NumberConfig.current.GetNumberType(questOrder));
                equation = "x + "+ number1.ToString() + " = " + kek2.ToString() ;
                break;
            case 1:
                kek = answer - number1;
                kek2 = new CustomNumber(operation, kek, NumberConfig.current.GetNumberType(questOrder));
                equation = "x - " + number1.ToString() + " = " + kek2.ToString();
                break;
            case 2:
                kek = answer * number2;
                kek2 = new CustomNumber(operation, kek, NumberConfig.current.GetNumberType(questOrder));
                equation = "x * " + number2.ToString() + " = " + kek2.ToString();
                break;
            case 3:
                kek = answer / number2;
                kek2 = new CustomNumber(operation, kek, NumberConfig.current.GetNumberType(questOrder));
                equation = "x / " + number2.ToString() + " = " + kek2.ToString();
                break;
            default:
                kek = 0;
                equation = "?";
                break;
        }

        return equation;
    }
}
