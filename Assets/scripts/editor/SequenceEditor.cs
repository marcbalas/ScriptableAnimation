using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;


[CustomEditor(typeof(AnimatedSequence))]
public class AnimatedSequenceEditor : Editor
{
    AnimatedSequence sequence;
   
    private void OnEnable()
    {
        sequence = (AnimatedSequence)target;
       
    }


    public override void OnInspectorGUI()
    {
        //if(sequence.sequence){
            
        //    while (sequence.sequence.itemNames.Count > sequence.animations.Count)
        //    {
                
        //    }
        //}
        base.OnInspectorGUI();

        //EditorGUI.BeginDisabledGroup(sequence.sequence.embeddedStates.Count < sequence.items.Count );
        //sequence.useEmbeddedStates  = EditorGUILayout.Toggle("useEmbeddedStates ", sequence.useEmbeddedStates);
        //EditorGUI.EndDisabledGroup();


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUI.BeginDisabledGroup(sequence.sequence.embeddedStates.Count < sequence.items.Count);
        GUILayout.Label("Will position, scale, rotate objects according to values embedded in sequence");
        if (GUILayout.Button("Apply Embedded states"))
        {

            sequence.ApplyEmbeddedStates();

        }
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();
        GUILayout.Label("Save current states inside sequence. If you want to reuse the sequence, no need to set initial values");
        if(sequence.sequence.embeddedStates.Count < sequence.items.Count)
        {
            EditorGUILayout.HelpBox("Sequence has no embedded datas",MessageType.Info);
        }


        if(GUILayout.Button("Embed current states")){

            while (sequence.sequence.embeddedStates.Count< sequence.items.Count )
                  {
                sequence.sequence.embeddedStates.Add(null);

                   }

            for (int i = 0; i < sequence.items.Count; i++)
            {
                GameObject item = sequence.items[i];

                sequence.sequence.embeddedStates[i] = new ObjectStateData(item);
            }
        }
    }
      


}

[CustomEditor(typeof(SequenceScript))]
public class SequenceScriptEditor : Editor {


    SequenceScript sequence;
    List<bool> showData = new List<bool>();
    bool showSequence = true;

    //bool orderedByTime = true;


    private void OnEnable()
    {
        sequence = (SequenceScript)target;
       
    }


    public override void OnInspectorGUI()
    {
        while (showData.Count < sequence.animations.Count){
            showData.Add(true);

        }
       // SequenceScript sequence = (SequenceScript)target;
        base.OnInspectorGUI();

      


        //EditorGUILayout.Space();
        //EditorGUI.BeginChangeCheck();

        //showSequence = EditorGUILayout.Foldout(showSequence, "Animations");


        //if (showSequence)
        //{
        //    EditorGUI.indentLevel++;
        //    foreach (AnimationInstance ai in sequence.animations)
        //    {

        //        DrawAnimInstance(ai);


        //    }
        //    EditorGUI.indentLevel--;
        //}

       

      
        //if (EditorGUI.EndChangeCheck())
        //{

        //    EditorUtility.SetDirty(sequence);

        //}

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField("Total Duration : " +sequence.TotalDuration);

    }

    void DrawAnimInstance(AnimationInstance ai){

        int index = sequence.animations.IndexOf(ai);

        showData[index]  = EditorGUILayout.Foldout(showData[index], ""+ai.animationData.name+"   "+ ai.startTime+" ->  " + (ai.startTime + ai.animationData.duration));


        if(showData[index]){


        ai.animationData = (AnimationData) EditorGUILayout.ObjectField("Data", ai.animationData, typeof(AnimationData),true);
            ai.itemIndex = EditorGUILayout.Popup("Item",ai.itemIndex, sequence.itemNames.ToArray());

            EditorGUIUtility.fieldWidth= 20;
            EditorGUIUtility.labelWidth = 80;
            EditorGUILayout.BeginHorizontal();
            ai.startTime = EditorGUILayout.FloatField("StartTime ",ai.startTime );
            if(!ai.animationData.SetValue){
                
                ai.animationData.duration = EditorGUILayout.FloatField("Duration ", ai.animationData.duration);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUIUtility.fieldWidth = 0;
            EditorGUIUtility.labelWidth = 0;
        EditorGUILayout.LabelField("EndTime : " + (ai.startTime + ai.animationData.duration));
        }
    }
}
