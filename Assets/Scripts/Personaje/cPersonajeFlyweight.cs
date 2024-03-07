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
    public int tradicionMarcial;
    public int tradicionArcana;
    public int equipo;
    public bool esMaton;
    public int cantidad;
    public int modGuardiaDeMaton;

    public int heridas;
    public bool drama;

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

    public void DescansoCompleto()
    {
        if (iA == cAI.PLAYER_CONTROLLED) drama = true;
        heridas = 0;
    }

    public void DescansoParcial()
    {
        heridas = Mathf.Max(0, heridas - 1);
    }
}
