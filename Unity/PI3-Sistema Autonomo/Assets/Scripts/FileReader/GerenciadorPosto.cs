using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GerenciadorPosto : MonoBehaviour {

    private int qtdAtendentes = 0;

    private string relPostos;
    private string relAtendentes;
    private List<string> tempoPostos = new List<string>();

    public GameObject postoPrefab;
    public List<GameObject> postos;
    void Start()
    {
        ReadTextFile();
        InstanciaPostos();
        PopulaObjetoPosto();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InstanciaPostos()
    {
        postos = new List<GameObject>();
        for (int i = 0; i < tempoPostos.Count; i++)
        {
            Vector3 pos = new Vector3(i * 1.3f, 0, 0);
            pos += postoPrefab.transform.position;
            var posto = Instantiate(postoPrefab, pos, Quaternion.identity) as GameObject;

            postos.Add(posto);
        }
    }

    private void ReadTextFile()
    {
        string txt = File.ReadAllText(Application.dataPath + "/StreamingAssets/Setup.txt");

        // Caso não conseguimos ler o texto (ou seja ele é nulo ou vazio)
        // Joga uma exception
        if (txt == null || txt == "")
        {
            throw new Exception("Não foi possível ler a entrada.");
        }

        // Quebra o texto recebido no \n (ENTER)
        string[] txtArray = txt.Split('\n');

        // A primeira posição do vetor gerado pelo Split
        // é a quantidade de atendentes
        qtdAtendentes = Convert.ToInt16(txtArray[0]);

        // Se a primeira posição for RP(Relação de Postos) então retiramos
        // o \r do final da string para ficar no formato certo e guardamos
        // na variável. O mesmo para RA, todo o resto da string guardamos 
        // na lista tempoPostos e depois tratamos.
        foreach (var item in txtArray)
        {
            var temp = item.Split(':');
            if (temp[0] == "RP")
            {
                relPostos = temp[1].Replace("\r", "");
            }
            else if (temp[0] == "RA")
            {
                relAtendentes = temp[1].Replace("\r", "");
            }
            else
            {
                tempoPostos.Add(temp[0]);
            }
        }

        // Remove o primeiro item da lista, pois é a qtd de atendentes
        // Remove o TROCA e o ultimo espaço em branco do vetor para 
        // termos so os postos e os turnos
        tempoPostos.RemoveAt(0);
        tempoPostos.Remove("TROCA");
        tempoPostos.Remove("");
    }
    private void PopulaObjetoPosto()
    {
        Dictionary<char, int> postos = new Dictionary<char, int>();
        List<Posto> postosList = new List<Posto>();

        // Iteramos na string e adcionamos os caracteres no dicionario
        // sem repetição e incrementamos a quantidade de postos iguais
        // caso nescessário.
        for (int i = 0; i < relPostos.Length; i++)
        {
            if (!postos.ContainsKey(relPostos[i]))
            {
                postos.Add(relPostos[i], 1);
            }
            else
            {
                postos[relPostos[i]]++;
            }
        }

        // Criamos um objeto posto apartir do dicionario criado, ainda sem
        // a quantidade de turnos
        for (int i = 0; i < this.postos.Count - 1; i++)
        {
            foreach (var item in postos)
            {
                this.postos[i].GetComponent<Posto>().letra = item.Key;
                this.postos[i].GetComponent<Posto>().quantidade = item.Value;
                this.postos[i].GetComponent<Posto>().turnos = 0;
                Debug.Log(this.postos[i].GetComponent<Posto>().letra = item.Key);
                break;
            }
        }

        // Adcionamos os turnos ao objeto posto
        foreach (var item in tempoPostos)
        {
            var letra = item.Substring(0, 1);
            var turnos = item.Substring(1, item.Length - 1);

            foreach (var posto in this.postos)
            {
                if (posto.GetComponent<Posto>().letra.ToString() == letra)
                {
                    posto.GetComponent<Posto>().turnos = Convert.ToInt16(turnos);
                    break;
                }
            }
        }
    }
}
