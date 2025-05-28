using System.Collections.Generic;


public class CSV_Importer
{
    public static List<Dictionary<string, object>> SpawnDesign = new List<Dictionary<string, object>>(CSVReader.Read("Spawner"));
    public static List<Dictionary<string, object>> SummonDesign = new List<Dictionary<string, object>>(CSVReader.Read("Summon"));
    
    public static int[] summonLevel = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90 };
}
