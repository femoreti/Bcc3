using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class BCCEditor : MonoBehaviour {

    //[MenuItem("BCC/read Setup txt file")]
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

    //[MenuItem("BCC/read Fila txt file")]
    public static void ReadFilaFile()
    {
        string myTxt = File.ReadAllText(Application.streamingAssetsPath + "/Fila.txt");

        string[] stringSeparators = new string[] { "\r\n" };

        StringReader reader = new StringReader(myTxt);
        string txtToSplit = myTxt;
        string[] lineSplitEnters = txtToSplit.Split(stringSeparators, System.StringSplitOptions.None);

        UserCreator uc = GameObject.Find("UserCreator").GetComponent<UserCreator>();
        uc._userLine.Clear();
        for (int i = 0; i < lineSplitEnters.Length; i++)
        {
            UserBasics ub = new UserBasics();

            string line = lineSplitEnters[i];
            Debug.Log("################## - " + line);
            string userName = lineSplitEnters[i].Split('C')[0];
            string turnoChegada = "C" + lineSplitEnters[i].Split('C')[1].Split('A')[0];
            string ordemGuiches = 'A' + line.Split('A')[1];

            ub.name = userName;
            ub.arrivalTurn = int.Parse(turnoChegada.Replace("C", ""));
            ub.walkOrder = new System.Collections.Generic.List<char>();
            Debug.Log("name - " + ub.name);
            Debug.Log("chegada - " + ub.arrivalTurn);

            //Debug.Log("guiches - " + ordemGuiches);
            for (int j = 0; j < ordemGuiches.Length; j++)
            {
                ub.walkOrder.Add(ordemGuiches[j]);
                Debug.Log("guiches - " + ordemGuiches[j]);
            }

            uc._userLine.Add(ub);
        }
    }

    [MenuItem("BCC/SetSprites")]
    public static void SetSprites()
    {
        Sprite[] atendentesSpt = FindSprites("Assets/Images/atendentes");
        GerenciadorPosto gp = GameObject.Find("Controller").GetComponent<GerenciadorPosto>();
        gp._sptAtendentes.Clear();
        foreach (Sprite spt in atendentesSpt)
        {
            if(spt.name[spt.name.Length-1] == 'd')
            {
                gp._sptAtendentes.Add(spt);
            }
        }

        Sprite[] usersSpt = FindSprites("Assets/Images/usuarios");
        Controller gc = GameObject.Find("Controller").GetComponent<Controller>();
        gc._sptUsers.Clear();
        foreach (Sprite spt in usersSpt)
        {
            if (spt.name[spt.name.Length - 1] == 'd')
            {
                gc._sptUsers.Add(spt);
            }
        }

        EditorUtility.SetDirty(gp);
        EditorUtility.SetDirty(gc);

        AssetDatabase.SaveAssets();
    }

    private static Sprite[] FindSprites(string textPath)
    {
        List<Sprite> sprites = new List<Sprite>();
        string[] container = AssetDatabase.FindAssets("t:texture", new string[] { textPath });
        if (container != null)
        {
            for (int x = 0; x < container.Length; x++)
            {
                string tPath = AssetDatabase.GUIDToAssetPath(container[x]);
                sprites.AddRange(AssetDatabase.LoadAllAssetsAtPath(tPath).OfType<Sprite>().ToArray());
            }
        }

        return sprites.ToArray();
    }
}
