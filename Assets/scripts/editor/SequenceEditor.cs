using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

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
