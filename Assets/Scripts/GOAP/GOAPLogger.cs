using UnityEngine;
using System.Collections;
using UnityEditor;

public class GOAPLogger : MonoBehaviour
{
#if UNITY_EDITOR
    public static GOAPLogger instance;

    //uint qsize = 15;  // number of messages to keep
    string currentGoal;
    string[] actionPlan;
    string[] agentState;

    GAgent agent = null;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

    }

    public static void Log(string logString, LogType type)
    {
        //instance.Log_Internal(logString, type);
    }

    private void Update()
    {
        if (Selection.activeGameObject)
        {
            agent = Selection.activeGameObject.GetComponent<GAgent>();
            if (agent)
            {
                currentGoal = agent.CurrentGoalName;
                actionPlan = agent.ActionPlanListStrings;
                agentState = agent.CurrentBeliefs;
            }
        }
        else if (agent != null)
        {
            agent = null;
        }
    }

    void OnGUI()
    {
        if (agent != null)
        {
            GUIStyle gUIStyle = new GUIStyle();
            gUIStyle.normal.textColor = Color.blue;
            GUILayout.BeginArea(new Rect(Screen.width - 400, 0, 400, Screen.height));
            GUILayout.Label("\n States: \n" + string.Join("\n", agentState), gUIStyle);
            GUILayout.EndArea();

            gUIStyle.normal.textColor = Color.red;
            GUILayout.BeginArea(new Rect(0, 0, 400, Screen.height));
            GUILayout.Label("\n Goal:" + currentGoal, gUIStyle);
            gUIStyle.normal.textColor = Color.black;
            GUILayout.Label("\n Plan: \n" + string.Join("\n", actionPlan), gUIStyle);
            GUILayout.EndArea();
        }
    }
#endif
}