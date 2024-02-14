using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]

public struct GWorldState
{
    public EState key;
    public int value;
}


public class GWorldStates
{
    public Dictionary<EState, int> WorldStates { get; private set; }

    public GWorldStates()
    {
        WorldStates = new Dictionary<EState, int>();
    }

    public GWorldStates(GWorldStates copy)
    {
        WorldStates = new Dictionary<EState, int>(copy.WorldStates);
    }

    public void SetAllStates(GWorldState[] states)
    {
        foreach (GWorldState state in states)
        {
            WorldStates.Add(state.key, state.value);
        }
    }

    public bool HasState(EState key)
    {
        return WorldStates.ContainsKey(key);
    }

    public int GetStateValue(EState key)
    {
        return WorldStates[key];
    }

    public void AddState(EState key, int value)
    {
        WorldStates.Add(key, value);
    }

    public void ModifyState(EState key, int delta)
    {
        if (WorldStates.ContainsKey(key))
        {
            WorldStates[key] += delta;
        }
        else
        {
            WorldStates.Add(key, delta);
        }
    }

    public void SetState(EState key, int value)
    {
        //if (WorldStates.ContainsKey(key))
        //{
            WorldStates[key] = value;
        //}
        //else
        //{
        //    AddState(key, value);
        //}
    }

    public void RemoveState(EState key)
    {
        if (WorldStates.ContainsKey(key))
        {
            WorldStates.Remove(key);
        }
    }

    public bool IsSatisfiedByState(EState state, int value)
    {
        bool hasState = this.HasState(state);
        //return !((value > 0 && !hasState) || (value == 0 && hasState && this.GetStateValue(state) > 0));
        return ((hasState && value >= this.GetStateValue(state)) || (!hasState && value == 0));
    }

    public bool DoesSatisfyWorldState(GWorldStates goalStates)
    {
        foreach (KeyValuePair<EState, int> state in goalStates.WorldStates)
        {
            //bool hasState = conditions.HasState(state.Key);
            if (!DoesSatisfyState(state.Key, state.Value))
            {
                return false;
            }
        }
        return true;
    }

    public bool DoesSatisfyState(EState state, int value)
    {
        bool hasState = this.HasState(state);
        if (value == 0)
        {
            return !hasState || this.GetStateValue(state) <= 0;
        }
        else
        {
            return hasState && this.GetStateValue(state) >= value;
        }
    }

}
