using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class ADS_Xml__Scriptable 
{
    [MenuItem("Tools/Create AndroidManifest.xml")]
    public static void CreateManifest()
    {
        string path = "Assets/Plugins/Android";
        string manifestPath = Path.Combine(path, "AndroidManifest.xml");

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        if (File.Exists(manifestPath))
        {
            Debug.LogWarning("AndroidManifest.xml already exists.");
            return;
        }

        string appId = "ca-app-pub-3482755551904464~4916670743"; // 여기에 본인 App ID 입력
        string manifest = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                            <manifest xmlns:android=""http://schemas.android.com/apk/res/android""
                                package=""com.SelfProject.IdleGame"">

                                <application>
                                    <meta-data
                                        android:name=""com.google.android.gms.ads.APPLICATION_ID""
                                        android:value=""{appId}""/>
                                </application>

                            </manifest>";

        File.WriteAllText(manifestPath, manifest);
        AssetDatabase.Refresh();
        Debug.Log("AndroidManifest.xml created at " + manifestPath);
    }
}
