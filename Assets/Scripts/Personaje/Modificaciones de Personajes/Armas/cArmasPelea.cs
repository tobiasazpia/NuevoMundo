using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cArmasPelea : cArma
{
    public bool armaImprovisadaActiva;
    public int tama�oDeArmaImprovisada;
    // Start is called before the first frame update
    void Start()
    {
        // Falta implementar armas improvisadas en su totalidad
        musMult = 3;
        da�oExplota = false;
        basePara2doMaton = 12;
        bonusAtaque = 0;
        guardiaMod = 0;
        dadosDelAtacanteMod = -1;
        p.totalDadosDelAtacante = dadosDelAtacanteMod;
        bonusDefensaPropia = 0;
        bonusDefensaAjena = -2;
        bonusDetenerMovimiento = 0;
        bonusIniciativa = 0;
        deRango = false;

        p.CalcularExtraParaMatones();
        p.CalcularGuardia();

        acciones.Add(gameObject.AddComponent<cAccionAtaqueBasico>());
        acciones.Add(gameObject.AddComponent<cAccionAtaqueBasicoImprovisadas>());
        acciones.Add(gameObject.AddComponent<cAccionEncontrarImprovisada>());
        reacciones.Add(gameObject.AddComponent<cReaccionDefensaBasica>());
        reacciones.Add(gameObject.AddComponent<cReaccionDefensaBasicaImprovisadas>());
        acciones.Add(gameObject.AddComponent<cAccionMovimientoAgresivo>());
        acciones.Add(gameObject.AddComponent<cAccionMovimientoImpro>());
        AgregarAccionablesAlPersonaje();

    }

    public void PerderArmaImprovisada()
    {
        armaImprovisadaActiva = false;
        musMult = 3;
        da�oExplota = false;
        basePara2doMaton = 12;
        bonusAtaque = 0;
    }

    public void AdquirirArmaImprovisada(int tama�o)
    {
        armaImprovisadaActiva = true;
        tama�oDeArmaImprovisada = tama�o;
        musMult = tama�o+1;
        da�oExplota = true;
        basePara2doMaton = 9;
        bonusAtaque = -tama�o;
    }

    public void ChequearSiArmaImprovisadaSeDestruyo(int atq, int def)
    {
        if (atq < def)
        {
            armaImprovisadaActiva = false;
            musMult = 3;
            da�oExplota = false;
            basePara2doMaton = 12;
            bonusAtaque = 0;
            deRango = false;
        }
    }

    override public bool AccionesFase0()
    {
        if (!(p.arma as cArmasPelea).armaImprovisadaActiva)
        {
            int armaI = Random.Range(0, 3);
            string armaINombre = "";
            switch (armaI)
            {
                case 0:
                    armaINombre = "arma improvisada peque�a";
                    break;
                case 1:
                    armaINombre = "arma improvisada mediana";
                    break;
                case 2:
                    armaINombre = "arma improvisada grande";
                    break;
                default:
                    break;
            }
            p.uiC.SetText(p.nombre + " busca un arma improvisada, y encuentra un " + armaINombre + ".");
            (p.arma as cArmasPelea).armaImprovisadaActiva = true;
            (p.arma as cArmasPelea).tama�oDeArmaImprovisada = armaI;
            return false;
        }
        return true;
    }
}
