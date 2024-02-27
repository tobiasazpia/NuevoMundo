using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class cZona : MonoBehaviour
{
    public int index;
    public string nombre;
    public int[] zonasLimitrofes;
    public int[] zonasEnRango;

    public bool objetivoValidoParaJugadorActivo;
    public PlayerInput py;
    public UICombate uiC;
    Label nombreUI;

    // Start is called before the first frame update
    void Start()
    {
        py = GameObject.Find("Sesion").GetComponent<PlayerInput>();
        uiC = GameObject.Find("UI").GetComponent<UICombate>();

        switch (index)
        {
            case 0:
                uiC.zona1.text = nombre;
                nombreUI = uiC.zona1;
                break;
            case 1:
                uiC.zona2.text = nombre;
                nombreUI = uiC.zona2;
                break;
            case 2:
                uiC.zona3.text = nombre;
                nombreUI = uiC.zona3;
                break;
            default:
                break;
        }
        //nombreUI.transform.position = uiC.WorldToUIToolkit(transform.position, -Screen.width/2, -Screen.height / 4);

        nombreUI.RegisterCallback<GeometryChangedEvent>(OnGeoChanged);


    }

    void OnGeoChanged(GeometryChangedEvent evt)
    {
        //nombreUI.schedule.Execute(unres).StartingIn(1);
        unres();
    }

    void unres() {
        nombreUI.UnregisterCallback<GeometryChangedEvent>(OnGeoChanged);
        float wProp = 1920.0f / Screen.width;
        Debug.Log("resst width" + nombreUI.resolvedStyle.width);
        Debug.Log("wprop" + wProp);
        uiC.MyWorldToScreen(transform.position, nombreUI, -nombreUI.resolvedStyle.width / 2.0f / wProp, Screen.height / 4);
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
                Debug.Log("zona cliked: ");
                uiC.OnZonaclicked(index);
                uiC.combate.esperandoZona = false;
                uiC.esperandoZona = false;
            }
        }
    }

    private void OnMouseEnter()
    {
        nombreUI.style.display = DisplayStyle.Flex;
    }

    private void OnMouseExit()
    {
        nombreUI.style.display = DisplayStyle.None;
    }
}
