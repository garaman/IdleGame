using System.Collections.Generic;


public class CSV_Importer
{
    public static List<Dictionary<string, object>> SpawnDesign = new List<Dictionary<string, object>>(CSVReader.Read("Spawner"));
}
