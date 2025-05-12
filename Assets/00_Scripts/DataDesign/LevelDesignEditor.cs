using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelDesign))]
public class LevelDesignEditor : Editor
{
    LevelDesign design = null;
    
    public override void OnInspectorGUI()
    {
        design = (LevelDesign)target;

        EditorGUILayout.LabelField("Level Design Editor", EditorStyles.boldLabel);

        LevelData data = design.levelData;
        StageData s_data = design.stageData;
        
        DrawGraph(data,s_data);
        EditorGUILayout.Space();

        DrawDefaultInspector();
    }

    private void DrawGraph(LevelData data, StageData s_data)
    {
        EditorGUILayout.LabelField("캐릭터 레벨 그래프", EditorStyles.boldLabel);
        Rect rect = GUILayoutUtility.GetRect(200, 100);
        Handles.DrawSolidRectangleWithOutline(rect, Color.black, Color.black);

        Vector3[] curvePoint_ATK = GraphDesign(rect, data.C_ATK);
        Handles.color = Color.green;
        Handles.DrawAAPolyLine(3, curvePoint_ATK);

        Vector3[] curvePoint_HP = GraphDesign(rect, data.C_HP);
        Handles.color = Color.red;
        Handles.DrawAAPolyLine(3, curvePoint_HP);

        Vector3[] curvePoint_EXP = GraphDesign(rect, data.C_EXP);
        Handles.color = Color.blue;
        Handles.DrawAAPolyLine(3, curvePoint_EXP);

        Vector3[] curvePoint_MAXEXP = GraphDesign(rect, data.C_MAXEXP);
        Handles.color = Color.white;
        Handles.DrawAAPolyLine(3, curvePoint_MAXEXP);

        Vector3[] curvePoint_MONEY = GraphDesign(rect, data.C_MONEY);
        Handles.color = Color.yellow;
        Handles.DrawAAPolyLine(3, curvePoint_MONEY);

        EditorGUILayout.Space(10);
        GetColorGUI("ATK", StringMethod.ToCurrencyString(Utils.DesignData.levelData.ATK()), Color.green);
        GetColorGUI("HP", StringMethod.ToCurrencyString(Utils.DesignData.levelData.HP()), Color.red);
        GetColorGUI("EXP", StringMethod.ToCurrencyString(Utils.DesignData.levelData.EXP()), Color.blue);
        GetColorGUI("MAXEXP", StringMethod.ToCurrencyString(Utils.DesignData.levelData.MAXEXP()), Color.white);
        GetColorGUI("MONEY", StringMethod.ToCurrencyString(Utils.DesignData.levelData.MONEY()), Color.yellow);


        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("스테이지 레벨 그래프", EditorStyles.boldLabel);
        Rect s_rect = GUILayoutUtility.GetRect(200, 100);
        Handles.DrawSolidRectangleWithOutline(s_rect, Color.black, Color.black);

        Vector3[] s_curvePoint_ATK = GraphDesign(s_rect, s_data.M_ATK);
        Handles.color = Color.green;
        Handles.DrawAAPolyLine(3, s_curvePoint_ATK);

        Vector3[] s_curvePoint_HP = GraphDesign(s_rect, s_data.M_HP);
        Handles.color = Color.red;
        Handles.DrawAAPolyLine(3, s_curvePoint_HP);

        Vector3[] s_curvePoint_MONEY = GraphDesign(s_rect, s_data.M_MONEY);
        Handles.color = Color.yellow;
        Handles.DrawAAPolyLine(3, s_curvePoint_MONEY);

        EditorGUILayout.Space(10);
        GetColorGUI("ATK", StringMethod.ToCurrencyString(Utils.DesignData.stageData.ATK()), Color.green);
        GetColorGUI("HP", StringMethod.ToCurrencyString(Utils.DesignData.stageData.HP()), Color.red);
        GetColorGUI("MONEY", StringMethod.ToCurrencyString(Utils.DesignData.stageData.MONEY()), Color.yellow);

    }

    void GetColorGUI(string baseTemp, string dataTemp, Color color)
    {
        GUIStyle colorLabel = new GUIStyle(EditorStyles.label);
        colorLabel.normal.textColor = color;

        EditorGUILayout.LabelField(baseTemp + " : " +  dataTemp, colorLabel);
    }
    

    private Vector3[] GraphDesign(Rect rect, float data)
    {
        Vector3[] curvePoint = new Vector3[100];
        for (int i = 0; i < 100; i++)
        {
            float t = i / 99.0f;
            float curveValue = Mathf.Pow(t, data);
            curvePoint[i] = new Vector3(
                rect.x +  t* rect.width, //x
                rect.y +  rect.height - curveValue*rect.height, //y
                0 //z
                );
        }
        return curvePoint;
    }
}
