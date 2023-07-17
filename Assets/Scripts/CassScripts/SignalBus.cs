using System.Collections.Generic;
using System;

namespace SignalBusNS
{
	/*
		SignalBus allows for event handling between objects that have signalbusses in order to subscribe to each other and emit signals to run functions
		This is similar to C# Actions and Funcs, except this lets you keep track of handlers and connect to any Action taking in an object
		Please remember to type case the object to your desired type
		This is very similar to javascript, but does not use coroutines - all actions are executed upon emit
	*/
    public class SignalBus{
        protected Dictionary< string, List< Action<object> > > handlers;
        protected Dictionary< string, Action<object> > signals;

		/*
			Constructor for signalbus
		*/
        public SignalBus(){
            handlers = new Dictionary< string, List< Action<object> > >();
            signals = new Dictionary< string, Action<object> >();
        }
		/*
			Creates a new signal with a certain name
		*/

        public void CreateSignal(string n){
            handlers[n] = new List<Action<object>>();
            signals[n] = null;
        }
		/*
			Subscribes an action (function) to a signal name
		*/

        public void Subscribe(string n, Action<object> listener){
            if(!HasSignal(n)){
                CreateSignal(n);
            }

            handlers[n].Add(listener);
            signals[n] += listener;

        }
		/*
			Unsubs an action from a signal name
		*/
        public void Unsubscribe(string n, Action<object> listener){
            if(HasSignal(n)){
                signals[n] -= listener;
                handlers[n].Remove(listener);
            }
        }
		/*
			Checks if a specific function is subbed to a certain signal
		*/
        public bool IsSubscribed(string n, Action<object> listener){
            if(HasSignal(n)){
                return handlers[n].Contains(listener);
            }
            return false;
        }
		/*
			Checks if a signal exists in this signalBus
		*/
        public bool HasSignal(string n){
            if(signals.ContainsKey(n))
                return true;
            return false;
        }
		/*
			Emit signal of that name to invoke all actions/functions connected to it
		*/
        public void Emit(string n){
            if(HasSignal(n)){
                signals[n]?.Invoke(null);
            }
        }
		/*
			Emit signal of that name to invoke all actions/functions connected to it with a parameter
		*/
        public void Emit(string n, object data){
            if(HasSignal(n)){
                signals[n]?.Invoke(data);
            }
        }
    }
}