using System.Collections;
using UnityEngine;

public abstract class Move : MonoBehaviour
{

    public void MoveTo(Vector3 dir)
    {
        StartCoroutine(InternalMove(dir));
    }

    protected abstract IEnumerator InternalMove(Vector3 dir);

}
