using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class cAccionIraDivina : cAccionDeImpacto
{
    // Start is called before the first frame update
    void Start()
    {
        GetObjets();
        nombre = "Terror de Dios";
        categoria = cAcciones.AC_CAT_MARCIAL;
        var root = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;
        boton = root.Q<Button>("ButtonRecargar");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
