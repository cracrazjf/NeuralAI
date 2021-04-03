using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;

public abstract class MotorSystem 
{
    protected Animal thisAnimal;
    protected AnimalBody thisBody;
    protected List<Action> actionList;

    protected Vector<float> states;
    protected List<string> stateLabelList;
    protected Dictionary<string, int> stateIndexDict;
    protected Dictionary<string, bool> stateDict;
    
    public Vector<float> GetStates() { return states; }
    public List<string> GetStateLabels() { return stateLabelList; }
    public Dictionary<string, int> GetStateIndices() { return stateIndexDict; }
    public Dictionary<string, bool> GetStateDict() { return stateDict; }
    
    protected Vector<float> args;
    protected List<string> argsLabelList;
    protected Dictionary<string, int> argsIndexDict;
    protected Dictionary<string, float> argsDict;

    public Vector<float> GetArgs() { return args; }
    public List<string> GetArgLabels() { return argsLabelList; }
    public Dictionary<string, int> GetArgIndices() { return argsIndexDict; }
    public Dictionary<string, float> GetArgDict() { return argsDict; }


    public MotorSystem(Animal passed) {
        thisAnimal = passed;
        this.thisBody = thisAnimal.GetBody();

        stateLabelList = new List<string> {
            "sitting down",// 0
            "sitting up",  // 1
            "laying down", // 2
            "standing up", // 3
            "rotating",    // 4
            "taking steps",// 5
            "picking up",  // 6
            "setting down",// 7 
            "consuming",   // 8
            "waking up",   // 9
            "sleeping",    // 10
            "resting",     // 11
            "looking"      // 12
        };
        this.InitStates(stateLabelList);

        argsLabelList = new List<string> {
            "step proportion",         // change to step size proportion                  
            "rotation proportion",               
            "held position",           // what is this again

            // in phenotype, max reach needs to be defined
            // max reach might be different for different body parts
            //  - mouth .1 m
            // - arms 1 m

            // should be proportions of 
            "target x",         // -1 and +1
            "target y",
            "target z"
        };

        this.InitActionArguments(argsLabelList);
        this.InitActionDict();
    }

    public void SetState(string label, bool val) {
        stateDict[label] = val;
        int currentIndex = stateIndexDict[label];
        //states[currentIndex, 0] = val;
    }

    public void SetArgs(string label, float val) {
        argsDict[label] = val;
        int currentIndex = argsIndexDict[label];
        //args[currentIndex] = val;
    }

    public void TakeAction(Matrix<float> actionStates) {
        for (int i = 0; i < actionStates.RowCount; i++)
        {
            if (((float)actionStates[i, 0]) > 0.0f)
            {
                //Debug.Log("Doing action at " + i + " number: " + ((float)actionStates[i, 0]));
            }
            // switched from i == 1... my bad
            //if (actionStates[i , 0] >= 0.5f) {
            //    Debug.Log("Doing action at " + i + " number: " + actionStates[1, 0]);
            //    //actionList[i].DynamicInvoke();
            //} 

        }
        
        
        
    }

    void InitStates(List<string> passedList) {
        states = Vector<float>.Build.Dense(stateLabelList.Count);
        stateLabelList = passedList;
        stateIndexDict = new Dictionary<string, int>();
        stateDict = new Dictionary<string, bool>();

        if (passedList != null){
            for (int i = 0; i < passedList.Count; i++) {
                //states[i] = 0;
                stateIndexDict[passedList[i]] = i;
                stateDict[passedList[i]] = false;
            }
        } else { Debug.Log("No actions passed to this animal"); }
    }

    void InitActionArguments(List<string> passedArgsLabels) {
        args = Vector<float>.Build.Dense(argsLabelList.Count);
        argsLabelList = passedArgsLabels;
        argsIndexDict = new Dictionary<string, int>();
        argsDict = new Dictionary<string, float>();

        if (passedArgsLabels != null){
            for (int i = 0; i < passedArgsLabels.Count; i++) {
                //args[i] = 0.0f;
                argsIndexDict[passedArgsLabels[i]] = i;
                argsDict[passedArgsLabels[i]] = 0.0f;
            }
        } else { Debug.Log("No args defined for this animal"); }
    }
    void InitActionDict() {
        actionList = new List<Action>();

        actionList.Add(SitDown);
        actionList.Add(SitUp);
        actionList.Add(LayDown);
        actionList.Add(StandUp);
        actionList.Add(Rotate);
        actionList.Add(TakeSteps);
        actionList.Add(PickUp);
        actionList.Add(SetDown);
        actionList.Add(Consume);
        actionList.Add(WakeUp);
        actionList.Add(Sleep);
        actionList.Add(Rest);
        actionList.Add(LookAt);
    }

    public abstract void SitDown();
    public abstract void SitUp();
    public abstract void LayDown();
    public abstract void StandUp();
    public abstract void Rotate();
    public abstract void TakeSteps();
    public abstract void PickUp();
    public abstract void SetDown();
    public abstract void Consume ();
    public abstract void WakeUp();
    public abstract void Sleep();
    public abstract void Rest();
    public abstract void LookAt();
}