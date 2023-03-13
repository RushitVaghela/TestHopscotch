using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeanPopup : MonoBehaviour
{
    public bool isOpen;
    [Range(.01f,5f)]
    public float animTime=.3f;
    public LeanTweenType curveType;
	public UnityEvent onClose,onOpen;
	public UnityEvent onCompleteClose,onCompleteOpen;

     public bool isPlaying
    {
        get
        {
            return LeanTween.isTweening(gameObject);
        }
    }
   
    void Start() {
        if(isOpen)  
        {
            SetToOpenPos();
        }  
        else
        {
            SetToClosePos();
        }
    }
    public void Open()
    {
        OpenTween();
    }    
    public void Close()
    {
        CloseTween();        
    }
    public virtual LTDescr OpenTween()
    {
        LeanTween.cancel(gameObject);
        return LeanTween.value(gameObject,0,1,animTime).setEase(curveType).setOnComplete(()=>{
            isOpen=true;
            onCompleteOpen.Invoke();
        });
    }
    public virtual LTDescr CloseTween()
    {
        isOpen=false;
        LeanTween.cancel(gameObject);
        return LeanTween.value(gameObject,1,2,animTime).setEase(curveType).setOnComplete(()=>{
            isOpen=false;
            onCompleteClose.Invoke();
        });
    }
    public virtual void SetToOpenPos()
    {
        isOpen=true;
    }
    public virtual void SetToClosePos()
    {
        isOpen=false;  
    }
}
#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(LeanPopup),true)]
public class LeanPopupEditor: UnityEditor.Editor
{
    
    public override  void OnInspectorGUI()
    {
        LeanPopup targetPopup = (LeanPopup)target;
        UnityEditor.EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Open")) 
        {
            if(UnityEditor.EditorApplication.isPlaying)
                targetPopup.Open();
            else 
                targetPopup.SetToOpenPos();
        }
        if(GUILayout.Button("Close")) 
        {
            if(UnityEditor.EditorApplication.isPlaying)
                targetPopup.Close();
            else 
                targetPopup.SetToClosePos();
        }
        UnityEditor.EditorGUILayout.EndHorizontal();
        DrawDefaultInspector  ();

    }
}
#endif