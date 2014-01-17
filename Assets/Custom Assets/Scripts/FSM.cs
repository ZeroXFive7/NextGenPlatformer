using UnityEngine;
using System.Collections;
using States;
using System;
using System.Collections.Generic;
using System.Linq;

public abstract class FSM : MonoBehaviour
{
    #region Fields

    private Dictionary<State, List<Transition>> transitionFromMap = new Dictionary<State,List<Transition>>();
    private Dictionary<State, List<Transition>> transitionToMap = new Dictionary<State, List<Transition>>();
    private List<State> stateLibrary = new List<State>();

    #endregion

    #region Properties

    private State currentState = new NullState();
    protected State CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            if (!(currentState is NullState))
            {
                currentState.Exit();
            }

            previousState = currentState;
            currentState = value;

            currentState.Enter();
        }
    }

    private State previousState = new NullState();
    protected State PreviousState
    {
        get
        {
            return previousState;
        }
    }

    protected List<Transition> CurrentTransitions
    {
        get
        {
            return transitionFromMap[CurrentState];
        }
    }

    #endregion

    #region Protected Methods

    protected void AddState(State state, bool isDefault = false)
    {
        if (stateLibrary.Contains(state))
        {
            Debug.LogError("Error adding State. State " + state + " already registered.");
            return;
        }

        stateLibrary.Add(state);

        if (isDefault)
        {
            CurrentState = state;
        }
    }

    protected State GetState(string name)
    {
        return stateLibrary.SingleOrDefault(s => s.Name == name);
    }
    
    protected void AddTransition(Transition transition)
    {
        if (!stateLibrary.Contains(transition.Source))
        {
            Debug.LogError("Error adding Transition.  Source State is not registered.");
        }

        if (!stateLibrary.Contains(transition.Destination))
        {
            Debug.LogError("Error adding Transition.  Destination State is not registered.");
        }

        if (!transitionFromMap.ContainsKey(transition.Source))
        {
            transitionFromMap.Add(transition.Source, new List<Transition>());
        }

        if (transitionFromMap[transition.Source].Contains(transition))
        {
            Debug.LogError("Error adding Transition.  Duplicate transition added from State: " + transition.Source);
            return;
        }

        transitionFromMap[transition.Source].Add(transition);

        if (!transitionToMap.ContainsKey(transition.Destination))
        {
            transitionToMap.Add(transition.Destination, new List<Transition>());
        }

        if (transitionToMap[transition.Destination].Contains(transition))
        {
            Debug.LogError("Error adding Transition.  Duplicate transition added to State: " + transition.Destination);
        }

        transitionToMap[transition.Destination].Add(transition);
    }

    #endregion

    #region Abstract Methods

    protected abstract void FSMAwake();

    #endregion

    #region Private Methods

    private void ValidateStateMachine()
    {
        bool errors = false;

        foreach (var state in stateLibrary)
        {
            if (!transitionFromMap.ContainsKey(state))
            {
                Debug.LogError("State Machine Layout Error.  There is no transition from " + state);
            }

            if (!transitionToMap.ContainsKey(state))
            {
                Debug.LogError("State Machine Layout Error.  There is no transition to " + state);
            }
        }

        if (errors)
        {
            throw new Exception("Invalid State Machine layout.");
        }
    }

    private void UpdateTransitions()
    {
        foreach (var transition in CurrentTransitions)
        {
            if (transition.Reason())
            {
                CurrentState = transition.Destination;
                break;
            }
        }
    }

    #endregion

    #region MonoBehaviour Messages

    void Awake()
    {
        FSMAwake();
        ValidateStateMachine();
    }

    void Start()
    {
        currentState.Start();
    }

    void Update()
    {
        UpdateTransitions();
        currentState.Update();
    }

    void FixedUpdate()
    {
        currentState.FixedUpdate();
    }

    void LateUpdate()
    {
        currentState.LateUpdate();
    }

    void OnAnimatorIK(int layerIndex)
    {
        currentState.OnAnimatorIK(layerIndex);
    }

    void OnAnimatorMove()
    {
        currentState.OnAnimatorMove();
    }

    void OnApplicationFocus(bool focusState)
    {
        currentState.OnApplicationFocus(focusState);
    }

    void OnApplicationPause(bool pauseState)
    {
        currentState.OnApplicationPause(pauseState);
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        currentState.OnAudioFilterRead(data, channels);
    }

    void OnBecameInvisible()
    {
        currentState.OnBecameInvisible();
    }

    void OnBecameVisible()
    {
        currentState.OnBecameVisible();
    }

    void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(collision);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnCollisionEnter2D(collision);
    }

    void OnCollisionExit(Collision collision)
    {
        currentState.OnCollisionExit(collision);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        currentState.OnCollisionExit2D(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        currentState.OnCollisionStay(collision);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        currentState.OnCollisionStay2D(collision);
    }

    void OnConnectedToServer()
    {
        currentState.OnConnectedToServer();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        currentState.OnControllerColliderHit(hit);
    }

    void OnDestroy()
    {
        currentState.OnDestroy();
    }

    void OnDisable()
    {
        currentState.OnDisable();
    }

    void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        currentState.OnDisconnectedFromServer(info);
    }

    void OnDrawGizmos()
    {
        currentState.OnDrawGizmos();
    }

    void OnDrawGizmosSealed()
    {
        currentState.OnDrawGizmosSelected();
    }

    void OnEnable()
    {
        currentState.OnEnable();
    }

    void OnFailedToConnect(NetworkConnectionError error)
    {
        currentState.OnFailedToConnect(error);
    }

    void OnFailedToConnectToMasterServer(NetworkConnectionError error)
    {
        currentState.OnFailedToConnectToMasterServer(error);
    }

    void OnGUI()
    {
        currentState.OnGUI();
    }

    void OnJointBreak(float breakForce)
    {
        currentState.OnJointBreak(breakForce);
    }

    void OnLevelWasLoaded(int level)
    {
        currentState.OnLevelWasLoaded(level);
    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        currentState.OnMasterServerEvent(msEvent);
    }

    void OnMouseDown()
    {
        currentState.OnMouseDown();
    }

    void OnMouseDrag()
    {
        currentState.OnMouseDrag();
    }

    void OnMouseEnter()
    {
        currentState.OnMouseEnter();
    }

    void OnMouseExit()
    {
        currentState.OnMouseExit();
    }

    void OnMouseOver()
    {
        currentState.OnMouseOver();
    }

    void OnMouseUp()
    {
        currentState.OnMouseUp();
    }

    void OnMouseUpAsButton()
    {
        currentState.OnMouseUpAsButton();
    }

    void OnNetworkInstantiate(NetworkMessageInfo info)
    {
        currentState.OnNetworkInstantiate(info);
    }

    void OnParticleCollision(GameObject obj)
    {
        currentState.OnParticleCollision(obj);
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        currentState.OnPlayerConnected(player);
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        currentState.OnPlayerDisconnected(player);
    }

    void OnPostRender()
    {
        currentState.OnPostRender();
    }

    void OnPreCull()
    {
        currentState.OnPreCull();
    }

    void OnPreRender()
    {
        currentState.OnPreRender();
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        currentState.OnRenderImage(source, destination);
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        currentState.OnSerializeNetworkView(stream, info);
    }

    void OnServerInitialized()
    {
        currentState.OnServerInitialized();
    }

    void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        currentState.OnTriggerEnter2D(other);
    }

    void OnTriggerExit(Collider other)
    {
        currentState.OnTriggerExit(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        currentState.OnTriggerExit2D(other);
    }

    void OnTriggerStay(Collider other)
    {
        currentState.OnTriggerStay(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        currentState.OnTriggerStay2D(other);
    }

    void OnValidate()
    {
        currentState.OnValidate();
    }

    void OnWillRenderObject()
    {
        currentState.OnWillRenderObject();
    }

    void Reset()
    {
        currentState.Reset();
    }
    
    #endregion
}
