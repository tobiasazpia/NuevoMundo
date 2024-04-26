using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIInterface : MonoBehaviour
{
    static private VisualElement pCurrent;

    static private VisualElement pPrincipal;

    static private VisualElement pRoguelike;
    static private VisualElement pEscarmuza;
    static private VisualElement pRogueEnd;

    static private VisualElement pUpgrade;

    public cRoguelikeUpgrade rU;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        pPrincipal = root.Q<VisualElement>("PantallaInicial");

        pEscarmuza = root.Q<VisualElement>("PantallaEscarmuza");
        pRoguelike = root.Q<VisualElement>("PantallaRoguelikeIni");
        pRogueEnd = root.Q<VisualElement>("PantallaRoguelikeFinal");

        pUpgrade = root.Q<VisualElement>("PantallaRoguelikeUpgradeElegir");
    }

    static public void SetCurrent(VisualElement obj)
    {
        if (pCurrent != null)
        {
            pCurrent.style.display = DisplayStyle.None;
        }
        pCurrent = obj;
        pCurrent.style.display = DisplayStyle.Flex;
    }

    static public void GoMainMenu()
    {
        SetCurrent(pPrincipal);
    }

    static public void GoEscarmuza()
    {
        SetCurrent(pEscarmuza);
    }

    static public void GoRoguelike()
    {
        SetCurrent(pRoguelike);
    }

    static public void GoRoguelikeEnd()
    {
        SetCurrent(pRogueEnd);
    }

    static public void GoUpgrade()
    {
        SetCurrent(pUpgrade);
    }

    static public void FillPlayerBasics(cPersonajeFlyweight p, VisualElement vE)
    {
        VisualElement encabezado = vE.ElementAt(0);

        Label nombre = encabezado.ElementAt(0) as Label;

        VisualElement herCont = encabezado.ElementAt(1).ElementAt(0);
        VisualElement dramaCont = encabezado.ElementAt(1).ElementAt(1);

        nombre.text = "<b>" + p.nombre + "</b>";
        (herCont.ElementAt(1) as Label).text = p.heridas.ToString();
        if (p.drama)
        {
            (dramaCont.ElementAt(1) as Label).text = "Sí";
        }
        else
        {
            (dramaCont.ElementAt(1) as Label).text = "No";
        }

    }

    static public void NoPlayerBasics(VisualElement vE)
    {
        VisualElement encabezado = vE.ElementAt(0);

        Label nombre = encabezado.ElementAt(0) as Label;

        VisualElement herCont = encabezado.ElementAt(1).ElementAt(0);
        VisualElement dramaCont = encabezado.ElementAt(1).ElementAt(1);

        nombre.text = "-";
        (herCont.ElementAt(1) as Label).text = "-";
        (dramaCont.ElementAt(1) as Label).text = "";
    }

    static public void FillPlayerNueva(cPersonajeFlyweight p, VisualElement vE, bool jugando)
    {
        // si no estamos jugando (cuando ya termino el roguelike, o cuando es un per para elegir, la primea columna entera, la que tiene ddrama, heridas y daño, queda dseshabilitada, y de la segunda solo queda guardia
        vE.parent.style.alignItems = Align.FlexStart;
        (vE.ElementAt(0) as Label).text = "<b>" + p.nombre; // Nombre
        if (jugando)
        {
            vE.ElementAt(1).ElementAt(0).style.display = DisplayStyle.Flex; // Columna izquierda
            (vE.ElementAt(1).ElementAt(0).ElementAt(0) as Label).text = "Heridas: " + p.heridas;
            vE.ElementAt(1).ElementAt(0).ElementAt(0).tooltip = "Si recibe una tercera Herida durante un combate quedará incapacitado por el resto de el.";
            (vE.ElementAt(1).ElementAt(0).ElementAt(2) as Label).text = "Drama: ";
            if (p.drama) (vE.ElementAt(1).ElementAt(0).ElementAt(2) as Label).text += "Sí";
            else (vE.ElementAt(1).ElementAt(0).ElementAt(2) as Label).text += "No";
        }
        else vE.ElementAt(1).ElementAt(0).style.display = DisplayStyle.None; // Columna izquierda
        vE.ElementAt(1).ElementAt(0).ElementAt(1).style.display = DisplayStyle.None; // Daño
        vE.ElementAt(1).ElementAt(1).ElementAt(1).style.display = DisplayStyle.None; // Bonus
        vE.ElementAt(1).ElementAt(1).ElementAt(2).style.display = DisplayStyle.None; // Sed
        
        (vE.ElementAt(1).ElementAt(1).ElementAt(0) as Label).text = "Guardia: " + p.GetGuardia();

        vE.ElementAt(2).ElementAt(0).ElementAt(0).tooltip = "Las aptitudes naturales del personaje.";
        (vE.ElementAt(2).ElementAt(0).ElementAt(1).ElementAt(0).ElementAt(1) as Label).text = p.atr.maña.ToString();
        (vE.ElementAt(2).ElementAt(0).ElementAt(1).ElementAt(1).ElementAt(1) as Label).text = p.atr.musculo.ToString();
        (vE.ElementAt(2).ElementAt(0).ElementAt(1).ElementAt(2).ElementAt(1) as Label).text = p.atr.ingenio.ToString();
        (vE.ElementAt(2).ElementAt(0).ElementAt(1).ElementAt(3).ElementAt(1) as Label).text = p.atr.brio.ToString();
        (vE.ElementAt(2).ElementAt(0).ElementAt(1).ElementAt(4).ElementAt(1) as Label).text = p.atr.donaire.ToString();

        int cantidadDeHabilidades;
        //arma nombre
        (vE.ElementAt(2).ElementAt(1).ElementAt(0) as Button).text = "<b>" + cArma.GetString(p.arma) + "</b>";
        UIRoguelikeUpgrade.ArmaTooltip(p.arma, vE.ElementAt(2).ElementAt(1).ElementAt(0) as Button, p.maestria);
        if (p.arma > cArma.FUEGO)
        {
            vE.ElementAt(2).ElementAt(1).ElementAt(0).AddToClassList("button");
            vE.ElementAt(2).ElementAt(1).ElementAt(0).AddToClassList("armaButton");
            cantidadDeHabilidades = 6;
            (vE.ElementAt(2).ElementAt(1).ElementAt(0) as Button).text += " - <b>" + cArma.GetMaestriaString(cArma.CalcularMaestria(p.tradicionMarcial)) + "</b>";
            for (int i = 2; i < 6; i++)
            {
                vE.ElementAt(2).ElementAt(1).ElementAt(1).ElementAt(i).style.display = DisplayStyle.Flex;
                //arma campos (menos AB y DB) nombres
                (vE.ElementAt(2).ElementAt(1).ElementAt(1).ElementAt(i).ElementAt(0) as Label).text = cArma.GetHabilidadString(p.arma,i);
            }
        }
        else
        {
            vE.ElementAt(2).ElementAt(1).ElementAt(0).RemoveFromClassList("button");
            vE.ElementAt(2).ElementAt(1).ElementAt(0).RemoveFromClassList("armaButton");
            cantidadDeHabilidades = 2;
            for (int i = 2; i < 6; i++)
            {
                vE.ElementAt(2).ElementAt(1).ElementAt(1).ElementAt(i).style.display = DisplayStyle.None;
            }
        }
        //arma valores
        for (int i = 0; i < cantidadDeHabilidades; i++)
        {
            (vE.ElementAt(2).ElementAt(1).ElementAt(1).ElementAt(i).ElementAt(1) as Label).text = p.tradicionMarcial[i].ToString();
        }
    }

    static public void FillPlayerNueva(cPersonaje p, VisualElement vE)
    {
        //Durante combate
        //unicas variables son si es un per o maton, y si la si es controlado por jugador
        vE.parent.style.alignItems = Align.FlexEnd;
        vE.transform.position = new Vector3(0, 0, 0);
        bool esMaton = p is cMatones;
        bool pj = p.aiCode == cAI.PLAYER_CONTROLLED && !esMaton;

        (vE.ElementAt(0) as Label).text = "<b>" + p.nombre; // Nombre

        vE.ElementAt(1).ElementAt(0).style.display = DisplayStyle.Flex; // Columna izquierda
        if (esMaton)
        {
            (vE.ElementAt(1).ElementAt(0).ElementAt(0) as Label).text = "Cantidad: " + (p as cMatones).Cantidad;
            vE.ElementAt(1).ElementAt(0).ElementAt(0).tooltip = "Matones de este tipo en pie, y acciones que tendrá en la próxima ronda.";
            vE.ElementAt(1).ElementAt(0).ElementAt(1).style.display = DisplayStyle.None;
        }
        else
        {
            vE.ElementAt(1).ElementAt(0).ElementAt(1).style.display = DisplayStyle.Flex;
            (vE.ElementAt(1).ElementAt(0).ElementAt(0) as Label).text = "Heridas: " + p.Heridas;
            vE.ElementAt(1).ElementAt(0).ElementAt(0).tooltip = "Si recibe una tercera Herida durante un combate quedará incapacitado por el resto de el.";
            (vE.ElementAt(1).ElementAt(0).ElementAt(1) as Label).text = "Daño: " + p.Daño;
        }

        if (pj)
        {
            vE.ElementAt(1).ElementAt(0).ElementAt(2).style.display = DisplayStyle.Flex;
            (vE.ElementAt(1).ElementAt(0).ElementAt(2) as Label).text = "Drama: ";
            if (p.Drama) (vE.ElementAt(1).ElementAt(0).ElementAt(2) as Label).text += "Sí";
            else (vE.ElementAt(1).ElementAt(0).ElementAt(2) as Label).text += "No";
        }
        else
            vE.ElementAt(1).ElementAt(0).ElementAt(2).style.display = DisplayStyle.None;

        (vE.ElementAt(1).ElementAt(1).ElementAt(0) as Label).text = "Guardia: " + p.GetGuardia();
        (vE.ElementAt(1).ElementAt(1).ElementAt(1) as Label).text = "Bonus: " + p.BonusPAtqBporDefB;
        if (p.tieneTradicionMarcial)
        {
            vE.ElementAt(2).ElementAt(1).ElementAt(0).AddToClassList("button");
            vE.ElementAt(2).ElementAt(1).ElementAt(0).AddToClassList("armaButton");
            if (p.arma is cLaVoluntadDelCreador)
            {
                vE.ElementAt(1).ElementAt(1).ElementAt(2).style.display = DisplayStyle.Flex;
                (vE.ElementAt(1).ElementAt(1).ElementAt(2) as Label).text = "Ds S. Sangre: " + (p.arma as cLaVoluntadDelCreador).DadosDeSedDeSangre;
            }
        }
        else
        {
            vE.ElementAt(2).ElementAt(1).ElementAt(0).RemoveFromClassList("button");
            vE.ElementAt(2).ElementAt(1).ElementAt(0).RemoveFromClassList("armaButton");
            vE.ElementAt(1).ElementAt(1).ElementAt(2).style.display = DisplayStyle.None; // Sed
        }
        vE.ElementAt(2).ElementAt(0).ElementAt(0).tooltip = "Las aptitudes naturales del personaje.";
        (vE.ElementAt(2).ElementAt(0).ElementAt(1).ElementAt(0).ElementAt(1) as Label).text = p.atr.maña.ToString();
        (vE.ElementAt(2).ElementAt(0).ElementAt(1).ElementAt(1).ElementAt(1) as Label).text = p.atr.musculo.ToString();
        (vE.ElementAt(2).ElementAt(0).ElementAt(1).ElementAt(2).ElementAt(1) as Label).text = p.atr.ingenio.ToString();
        (vE.ElementAt(2).ElementAt(0).ElementAt(1).ElementAt(3).ElementAt(1) as Label).text = p.atr.brio.ToString();
        (vE.ElementAt(2).ElementAt(0).ElementAt(1).ElementAt(4).ElementAt(1) as Label).text = p.atr.donaire.ToString();

        int cantidadDeHabilidades;
        //arma nombre
        (vE.ElementAt(2).ElementAt(1).ElementAt(0) as Button).text = "<b>" + cArma.GetString(p.armaCode) + "</b>";
        UIRoguelikeUpgrade.ArmaTooltip(p.armaCode, vE.ElementAt(2).ElementAt(1).ElementAt(0) as Button, cArma.CalcularMaestria(p.tradicionMarcial));

        if (p.tieneTradicionMarcial)
        {
            Debug.Log("tiene tradicion marcial " + p.nombre);
            cantidadDeHabilidades = 6;
            (vE.ElementAt(2).ElementAt(1).ElementAt(0) as Button).text += " - <b>" + cArma.GetMaestriaString(cArma.CalcularMaestria(p.tradicionMarcial)) + "</b>";
            for (int i = 2; i < 6; i++)
            {
                vE.ElementAt(2).ElementAt(1).ElementAt(1).ElementAt(i).style.display = DisplayStyle.Flex;
                //arma campos (menos AB y DB) nombres
                (vE.ElementAt(2).ElementAt(1).ElementAt(1).ElementAt(i).ElementAt(0) as Label).text = cArma.GetHabilidadString(p.armaCode,i);
            }
        }
        else
        {
            cantidadDeHabilidades = 2;
            for (int i = 2; i < 6; i++)
            {
                vE.ElementAt(2).ElementAt(1).ElementAt(1).ElementAt(i).style.display = DisplayStyle.None;
            }
        }
        //arma valores
        for (int i = 0; i < cantidadDeHabilidades; i++)
        {
            (vE.ElementAt(2).ElementAt(1).ElementAt(1).ElementAt(i).ElementAt(1) as Label).text = p.tradicionMarcial[i].ToString();
        }
    }

    static public void FillPlayerNueva(VisualElement vE)
    {
        // si no estamos jugando (cuando ya termino el roguelike, o cuando es un per para elegir, la primea columna entera, la que tiene ddrama, heridas y daño, queda dseshabilitada, y de la segunda solo queda guardia
        (vE.ElementAt(0) as Label).text = ""; // Nombre
        vE.ElementAt(1).ElementAt(0).style.display = DisplayStyle.None; // Columna izquierda
        vE.ElementAt(1).ElementAt(0).ElementAt(1).style.display = DisplayStyle.None; // Daño
        vE.ElementAt(1).ElementAt(1).ElementAt(1).style.display = DisplayStyle.None; // Bonus
        vE.ElementAt(1).ElementAt(1).ElementAt(2).style.display = DisplayStyle.None; // Sed

        (vE.ElementAt(1).ElementAt(1).ElementAt(0) as Label).text = "Guardia: " + " ";

        (vE.ElementAt(2).ElementAt(0).ElementAt(1).ElementAt(0).ElementAt(1) as Label).text = " ";
        (vE.ElementAt(2).ElementAt(0).ElementAt(1).ElementAt(1).ElementAt(1) as Label).text = " ";
        (vE.ElementAt(2).ElementAt(0).ElementAt(1).ElementAt(2).ElementAt(1) as Label).text = " ";
        (vE.ElementAt(2).ElementAt(0).ElementAt(1).ElementAt(3).ElementAt(1) as Label).text = " ";
        (vE.ElementAt(2).ElementAt(0).ElementAt(1).ElementAt(4).ElementAt(1) as Label).text = " ";

        int cantidadDeHabilidades;
        //arma nombre
        (vE.ElementAt(2).ElementAt(1).ElementAt(0) as Button).text = " ";

        cantidadDeHabilidades = 2;
        for (int i = 2; i < 6; i++)
        {
            vE.ElementAt(2).ElementAt(1).ElementAt(1).ElementAt(i).style.display = DisplayStyle.None;
        }
        //arma valores
        for (int i = 0; i < cantidadDeHabilidades; i++)
        {
            (vE.ElementAt(2).ElementAt(1).ElementAt(1).ElementAt(i).ElementAt(1) as Label).text = " ";
        }
    }

    static public void FillPlayerTacticaNueva(cPersonaje p, VisualElement vE)
    {
        (vE.ElementAt(0) as Label).text = "<b>" + p.nombre;
        VisualElement v = vE.ElementAt(1);
        VisualElement t = vE.ElementAt(2);

        if (p is cMatones) (v.ElementAt(0) as Label).text = "Cantidad: " + (p as cMatones).Cantidad;
        else (v.ElementAt(0) as Label).text = "Heridas: " + p.Heridas;
        if (p is cMatones) v.ElementAt(1).style.display = DisplayStyle.None;
        else
        {
            v.ElementAt(1).style.display = DisplayStyle.Flex;
            (v.ElementAt(1) as Label).text = "Daño: " + p.Daño.ToString();
        }
        if (p.aiCode == cAI.PLAYER_CONTROLLED && !(p is cMatones))
        {
            v.ElementAt(2).style.display = DisplayStyle.Flex;
            (v.ElementAt(2) as Label).text = "Drama: ";
            if (p.Drama) (v.ElementAt(2) as Label).text += "Sí";
            else (v.ElementAt(2) as Label).text += "No";
        }
        else v.ElementAt(2).style.display = DisplayStyle.None;
        (v.ElementAt(3) as Label).text = "Guardia: " + p.GetGuardia().ToString();

        (t.ElementAt(0) as Label).text = "Bonus: " + p.BonusPAtqBporDefB;
        if (p.arma is cLaVoluntadDelCreador)
        {
            t.ElementAt(1).style.display = DisplayStyle.Flex;
            (t.ElementAt(1) as Label).text = "Sed: " + (p.arma as cLaVoluntadDelCreador).DadosDeSedDeSangre;
        }
        else t.ElementAt(1).style.display = DisplayStyle.None;
        (t.ElementAt(2) as Button).text = cArma.GetString(p.armaCode);

    }

    static public void FillPlayerHoverNueva(cPersonaje p, VisualElement vE)
    {
        (vE.ElementAt(0) as Label).text = "<b>" + p.nombre;
        if (p is cMatones) (vE.ElementAt(1) as Label).text = "Cantidad: " + (p as cMatones).Cantidad;
        else (vE.ElementAt(1) as Label).text = "Heridas: " + p.Heridas;
        if (p is cMatones) vE.ElementAt(2).style.display = DisplayStyle.None;
        else
        {
            vE.ElementAt(2).style.display = DisplayStyle.Flex;
            (vE.ElementAt(2) as Label).text = "Daño: " + p.Daño.ToString();
        }
        if (p.aiCode == cAI.PLAYER_CONTROLLED && !(p is cMatones))
        {
            vE.ElementAt(3).style.display = DisplayStyle.Flex;
            (vE.ElementAt(3) as Label).text = "Drama: ";
            if (p.Drama) (vE.ElementAt(3) as Label).text += "Sí";
            else (vE.ElementAt(3) as Label).text += "No";
        }
        else vE.ElementAt(3).style.display = DisplayStyle.None;
        (vE.ElementAt(4) as Label).text = "Guardia: " + p.GetGuardia().ToString();
    }


    static public string NombreDePersonajeEnNegrita(cPersonaje per)
    {
        return "<b>" + per.nombre + "</b>";
    }

    static public string IntEnNegrita(int num)
    {
        return "<b>" + num + "</b>";
    }

    static public string IntExitoso(int num)
    {
        return "<b><color=#15942eff>" + num + "</color></b>";
    }

    static public string IntFallido(int num)
    {
        return "<b><color=red>" + num + "</color></b>";
    }

    static public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
