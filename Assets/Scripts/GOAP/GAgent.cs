using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class GGoal
{
    public string goalName;
    public List<GWorldState> goalPreRequisites;

    [SerializeField] List<GWorldState> goalStateList;

    public GWorldStates goalStates { get; private set; }

    public GGoal()
    {
        goalStateList = new();
        goalPreRequisites = new List<GWorldState>();
    }

    public bool IsValid(GWorldStates currentState)
    {
        foreach (GWorldState state in goalPreRequisites)
        {
            if (!currentState.DoesSatisfyState(state.key, state.value))
            {
                return false;
            }
        }
        return true;
    }

    public void Init()
    {
        goalStates = new GWorldStates();
        goalStates.SetAllStates(goalStateList.ToArray());
    }
}

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody), typeof(GSensor))]
public class GAgent : MonoBehaviour
{
    [SerializeField] List<GAction> actions;

    [SerializeField] List<GGoal> goals;

    GPlanner planner;

    public GWorldStates agentBeliefs;

    Queue<GAction> actionQueue;

    GAction currentAction;

    public GSensor sensor { get; private set; }
    public Memory memory
    {
        get
        {
            return sensor.memory;
        }
    }

    int currentGoalIndex;
    public float timeBetweenPlanning = 0.1f;

    bool IsExecutingPlan;

    public string CurrentGoalName
    {
        get
        {
            if (currentGoalIndex == -1)
                return null;
            return goals[currentGoalIndex].goalName;
        }
    }

    public string[] ActionPlanListStrings
    {
        get
        {
            if(actionQueue == null)
            {
                return new string[] { "No Actions Planned" };
            }
            else
            {
                string[] result = new string[actionQueue.Count + 1];
                int i = 0;
                if (currentAction != null)
                {
                    result[i] = "Current Action: " + currentAction.GetName();
                    i++;
                }
                foreach (GAction action in actionQueue)
                {
                    result[i] = action.GetName();
                }
                return result;
            }
        }
    }

    public string[] CurrentBeliefs
    {
        get
        {   if(agentBeliefs == null)
            {
                return new string[] { "" };
            }
            string[] ret = new string[agentBeliefs.WorldStates.Count];
            int i = 0;
            foreach(KeyValuePair<EState, int> state in agentBeliefs.WorldStates)
            {
                ret[i] = $"{state.Key} : {state.Value}";
                i++;
            }
            return ret;
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        agentBeliefs = new GWorldStates();
        actions = new List<GAction>(GetComponentsInChildren<GAction>());
        foreach (GAction action in actions)
        {
            action.enabled = false;
            action.gAgentRef = this;
        }

        foreach (GGoal item in goals)
        {
            item.Init();
        }
        sensor = GetComponent<GSensor>();
        planner = new GPlanner();
        currentGoalIndex = -1;
    }

    void Start()
    {
        StartCoroutine(PlanCheck());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator PlanCheck()
    {
        while (true)
        {
            if (actionQueue == null)
            {
                for (int i = 0; i < goals.Count; i++)
                {
                    GGoal item = goals[i];

                    if (!item.IsValid(agentBeliefs)) continue;

                    actionQueue = planner.MakePlane(actions, item, agentBeliefs);
                    if (actionQueue != null)
                    {
                        currentGoalIndex = i;
                        break;
                    }

                }
            }

            if (actionQueue == null)
            {
                yield return new WaitForSeconds(timeBetweenPlanning);
            }
            else
            {
                StartCoroutine(ProcessActionQueue());

                yield return new WaitUntil(() => actionQueue == null);
            }
        }
    }


    private IEnumerator ProcessActionQueue()
    {
        while (actionQueue.Count > 0)
        {
            currentAction = actionQueue.Dequeue();
            currentAction.StartAction();
            yield return new WaitUntil(() => currentAction.enabled == false);
            if (currentAction.WasSuccess == false)
                break;
        }
        currentAction = null;
        currentGoalIndex = -1;
        actionQueue = null;
    }
}
