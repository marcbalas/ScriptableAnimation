using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(menuName = "SequenceScript")]
public class SequenceScript : ScriptableObject {


    public ReorderableList reorderableList;
	public List<string> itemNames;
    //[HideInInspector]
    public List<AnimationInstance> animations;

    //public int nbItems;

    public float TotalDuration {
        get{
            float total = 0;

            foreach(AnimationInstance animScript in animations)
            {
                if (animScript.animationData.duration + animScript.startTime > total){
                    total = animScript.animationData.duration + animScript.startTime ;
                } 
				
            }
            return total;
        }
    }

   

    private void OnEnable()
    {
        //reorderableList = new ReorderableList(animations, typeof(AnimationInstance), true, true, true, true);

        // This could be used aswell, but I only advise this your class inherrits from UnityEngine.Object or has a CustomPropertyDrawer
        // Since you'll find your item using: serializedObject.FindProperty("list").GetArrayElementAtIndex(index).objectReferenceValue
        // which is a UnityEngine.Object
        // reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("list"), true, true, true, true);

        // Add listeners to draw events
        //reorderableList.drawHeaderCallback += DrawHeader;
        //reorderableList.drawElementCallback += DrawElement;

        //reorderableList.onAddCallback += AddItem;
        //reorderableList.onRemoveCallback += RemoveItem;
    }



}
