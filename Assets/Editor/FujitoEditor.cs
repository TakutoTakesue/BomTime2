using UnityEngine;
using UnityEditor;
using System.IO;

public class EditorWindowSample : EditorWindow
{
    private const string ASSET_PATH = "Assets/_WorkSpace/Fujito/ScriptableObject/k_PlayerData.asset";

    private k_PlayerData playerData;

    [MenuItem("Editor/OriginalEditor")]
    private static void Create()
    {
        // 生成
        GetWindow<EditorWindowSample>("ScriptableObject");
    }

    private void OnGUI()
    {
        if(!playerData)
        {
            Import();
        }

        Color defaultColor = GUI.backgroundColor;
        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            //GUI.backgroundColor = Color.gray;
            using(new GUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Label("Player設定");
            }
            GUI.backgroundColor = defaultColor;

            Debug.Log(playerData.SampleIntValue);

            playerData.SampleIntValue = EditorGUILayout.IntField("移動速度", playerData.SampleIntValue);
        }
        using(new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            using(new GUILayout.HorizontalScope(GUI.skin.box))
            {
                GUI.backgroundColor = Color.green;
                // 読み込みボタン
                if (GUILayout.Button("読込"))
                {
                    Import();
                }

                GUI.backgroundColor = Color.magenta;
                // 書き込みボタン
                if (GUILayout.Button("入力"))
                {
                    Export();
                }

                GUI.backgroundColor = defaultColor;
            }
        }
    }

    private void Import()
    {
        if (!playerData)
        {
            playerData = CreateInstance<k_PlayerData>();
        }

        k_PlayerData sample = AssetDatabase.LoadAssetAtPath<k_PlayerData>(ASSET_PATH);

        if(!sample)
        {
            return;
        }

        EditorUtility.CopySerialized(sample, playerData);
    }
    private void Export()
    {
        k_PlayerData sample = AssetDatabase.LoadAssetAtPath<k_PlayerData>(ASSET_PATH);
        if (!sample)
        {
            sample = CreateInstance<k_PlayerData>();
        }

        // 新規の場合は作成
        if (!AssetDatabase.Contains(sample))
        {
            string directory = Path.GetDirectoryName(ASSET_PATH);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // アセット作成
            AssetDatabase.CreateAsset(playerData, ASSET_PATH);
        }

        //sample.Copy(_sample);
        EditorUtility.CopySerialized(playerData, sample);

        //直接編集できないようにする
        sample.hideFlags = HideFlags.NotEditable;

        // 更新通知
        EditorUtility.SetDirty(playerData);

        // 保存
        AssetDatabase.SaveAssets();

        // エディタを最新の状態にする
        AssetDatabase.Refresh();
    }
}