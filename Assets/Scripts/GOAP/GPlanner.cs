using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPlanNode
{
    public GPlanNode parent;
    public float cost;
    public GWorldStates state;
    public GAction action;

    public GPlanNode(GPlanNode parent, float cost, GWorldStates state, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = state;
        this.action = action;
    }
}


public class GPlanner
{
    public Queue<GAction> MakePlane(List<GAction> actions, GGoal goal, GWorldStates startingState)
    {
        
        List<GAction> useableActions = new List<GAction>(actions.Count);
        foreach (GAction action in actions)
        {
            if (action.IsValid())
            {
                useableActions.Add(action);
            }
        }

        List<GPlanNode> leaves = new List<GPlanNode>();
        GPlanNode start = new GPlanNode(null, 0, startingState, null);

        //BuildGraph_v2(start, leaves, useableActions, goal);
        //return null;
        bool succss = BuildGraph(start, leaves, useableActions, goal.goalStates);

        if (!succss)
        {
            Debug.Log("No Plan for " + goal.goalName);
            return null;
        }

        GPlanNode cheapest = null;
        foreach (GPlanNode leaf in leaves)
        {
            if (cheapest == null)
            {
                cheapest = leaf;
            }
            else if (leaf.cost < cheapest.cost)
            {
                cheapest = leaf;
            }
        }

        List<GAction> result = new List<GAction>();
        GPlanNode n = cheapest;
        while (n != null)
        {
            if (n.action != null)
            {
                result.Insert(0, n.action);
            }
            n = n.parent;
        }

        Queue<GAction> queue = new Queue<GAction>();
        foreach (var item in result)
        {
            queue.Enqueue(item);
        }

        return queue;
    }

    bool BuildGraph(GPlanNode parent, List<GPlanNode> leaves, List<GAction> usableActions, GWorldStates goal)
    {
        bool foundPath = false;

        foreach (GAction action in usableActions)
        {
            if (action.IsValidGiven(parent.state))
            {
                GWorldStates currentState = new GWorldStates(parent.state);
                foreach (KeyValuePair<EState, int> eff in action.afterEffects.WorldStates)
                {
                    if (!currentState.HasState(eff.Key))
                    {
                        currentState.AddState(eff.Key, eff.Value);
                    }
                    else
                    {
                        if(eff.Value == 0)
                        {
                            currentState.SetState(eff.Key, eff.Value);
                        }
                        else
                        {
                            currentState.ModifyState(eff.Key, eff.Value);
                        }
                    }
                }
                GPlanNode node = new GPlanNode(parent, parent.cost + action.GetCost(), currentState, action);

                if (GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<GAction> subset = ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if (found)
                    {
                        foundPath = true;
                    }
                }
            }
        }

        return foundPath;
    }

    //GPlanNode BuildGraph_v2(GPlanNode parent, List<GPlanNode> leaves, List<GAction> usableActions, GWorldStates goal)
    //{
    //    bool foundPath = false;
    //    GPlanNode returnNode = null;
    //    Queue<GPlanNode> actionsToTest = new();
    //    foreach(var action in usableActions)
    //    {
    //        if(action.afterEffects.DoesSatisfyWorldState(goal))
    //        {
    //            GPlanNode node = new GPlanNode(null, parent.cost + action.GetCost(), action.preConditions, action);
    //            actionsToTest.Enqueue(node);
    //            if (returnNode == null) returnNode = node;
    //        }
    //    }
    //    while(actionsToTest.Count > 0)
    //    {
    //        GPlanNode testingAction = actionsToTest.Dequeue();
    //        = BuildGraph_v2(parent, leaves, ActionSubset(usableActions, testingAction.action), testingAction.state);
    //    }
    //    return returnNode;
    //}

    private List<GAction> ActionSubset(List<GAction> usableActions, GAction removeMe)
    {
        List<GAction> result = new List<GAction>(usableActions.Count - 1);
        foreach (GAction gAction in usableActions)
        {
            if (!gAction.Equals(removeMe)) result.Add(gAction);
        }
        return result;
    }

    private bool GoalAchieved(GWorldStates goal, GWorldStates currentState)
    {
        //foreach (KeyValuePair<EState, int> g in goal.WorldStates)
        //{
        //    if (!currentState.HasState(g.Key) || currentState.GetStateValue(g.Key) < g.Value)
        //    { 
        //        return false; 
        //    }

        //}
        return currentState.DoesSatisfyWorldState(goal);
    }
}
