﻿using FastCampus.Characters;
using System;
using UnityEngine;

namespace FastCampus.AI
{
    [Serializable]
    public class DeadState : State<EnemyController>
    {
        private Animator animator;

        protected int isAliveHash = Animator.StringToHash("IsAlive");

        public override void OnInitialized()
        {
            animator = context.GetComponent<Animator>();
        }

        public override void OnEnter()
        {
            animator?.SetBool(isAliveHash, false);
        }

        public override void Update(float deltaTime)
        {
            if (stateMachine.ElapsedTimeInState > 3.0f)
            {
                GameObject.Destroy(context.gameObject);
            }
        }

        public override void OnExit()
        {
        }
    }
}