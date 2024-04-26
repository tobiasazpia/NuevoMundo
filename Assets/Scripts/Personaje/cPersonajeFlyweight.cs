using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cPersonajeFlyweight : MonoBehaviour
{

    public sAtributos atr;
    public int[] tradicionMarcial = new int[6];

    public string nombre;
    public int iA;
    public int arma;
    public int maestria;
    public int tradicionMarcialId = -1;
    public int tradicionArcanaId = -1;
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
        tradicionMarcial = aCopiar.tradicionMarcial;
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

    public int GetGuardia()
    {
        int guardia = 0;
        int length = 2;
        if (arma > cArma.FUEGO) length = 6;
        for (int i = 0; i < length; i++)
        {
            guardia += tradicionMarcial[i];
        }
        if (arma > cArma.FUEGO) guardia = guardia / 2;
        guardia += 15 + cArma.GetGuardiaMod(arma);
        if (guardia > 30)
        {
            guardia = 30;
        }
        return guardia;
    }
}
