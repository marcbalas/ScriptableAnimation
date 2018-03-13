using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AnimationData))]
public class AnimationDataEditor : Editor {


    AnimationData animation;

	private void OnEnable()
	{
        animation = (AnimationData)target;
	}

	public override void OnInspectorGUI()
	{
        base.OnInspectorGUI();

        if (animation.type != AnimationType.Color)
        {
            animation.subtype = (AnimationSubType)EditorGUILayout.EnumPopup("Reference", animation.subtype);
        }

        switch (animation.type)
        {
            case AnimationType.Fade:

                animation.targetValue = EditorGUILayout.FloatField( "Value",animation.targetValue);
               
                break;
            case AnimationType.Rotate:
            case AnimationType.Translate:
            case AnimationType.Scale:
                animation.targetVector = EditorGUILayout.Vector3Field("Value", animation.targetVector);
                break;

            case AnimationType.Color:

                animation.targetColor = EditorGUILayout.ColorField("color", animation.targetColor);
                break;
          
            default:
                break;
        }

        if(animation.type == AnimationType.Translate){


            animation.unitType = (AnimationUnitType)EditorGUILayout.EnumPopup("Units", animation.unitType);

            
        }

        if (!animation.SetValue)
        {
            animation.duration = EditorGUILayout.FloatField("Duration", animation.duration);
			animation.animationCurve = EditorGUILayout.CurveField("Curve", animation.animationCurve);
        }


	}


}
