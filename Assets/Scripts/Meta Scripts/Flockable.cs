using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flockable : Kinematic
{
    public GameObject flockCoMTarget;
    BlendedSteering _blendSteering;
    GameObject[] kBirds;


    void Start()
    {
        ///Start Separation
        //Separate from others and not self
        Separation _separate = new Separation();
        _separate.character = this;
        GameObject[] _goBirds = GameObject.FindGameObjectsWithTag("birds");
        kBirds = new GameObject[_goBirds.Length - 1];


        int _kBirdSize = 0;
        for (int _bird = 0; _bird<_goBirds.Length-1; _bird++)
        {
            if(_goBirds[_bird] == this)
            {
                //Ignore self for separation
                continue;
            }
            ///Else separate from them
            kBirds[_kBirdSize++] = _goBirds[_bird];
        }
        _separate.neighborhood = kBirds;

        ///End Separation

        ///Start Cohere
        Arrive _cohere = new Arrive();
        _cohere.character = this;
        _cohere.target = flockCoMTarget;
        ///End Cohere

        ///Start LWYG
        LookWhereGoing _lWYG = new LookWhereGoing();
        _lWYG.character = this;
        //End LWYG



        ///Start Blending
        _blendSteering = new BlendedSteering();
        _blendSteering.behaviours = new BehaviourAndWeight[3];

        //Separation
        _blendSteering.behaviours[0] = new BehaviourAndWeight();
        _blendSteering.behaviours[0].behaviour = _separate;
        _blendSteering.behaviours[0].weight = 1f; //Weights are still not great, but functional

        //Cohere
        _blendSteering.behaviours[1] = new BehaviourAndWeight();
        _blendSteering.behaviours[1].behaviour = _cohere;
        _blendSteering.behaviours[1].weight = .4f;

        //Look Where Going
        _blendSteering.behaviours[2] = new BehaviourAndWeight();
        _blendSteering.behaviours[2].behaviour = _lWYG;
        _blendSteering.behaviours[2].weight = 1f; 

        ///End Blending

    }

    protected override void Update()
    {
        controlledSteeringUpdate = new SteeringOutput();
        controlledSteeringUpdate = _blendSteering.getSteering();
        base.Update();
    }
}
