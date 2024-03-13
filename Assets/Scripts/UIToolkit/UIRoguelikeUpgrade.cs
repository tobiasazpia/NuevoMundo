using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class UIRoguelikeUpgrade : MonoBehaviour
{
    private VisualElement pRoguelikeUListo;
    private VisualElement pRoguelikeUAnticipacion;

    VisualElement[] party = new VisualElement[3];
    Button continuar;

    Label anticiparText;
    Button siguienteNivel;

    Button[] upgrades = new Button[3];

    VisualElement[] personajes = new VisualElement[3];
    VisualElement partyElegir;
    Button[] PartyElegirPers = new Button[3];
    VisualElement infoCompleta;

    public cRoguelikeUpgrade rU;

    public PlayerInput py;
    Label tooltip;
    bool hovering;
    float hoverTimer;
    const float buttonTimeTillTooltip = 0.75f;
    const float labelTimeTillTooltip = 0.2f;
    float timeTillTooltip;

    public bool eligiendoNuevoP;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i] = root.Q<Button>("ButtonUpgrade" + (i + 1));
        }

        upgrades[0].RegisterCallback<ClickEvent>(OnUpgrade1Clicked);
        upgrades[1].RegisterCallback<ClickEvent>(OnUpgrade2Clicked);
        upgrades[2].RegisterCallback<ClickEvent>(OnUpgrade3Clicked);

        for (int i = 0; i < personajes.Length; i++)
        {
            personajes[i] = root.Q<VisualElement>("RoguelikePersonaje" + (i + 1));
        }


        pRoguelikeUListo = root.Q<VisualElement>("PantallaRoguelikeUpgradeRevisar");

        partyElegir = root.Q<VisualElement>("PartyElegir");

        for (int i = 0; i < party.Length; i++)
        {
            party[i] = root.Q<Button>("RoguelikePersonaje" + (i + 1));
            RegisterTooltip(upgrades[i]);
        }

        pRoguelikeUAnticipacion = root.Q<VisualElement>("PantallaRoguelikeUpgradeAnticipar");
        anticiparText = root.Q<Label>("mRoguelikeAnticipar");
        siguienteNivel = root.Q<Button>("ButtonRoguelikeSiguienteNivel");
        siguienteNivel.RegisterCallback<ClickEvent>(OnSiguienteNivelClicked);

        continuar = root.Q<Button>("ButtonRevisarContinuar");
        continuar.RegisterCallback<ClickEvent>(OnContinuarClicked);

        infoCompleta = root.Q<VisualElement>("ElegirInfo");
        tooltip = root.Q<Label>("tooltip");
        for (int i = 0; i < 4; i++)
        {
            RegisterTooltip(infoCompleta.ElementAt(0).ElementAt(i));
        }
        for (int i = 0; i < 2; i++)
        {
            RegisterTooltip(infoCompleta.ElementAt(1).ElementAt(i));
        }
        for (int i = 0; i < 5; i++)
        {
            RegisterTooltip(infoCompleta.ElementAt(2).ElementAt(i));
        }

        for (int i = 0; i < party.Length; i++)
        {
            PartyElegirPers[i] = root.Q<Button>("RPerElegir" + (i + 1));
        }
        PartyElegirPers[0].RegisterCallback<ClickEvent>(OnPerElegir1Clicked);
        PartyElegirPers[1].RegisterCallback<ClickEvent>(OnPerElegir2Clicked);
        PartyElegirPers[2].RegisterCallback<ClickEvent>(OnPerElegir3Clicked);
    }

    void Update()
    {
        if (py.actions["Deselect"].WasPressedThisFrame())
        {
            infoCompleta.style.display = DisplayStyle.None;
        }
        if (hovering)
        {
            hoverTimer += Time.deltaTime;
            if (hoverTimer >= timeTillTooltip)
            {
                hoverTimer = 0;
                hovering = false;
                if (eligiendoNuevoP)
                {
                    infoCompleta.style.display = DisplayStyle.Flex;
                    infoCompleta.transform.position = new Vector3(infoCompleta.transform.position.x, infoCompleta.transform.position.y - 840, infoCompleta.transform.position.z);
                }
                else
                {
                    tooltip.style.display = DisplayStyle.Flex;
                    tooltip.transform.position = new Vector3(tooltip.transform.position.x, tooltip.transform.position.y - 125, tooltip.transform.position.z);
                }
            }
        }
    }

    void OnUpgrade1Clicked(ClickEvent evt)
    {
        rU.GetEleccion(0);
        UIInterface.SetCurrent(pRoguelikeUListo);
    }

    void OnUpgrade2Clicked(ClickEvent evt)
    {
        rU.GetEleccion(1);
        UIInterface.SetCurrent(pRoguelikeUListo);
    }

    void OnUpgrade3Clicked(ClickEvent evt)
    {
        rU.GetEleccion(2);
        UIInterface.SetCurrent(pRoguelikeUListo);
    }

    void OnPerElegir1Clicked(ClickEvent evt)
    {
        PerElegirLogic(0, evt);
    }

    void OnPerElegir2Clicked(ClickEvent evt)
    {
        PerElegirLogic(1, evt);
    }

    void OnPerElegir3Clicked(ClickEvent evt)
    {
        PerElegirLogic(2, evt);
    }

    void PerElegirLogic(int index, ClickEvent evt)
    {
        if (rU.rM.party.Count > index)
        {
            Debug.Log("per elec");
            infoCompleta.style.display = DisplayStyle.Flex;
            infoCompleta.transform.position = evt.position -evt.localPosition;
            infoCompleta.transform.position = new Vector3(infoCompleta.transform.position.x, infoCompleta.transform.position.y + 200, infoCompleta.transform.position.z);
            infoCompleta.ElementAt(0).ElementAt(2).style.display = DisplayStyle.Flex;
            infoCompleta.ElementAt(0).ElementAt(3).style.display = DisplayStyle.Flex;
            UIInterface.FillPlayer(rU.rM.party[index], infoCompleta, false);
        }
    }

    public static void ArmaTooltip(int arma, VisualElement vE)
    {
        string text = "";
        text = "Multiplicador de Musculo: " + cArma.GetMusMult(arma) + ", Base para Matones: " + cArma.GetBaseMatones(arma) + ".";
        int g = cArma.GetGuardiaMod(arma);
        if (g != 0) text += " " + g + " a tu Guardia.";
        switch (arma)
        {
            case cArma.LIGERAS:
                text += " +2d a la Inicitiva, +1 al Defenderse a si mismo, -1d a defender a otros y a detener movimiento. +1d al actuar rapido.";
                break;
            case cArma.MEDIAS:
                break;
            case cArma.PESADAS:
                text += " +2d a detener movimiento.";
                break;
            case cArma.ARCO:
                text += " Rango. +2d a defender a otros y a detener movimiento. -1d si defiende de o ataca a un enemigo en su misma zona.";
                break;
            case cArma.FUEGO:
                text += " Rango. No tira Daño, genera una Heridas al aertar. Necesita recargar si falla al atacar, defender a otros o detener movimiento.";
                break;
            case cArma.PELEA:
                text += " Sus dados no explotan en las tiradas de Daño. Enemigos tienen -1d al atacarlo. Puede usar armas improvisadas."; 
                break;
            case cArma.VOLUNTAD_CREADOR:
                text += " +2d a detener movimiento. +1d para Daño.";
                break;
            default:
                break;
        }
        switch (arma)
        {
            case cArma.VOLUNTAD_CREADOR:
                //Principiante
                // Tiras 1 dado adicional en tus tiradas de Daño, y tu Base a superar para Matones adicionales se reduce en 1.
                
                //Veterano
                //Las Explosiones en tus tiradas de Daño dan 2 dados adicionales en vez de 1, y tus 10 en tiradas de Ataque contra Matones valen 11 (Maximo 30).
   
                //Maestro
                //Tu Multiplicador de Músculo es 4.
                    break;
            default:
                break;
        }
        vE.tooltip = text;
    }

    public void UpgradeTooltip(int index, cRoguelikeUpgradeData aUpgradear)
    {
        VisualElement vE = upgrades[index];
        string text = "";
        if (aUpgradear.tipoDeUpgrade == cRoguelikeUpgradeData.RU_PJ) text = "";
        else if (aUpgradear.tipoDeUpgrade == cRoguelikeUpgradeData.RU_NO_UPGRADE) text = "Nada que mejorar.";
        else if (aUpgradear.tipoDeUpgrade == cRoguelikeUpgradeData.RU_DESCANSO_COMPLETO) text = "Descanso completo.";
        else
        {
            int[] bufs = new int[8];
            // guardia = 0;
            // ataques = 0;
            // aB = 0;
            // defensas = 0;
            // dB = 0;
            // daño = 0;
            // heridas = 0;
            // iniciativa = 0;

            for (int i = 0; i < aUpgradear.upgradesList.Count; i++)
            {
                switch (aUpgradear.upgradesList[i].subTipoDeUpgrade)
                {
                    case cRoguelikeUpgradeData.RUST_HAB: // habs
                        switch (aUpgradear.upgradesList[i].elementoAUpgradear)
                        {
                            case 0:
                                bufs[2]++;
                                bufs[0]++;
                                break;
                            case 1:
                                bufs[4]++;
                                bufs[0]++;
                                break;
                            default:
                                break;
                        }
                        break;
                    case cRoguelikeUpgradeData.RUST_ATR: //atr
                        switch (aUpgradear.upgradesList[i].elementoAUpgradear)
                        {
                            case 0:
                                bufs[1]++;
                                break;
                            case 1:
                                bufs[5]++;
                                break;
                            case 2:
                                bufs[3]++;
                                break;
                            case 3:
                                bufs[6]++;
                                break;
                            case 4:
                                bufs[7]++;
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }

            }
            int count = 0;
            foreach (var item in bufs)
            {
                if (item > 0) count++;
            }
            int j = 0;

            if (bufs[0] > 0)
            {
                text += "Guardia +" + bufs[0];
                if (j == count - 2) text += " y ";
                else if (j != count - 1) text += ", ";
                j++;
            }
            if (bufs[1] > 0)
            {
                text += "Ataques +" + bufs[1] + "d";
                if (j == count - 2) text += " y ";
                else if (j != count - 1) text += ", ";
                j++;
            }
            if (bufs[2] > 0)
            {
                text += "Ataque Básico +" + bufs[2] + "d";
                if (j == count - 2) text += " y ";
                else if (j != count - 1) text += ", ";
                j++;
            }
            if (bufs[3] > 0)
            {
                text += "Defensas +" + bufs[3] + "d";
                if (j == count - 2) text += " y ";
                else if (j != count - 1) text += ", ";
                j++;
            }
            if (bufs[4] > 0)
            {
                text += "Defensa Básica +" + bufs[4] + "d";
                if (j == count - 2) text += " y ";
                else if (j != count - 1) text += ", ";
                j++;
            }
            if (bufs[5] > 0)
            {
                text += "Daño +" + bufs[5] + "d * tu Mus Mult";
                if (j == count - 2) text += " y ";
                else if (j != count - 1) text += ", ";
                j++;
            }
            if (bufs[6] > 0)
            {
                text += "Heridas +" + bufs[6] * 3 + "d";
                if (j == count - 2) text += " y ";
                else if (j != count - 1) text += ", ";
                j++;
            }
            if (bufs[7] > 0)
            {
                text += "Iniciativa +" + bufs[7] * 3 + "d";
                if (j == count - 2) text += " y ";
                else if (j != count - 1) text += ", ";
            }

            if (aUpgradear.tipoDeUpgrade == cRoguelikeUpgradeData.RU_DESCANSO_PARCIAL_Y_SIMPLE) text += " y curamos una Herida.";
            else if (aUpgradear.tipoDeUpgrade == cRoguelikeUpgradeData.RU_DESCANSOS_Y_DOBLES)
            {
                text += " y curamos todo.";
            }
        }
        vE.tooltip = text;
    }

    public void SetUpgradeText(int boton, string text)
    {
        upgrades[boton].text = text;
    }

    public void RevisarParty()
    {
        infoCompleta.style.display = DisplayStyle.None;
        int i = 0;
        for (; i < rU.rM.party.Count; i++)
        {
            UIInterface.FillPlayer(rU.rM.party[i], personajes[i], false);
            for (int j = 0; j < 4; j++)
            {
                RegisterTooltip(personajes[i].ElementAt(0).ElementAt(j));
            }
            for (int j = 0; j < 2; j++)
            {
                RegisterTooltip(personajes[i].ElementAt(1).ElementAt(j));
            }
            for (int j = 0; j < 5; j++)
            {
                RegisterTooltip(personajes[i].ElementAt(2).ElementAt(j));
            }
        }
        for (; i < 3; i++)
        {
            UIInterface.NoPlayer(personajes[i]);
        }
    }

    public void OnContinuarClicked(ClickEvent evt)
    {
        anticiparText.text = ("Toma un respiro y preparate, empieza tu pelea numero " + ++rU.rM.nivel);
        UIInterface.SetCurrent(pRoguelikeUAnticipacion);
    }

    public void OnSiguienteNivelClicked(ClickEvent evt)
    {
        infoCompleta.ElementAt(0).ElementAt(2).style.display = DisplayStyle.Flex;
        infoCompleta.ElementAt(0).ElementAt(3).style.display = DisplayStyle.Flex;
        rU.rM.EmpezarCombate();
    }

    public void InfoBasicaPersonajesUpgrade()
    {
        int i = 0;
        for (; i < rU.rM.party.Count; i++)
        {
            UIInterface.FillPlayerBasics(rU.rM.party[i], partyElegir.ElementAt(i));
        }
        for (; i < 3; i++)
        {
            UIInterface.NoPlayerBasics(partyElegir.ElementAt(i));
        }
    }

    void  RegisterTooltip(Button b)
    {
        b.RegisterCallback<MouseEnterEvent>(OnMouseEnterButton);
        b.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButtonOrElement);
    }

    void RegisterTooltip(VisualElement vE)
    {
        vE.RegisterCallback<MouseEnterEvent>(OnMouseEnterVE);
        vE.RegisterCallback<MouseLeaveEvent>(OnMouseLeaveButtonOrElement);
    }

    public void OnMouseEnterButton(MouseEnterEvent evt)
    {
        timeTillTooltip = buttonTimeTillTooltip;
        hovering = true;
        Debug.Log("hovering: " + hovering);
        //tooltip.transform.position = (evt.target as Button).transform.position;
        //Por la posicion del mouse, ver donde esta

        if (eligiendoNuevoP)
        {
            infoCompleta.style.display = DisplayStyle.None;
            cPersonajeFlyweight p;
            if (evt.mousePosition.x < 625)
            {
                p = rU.GetPerInUpgrade(0);
            }else if (evt.mousePosition.x < 1250)
            {
                p = rU.GetPerInUpgrade(1);
            }
            else
            {
                p = rU.GetPerInUpgrade(2);
            }
            infoCompleta.transform.position = evt.mousePosition - evt.localMousePosition;
            infoCompleta.transform.position = new Vector3(infoCompleta.transform.position.x, infoCompleta.transform.position.y + 200, infoCompleta.transform.position.z);
            PersonajeTooltip(p);
        }
        else
        {
            tooltip.transform.position = evt.mousePosition - evt.localMousePosition;
            tooltip.text = (evt.target as Button).tooltip;
        }
    }

    public void OnMouseEnterVE(MouseEnterEvent evt)
    {
        timeTillTooltip = labelTimeTillTooltip;
        hovering = true;
        tooltip.transform.position = evt.mousePosition - evt.localMousePosition;
        tooltip.transform.position = new Vector3(tooltip.transform.position.x, tooltip.transform.position.y + 200, tooltip.transform.position.z);
        tooltip.text = (evt.target as VisualElement).tooltip;
    }

    public void OnMouseLeaveButtonOrElement(MouseLeaveEvent evt)
    {
        tooltip.style.display = DisplayStyle.None;
        hoverTimer = 0;
        hovering = false;
        if (eligiendoNuevoP) infoCompleta.style.display = DisplayStyle.None;
    }

    public void PersonajeTooltip(cPersonajeFlyweight p)
    {
        infoCompleta.ElementAt(0).ElementAt(2).style.display = DisplayStyle.None;
        infoCompleta.ElementAt(0).ElementAt(3).style.display = DisplayStyle.None;
        UIInterface.FillPlayer(p,infoCompleta,false);
    }

}
