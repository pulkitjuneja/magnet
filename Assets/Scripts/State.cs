using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;

  public abstract class State<T>
    {
        public T ParentMachine;
        public abstract IEnumerator run();
        public State(T p)
        {
            ParentMachine = p;
        }

        public virtual void TriggerExit2D(Collider2D other){}
    }

    public class StateMachine<T,Z,M> : State<T> where M : MonoBehaviour
    {
        public State<Z> Current;
        public Dictionary<System.Type, State<Z>> States;
        public M Component;
        public StateMachine(T s,M comp) : base(s) 
        {
            States = new Dictionary<System.Type, State<Z>>(20);
            Current = null;
            Component = comp;
        }

        public override System.Collections.IEnumerator run()
        {
            while(Current!=null && Component.isActiveAndEnabled)
            {
                yield return Component.StartCoroutine(Current.run());
            }
        }

        public virtual void AddState(System.Type type)
        {
            if (!States.ContainsKey(type))
            {
                States.Add(type, null);
            }
        }

        public void SetState(System.Type type , bool preserve,object[] args)
        {
            if(!States.ContainsKey(type))
            {
                throw new Exception("State not a part of state machine");
            }
            if (preserve)
                States[Current.GetType()] = Current;
            if(States[type]==null)
            {
                Current = (State<Z>)Activator.CreateInstance(type, args);
            }
            else
                Current = States[type];
        }
        
    }

    public class MainStateMachine:StateMachine<object,MainStateMachine,FSMgenerator>
    {
        public static MainStateMachine instance= null;
        private MainStateMachine(object parent, FSMgenerator mono) : base(parent, mono) 
        {
            AddState(typeof(MenuState));
            AddState(typeof(GamePlay));
            AddState(typeof(GameOverState));
            Current = new MenuState(this);
        }

        public static MainStateMachine GetNewInstance(object parent,FSMgenerator mono)
        {
            instance = new MainStateMachine(parent, mono);
            return instance;
        }

        public override void TriggerExit2D(Collider2D other)
        {
            Current.TriggerExit2D(other);
        }

    }

