using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(WorldGridStatus))]
public class CustomInspector : Editor
{

    WorldGridStatus targetScript;

    void OnEnable()
    {
        targetScript = target as WorldGridStatus;
    }

    public override void OnInspectorGUI()
    {

        //WorldGridStatus.X = EditorGUILayout.IntField("Width: ",WorldGridStatus.X);
        //WorldGridStatus.Y = EditorGUILayout.IntField("Height: ",WorldGridStatus.Y);

        targetScript.gridStatus = EditorGUILayout.ObjectField("GridStatus_SO SO: ",targetScript.gridStatus,typeof(GridStatus_SO),true) as GridStatus_SO;
        targetScript.gridData = EditorGUILayout.ObjectField("GridData SO: ", targetScript.gridData, typeof(GridData_SO), true) as GridData_SO;

        if (targetScript.gridData)
        {
            WorldGridStatus.X = targetScript.gridData.width;
            WorldGridStatus.Y = targetScript.gridData.length;

            EditorGUILayout.BeginHorizontal();
            for (int y = 0; y < WorldGridStatus.Y; y++)
            {
                EditorGUILayout.BeginVertical();
                for (int x = 0; x < WorldGridStatus.X; x++)
                {
                    bool val = EditorGUILayout.Toggle(targetScript.columns[x].rows[y]);

                    if (targetScript.columns[x].rows[y] != val)
                    {
                        targetScript.OnColumnChanged();
                    }

                    targetScript.columns[x].rows[y] = val;
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        if(targetScript.gridStatus && targetScript.gridStatus.gridLayout!=null && targetScript.gridStatus.gridLayout.Length== WorldGridStatus.X 
            && targetScript.gridStatus.gridLayout[0].rows.Length == WorldGridStatus.Y)
        {
            for (int y = 0; y < WorldGridStatus.Y; y++)
            {
                for (int x = 0; x < WorldGridStatus.X; x++)
                {
                    targetScript.columns[x].rows[y]= targetScript.gridStatus.gridLayout[x].rows[y];
                }
            }
        }
    }
}