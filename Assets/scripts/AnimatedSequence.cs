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

[System.Serializable]
public class ObjectStateData
{

    [HideInInspector]
    public GameObject go;
    //[HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Quaternion rotation;
    [HideInInspector]
    public Vector3 scale;


    [HideInInspector]
    public Color color;



    [SerializeField, HideInInspector]
    Vector2 anchoredPosition;

    [SerializeField, HideInInspector]
    Vector3 rectPosition;
    public Vector2 AnchoredPosition { get { return anchoredPosition; } }

    public Vector3 RectPosition { get { return rectPosition; } }

    [SerializeField, HideInInspector]
    Vector3 localEulerAngles;
    public Vector3 LocalEulerAngles { get { return localEulerAngles; } }

    /// getters
    public SpriteRenderer sprite { get { return go.GetComponent<SpriteRenderer>(); } }
    public Image image { get { return go.GetComponent<Image>(); } }
    public Text text { get { return go.GetComponent<Text>(); } }
    public RectTransform rectTransform { get { return go.GetComponent<RectTransform>(); } }

    //copy GO state

    public ObjectStateData(GameObject gameObject)
    {
        go = gameObject;
        SaveState();
    }



    public void SaveState()
    {

        position = go.transform.position;
        rotation = go.transform.rotation;
        scale = go.transform.localScale;

        localEulerAngles = go.transform.localEulerAngles;

        if (rectTransform)
        {
            anchoredPosition = rectTransform.anchoredPosition;
            rectPosition = rectTransform.position;
        }

        if (text)
        {

            color = text.color;
        }

        if (image)
        {
            color = image.color;
        }
        if (sprite)
        {
            color = sprite.color;
        }
        //if it s a mesh : save material color
        if (go.GetComponent<MeshRenderer>())
        {
            color = go.GetComponent<MeshRenderer>().material.color;

        }

    }

    public void RestoreState()
    {

        if (!go)
        {
            //no go attached to data
            Debug.LogWarning("no go attached to object data");
            return;
        }

        go.transform.position = position;
        go.transform.rotation = rotation;
        go.transform.localScale = scale;
        if (rectTransform)
        {

            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.position = rectPosition;
        }


        if (text)
        {
            text.color = color;
        }

        if (image)
        {
            image.color = color;
        }
        if (sprite)
        {
            sprite.color = color;
        }


        if (go.GetComponent<MeshRenderer>())
        {
            go.GetComponent<MeshRenderer>().material.color = color;

        }

    }

}


[DisallowMultipleComponent]
public class AnimatedSequence : MonoBehaviour
{

    static Rect BUTTON_SIZE = new Rect(0, 0, 250, 50);  

    static int nbSequences = 0;

    public SequenceScript sequence;
    public bool fireOnStart = true;

    public float startingTime = 0;

    bool playing = false;
    public SequenceReplayType replayType = SequenceReplayType.Stop;

    public int nbCoroutines { get { return sequence.animations.Count; } }
    int nbCoroutinesCompleted = 0;
    int index = 0;

    public List<GameObject> items;

    //List<ObjectStateData> itemsStateData = new List<ObjectStateData>();


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
    void Play()
    {
        StopAllCoroutines();
        playing = true;
        StartCoroutine(PlayAllAnimationsCoroutine());
    }

    IEnumerator PlayAllAnimationsCoroutine()
    {

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

    public void RestoreItemStates()
    {
        foreach (AnimationInstance animInstance in sequence.animations)
        {
            animInstance.RestoreState();

        }
    }

    //public void SaveItemStates()
    //{
    //    foreach (AnimationInstance animInstance in sequence.animations)
    //    {
    //        animInstance.SaveState();

    //    }
    //}


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
