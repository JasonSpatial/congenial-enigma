using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

using System.Linq;
using System.Linq.Expressions;

    // postfix _Enter init state, _Update update state, _Exit exit state
    public class FiniteStateMachine
    {
        
        MonoBehaviour parent;

        ActionsCache actionsCache;

        private bool _active = true;
        public bool active
        {
            get {
                return _active;
            }
            set {
                _active = value;
            }

        }

        private string _state;
        public string state
        {
            get
            {
                return _state;
            }
            set
            {

                ForceState(value);

            }
        }

        private string _prevState;
        public string prevState
        {
            get
            {
                return _prevState;
            }
            private set
            {
                _prevState = value;
            }
        }

        bool transitionApproved;

        public bool bDebug { get; set; }

        float time;
        float timer;
        public float Timer { get { return timer / time; } set { time = value; timer = 0; } }
        public float TimerReal { get { return timer; } }
        public float TimerRealInverse { get { return time - timer; } }
        public bool TimerOut { get { return (timer >= time); } }

        public FiniteStateMachine(MonoBehaviour parent)
        {
            this.parent = parent;

            actionsCache = new ActionsCache(parent);

            time = 0.0f;
            timer = 0.0f;
        }

        public string LogParent() { return parent.ToString(); }

        public void Destruct()
        {
        }

        public void Update()
        {
            if (!_active) return;

            timer = Mathf.Clamp(timer+Time.deltaTime, 0.0f, time);

            actionsCache.Invoke(_state + "_Update"); //update state
        }

        public void ForceState(string value)
        {
            _prevState = _state;
            _state = value;

            if (bDebug) Debug.Log(parent + " state : from " + _prevState + " to " + _state);

            actionsCache.Invoke(_prevState + "_Exit"); //exit state
            actionsCache.Invoke(_state + "_Enter"); //enter state
        }
    }
