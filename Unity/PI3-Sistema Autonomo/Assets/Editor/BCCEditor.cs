using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class BCCEditor : MonoBehaviour {

    [MenuItem("BCC/read Setup txt file")]
    public static void ReadSetupFile()
    {
        string myTxt = File.ReadAllText(Application.streamingAssetsPath + "/Setup.txt");

        string[] stringSeparators = new string[] { "\r\n" };

        StringReader reader = new StringReader(myTxt);
        string line = myTxt;
        string[] lineSplitEnters = line.Split(stringSeparators, System.StringSplitOptions.None);

        for (int i = 0; i < lineSplitEnters.Length; i++)
        {
            Debug.Log(lineSplitEnters[i]);
        }
    }

    [MenuItem("BCC/read Fila txt file")]
    public static void ReadFilaFile()
    {
        string myTxt = File.ReadAllText(Application.streamingAssetsPath + "/Fila.txt");

        string[] stringSeparators = new string[] { "\r\n" };

        StringReader reader = new StringReader(myTxt);
        string txtToSplit = myTxt;
        string[] lineSplitEnters = txtToSplit.Split(stringSeparators, System.StringSplitOptions.None);

        UserCreator uc = GameObject.Find("UserCreator").GetComponent<UserCreator>();

        for (int i = 0; i < lineSplitEnters.Length; i++)
        {
            User u = new User();
            UserBasics ub = new UserBasics();

            string line = lineSplitEnters[i];
            string userName = lineSplitEnters[i].Split('C')[0];
            string turnoChegada = "C" + lineSplitEnters[i].Split('C')[1].Split('A')[0];
            string ordemGuichês = 'A' + lineSplitEnters[i].Split('C')[1].Split('A')[1];

            ub.name = userName;
            ub.arrivalTurn = int.Parse(turnoChegada.Substring(0));

            Debug.Log("name - " + userName);
            Debug.Log("chegada - " + turnoChegada);
            Debug.Log("guiches - " + ordemGuichês);
        }
    }


}
