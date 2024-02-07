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
        Debug.Log("go escarmuza");
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

    static public void FillPlayer(cPersonajeFlyweight p, VisualElement vE)
    {
        //Buscar elementos UI
        VisualElement encabezado = vE.ElementAt(0);
        VisualElement habilidades = vE.ElementAt(1);
        VisualElement atributos = vE.ElementAt(2);

        Label nombre = encabezado.ElementAt(0) as Label;
        Label tipo = encabezado.ElementAt(1) as Label;
        Label arma = encabezado.ElementAt(2) as Label;

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
        if (p.esMaton) tipo.text = "(matones)";
        else tipo.text = "(heroe)";
        arma.text = cArma.GetString(p.arma);

        habValores[0].text = p.hab.ataqueBasico.ToString();
        habValores[1].text = p.hab.defensaBasica.ToString();

        atrValores[0].text = p.atr.maña.ToString();
        atrValores[1].text = p.atr.musculo.ToString();
        atrValores[2].text = p.atr.ingenio.ToString();
        atrValores[3].text = p.atr.brio.ToString();
        atrValores[4].text = p.atr.donaire.ToString();
    }

    static public void FillPlayer(cPersonaje p, VisualElement vE)
    {
        VisualElement vital = vE.ElementAt(0);
        VisualElement tactica = vE.ElementAt(1);
        VisualElement completa = vE.ElementAt(2);

        Label nombre = vital.ElementAt(0) as Label;
        Label herCan = vital.ElementAt(1) as Label;
        Label Guardia = vital.ElementAt(2) as Label;

        Label arma = tactica.ElementAt(0) as Label;
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
        if(p is cMatones)
        {
            herCan.text = "Cantidad: " + (p as cMatones).cantidad;
        }
        else
        {
            herCan.text = "Heridas: " + p.hDram;
        }
        Guardia.text = "Guardia: " + p.GetGuardia();
        bonus.text = "Bonus: " + p.bonusPAtqBporDefB;
        daño.text = "Daño: " +p.hSupe;
        arma.text = p.arma.GetString();

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
        VisualElement habilidades = vE.ElementAt(1);
        VisualElement atributos = vE.ElementAt(2);

        Label nombre = encabezado.ElementAt(0) as Label;
        Label tipo = encabezado.ElementAt(1) as Label;
        Label arma = encabezado.ElementAt(2) as Label;

        //Asignar valores
        nombre.text = " - ";
        tipo.text = " - ";
        arma.text = " - ";
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
