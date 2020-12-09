using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWarp : PortablePlayer
{
    private CameraMove cameraMove;

    protected override void Awake()
    {
        base.Awake();

        cameraMove = GetComponent<CameraMove>();
    }

    public override void Warp()
    {
        base.Warp();
        //cameraMove.ResetTargetRotation();
    }
}
