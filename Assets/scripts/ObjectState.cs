using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ObjectStateData
{

    [HideInInspector]
    public GameObject go;
    //[HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Quaternion rotation;
    //[HideInInspector]
    public Vector3 scale;


    [HideInInspector]
    public Color color;



    [SerializeField, HideInInspector]
    Vector2 anchoredPosition;

    Vector2 rectSize;

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

    public void SetObject(GameObject newGo){

        go = newGo;
        SaveState();
    }

    //assign properties of gameObject to sate, value is copied
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
            rectSize = rectTransform.sizeDelta;
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

    //set properties value of gameObject to saved sate
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
            rectTransform.sizeDelta = rectSize;

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

    //public static void SetSize(this RectTransform self, Vector2 size)
    //{
    //    Vector2 oldSize = self.rect.size;
    //    Vector2 deltaSize = size - oldSize;

    //    self.offsetMin = self.offsetMin - new Vector2(
    //        deltaSize.x * self.pivot.x,
    //        deltaSize.y * self.pivot.y);
    //    self.offsetMax = self.offsetMax + new Vector2(
    //        deltaSize.x * (1f - self.pivot.x),
    //        deltaSize.y * (1f - self.pivot.y));
    //}

    //public static void SetWidth(this RectTransform self, float size)
    //{
    //    self.SetSize(new Vector2(size, self.rect.size.y));
    //}

    //public static void SetHeight(this RectTransform self, float size)
    //{
    //    self.SetSize(new Vector2(self.rect.size.x, size));
    //}

}

public class ObjectState : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
