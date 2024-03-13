using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class cArma : MonoBehaviour
{
    public string nombre;
    public static string Reglas;
    public static string Descripcion;

    public cPersonaje p; // Personaje con esta arma. Capaz mejor al revez, que el personaje tenga un puntero a su arma
    public List<cAcciones> acciones = new List<cAcciones>();
    public List<cReacciones> reacciones = new List<cReacciones>();

    //Caracteristicas
    public int musMult = 100;
    public bool dañoExplota = true;
    public int basePara2doMaton = 1;
    public int bonusAtaque = 100;
    public int guardiaMod = 0;
    public int dadosDelAtacanteMod = 0;
    public int bonusDefensaPropia = 100;
    public int bonusDefensaAjena = 100;
    public int bonusDetenerMovimiento = 100;
    public int bonusIniciativa = 0;
    public bool deRango = true;

    // Armas
    public const int LIGERAS = 0;
    public const int MEDIAS = 1;
    public const int PESADAS = 2;
    public const int PELEA = 3;
    public const int ARCO = 4;
    public const int FUEGO = 5;
    // Tradiciones Armas Pesadas
    public const int VOLUNTAD_CREADOR = 6;

    public int valor_AB;
    public int valor_DB;

    public void AgregarAccionablesAlPersonaje()
    {
        foreach (var item in acciones)
        {
            item.personaje = p;
            p.acciones.Add(item);     
        }
        foreach (var item in reacciones)
        {
            item.personaje = p;
            p.reacciones.Add(item);
        }
    }

    virtual public bool AccionesFase0() { return true; }

    public int GetMusMult()
    {
        return musMult;
    }

    public bool GetDañoExpl()
    {
        return dañoExplota;
    }

    public int GetBaseParaMatonExtra()
    {
        return basePara2doMaton;
    }

    public int GetBonusAtaque()
    {
        return bonusAtaque;
    }

    public int GetGuardiaMod()
    {
        return guardiaMod;
    }

    public int GetDadosDelAtacanteMod()
    {
        return dadosDelAtacanteMod;
    }

    public int GetBonusDefensaPropia()
    {
        return bonusDefensaPropia;
    }

    public int GetBonusDefensaAjena()
    {
        return bonusDefensaAjena;
    }

    public int GetBonusDetenerMovimiento()
    {
        return bonusDetenerMovimiento;
    }

    public int GetBonusIniciativa()
    {
        return bonusIniciativa;
    }
    public bool GetDeRango()
    {
        return deRango;
    }

    public string GetString()
    {
        if(this is cArmasLigeras)
        {
            return "Armas Ligeras";
        }
        else if (this is cArmasMedias)
        {
            return "Armas Medias";
        }
        else if (this is cArmasPesadas)
        {
            return "Armas Pesadas";
        }
        else if (this is cArmasFuego)
        {
            return "Armas de Fuego";
        }
        else if (this is cArmasArco)
        {
            return "Armas Arco";
        }
        else if (this is cArmasPelea)
        {
            return "Armas de Pelea";
        }
        else
        {
            return "error, ningun tipo";
        }
    }

    public string GetStringCorto()
    {
        if (this is cArmasLigeras)
        {
            return "Ligeras";
        }
        else if (this is cArmasMedias)
        {
            return "Medias";
        }
        else if (this is cArmasPesadas)
        {
            return "Pesadas";
        }
        else if (this is cArmasFuego)
        {
            return "Fuego";
        }
        else if (this is cArmasArco)
        {
            return "Arco";
        }
        else if (this is cArmasPelea)
        {
            return "Pelea";
        }
        else
        {
            return "error, ningun tipo";
        }
    }

    static public string GetString(int code)
    {
        switch (code)
        {
            case cArma.LIGERAS:
                return "Armas ligeras";
            case cArma.MEDIAS:
                return "Armas medias";
            case cArma.PESADAS:
                return "Armas pesadas";
            case cArma.ARCO:
                return "Armas arco";
            case cArma.FUEGO:
                return "Armas fuego";
            case cArma.PELEA:
                return "Armas pelea";
            default:
                return "Error Return String";
        }
    }

    static public string GetHabilidadString(int code)
    {
        switch (code)
        {
            case 0:
                return "Ataque Básico";
            case 1:
                return "Defensa Básica";
            default:
                return "Error Return String";
        }
    }

    static public int GetGuardiaMod(int armaCode)
    {
        switch (armaCode)
        {
            case LIGERAS:
                return 0;
            case MEDIAS:
                return 0;
            case PESADAS:
                return -1;
            case ARCO:
                return 0;
            case FUEGO:
                return -2;
            case PELEA:
                return 0;
            case VOLUNTAD_CREADOR:
                return -1;
            default:
                return 0;
        }
    }

    static public int GetMusMult(int armaCode)
    {
        switch (armaCode)
        {
            case LIGERAS:
                return 1;
            case MEDIAS:
                return 2;
            case PESADAS:
                return 3;
            case ARCO:
                return 2;
            case FUEGO:
                return 0;
            case PELEA:
                return 3;
            case VOLUNTAD_CREADOR:
                return 4; // en realidad esto solo seria asi si ya fuesemos maestros, tengo que repensar este sistema
            default:
                return 0;
        }
    }

    static public int GetBaseMatones(int armaCode)
    {
        switch (armaCode)
        {
            case LIGERAS:
                return 9;
            case MEDIAS:
                return 9;
            case PESADAS:
                return 9;
            case ARCO:
                return 9;
            case FUEGO:
                return 6;
            case PELEA:
                return 12;
            case VOLUNTAD_CREADOR:
                return 8;
            default:
                return 0;
        }
    }
}
