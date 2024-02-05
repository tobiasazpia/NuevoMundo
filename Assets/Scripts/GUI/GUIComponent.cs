using System.Collections;
using UnityEngine;


public class GUIComponent : MonoBehaviour
{
    public cPersonaje p;
    public GUISkin guiSkin; // choose a guiStyle (Important!)

    public string text = "Player Name"; // choose your name // index, name, team
    public string guardiaYHeridas = "GH"; // choose your name
    public string dadosDeAccion = "DA";

    public Color color = Color.white;   // choose font color/size
    public float fontSize;
    public float offsetX = 0;
    public float offsetY = (int)(40.0f * Screen.width / 1920.0f);

    float boxW = 150f;
    float boxH = 20f;

    public bool messagePermanent = true;
       
    public float messageDuration { get; set; }

    Vector2 boxPosition;
    void Start()
    {
        fontSize = (int)(30.0f * Screen.width / 1920.0f);
        guiSkin = Resources.Load("First GUI Skin") as GUISkin; 
        if (messagePermanent)
        {
            messageDuration = 1;
        }
    }
    void OnGUI()
    {
        //    if (p.hovered)
        //    {
        //        if (messageDuration > 0)
        //        {
        //            if (!messagePermanent) // if you set this to false, you can simply use this script as a popup messenger, just set messageDuration to a value above 0
        //            {
        //                messageDuration -= Time.deltaTime;
        //            }


        //            GUI.skin = guiSkin;

        //            boxPosition = Camera.main.WorldToScreenPoint(transform.position);
        //            if (GetComponent<Renderer>().isVisible)
        //            {
        //                boxPosition.y = Screen.height - boxPosition.y;
        //                boxPosition.x -= boxW * 0.1f;
        //                boxPosition.y -= boxH * 0.5f;

        //                GUI.contentColor = color;
        //                bool esMaton = p is cMatones;
        //                if (p.selected)
        //                {
        //                    text = p.nombre + " - " + p.equipo.ToString() + " - " + p.arma.GetStringCorto();
        //                }
        //                else
        //                {
        //                    if (esMaton)
        //                    {
        //                        text = p.nombre + " - Cant: " + (p as cMatones).cantidad;
        //                    }
        //                    else
        //                    {
        //                        text = p.nombre + " - Her: " + p.hDram;
        //                    }
        //                }
        //                Vector2 content = guiSkin.box.CalcSize(new GUIContent(text));
        //                float myX = boxPosition.x - content.x / 2;
        //                GUI.Box(new Rect(myX, boxPosition.y + offsetY, content.x, content.y), text);

        //                if (p.selected)
        //                {
        //                    if (esMaton)
        //                    {
        //                        guardiaYHeridas = "Guardia: " + p.GetGuardia() + " - Cant: " + (p as cMatones).cantidad;
        //                    }
        //                    else
        //                    {
        //                        guardiaYHeridas = "Guardia: " + p.GetGuardia() + " - Her: " + p.hDram + " - Daño: " + p.hSupe;
        //                    }
        //                    content = guiSkin.box.CalcSize(new GUIContent(guardiaYHeridas));
        //                    GUI.Box(new Rect(myX, boxPosition.y + offsetY * 2, content.x, content.y), guardiaYHeridas);

        //                    dadosDeAccion = "Bonus Acumulado: " + p.bonusPAtqBporDefB;
        //                    content = guiSkin.box.CalcSize(new GUIContent(dadosDeAccion));
        //                    GUI.Box(new Rect(myX, boxPosition.y + offsetY * 3, content.x, content.y), dadosDeAccion);
        //                }
        //            }
        //        }
        //    }
        //}
    }
}