using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace States
{
    public abstract class State
    {
        #region Fields

        protected GameObject gameObject;

        #endregion

        #region Properties

        public string Name
        {
            get
            {
                return GetType().Name;
            }
        }

        #endregion

        #region Public Methods

        public State(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion

        #region Abstract Methods

        public abstract void Enter();

        public abstract void Exit();

        #endregion

        #region Unity Overridable Methods

        public virtual void FixedUpdate() { }

        public virtual void LateUpdate() { }

        public virtual void OnAnimatorIK(int layerIndex) { }

        public virtual void OnAnimatorMove() { }

        public virtual void OnApplicationFocus(bool focusStatus) { }

        public virtual void OnApplicationPause(bool pauseStatus) { }

        public virtual void OnApplicationQuit() { }

        public virtual void OnAudioFilterRead(float[] data, int channels) { }

        public virtual void OnBecameInvisible() { }

        public virtual void OnBecameVisible() { }

        public virtual void OnCollisionEnter(Collision collision) { }

        public virtual void OnCollisionEnter2D(Collision2D collision) { }

        public virtual void OnCollisionExit(Collision collision) { }

        public virtual void OnCollisionExit2D(Collision2D collision) { }

        public virtual void OnCollisionStay(Collision collision) { }

        public virtual void OnCollisionStay2D(Collision2D collision) { }

        public virtual void OnConnectedToServer() { }

        public virtual void OnControllerColliderHit(ControllerColliderHit hit) { }

        public virtual void OnDestroy() { }

        public virtual void OnDisable() { }

        public virtual void OnDisconnectedFromServer(NetworkDisconnection info) { }

        public virtual void OnDrawGizmos() { }

        public virtual void OnDrawGizmosSelected() { }

        public virtual void OnEnable() { }

        public virtual void OnFailedToConnect(NetworkConnectionError error) { }

        public virtual void OnFailedToConnectToMasterServer(NetworkConnectionError error) { }

        public virtual void OnGUI() { }

        public virtual void OnJointBreak(float breakForce) { }

        public virtual void OnLevelWasLoaded(int level) { }

        public virtual void OnMasterServerEvent(MasterServerEvent msEvent) { }

        public virtual void OnMouseDown() { }

        public virtual void OnMouseDrag() { }

        public virtual void OnMouseEnter() { }

        public virtual void OnMouseExit() { }

        public virtual void OnMouseOver() { }

        public virtual void OnMouseUp() { }

        public virtual void OnMouseUpAsButton() { }

        public virtual void OnNetworkInstantiate(NetworkMessageInfo info) { }

        public virtual void OnParticleCollision(GameObject obj) { }

        public virtual void OnPlayerConnected(NetworkPlayer player) { }

        public virtual void OnPlayerDisconnected(NetworkPlayer player) { }

        public virtual void OnPostRender() { }

        public virtual void OnPreCull() { }

        public virtual void OnPreRender() { }

        public virtual void OnRenderImage(RenderTexture source, RenderTexture destination) { }

        public virtual void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) { }

        public virtual void OnServerInitialized() { }

        public virtual void OnTriggerEnter(Collider other) { }

        public virtual void OnTriggerEnter2D(Collider2D other) { }

        public virtual void OnTriggerExit(Collider other) { }

        public virtual void OnTriggerExit2D(Collider2D other) { }

        public virtual void OnTriggerStay(Collider other) { }

        public virtual void OnTriggerStay2D(Collider2D other) { }

        public virtual void OnValidate() { }

        public virtual void OnWillRenderObject() { }

        public virtual void Reset() { }

        public virtual void Start() { }

        public virtual void Update() { }

        #endregion
    }
}
