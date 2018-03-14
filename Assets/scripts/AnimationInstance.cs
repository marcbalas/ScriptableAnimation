using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AnimationInstance
{

    bool waiting = false;
    float percent = 0;

    public AnimationData animationData;

    public int itemIndex = 0;

    public float startTime = 0;
    //[SerializeField]
    // [HideInInspector]


    //save when start to play animation
    public ObjectStateData objectData;


    public void Reset()
    {
        percent = 0;
        waiting = false;
       // RestoreState();
        //SaveState();

    }

    //public void SaveState()
    //{
    //    objectData.SaveState();

    //}

    //public void RestoreState()
    //{
    //    objectData.RestoreState();
    //}


    public void Pause()
    {
        waiting = true;
    }

    public void Resume()
    {
        waiting = false;
    }



    public void SetAnimation(float percent)
    {
        float curvePercent = animationData.animationCurve.Evaluate(percent);
        switch (animationData.type)
        {
            case AnimationType.Fade:

                //use current color 
                animationData.targetValue = Mathf.Clamp01(animationData.targetValue);

                if (objectData.sprite)
                {
                    objectData.sprite.color = new Color(objectData.sprite.color.r, objectData.sprite.color.g, objectData.sprite.color.b, objectData.sprite.color.a * (1 - curvePercent) + curvePercent * animationData.targetValue);

                }
                else if (objectData.text)
                {
                    objectData.text.color = new Color(objectData.text.color.r, objectData.text.color.g, objectData.text.color.b, objectData.text.color.a * (1 - curvePercent) + curvePercent * animationData.targetValue);

                }
                else if (objectData.image)
                {
                    objectData.image.color = new Color(objectData.image.color.r, objectData.image.color.g, objectData.image.color.b, objectData.image.color.a * (1 - curvePercent) + curvePercent * animationData.targetValue);

                }
                //it s a mesh : modify material
                else
                {
                    Material mat = objectData.go.GetComponent<MeshRenderer>().material;
                    mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, mat.color.a * (1 - curvePercent) + curvePercent * animationData.targetValue);
                }

                break;
            case AnimationType.Color:
                
                if (objectData.sprite)
                {
                    objectData.sprite.color = Color.Lerp(objectData.color, animationData.targetColor, curvePercent);

                }
                else if (objectData.text)
                {
                    objectData.text.color = Color.Lerp(objectData.color, animationData.targetColor, curvePercent);

                }
                else if (objectData.image)
                {
                    objectData.image.color = Color.Lerp(objectData.color, animationData.targetColor, curvePercent);

                }
                //it s a mesh : modify material
                else
                {
                    Material mat = objectData.go.GetComponent<MeshRenderer>().material;
                    mat.color =  Color.Lerp(objectData.color, animationData.targetColor, curvePercent);
                }



                break;
            case AnimationType.Translate:
                if (objectData.rectTransform)
                {
                    Vector3 targetVectorAbsolute = animationData.targetVector;
                    if (animationData.unitType == AnimationUnitType.Screen)
                    {
                        targetVectorAbsolute = new Vector3(Screen.width * animationData.targetVector.x,Screen.height* animationData.targetVector.y,0);


                    }
                    objectData.rectTransform.position = Vector3.Lerp(objectData.RectPosition, targetVectorAbsolute + (animationData.subtype == AnimationSubType.Relative ? objectData.RectPosition: Vector3.zero), curvePercent);

                }
                else
                {

                    objectData.go.transform.position = Vector3.Lerp(objectData.position, animationData.targetVector + (animationData.subtype == AnimationSubType.Relative ? objectData.position : Vector3.zero), curvePercent);
                }
                break;
            case AnimationType.Rotate:
                Quaternion targetQuaternion = Quaternion.Euler(animationData.targetVector + (animationData.subtype == AnimationSubType.Relative ? objectData.LocalEulerAngles : Vector3.zero));
                objectData.go.transform.rotation = Quaternion.Lerp(objectData.rotation, targetQuaternion, curvePercent);
                break;

            case AnimationType.Scale:
                objectData.go.transform.localScale = Vector3.Lerp(objectData.scale, animationData.targetVector, curvePercent);
                break;


            
            default:
                break;
        }

    }


    public IEnumerator Play()
    {


        float journey = 0f;

        Debug.Log("play anim on gameObject " + objectData.go.name);

            while (journey <= animationData.duration)
            {
                journey = journey + Time.deltaTime;

            if (animationData.duration <= 0 || animationData.SetValue){
                
				journey = animationData.duration+1;
            }
               
			    percent = Mathf.Clamp01(journey / animationData.duration);
                SetAnimation(percent);

                if (waiting == true) Debug.Log("anim paused...");
                //for UI repaint
                yield return new WaitForEndOfFrame();

                //when sequence animation is paused
                yield return new WaitUntil(() => waiting == false);
            }
        yield return null;
    }

}