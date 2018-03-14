using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SequenceReplayType : int
{

    Stop,
    Reset,
    Loop,
    PingPong,
    Accumulate,

}



[DisallowMultipleComponent]
public class AnimatedSequence : MonoBehaviour
{

    static Rect BUTTON_SIZE = new Rect(0, 0, 250, 50);  

    static int nbSequences = 0;

    public SequenceScript sequence;


    public bool fireOnStart = true;

    //[HideInInspector]
    //public bool useEmbeddedStates = false;

    public float startingTime = 0;

    bool playing = false;
    public SequenceReplayType replayType = SequenceReplayType.Stop;

    public int nbCoroutines { get { return sequence.animations.Count; } }
    int nbCoroutinesCompleted = 0;
    int index = 0;

    public List<GameObject> items;

    List<ObjectStateData> itemsStateData = new List<ObjectStateData>();




    private void Awake()
    {
        index = nbSequences++;

        Debug.Log("Screen Width : " + Screen.width);

        //assign objectdata to animInstance
        foreach (AnimationInstance animInstance in sequence.animations)
        {

            if (animInstance == null)
            {
                Debug.LogError("An Anim instance  is null: remove null animtion from sequence");
            }

            if (animInstance.itemIndex >= items.Count || items[animInstance.itemIndex] == null)
            {

                Debug.LogError("Anim requires item " + animInstance.itemIndex + " to exist: drag a gameObject in the animatedSequence.items");
            }


            animInstance.objectData = new ObjectStateData(items[animInstance.itemIndex]);

        }

      


        Reset();
    }


    private void OnEnable()
    {

        if (fireOnStart)
            Play();
    }

    private void OnDisable()
    {

        Reset();

    }

    private void Reset()
    {
        StopAllCoroutines();
        nbCoroutinesCompleted = 0;
        playing = false;
        ResetItems();

    }


    public void ApplyEmbeddedStates()
    {

        //copy states from sequence script
        itemsStateData = new List<ObjectStateData>(sequence.embeddedStates);
        //restore states from to all items from list
        //set new embedded values to gameobjects
        RestoreItemStates();
    }

    void Play()
    {
        StopAllCoroutines();

        //if(useEmbeddedStates){


        //    ApplyEmbeddedStates();

        //}else{
        //    //fill states list with current items states
        //    SaveItemStates(); 
        //}

        SaveItemStates(); 
        //assign state to animationInstance

        playing = true;
        StartCoroutine(PlayAllAnimationsCoroutine());
    }

    IEnumerator PlayAllAnimationsCoroutine()
    {

        //global delay
        yield return new WaitForSeconds(startingTime);


        foreach (AnimationInstance animInstance in sequence.animations)
        {

            //always check in case it was removed at runtime
            if (animInstance.animationData == null)
            {
                Debug.LogError("Anim instance #" + sequence.animations.IndexOf(animInstance) + "has no animation data: drag one from assets");
            }
            else
            {
                //set the data again : cause item could have changed at runtime: BUG erase old data
                animInstance.objectData = itemsStateData[animInstance.itemIndex];

                //animInstance.objectData = new ObjectStateData(items[animInstance.itemIndex]);
             
                StartCoroutine(StartAnimCoroutine(animInstance));
            }
        }
    }

    IEnumerator StartAnimCoroutine(AnimationInstance animInstance)
    {

        if (animInstance.startTime > 0)
        {
            yield return new WaitForSeconds(animInstance.startTime);
        }
        // animInstance.objectData = new ObjectStateData(items[animInstance.itemIndex]);
        print("Start Animation instance " + animInstance.animationData.name);
        yield return StartCoroutine(animInstance.Play());

        nbCoroutinesCompleted++;
        if (nbCoroutinesCompleted == nbCoroutines)
        {
            print("Sequence completed");
            //restore items' state
            if (replayType == SequenceReplayType.Reset)
            {
                Reset();
                RestoreItemStates();
            }

            //restore items' state and play again
            else if (replayType == SequenceReplayType.Loop)
            {
                Reset();
                RestoreItemStates();
                Play();
            }
            //Don't restore items' state and play again
            else if (replayType == SequenceReplayType.Accumulate)
            {
                Play();
            }
            //Don't restore items' state
            else if (replayType == SequenceReplayType.Stop)
            {
                Reset();
            }

        }

    }

    public void ResetItems()
    {
        foreach (AnimationInstance animInstance in sequence.animations)
        {
            animInstance.Reset();

        }
    }

    //restore states from sequence to animInstance
    public void RestoreItemStates()
    {
        foreach (ObjectStateData data in itemsStateData)
        {
            
            data.RestoreState();

        }
    }

    //save current item states 
    public void SaveItemStates()
    {

        while (itemsStateData.Count <items.Count){

            itemsStateData.Add(null);
        }

        for (int i = 0; i < items.Count; i++)
        {
            if (itemsStateData[i]==null){
                itemsStateData[i] = new ObjectStateData(items[i]);
            }
                itemsStateData[i].SetObject(items[i]);
        }
    }


    public void Pause()
    {
        foreach (AnimationInstance animInstance in sequence.animations)
        {
            animInstance.Pause();
        }

    }

    public void Resume()
    {
        foreach (AnimationInstance animscript in sequence.animations)
        {
            animscript.Resume();
        }
    }


    //public void SeekSequenceToPercent(float percent)
    //{
    //    float time = sequence.TotalDuration * percent;
    //    SeekSequenceToTime(time);
    //}
    ////instant play sequence until a percent of itself :  from 0 to 1
    //public void SeekSequenceToTime(float time)
    //{
    //    StopAllCoroutines();
    //    foreach (AnimationInstance animscript in sequence.animations)
    //    {
    //        //animscript.SeekTo(time);
    //    }
    //}

    private void OnGUI()
    {

        if (playing)
        {
            if (GUI.Button(new Rect(10, 10 + index * (2*BUTTON_SIZE.height+10), BUTTON_SIZE.width, BUTTON_SIZE.height), "Pause #" + index + " " + name))
            {
                playing = false;
                Pause();
            }
        }
        else if (GUI.Button(new Rect(10, 10 + index * (2*BUTTON_SIZE.height + 10), BUTTON_SIZE.width, BUTTON_SIZE.height), "Resume #" + index + " " + name))
        {
            playing = true;
            Resume();
        }

        if (GUI.Button(new Rect(10, BUTTON_SIZE.height + 10 + index * (2*BUTTON_SIZE.height + 10), BUTTON_SIZE.width, BUTTON_SIZE.height), "Restart #" + index + " " + name))
        {
            Reset();
			RestoreItemStates();
            Play();

        }
    }

}
