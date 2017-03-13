using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GerenciadorPosto : MonoBehaviour {
    private int qtdAtendentes = 0;
    private float postoWidth = 0;
    private string relPostos;
    private string relAtendentes;
    private List<string> tempoPostos = new List<string>();
    private Dictionary<char, int> relPostosCount = new Dictionary<char, int>();

    public Text nomePostoTxt;
    public Canvas canvas;
    public GameObject postoPrefab;
    public List<GameObject> postos;
    public List<Text> postosNomes;
    void Start()
    {
        postoWidth = postoPrefab.transform.localScale.x;
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
        postosNomes = new List<Text>();
        
        int count = 0;
        float separator = 0.3f;
        Vector3 postoPos = postoPrefab.transform.position;
        Vector3 labelPos = nomePostoTxt.transform.position;

        foreach (char item in relPostos) {
            GameObject newPosto = Instantiate(postoPrefab, postoPos, Quaternion.identity) as GameObject;
            postoPos += new Vector3(postoWidth + separator, 0, 0);
            newPosto.GetComponent<Posto>().letra = item;

            Text postoLabel = Instantiate(nomePostoTxt) as Text;
            postoLabel.transform.SetParent(canvas.transform);
            postoLabel.text = item.ToString();

            postosNomes.Add(postoLabel);
            postos.Add(newPosto);

            count++;
        }
    }

    private void ReadTextFile()
    {
        string txt = File.ReadAllText(Application.streamingAssetsPath + "/Setup.txt");

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
                }

                Debug.Log("LETRA: " + posto.GetComponent<Posto>().letra);
                Debug.Log("TURNO: " + posto.GetComponent<Posto>().turnos);
            }
        }
    }
}
