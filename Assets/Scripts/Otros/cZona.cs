using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class cZona : MonoBehaviour
{
    public int index;
    public string nombre;
    public int[] zonasLimitrofes;
    public int[] zonasEnRango;
    public cGUIZona gz;

    public bool objetivoValidoParaJugadorActivo;
    public PlayerInput py;
    public UICombate uiC;

    // Start is called before the first frame update
    void Start()
    {
        py = GameObject.Find("Sesion").GetComponent<PlayerInput>();
        uiC = GameObject.Find("UI").GetComponent<UICombate>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseOver()
    {
        if (uiC.esperandoZona && objetivoValidoParaJugadorActivo)
        {
            //aplicar shader para resaltarlo como opcion

            //seleccionar
            if (py.actions["Select"].WasPressedThisFrame())
            {
                uiC.OnZonaclicked(index);
            }
        }
    }
}
