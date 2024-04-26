using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct tirada
{
    public int[] dados;
    public int mod;

    public tirada(int nDados, int lMod)
    {
        dados = new int[nDados];
        mod = lMod;
    }
}

public static class cDieMath
{
    // Tirar x Dados
    public static tirada TirarDados(int nDados)
    {
        int lessThan3 = 0;
        if(nDados < 3)
        {
            lessThan3 = 3 - nDados;
            nDados = 3;
        }
        tirada tr = new tirada(nDados, lessThan3);
        for (int i = 0; i < nDados; i++){
            tr.dados[i] = Random.Range(1,11);
        }
        return tr;
    }

    public static tirada TirarDados10v11(int nDados)
    {
        int lessThan3 = 0;
        if (nDados < 3)
        {
            lessThan3 = 3 - nDados;
            nDados = 3;
        }
        tirada tr = new tirada(nDados, lessThan3);
        for (int i = 0; i < nDados; i++)
        {
            tr.dados[i] = Random.Range(1, 11);
            if (tr.dados[i] == 10) tr.dados[i] = 11;
        }
        return tr;
    }

    public static tirada TirarDados(int nDados, bool expl, bool nueveExplota)
    {
        int otroExpl = 10;
        if (nueveExplota) otroExpl = 9;
        int lessThan3 = 0;
        if (nDados < 3)
        {
            lessThan3 = 3 - nDados;
            nDados = 3;
        }
        tirada tr = new tirada(nDados, lessThan3);
        int res;
        for (int i = 0; i < nDados; i++){
            res = Random.Range(1,11);
            if(expl && (res == 10 || res == otroExpl)){
                int newRes;
                do{
                    newRes = Random.Range(1,11);
                    res += newRes;
                    //if(res == 20){
                    //    Debug.Log("DOBLE CRIT");
                    //}
                    //else if(res == 30){
                    //    Debug.Log("TRIPLE CRIT");
                    //}
                    //else if(res == 40){
                    //    Debug.Log("TETRA CRIT");
                    //}
                    //else if(res == 50){
                    //    Debug.Log("PENTA CRIT");
                    //}
                } while (newRes == 10 || newRes == otroExpl);
            }
            tr.dados[i] = res;
        }
        return tr;
    } 
    
    public static tirada TirarDadosDobleExplosion(int nDados, bool nueveExplota)
    {
        int otroExpl = 10;
        if (nueveExplota) otroExpl = 9;
        int lessThan3 = 0;
        if (nDados < 3)
        {
            lessThan3 = 3 - nDados;
            nDados = 3;
        }
        tirada tr = new tirada(nDados, lessThan3);
        int res;
        for (int i = 0; i < nDados; i++){
            res = Random.Range(1,11);
            if (res == 10 || res == otroExpl) res += DobleExplo(nueveExplota);
            tr.dados[i] = res;
        }
        return tr;
    }

    public static int DobleExplo(bool nueveExplota)
    {
        int otroExpl = 10;
        if (nueveExplota) otroExpl = 9;
        int res = 0;
        int[] newRes = new int[2];
        for (int i = 0; i < 2; i++)
        {
            newRes[i] = Random.Range(1, 11);
            res += newRes[i];
            if (newRes[i] == 10 || newRes[i] == otroExpl) res += DobleExplo(nueveExplota);
        }
        return res;
    }

    public static int sumaDe3Mayores(tirada tr)
    {
        int numeroDeDados = tr.dados.Length;
        int[] maxValues = new int[3];

        for (int i = 0; i < numeroDeDados; i++)
        {
            if (tr.dados[i] > maxValues[0])
            {        
                maxValues[2] = maxValues[1];
                maxValues[1] = maxValues[0];
                maxValues[0] = tr.dados[i];
            }
            else  if (tr.dados[i] > maxValues[1])
            {
                maxValues[2] = maxValues[1];
                maxValues[1] = tr.dados[i];
            }
            else  if (tr.dados[i] > maxValues[2])
            {
                maxValues[2] = tr.dados[i];
            }   
        }
        return maxValues[0] + maxValues[1] + maxValues[2] - tr.mod;
    }

    // 3 menores de una tirada
    public static int[] tomar3Menores(tirada tr)
    {
        return tomarXMenores(tr, 3);
    }

    // X menores de una tirada
    public static int[] tomarXMenores(tirada tr, int x)
    {
        int[] minValues = new int[x];
        for (int j = 0; j < x; j++)
        {
            minValues[j] = 11;
        }
        for (int i = 0; i < tr.dados.Length; i++)
        {
            for (int j = 0; j < x; j++)
            {
                if (tr.dados[i] < minValues[j])
                {
                    for (int k = x-1; k > j; k--)
                    {
                        minValues[k] = minValues[k-1];
                    }
                    minValues[j] = tr.dados[i];
                    break;
                }
            }
        }
        return minValues;
    }

    public static int MyRandomUpgrade()
    {
        int r = Random.Range(0, 100);
        if (r == 99) r--;
        return r;
    }
}
