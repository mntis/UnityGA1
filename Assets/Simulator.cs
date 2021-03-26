using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interface;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

public class Simulator : MonoBehaviour
{
    private Academy.Academy academy;
    private GraphManager GM;
    private int Population;
    private int EPA;
    private float Mutation_Rate;
    private float Density;
    private int StopAtGen;
    private int currentGen;
    private bool enable;
    public GameObject ToggleLabel;
    public Text ToggleText;
    public Text GenNo;
    public Text AgentNo;
    public Text BestFit;
    
    
    //Initialization
    public void InitSim(int pop, int epa, float mr, float d)
    {
        Population = pop;
        EPA = epa;
        Mutation_Rate = mr;
        Density = d;
        StopAtGen = 1000;
        currentGen = 0;
        enable = false;
        GenNo.text = "GEN:\n";
        AgentNo.text = "AGENT:\n";
        InitGM();
        InitAcademy();
        ToggleText = ToggleLabel.GetComponent<Text>();
        StartCoroutine(RunSimulation());
    }

    private void InitGM()
    {
        GameObject GMobj = GameObject.Find("Graph_Manager");
        Debug.Log(GMobj.name);
        GM = GMobj.GetComponent<GraphManager>();
        GM.InitGM();
    }
    
    private void InitAcademy()
    {
        academy = new Academy.Academy(Population, EPA, Mutation_Rate, Density, this);
        academy.Setup();
    }
    
    //==================================================================================================================
    
    //Control Interface
    public void SetStop(string input)
    {
        int test = int.Parse(input);
        if (test < 1)
        {
            test = 0;
        }

        StopAtGen = test;
    }

    public void ToggleSim(bool value)
    {
        ToggleText.text = value ? "STOP" : "START";
        Debug.Log(ToggleText.text);
        enable = value;
    }
    
    //==================================================================================================================
    
    //Simulation
    private IEnumerator RunSimulation()
    {
        for (int i = 0; i < 1000; i++) // Stops at 1000 generations for practical purposes
        {
            yield return StartCoroutine(RunGeneration());
        }
    }
    
    private IEnumerator RunGeneration()
    {
        while (!enable) yield return null;
        
        currentGen++;
        
        GenNo.text = "GEN:\n" + currentGen.ToString();

        Debug.Log("Starting new gen" + currentGen);
        
        for (int i = 0; i < Population; i++)
        {
            yield return StartCoroutine(RunAgent(i));
        }
        
        academy.ExtractData();
        
        UpdateGraph();

        BestFit.text = "Best fitness:\n" + academy.GetMaxFit();
        
        academy.NextGen();
        
        if (currentGen == StopAtGen) ToggleSim(false);
    }
    
    private IEnumerator RunAgent(int agentIndex)
    {
        AgentNo.text = "AGENT:\n" + (agentIndex+1).ToString();
        Task trial = Task.Factory.StartNew(() => academy.AgentTrial(agentIndex));
        trial.Wait();

        yield return null;
    }
    
    private void UpdateGraph()
    {
        GM.InsertNewValues(
            Mathf.Max(0, academy.GetMaxFit()), 
            Mathf.Max(0, academy.GetMinFit()), 
            Mathf.Max(0, academy.GetMeanFit()), 
            Mathf.Max(0, academy.GetLowSD()), 
            Mathf.Max(0, academy.GetHighSD()));
    }
    
    //==================================================================================================================

    //HourGlass
    private void HG_UpdateGen()
    {
        GenNo.text = "GEN:\n";
    }
    
    private void HG_UpdateAg()
    {
        AgentNo.text = "AGENT:\n";
    }
}

