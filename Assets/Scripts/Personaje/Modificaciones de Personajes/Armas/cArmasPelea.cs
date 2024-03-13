using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cArmasPelea : cArma
{
    public static new string Descripcion = "Versatil, dificil de golpear y con la opci�n ocasional de atacar desde lejos.";
    public static new string Reglas = "Multiplicador de Musculo: 3, Base para Matones adicionales: 12. Sus dados no explotan en las tiradas de Da�o. Enemigos tienen -1d al atacarlo. Puede adquirir armas improvisadas al comienzo de cada ronda o usar una acci�n para hacerlo. Estas tendr�n un modificador al ataque de 0d, -1d o -2d y modificaran el Multiplciador de M�sculo a 2, 3 o 4, dependiendo de su tama�o. Al usarlas la Base para Matones adicionales es de 9 y los dados de las tiradas de Da�o explotan. Si se tiene �xito en un ataque con un arma improvisada, o si se usara para actuar sobre otra zona, el arma se pierde.";

    public bool armaImprovisadaActiva;
    public int tama�oDeArmaImprovisada;

    const string ABImproTooltipBase = "Ataque B�sico con Arma Improvisada";
    const string MovImproTooltipBase = "Carga con Arma Improvisada";
    const string DBImproTooltipBase = "Defensa B�sica con Arma Improvisada";
    public UICombate uiC;

    // Start is called before the first frame update
    void Start()
    {
        nombre = "Armas de Pelea";

        uiC = GameObject.Find("UI").GetComponent<UICombate>();

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

    public void ActualizarDataDeImprovisada(bool usaImpro)
    {
        Debug.Log("actualizar impro");
        if (usaImpro)
        {
            armaImprovisadaActiva = true;
            Debug.Log("tama�o de aimp " + tama�oDeArmaImprovisada);
            musMult = tama�oDeArmaImprovisada+2;
            da�oExplota = true;
            basePara2doMaton = 9;
            bonusAtaque = 0-tama�oDeArmaImprovisada;
            deRango = true;
        }
        else
        {
            armaImprovisadaActiva = false;
            musMult = 3;
            da�oExplota = false;
            basePara2doMaton = 12;
            bonusAtaque = 0;
            deRango = false;
        }
        p.CalcularExtraParaMatones();
    }

    public void PerderArmaImprovisada()
    {
        armaImprovisadaActiva = false;
    }

    public void AdquirirArmaImprovisada(int tama�o)
    {
        armaImprovisadaActiva = true;
        tama�oDeArmaImprovisada = tama�o;
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
            AdquirirArmaImprovisada(armaI);
            uiC.MostrarArmaEnTooltip(ABImproTooltipBase, DBImproTooltipBase, MovImproTooltipBase, armaI);
            return false;
        }
        return true;
    }
}
