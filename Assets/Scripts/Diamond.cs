using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Diamond : MonoBehaviour
{
    private void Start()
    {
        transform.DORotate(new Vector3(transform.localEulerAngles.x, 30, transform.localEulerAngles.z), .1f, RotateMode.Fast).SetLoops(-1, LoopType.Incremental);
        transform.DOMoveY(transform.position.y - .1f, .7f).SetLoops(-1, LoopType.Yoyo);

    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IDiamondCollect>()?.DiamondCollect(gameObject);
        Destroy(GetComponent<Collider>());
    }
}
