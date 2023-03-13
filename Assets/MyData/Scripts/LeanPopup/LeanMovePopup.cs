using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum LeanMovePosType {
    Anchored_UI,
    WorldPos,
    LocalPos
}
public class LeanMovePopup : LeanPopup {
    public LeanMovePosType movePosType;
    public Vector3 openPos;
    public Vector3 closePos;
    public override LTDescr OpenTween () {
        LeanTween.cancel (gameObject);
        LTDescr temp = null;
        if (movePosType == LeanMovePosType.Anchored_UI && gameObject.GetComponent<RectTransform> ()) {
            temp = LeanTween.move (gameObject.GetComponent<RectTransform> (), openPos, animTime).setEase (curveType);
        } else if (movePosType == LeanMovePosType.LocalPos) {
            temp = LeanTween.moveLocal (gameObject, openPos, animTime).setEase (curveType);
        } else {
            temp = LeanTween.move (gameObject, openPos, animTime).setEase (curveType);
        }
        onOpen.Invoke ();
        temp.setOnComplete (() => {
            isOpen = true;
            onCompleteOpen.Invoke ();
        });
        return temp;
    }
    public override LTDescr CloseTween () {
        LeanTween.cancel (gameObject);
        LTDescr temp = null;
        if (movePosType == LeanMovePosType.Anchored_UI && gameObject.GetComponent<RectTransform> ()) {
            temp = LeanTween.move (gameObject.GetComponent<RectTransform> (), closePos, animTime).setEase (curveType);
        } else if (movePosType == LeanMovePosType.LocalPos) {
            temp = LeanTween.moveLocal (gameObject, closePos, animTime).setEase (curveType);
        } else {
            temp = LeanTween.move (gameObject, closePos, animTime).setEase (curveType);
        }
        onClose.Invoke ();
        temp.setOnComplete (() => {
            isOpen = false;
            onCompleteClose.Invoke ();
        });
        return temp;
    }

    public override void SetToOpenPos () {
        isOpen = true;
        if (movePosType == LeanMovePosType.Anchored_UI && gameObject.GetComponent<RectTransform> ()) {
            gameObject.GetComponent<RectTransform> ().anchoredPosition3D = openPos;
        } else if (movePosType == LeanMovePosType.LocalPos) {
            gameObject.transform.localPosition = openPos;
        } else {
            gameObject.transform.position = openPos;
        }
    }
    public override void SetToClosePos () {
        isOpen = false;

        if (movePosType == LeanMovePosType.Anchored_UI && gameObject.GetComponent<RectTransform> ()) {
            gameObject.GetComponent<RectTransform> ().anchoredPosition3D = closePos;
        } else if (movePosType == LeanMovePosType.LocalPos) {
            gameObject.transform.localPosition = closePos;
        } else {
            gameObject.transform.position = closePos;
        }
    }
}