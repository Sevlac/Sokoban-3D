using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Roll : Move
{
    [SerializeField] 
    private float _rollSpeedDegPerSec = 270;
    private float _RotationAngle;

    [SerializeField]
    private AudioClip _plic;
    [SerializeField]
    private AudioClip _ploc;

    private AudioSource _audio;


    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    protected override IEnumerator InternalMove(Vector3 dir)
    {
        float remainingAngle = 90;
        Vector3 anchor = transform.position + (Vector3.down + dir) * 0.5f;
        Vector3 axis = Vector3.Cross(Vector3.up, dir);

        while (remainingAngle > 0)
        {
            _RotationAngle = MathF.Min(_rollSpeedDegPerSec * Time.deltaTime, remainingAngle);
            transform.RotateAround(anchor, axis, _RotationAngle);
            remainingAngle -= _RotationAngle;
            yield return null;
        }
        PlayerControls._isMoving = false;
        if (PlayerPrefs.GetInt("isSoundActive", 1) == 1)
        {
            if (_audio.clip == _plic)
            {
                _audio.Play();
                yield return new WaitForSeconds(_audio.clip.length);
                _audio.clip = _ploc;
            }
            else
            {
                _audio.Play();
                yield return new WaitForSeconds(_audio.clip.length);
                _audio.clip = _plic;

            }

        }
    }

}