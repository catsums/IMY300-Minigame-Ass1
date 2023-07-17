using System;
using System.Collections.Generic;
using UnityEngine;

using SignalBusNS;

/*
	TimerComp lets you add a Timer to any GameObject to run a certain time on the inspector or control a timer visually within code
	This is different from coroutines, and this makes use of a signalbus to run a function after a certain time
	This also relies on the game process than C# process time
*/

public class TimerComp : MonoBehaviour{
	public SignalBus signalBus = new SignalBus();

	public enum TimeProcessMode {
		IDLE = 0, FIXED = 1
	}

	// bool started = false; /* check if timer is started */
	public bool paused = false; /* check if timer is paused */
	public bool oneshot = true; /* check if timer is only running once and not repeating */
	public bool autostart = false; /* check if timer is to start when enabled */

	public float waitTime = 1f; /* total time for timer overall */
	public float timeLeft = 0f; /* amount of time left for timer to end */

	public TimeProcessMode processMode = TimeProcessMode.IDLE; /* process mode for timer if its meant to be fixed or idle (based on true runtime) */

	/*
		On Start, timer will check to autostart timer
	*/

	void Start(){
		if(autostart){
			StartTimer();
		}else{
			PauseTimer();
		}
	}

	/*
		On Update, if on IDLE mode, the timer will countdown its timer based on the deltatime
	*/
	
	void Update(){
		if(processMode != TimeProcessMode.IDLE) return;

		var delta = Time.deltaTime;
		ProcessCountDown(delta);
	}
	/*
		On Update, if on FIXED mode, the timer will countdown based on the game FPS deltatime
	*/
	void FixedUpdate(){
		if(processMode != TimeProcessMode.FIXED) return;

		var delta = Time.fixedDeltaTime;
		ProcessCountDown(delta);
	}

	/*
		This starts the timer based on the waittime, which will be the duration of the timer 
	*/

	void StartTimer(){
		StartTimer(waitTime);
	}

	/*
		This starts timer with a newly set duration
	*/

	void StartTimer(float time){
		if(time > 0){
			waitTime = time;
			ResetTimer();
			ResumeTimer();
		}
	}

	/*
		Resumes the timer if it was paused
	*/

	void ResumeTimer(){
		paused = false;
	}

	/*
		Pauses timer if it was not paused
	*/

	void PauseTimer(){
		paused = true;
	}

	/*
		Resets the timer, thus making the timeleft set back to the waittime
	*/

	void ResetTimer(){
		timeLeft = waitTime;
	}

	/*
		Processes timer by counting down based on a certain value and will not process it if its paused or stopped
	*/

	void ProcessCountDown(float delta){
		if(paused) return;

		timeLeft -= delta;

		if(timeLeft <= 0){
			timeLeft = 0;

			signalBus.Emit("timeout");

			if(!oneshot){
				StartTimer();
			}else{
				PauseTimer();
			}
		}
	}
}