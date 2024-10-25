using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;

public static class Database 
{
    private static StreamWriter sw;
    private static StreamReader sr;

    public static Dictionary<int, List<Dialogue>> DialogueDatabase
    { get; private set; } = new Dictionary<int, List<Dialogue>>();

    public static void InitializeDatabases()
    {
        InitializeDialogues();
    }
    private static void InitializeDialogues()
    {
        List<string> tempDialogues = ParseCSV("DialogueDatabase.csv");

        foreach(string tempDialogue in tempDialogues)
        {
            string[] values = tempDialogue.Split(','); //Splits the csv using commas 
            string[] id = values[0].Split("_"); //Splits the id into two parts, the group and the id in that group

            if (!DialogueDatabase.ContainsKey(int.Parse(id[0]))) //Checks if the dialogue group exists  
                DialogueDatabase.Add(int.Parse(id[0]), new List<Dialogue>()); //Adds the group with a list if it isn't 

            //Adds the dialogue values into the group based on its id 
            DialogueDatabase[int.Parse(id[0])].Add(new Dialogue(id[1], values[1], values[2], values[3]));
        }
    }
    private static List<string> ParseCSV(string filePath)
    {
        List<string> result = new List<string>();

        sr = File.OpenText(Application.streamingAssetsPath + "/Databases/" + filePath);

        sr.ReadLine();
        while (!sr.EndOfStream)
        {
            result.Add(sr.ReadLine());
        }

        return result;
    }
}
