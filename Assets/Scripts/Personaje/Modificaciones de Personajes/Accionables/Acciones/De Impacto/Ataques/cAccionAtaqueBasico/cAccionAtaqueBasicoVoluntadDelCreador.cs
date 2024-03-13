using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cAccionAtaqueBasicoVoluntadDelCreador : cAccionAtaqueBasico
{
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    protected override void TiramosDa�o()
    {
        const int bonusDeVoluntad = 1;
        if (!(c.personajeObjetivo is cMatones))
        {
            int numeroDeDados = 3 + personaje.atr.musculo * personaje.arma.GetMusMult() + personaje.BonusPAtqBporDefB + bonusDeVoluntad;
            if (c.movAgro) numeroDeDados -= 3;
            tirada tr = cDieMath.TirarDados(numeroDeDados, personaje.arma.GetDa�oExpl());
            c.da�o = cDieMath.sumaDe3Mayores(tr);
            uiC.SetText(UIInterface.NombreDePersonajeEnNegrita(personaje) + ", con sus " + personaje.atr.musculo + " en Musculo y multiplicador de " + personaje.arma.GetMusMult() + ", tira " + numeroDeDados + " dados �Haciendo " + UIInterface.IntEnNegrita(c.da�o) + " de da�o!");
            uiC.perCambio = c.personajeObjetivo.nombre;
            c.personajeObjetivo.Da�o += c.da�o;
            //if (personaje.drama) uiC.PedirDrama();
        }
    }
}
