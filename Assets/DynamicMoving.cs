using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class DynamicMoving : DynamicMoveProvider
{
    public float animSpeed = 1.5f;

    public float useCurvesHeight = 0.5f;

    private CapsuleCollider col;
    private Rigidbody rb;

    private AnimatorStateInfo currentBaseState;

    public Animator anim;

    protected override void Awake()
    {
        base.Awake();
        if(anim == null)
        anim = GetComponent<Animator>();        
    }

    public void Move(float moveSpeed, float direction)
    {
        anim.speed = animSpeed;
        currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
        anim.SetFloat("Speed", moveSpeed);                          // Animator側で設定している"Speed"パラメタにvを渡す
        anim.SetFloat("Direction", direction);
    }

    protected override Vector3 ComputeDesiredMove(Vector2 input)
    {
        //var originTransform = system.xrOrigin.Origin.transform;
        //var speedFactor = moveSpeed * Time.deltaTime * originTransform.localScale.x;
        if(anim != null)
            if (input != Vector2.zero)
                anim?.SetFloat("speed", moveSpeed);
            else
                anim?.SetFloat("speed", 0f);
        return base.ComputeDesiredMove(input);
    }


    }
