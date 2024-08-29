using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Number : MonoBehaviour
{
    public CustomNumber value = new CustomNumber();
    public TextMeshPro valueText;

    public void Start()
    {
        valueText.SetText(value.ToString());
    }

    public void DestroyNumber()
    {
        Destroy(gameObject);
    }
}
