using System;
using System.Collections.Generic;
using UnityEngine;
using SignalBusNS;

namespace StateMachineNS{
	/*
		A state for the StateMachine so it can be read and used in a Graph data structure. While this can be visualised in a graph,
		its better to sort the nodes as a list of them when they are too many

		The details for a state are not important since they only have the connections to other states and their name
	*/
	[Serializable]
	public class State{
		public string name;
		public List<State> connectedStates = new List<State>();

		public State(string n){
			name = n;
		}

		public bool ConnectState(State st){
			if(st==null) return false;
			if(!IsConnectedTo(st)){
				connectedStates.Add(st);

				return true;
			}
			return false;
		}
		public bool IsConnectedTo(State st){
			if(st==null) return false;
			if(connectedStates.Contains(st)) return true;
			return false;
		}
		public bool IsConnectedFrom(State st){
			if(st==null) return false;
			return st.IsConnectedTo(this);
		}
		public bool DisconnectState(State st){
			return connectedStates.Remove(st);
		}
	}
	/*
		The graph of the States that manages transistions
		This connects and transtions through states like a directional graph
		allows for emiting signals to check for state changes so that the object with the machine can react in order
	*/
	[Serializable]
	public class StateMachine{
		public SignalBus signalBus = new SignalBus();
		protected Dictionary<string, State> states = new Dictionary<string, State>();
		
		protected Stack<string> stateStack = new Stack<string>();
		[SerializeField]
		protected string _current;
		[SerializeField]
		protected string _prev;
		private string _top{
			get{
				return GetNextStateName();
			}
		}

		/*
			Getter for the current state
		*/

		public State CurrentState{
			get{
				if(HasState(_current)){
					return states[_current];
				}
				return null;
			}
		}
		public State PreviousState{
			get{
				if(HasState(_prev)){
					return states[_prev];
				}
				return null;
			}
		}

		/*
			StateMachine constructor(s)
		*/

		public StateMachine(string curr = null){
			if(curr!=null){
				_current = curr;
				states[_current] = new State(curr);
			}

			signalBus.CreateSignal("stateCreate");
			signalBus.CreateSignal("stateDelete");
			signalBus.CreateSignal("stateConnect");
			signalBus.CreateSignal("stateDisconnect");
			signalBus.CreateSignal("stateChange");
		}

		public StateMachine(State st = null){
			if(st!=null){
				_current = st.name;
				states[_current] = st;
			}

			signalBus.CreateSignal("stateCreate");
			signalBus.CreateSignal("stateDelete");
			signalBus.CreateSignal("stateConnect");
			signalBus.CreateSignal("stateDisconnect");
			signalBus.CreateSignal("stateChange");
		}

		/*
			Creates a new state when inside an object to initialise it
		*/

		public bool CreateState(string stateName){
			if(HasState(stateName)) return false;
			states[stateName] = new State(stateName);
			
			signalBus.Emit("stateCreate", states[stateName]);
			return true;
		}
		public bool CreateStates(string[] stateNames){
			bool succ = true;
			foreach(string st in stateNames){
				bool x = CreateState(st);
				if(!x) succ = false;
			}
			return succ;
		}
		/*
			Removes state from the statemachine
		*/
		public State DeleteState(string stateName){
			if(!HasState(stateName)) return null;
			State stateToDelete = states[stateName];

			states.Remove(stateName);
			
			signalBus.Emit("stateDelete", stateToDelete);
			return stateToDelete;
		}

		/*
			Checks if state exists in the machine by name
		*/

		public bool HasState(string stateName){
			if(states.ContainsKey(stateName)) return true;
			return false;
		}
		/*
			Connects two states together so they can transition from each other (in direction from to)
		*/
		public bool ConnectState(string fromState, string toState){
			if(!HasState(fromState) || !HasState(toState)) return false;
			var stFrom = states[fromState];
			var stTo = states[toState];
			if(stFrom == null || stTo == null) return false;
			
			if(stFrom.ConnectState(stTo)){
				signalBus.Emit("stateConnect", new Dictionary<string,object>(){
					{"from",stFrom}, {"to",stFrom},
				});
				return true;
			}
			return false;
		}
		public bool ConnectStates(string fromState, string[] toStates){
			bool succ = true;
			foreach(string st in toStates){
				bool x = ConnectState(fromState, st);
				if(!x) succ = false;
			}
			return succ;
		}
		public bool InterConnectStates(string[] _states){
			bool succ = true;
			foreach(string stA in _states){
				bool x = ConnectStates(stA, _states);
				if(!x) succ = false;
			}
			return succ;
		}
		/*
			Removes connection between two states
		*/
		public bool DisconnectState(string fromState, string toState){
			if(!HasState(fromState) || !HasState(toState)) return false;
			var stFrom = states[fromState];
			var stTo = states[toState];
			if(stFrom == null || stTo == null) return false;
			
			if(stFrom.DisconnectState(stTo)){
				signalBus.Emit("stateDisconnect", new Dictionary<string,object>(){
					{"from",stFrom}, {"to",stFrom},
				});
				return true;
			}
			return false;
		}
		public bool DisconnectStates(string fromState, string[] toStates){
			bool succ = true;
			foreach(string st in toStates){
				bool x = DisconnectState(fromState, st);
				if(!x) succ = false;
			}
			return succ;
		}
		public bool InterDisconnectStates(string[] _states){
			bool succ = true;
			foreach(string stA in _states){
				bool x = DisconnectStates(stA, _states);
				if(!x) succ = false;
			}
			return succ;
		}
		/*
			Switches from the current state to the new state, if it has a connection
		*/
		public bool SwitchToState(string stateName){
			if(!HasState(stateName)) return false;

			var newState = states[stateName];

			var oldState = CurrentState;
			if(oldState == null || oldState.IsConnectedTo(newState)){
				_current = newState.name;
				_prev = (oldState!=null ? oldState.name : null);

				signalBus.Emit("stateChange", new Dictionary<string, object>(){
					{"old",oldState}, {"new",newState},
				});
				return true;
			}
			return false;
		}
		/*
			Switches to the last state it was on
		*/
		public bool SwitchToPreviousState(){
			return SwitchToState(_prev);
		}

		public bool CanSwitchTo(string stateName){
			if(!HasState(stateName) || CurrentState==null) return false;
			State st = states[stateName];

			return CurrentState.IsConnectedTo(st);
		}

		public bool SwitchToNextStateInStack(){
			string nextState;
			stateStack.TryPop(out nextState);

			if(nextState!=null) return SwitchToState(nextState);
			return false;
		}

		public void PushToStack(string stateName){
			if(HasState(stateName)){
				stateStack.Push(stateName);
			}
		}

		public string GetCurrentStateName(){
			if(CurrentState!=null) return CurrentState.name;
			return null;
		}
		public string GetPreviousStateName(){
			if(PreviousState!=null) return PreviousState.name;
			return null;
		}
		public string GetNextStateName(){
			string n = null;
			stateStack.TryPeek(out n);
			return n;
		}
		public bool isCurrentState(string n){
			return (_current == n);
		}
		public bool isPreviousState(string n){
			return (_prev == n);
		}
		public bool isNextState(string n){
			return (GetNextStateName() == n);
		}

	}
}