using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapEditor : EditorWindow
{
    int chang = 0;
    int kuan = 0;
    public static GameObject game;
    [MenuItem("地图编辑器/绘制地图")]
    public static void OpenMap()
    {
       
        if (EditorApplication.isPlaying)
        {
            game = GameObject.Find("GameObject");
            MapEditor win = GetWindow<MapEditor>("地图编辑");
            win.Show();
        }
      
    }
    private void OnGUI()
    {
        GUILayout.Label("请输入行数");
        chang = EditorGUILayout.IntField(chang);
        GUILayout.Label("请输入列数");
        kuan = EditorGUILayout.IntField(kuan);
        if (GUILayout.Button("开始"))
        {
            MapEditor win = GetWindow<MapEditor>("地图编辑");
          Map map=  game.AddComponent<Map>();
            map.Init(chang, kuan);
            win.Close();
        }
    }
}
