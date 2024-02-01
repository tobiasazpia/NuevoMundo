using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NuevoPersonajeMenuController : MonoBehaviour
{
    public GameObject personajePrefab, matonPrefab;
    public cMenuManager mm;
    cPersonaje p;
    EscarmuzaManager s;
    TMP_Dropdown maña, musculo, ingenio, vigor, donaire, golpe, bloqueo, arma, ai, equipo, esMaton;
    TMP_InputField cantidad, nombre;
    

    // Start is called before the first frame update
    void Start()
    {
        
        maña = GameObject.Find("Dropdown Maña").GetComponent<TMP_Dropdown>();
        musculo = GameObject.Find("Dropdown Músculo").GetComponent<TMP_Dropdown>();
        ingenio = GameObject.Find("Dropdown Ingenio").GetComponent<TMP_Dropdown>();
        donaire = GameObject.Find("Dropdown Donaire").GetComponent<TMP_Dropdown>();
        vigor = GameObject.Find("Dropdown Vigor").GetComponent<TMP_Dropdown>();

        golpe = GameObject.Find("Dropdown Golpe").GetComponent<TMP_Dropdown>();
        bloqueo = GameObject.Find("Dropdown Bloqueo").GetComponent<TMP_Dropdown>();

        arma = GameObject.Find("Dropdown Arma").GetComponent<TMP_Dropdown>();
        ai = GameObject.Find("Dropdown Inteligencia").GetComponent<TMP_Dropdown>();
        equipo = GameObject.Find("Dropdown Equipo").GetComponent<TMP_Dropdown>();
        esMaton = GameObject.Find("Dropdown PerMat").GetComponent<TMP_Dropdown>();

        cantidad = GameObject.Find("InputField Cantidad").GetComponent<TMP_InputField>();
        nombre = GameObject.Find("InputField Nombre").GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    /*public void OnClickedDescartar()
    {

    }*/


    public void OnClickedGuardar()
    {
        // chequear que nombre sea unico
      
        if (esMaton.value == 0)
        {
            GameObject temp = Instantiate(personajePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            temp.transform.parent = GameObject.Find("Sesion").transform;
            p = temp.GetComponent<cPersonaje>();
        }
        else
        {
            GameObject temp = Instantiate(matonPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            temp.transform.parent = GameObject.Find("Sesion").transform;
            temp.GetComponent<cMatones>().cantidadInicial = int.Parse(cantidad.text);
            temp.GetComponent<cMatones>().cantidad = int.Parse(cantidad.text);
            p = temp.GetComponent<cMatones>();
        }

        p.nombre = nombre.text;

        p.atr.maña = maña.value;
        p.atr.musculo = musculo.value;
        p.atr.ingenio = ingenio.value;
        p.atr.brio = vigor.value;
        p.atr.donaire = donaire.value;

        p.hab.ataqueBasico = golpe.value;
        p.hab.defensaBasica = bloqueo.value;

        p.SetAI(ai.value);
        p.SetArma(arma.value);

        p.equipo = equipo.value + 1;

        s.personajes.Add(p);
        mm.OnClickedDescartar();        
    }
}
