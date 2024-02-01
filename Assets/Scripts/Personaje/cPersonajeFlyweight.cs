using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cPersonajeFlyweight : MonoBehaviour
{
    public sAtributos atr;
    public sHabilidades hab;

    public string nombre;
    public int iA;
    public int arma;
    public int equipo;
    public bool esMaton;
    public int cantidad;

    public int modGuardiaDeMaton;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Copiar(cPersonajeFlyweight aCopiar)
    {
        atr = aCopiar.atr;
        hab = aCopiar.hab;
        nombre = aCopiar.nombre;
        iA = aCopiar.iA;
        arma = aCopiar.arma;
    }
}
