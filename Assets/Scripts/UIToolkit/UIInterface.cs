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

    static public void FillPlayer(cPersonajeFlyweight p, VisualElement vE, bool isEnd)
    {
        //Buscar elementos UI
        VisualElement encabezado = vE.ElementAt(0);
        VisualElement habilidades = vE.ElementAt(1);
        VisualElement atributos = vE.ElementAt(2);

        Label nombre = encabezado.ElementAt(0) as Label;
        Label arma = encabezado.ElementAt(1) as Label;
        UIRoguelikeUpgrade.ArmaTooltip(p.arma,arma);
        Debug.Log("arma tooltip 1" + arma.tooltip);
        if (!isEnd)
        {
            VisualElement herCont = encabezado.ElementAt(2);
            VisualElement dramaCont = encabezado.ElementAt(3);

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

        int numeroDeHabilidades = 2;
        Label[] habValores = new Label[numeroDeHabilidades];
        for (int i = 0; i < numeroDeHabilidades; i++)
        {
            habValores[i] = habilidades.ElementAt(i).ElementAt(1) as Label;
            
        }

        int numeroDeAtributos = 5;
        Label[] atrValores = new Label[numeroDeAtributos];
        for (int i = 0; i < numeroDeAtributos; i++)
        {
            atrValores[i] = atributos.ElementAt(i).ElementAt(1) as Label;
        }

        //Asignar valores
        nombre.text = p.nombre;
        arma.text = cArma.GetString(p.arma);

        habValores[0].text = p.hab.ataqueBasico.ToString();
        habValores[1].text = p.hab.defensaBasica.ToString();

        atrValores[0].text = p.atr.maña.ToString();
        atrValores[1].text = p.atr.musculo.ToString();
        atrValores[2].text = p.atr.ingenio.ToString();
        atrValores[3].text = p.atr.brio.ToString();
        atrValores[4].text = p.atr.donaire.ToString();
    }

    static public void FillPlayerBasics(cPersonajeFlyweight p, VisualElement vE)
    {
        VisualElement encabezado = vE.ElementAt(0);

        Label nombre = encabezado.ElementAt(0) as Label;

        VisualElement herCont = encabezado.ElementAt(1).ElementAt(0);
        VisualElement dramaCont = encabezado.ElementAt(1).ElementAt(1);

        nombre.text = p.nombre;
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
        (dramaCont.ElementAt(1) as Label).text = "-";
    }

    static public void FillPlayer(cPersonaje p, VisualElement vE)
    {
        VisualElement vital = vE.ElementAt(0);
        VisualElement tactica = vE.ElementAt(1);
        VisualElement completa = vE.ElementAt(2);

        Label nombre = vital.ElementAt(0) as Label;
        Label herCan = vital.ElementAt(1) as Label;
        Label drama = vital.ElementAt(2) as Label;
        Label Guardia = vital.ElementAt(3) as Label;

        Label arma = tactica.ElementAt(0) as Label;
        UIRoguelikeUpgrade.ArmaTooltip(p.armaCode, arma);
        Debug.Log("arma tooltip 2" + arma.tooltip);
        Label bonus = tactica.ElementAt(1) as Label;
        Label daño = tactica.ElementAt(2) as Label;

        VisualElement habilidades = completa.ElementAt(0);
        VisualElement atributos = completa.ElementAt(1);

        int numeroDeHabilidades = 2;
        Label[] habValores = new Label[numeroDeHabilidades];
        for (int i = 0; i < numeroDeHabilidades; i++)
        {
            habValores[i] = habilidades.ElementAt(i).ElementAt(1) as Label;
        }

        int numeroDeAtributos = 5;
        Label[] atrValores = new Label[numeroDeAtributos];
        for (int i = 0; i < numeroDeAtributos; i++)
        {
            atrValores[i] = atributos.ElementAt(i).ElementAt(1) as Label;
        }

        //Asignar valores
        nombre.text = p.nombre;
        if (p is cMatones)
        {
            herCan.text = "Cantidad: " + (p as cMatones).Cantidad;
        }
        else
        {
            herCan.text = "Heridas: " + p.Heridas;
        }
        Guardia.text = "Guardia: " + p.GetGuardia();
        bonus.text = "Bonus: " + p.BonusPAtqBporDefB;
        if (p is cMatones) daño.style.display = DisplayStyle.None;
        else
        {
            daño.style.display = DisplayStyle.Flex;
            daño.text = "Daño: " + p.Daño;
        }
        arma.text = p.arma.GetString();
        if (p.Drama)
        {
            drama.text = "Drama - Sí";
        }
        else
        {
            drama.text = "Drama - No";
        }

        habValores[0].text = p.hab.ataqueBasico.ToString();
        habValores[1].text = p.hab.defensaBasica.ToString();

        atrValores[0].text = p.atr.maña.ToString();
        atrValores[1].text = p.atr.musculo.ToString();
        atrValores[2].text = p.atr.ingenio.ToString();
        atrValores[3].text = p.atr.brio.ToString();
        atrValores[4].text = p.atr.donaire.ToString();
    }

    static public void NoPlayer(VisualElement vE)
    {
        VisualElement encabezado = vE.ElementAt(0);

        Label nombre = encabezado.ElementAt(0) as Label;
        Label arma = encabezado.ElementAt(1) as Label;

        //Asignar valores
        nombre.text = " - ";
        arma.text = " - ";
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
