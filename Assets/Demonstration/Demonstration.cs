using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Environment = Academy.Environment;
using Random = UnityEngine.Random;


public class Demonstration
{
    private GameObject UEnv;
    private UGrid ugrid;
    private Environment environment;
    private readonly string strategy;
    private float epsilon;
    
    
    public void Setup(float ep, int NPE)
    {
        epsilon = ep;
        
        
        environment = new Environment();
        environment.EnvSetupBase(NPE);
        environment.EnvSet(epsilon, strategy);
        
        
        UEnv = new GameObject("Environment");
        ugrid = UEnv.AddComponent<UGrid>();
        ugrid.createUGrid(environment.getNomGrid());
    }


    public void Finish()
    {
        ugrid.ClearNomList();
        ugrid.ClearGrid();
    }
}
