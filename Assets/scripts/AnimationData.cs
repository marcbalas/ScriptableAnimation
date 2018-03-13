using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum AnimationType : int
{

    Translate,
    Rotate,
    Scale,

    Fade,
    Color,

   // SetValue,
    Blur,
	//Value,

    //compound animations to define
    //vibrate,
    //shake,
    //pop,


}

//public enum AnimationSetValueType : int
//{

//    Position, //relative would mean % of screen
//    EulerAngles,
//    Scale,

//}

//only for relative translation
public enum AnimationUnitType:int
{
    World,
	Screen,
}

public enum AnimationSubType : int
{
    Relative,
    Absolute,

}



[CreateAssetMenu(menuName = "AnimationData")]
public class AnimationData : ScriptableObject {

   // float percent = 0;

	[SerializeField]
	public AnimationType type;

    [HideInInspector]
    public AnimationSubType subtype;

    public bool SetValue = false;

   [HideInInspector]
    public AnimationUnitType unitType;

    [HideInInspector]
    public float duration = 0.5f;

    [HideInInspector]
    public Vector3 targetVector;
    [HideInInspector]
    public Color targetColor;

    [HideInInspector]
	public float targetValue;
    public Event onComplete;


    [HideInInspector]
    public AnimationCurve animationCurve = AnimationCurve.Linear(0,0,1,1);
   
	
}
