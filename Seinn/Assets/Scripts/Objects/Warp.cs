using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Warp : MonoBehaviour
{

    //Para almacenar el punto de destino
    public GameObject target;

    //Para almacenar el mapa de destino
    public GameObject targetMap;

    //Para controlar si empieza o no la transición
    bool start = false;

    //Para controlar si la transición es de entrada o salida
    bool isFadeIn = false;

    //Opacidad inicial del cuadro de transición
    float alpha = 0;

    //Transición de 1 segundo
    float fadeTime = 1f;

    private void Awake()
    {
        //Verificar que el punto de destino este seteado
        Assert.IsNotNull(target);

        //Deshabilitamos los sprites para que no se vean en la pantalla
        GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        //Verificar que el mapa de destino este seteado
        Assert.IsNotNull(targetMap);
    }

    //Funcion del warp
    IEnumerator  OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Deshabilitar la animación y el movimiento
            other.GetComponent<PlayerController2D>().enabled = false;
            other.GetComponent<Animator>().enabled = false;

            //Efecto difuminación
            FadeIn();

            yield return new WaitForSeconds(fadeTime / 2);
            other.transform.position = target.transform.GetChild(0).transform.position;
            Camera.main.GetComponent<MainCamera>().SetBound(targetMap);

            //Efecto difuminación
            FadeOut();

            //Habilitar la animación y el movimiento
            other.GetComponent<PlayerController2D>().enabled = true;
            other.GetComponent<Animator>().enabled = true;

        }

    }

    private void OnGUI()
    {
        //Si no empieza la transicion, salimos del evento 
        if (!start)
            return;

        //Si ha empezado, creamos un color con una opacidad inicial a 0
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);

        //Creamos una textura temporal para rellenar la pantalla
        Texture2D texture;
        texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.black);
        texture.Apply();

        //Dibujamos la textura sobre toda la pantalla
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);

        //Controlamos la transparencia
        if (isFadeIn)
        {
            //Si es aparecer, sumamos opacidad
            alpha = Mathf.Lerp(alpha, 1.1f, fadeTime * Time.deltaTime);
        }
        else
        {
            // Si es desaparecer, restamos opacidad
            alpha = Mathf.Lerp(alpha, -0.1f, fadeTime * Time.deltaTime);
            if (alpha < 0) start = false;
        }

    }


    //Activar transición de entrada
    void FadeIn()
    {
        start = true;
        isFadeIn = true;
    }

    //Activar transición de salida
    void FadeOut()
    {
        isFadeIn = false;
    }
}
