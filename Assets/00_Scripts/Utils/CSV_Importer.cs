using System.Collections.Generic;


public class CSV_Importer
{
    public static List<Dictionary<string, object>> Exp = new List<Dictionary<string, object>>(CSVReader.Read("EXP"));
}
