using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class cMenuManager : MonoBehaviour
{
    public EscarmuzaManager s;

    public GameObject mainMenu, jugarMenu, persYEqMenu, nuevoPerMenu, rogueMenu;
    public GameObject equipos, personajes;
    public TMP_Dropdown dDEquipos, dDPersonajes;

    public delegate void ClickAction(); // capaz tengo que hacer un "delegate int" para retornar la eleccion?
    //Main Menu
    //public static event ClickAction eventJugar;
    // public static event ClickAction eventPersonajesYEquipos;
    //public static event ClickAction eventRoguelike;

    // jugar menu
    public static event ClickAction eventGo;
    public static event ClickAction eventSim50;
    public static event ClickAction eventSim5000;

    // acciones menu
    public static event ClickAction eventAtacar;
    public static event ClickAction eventMover;
    public static event ClickAction eventGuardar;

    //si ono
    public static event ClickAction eventSi;
    public static event ClickAction eventNo;

    //nuevo per
    /*public static event ClickAction eventJugar;
    public static event ClickAction eventJugar;*/

    // pers y eq
    public static event ClickAction eventCambiarPersonajeDeEquipo;

    public GameObject nombreEquipo;
    TMP_InputField nombreEquipoIF;

    // Start is called before the first frame update
    void Start()
    {
        s = FindObjectOfType<EscarmuzaManager>();
        nombreEquipoIF = nombreEquipo.GetComponent<TMP_InputField>();
        dDEquipos = equipos.GetComponent < TMP_Dropdown > ();
        dDPersonajes = personajes.GetComponent<TMP_Dropdown>();
    }

    private void OnEnable()
    {
        cCombate.eventTerminoCombate += TerminoCombate;
    }

    private void OnDisable()
    {
        cCombate.eventTerminoCombate -= TerminoCombate;
    }

    //MAIN MENU
    public void OnClickedJugar()
    {
        mainMenu.SetActive(false);
        jugarMenu.SetActive(true);
    }

    public void OnClickedPersonajes()
    {
        mainMenu.SetActive(false);
        persYEqMenu.SetActive(true);
        var dd = dDEquipos.GetComponent<DropdownEquipos>();
        dd.UpdateDropDown(s.equipos);
        int v = dDEquipos.GetComponent<TMP_Dropdown>().value;
        List<string> tempPers = new List<string>();
        foreach (var item in s.personajes)
        {
            if (item.equipo-1 == v)
            {
                tempPers.Add(item.nombre);
            }
        }
        dDPersonajes.GetComponent<TMP_Dropdown>().ClearOptions();
        dDPersonajes.GetComponent<TMP_Dropdown>().AddOptions(tempPers);
        
        dDEquipos.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { DropdownItemSelected(dDEquipos.GetComponent<TMP_Dropdown>()); });
    }

    void DropdownItemSelected(TMP_Dropdown dD)
    {
        int v = dD.value;
        List<string> tempPers = new List<string>(); 
        foreach (var item in s.personajes)
        {
            if (item.equipo - 1 == v)
            {
                tempPers.Add(item.nombre);
            }
        }
        dDPersonajes.GetComponent<TMP_Dropdown>().ClearOptions();
        dDPersonajes.GetComponent<TMP_Dropdown>().AddOptions(tempPers);
    }

    public void OnClickedRoguelike()
    {
        mainMenu.SetActive(false);
        rogueMenu.SetActive(true);
    }

    //JUGAR MENU
    public void OnClickedGo()
    {
        if (eventGo != null)
        {
            jugarMenu.SetActive(false);
            eventGo();
        }
    }

    public void OnClickedSim50()
    {
        if (eventSim50 != null)
        {
            eventSim50();
        }
    }

    public void OnClickedSim5000()
    {
        if (eventSim5000 != null)
        {
            eventSim5000();
        }
    }

    //SIONO MENU
    public void OnClickedSi()
    {
        if (eventSi != null)
        {
            eventSi();
        }
    }

    public void OnClickedNo()
    {
        if (eventNo != null)
        {
            eventNo();
        }
    }

    //ACCIONES MENU
    public void OnClickedAtacar()
    {
        if (eventAtacar != null)
        {
            eventAtacar();
        }
    }

    public void OnClickedMover()
    {
        if (eventMover != null)
        {
            eventMover();
        }
    }

    public void OnClickedGuardar()
    {
        if (eventGuardar != null)
        {
            eventGuardar();
        }
    }

    /*//NUEVO PERSONAJE MENU
    public void OnClickedGuardarCambios()
    {

    }
    public void OnClickedDescartarCambios()
    {

    }*/

    ////PERSONAJES Y EQUIPOS MENU
    public void OnClickedNuevoPersonaje()
    {
        persYEqMenu.SetActive(false);
        nuevoPerMenu.SetActive(true);
    }

    public void OnClickedNuevoEquipo()
    {
        List<string> t = new List<string>();
        t.Add(nombreEquipoIF.text);
        nombreEquipoIF.text = nombreEquipoIF.placeholder.GetComponent<TMP_Text>().text;
        s.equipos.Add(t[0]);
        dDEquipos.AddOptions(t);
    }

    public void OnClickedEliminarPersonaje()
    {
        string toDeleteName = dDPersonajes.options[dDPersonajes.value].text;
        foreach (var item in s.personajes)
        {
            if(item.nombre == toDeleteName)
            {
                dDPersonajes.options.RemoveAt(dDPersonajes.value);
                dDPersonajes.value = 0;
                dDPersonajes.RefreshShownValue();
                s.personajes.Remove(item);
                break;
            }
        }
    }

    public void OnClickedEliminarEquipo()
    {
        for (int i = s.personajes.Count-1; i >= 0; i--)
        {
            if (s.personajes[i].equipo-1 == dDEquipos.value)
            {
                s.personajes.RemoveAt(i);
            }
            else if (s.personajes[i].equipo - 1 > dDEquipos.value)
            {
                s.personajes[i].equipo--;
            }
        }

        dDPersonajes.ClearOptions();
        dDEquipos.options.RemoveAt(dDEquipos.value);        
        s.equipos.RemoveAt(dDEquipos.value);
        dDEquipos.value = 0;
        dDEquipos.RefreshShownValue();

        int v = dDEquipos.value;
        List<string> tempPers = new List<string>();
        foreach (var item in s.personajes)
        {
            if (item.equipo - 1 == v)
            {
                tempPers.Add(item.nombre);
            }
        }
        dDPersonajes.ClearOptions();
        dDPersonajes.AddOptions(tempPers);
    }

    public void OnClickedCambiarPersonajeDeEquipo()
    {
        if (eventCambiarPersonajeDeEquipo != null)
        {
            eventCambiarPersonajeDeEquipo();
        }
    }

    public void OnClickedListo()
    {
        persYEqMenu.SetActive(false);
        mainMenu.SetActive(true);       
    }


    public void OnClickedDescartar()
    {
        nuevoPerMenu.SetActive(false);
        persYEqMenu.SetActive(true);
        int v = dDEquipos.value;
        List<string> tempPers = new List<string>();
        foreach (var item in s.personajes)
        {
            if (item.equipo - 1 == v)
            {
                tempPers.Add(item.nombre);
            }
        }
        dDPersonajes.GetComponent<TMP_Dropdown>().ClearOptions();
        dDPersonajes.GetComponent<TMP_Dropdown>().AddOptions(tempPers);
        dDPersonajes.RefreshShownValue();
        dDPersonajes.value = tempPers.Count - 1;
    }

    void TerminoCombate()
    {
        mainMenu.SetActive(true);
    }

    //Roguelike?
}
