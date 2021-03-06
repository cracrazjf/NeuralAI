using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;

public class Body {

    protected Entity thisEntity;
    protected float height;
    protected float heightScale;
    public Rigidbody rigidbody;
    protected GameObject gameObject;

    protected Vector<float> states;
    protected List<string> stateLabelList;
    protected Dictionary<string, int> stateIndexDict;
    protected Dictionary<string, bool> stateDict;
    
    public Vector<float> GetStates() { return states; }
    public bool GetState(string place) { return stateDict[place]; }
    public List<string> GetStateLabels() { return stateLabelList; }
    public Dictionary<string, int> GetStateIndices() { return stateIndexDict; }
    public Dictionary<string, bool> GetStateDict() { return stateDict; }
    public GameObject GetGameObject() { return gameObject; }
    public void SetGameObject(GameObject toSet) { this.gameObject = toSet; }

    public Transform globalPos;

    public Body(Entity passed, Vector3 position) {
        thisEntity = passed;

        InitHeight();
        InitGameObject(position);
        passed.SetGameObject(this.gameObject);
    }

    public virtual void InitGameObject(Vector3 pos) {
        string filePath = "Prefabs/" + thisEntity.GetSpecies() + "Prefab";
        GameObject loadedPrefab = Resources.Load(filePath, typeof(GameObject)) as GameObject;
        this.gameObject = (GameObject.Instantiate(loadedPrefab, pos, World.CreateRandomRotation()) as GameObject);
        this.gameObject.name = thisEntity.GetName();

        rigidbody = GetGameObject().GetComponent<Rigidbody>();
        globalPos = this.gameObject.transform;
        this.VerticalBump(thisEntity.GetPhenotype().GetTraitDict()["displacement"]);
        this.gameObject.SetActive(true);
    }
    
    public void InitHeight() {
        heightScale = thisEntity.GetPhenotype().GetTraitDict()["size"]; 
        height = thisEntity.GetPhenotype().GetTraitDict()["height"] * heightScale; 
    }

    public float GetHeight() { return height; }

    public Vector3 GetXZPosition() {
        return new Vector3(globalPos.position.x, 0f, globalPos.position.z);
    }

    public void VerticalBump(float height) {
        globalPos.position += new Vector3(0, height, 0);
    }

    public void TranslateBodyTo(Vector3 goalPos) {
        Debug.Log("Tried to translate body to " + goalPos);
        Vector3 currentPos = globalPos.position;
        currentPos = Vector3.MoveTowards(currentPos, goalPos, 1.5f * Time.deltaTime);
    }
}