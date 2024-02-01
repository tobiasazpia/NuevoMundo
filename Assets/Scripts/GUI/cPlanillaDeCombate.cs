using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cPlanillaDeCombate : MonoBehaviour
{
    public UICombate uiC;

    public GUISkin guiSkin;
    Vector2 boxPosition;
    public Color color = Color.white;   // choose font color/size
    public string text;
    Vector2 content;

    public cCombate c;
    public string m;

    float leftSide;
    float topSide;

    int columnaAncho = (int)(60.0f * Screen.width / 1920.0f);
    int filaAltura = (int)(40.0f * Screen.width / 1920.0f);

    public int fontSize;

    // Start is called before the first frame update
    void Start()
    {
        guiSkin = Resources.Load("First GUI Skin") as GUISkin;

        fontSize = (int)(30.0f * Screen.width / 1920.0f);
        guiSkin.label.fontSize = fontSize;
        leftSide = Screen.width * 3 / 5;
        topSide = Screen.height / 20;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        //if (c != null)
        //{
        //    if (!c.auto)
        //    {
        //        GUI.skin = guiSkin;
        //        GUI.contentColor = color;
            
        //        string t;
        //        if (c.mostrandoMensaje)
        //        {
        //            t = m;
        //        }
        //        else if (c.esperandoAccion)
        //        {
        //             t= "";

        //        } else if (c.esperandoCarga)
        //        {
        //            t = "No podes atacar porque no tenes el arma cargada, queres cargarla?\n\nZ - Si\nX - No";
        //        }
        //        else if (c.esperandoObjetivo)
        //        {
        //            t = "A quien atacamos?\n";
        //            for (int i = 0; i < c.personajes.Count; i++)
        //            {
        //                t += "\n" + (i+1).ToString() + " - " + c.personajes[i].nombre;
        //            }
        //        }
        //        /*else if (c.esperandoReaccion)
        //        {
        //            t = "Interrumpis?\n\nZ - Si\nX - No";
        //        }*/
        //        else if (c.esperandoZona)
        //        {
        //            //t = "A que Zona vamos?\n";
        //            t = m + "\n";
        //            for (int i = 0; i < c.zonas.Count; i++)
        //            {
        //                t += "\n" + (i+1).ToString() + " - " + c.zonas[i].nombre;
        //            }
        //        }
        //        else
        //        {
        //            t = m;
        //        }
        //        text = t;
                
        //        content = guiSkin.box.CalcSize(new GUIContent(text));
        //        GUI.Label(new Rect(50, 55, content.x, content.y),text);
        //    }
        //}
    }

}
