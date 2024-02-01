using System.Collections;
using UnityEngine;


public class cGUIZona : MonoBehaviour
{
    public cZona z;
    public GUISkin guiSkin; // choose a guiStyle (Important!)

    public string text = "Zona Name"; // choose your name // index, name, team

    public Color color = Color.white;   // choose font color/size
    public int fontSize = (int)(30.0f * Screen.width / 1920.0f);
    public float offsetX = 1;
    public float offsetY = (int)(-1050 * Screen.width / 1920.0f);

    float boxW = 150f;
    float boxH = 20f;

    public bool messagePermanent = true;

    public float messageDuration { get; set; }

    Vector2 boxPosition;
    void Start()
    {
        guiSkin = Resources.Load("First GUI Skin") as GUISkin; // choose a guiStyle (Important!)
        if (messagePermanent)
        {
            messageDuration = 1;
        }
    }
    void OnGUI()
    {
        if (messageDuration > 0)
        {
            if (!messagePermanent) // if you set this to false, you can simply use this script as a popup messenger, just set messageDuration to a value above 0
            {
                messageDuration -= Time.deltaTime;
            }


            GUI.skin = guiSkin;
            boxPosition = Camera.main.WorldToScreenPoint(transform.position);
            if (GetComponent<Renderer>().isVisible)
            {
                boxPosition.y = Screen.height+50;//Screen.height - boxPosition.y;
                boxPosition.x -= boxW * 0.1f;
                boxPosition.y += boxH * 1.5f;
                guiSkin.box.fontSize = fontSize;

                GUI.contentColor = color;

                Vector2 content = guiSkin.box.CalcSize(new GUIContent(text));
                GUI.Box(new Rect(boxPosition.x - content.x / 2 * offsetX, boxPosition.y + offsetY, content.x, content.y), text);
            }
        }
    }
}