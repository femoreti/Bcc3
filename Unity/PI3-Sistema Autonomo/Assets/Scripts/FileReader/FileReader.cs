using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor;

public class FileReader : MonoBehaviour {
	public int qtdAtendentes = 0;
	public List<Posto> postos;

	private string relPostos;
	private string relAtendentes;
	private List<string> tempoPostos = new List<string>();
	void Start() {
		ReadTextFile();
		postos = PopulaObjetoPosto();
	}

	private void ReadTextFile() {
		string txt = File.ReadAllText(Application.dataPath + "/StreamingAssets/Setup.txt");
		
		if (txt == null || txt == "") {
			throw new Exception("Não foi possível ler a entrada.");
		}

		string[] txtArray = txt.Split('\n');
		qtdAtendentes = Convert.ToInt16(txtArray[0]);

		foreach (var item in txtArray) {
			var temp = item.Split(':');
			if (temp[0] == "RP") {
				relPostos = temp[1].Replace("\r", "");
			} else if (temp[0] == "RA") {
				relAtendentes = temp[1].Replace("\r", "");
			} else {
				tempoPostos.Add(temp[0]);
			}
		}

		tempoPostos.RemoveAt(0);
		tempoPostos.Remove("TROCA");
		tempoPostos.Remove("");
	}
	private List<Posto> PopulaObjetoPosto() {
		Dictionary<char, int> postos = new Dictionary<char, int>();
		List<Posto> postosList = new List<Posto>();

		for (int i = 0; i < relPostos.Length; i++) {
			if (!postos.ContainsKey(relPostos[i])) {
				postos.Add(relPostos[i], 1);
			} else {
				postos[relPostos[i]]++;
			}
		}
	
		foreach (var item in postos) {
			postosList.Add(new Posto(item.Key, 0, item.Value));
		}

		foreach (var item in tempoPostos) {
			var letra = item.Substring(0, 1);
			var turnos = item.Substring(1, item.Length - 1);
			
			foreach (var posto in postosList) {
				if (posto.letra.ToString() == letra) {
					posto.turnos = Convert.ToInt16(turnos);
					break;
				}
			}
		}

		return postosList;
	}
}
