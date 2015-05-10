using UnityEngine;
using System.Collections;

public abstract class BilliardCue_BasicCommand : MonoBehaviour {
	
	bool activated;

	public void Activate()
	{
		if (!activated) {
			BilliardCue_Control.OnRotateCue += this.RotateCue;
			BilliardCue_Control.OnShootingCue += this.ShootCue;
			BilliardCue_Control.OnReleaseCue += this.ReleaseCue;
			BilliardCue_Control.OnUsePullingeCue += this.UsePull;
			BilliardCue_Control.ioHandView += this.IoHand;
			BilliardCue_Control.OnPullingCue += this.GrabbingCue;
			activated = true;
		}
	}

	public void Desactivate()
	{
		if (activated) {
			BilliardCue_Control.OnRotateCue -= this.RotateCue;
			BilliardCue_Control.OnShootingCue -= this.ShootCue;
			BilliardCue_Control.OnReleaseCue -= this.ReleaseCue;
			BilliardCue_Control.OnUsePullingeCue -= this.UsePull;
			BilliardCue_Control.ioHandView -= this.IoHand;
			BilliardCue_Control.OnPullingCue -= this.GrabbingCue;
			activated = false;
		}
	}

	// common abstract methods
	public abstract string DisplayName{get;}
	protected abstract float RotateCue();
	protected abstract bool ShootCue();
	protected abstract bool ReleaseCue();

	// semi-specific methods
	protected virtual bool UsePull()
	{
		return false;
	}
	protected virtual int IoHand ()
	{
		return 0;
	}
	protected virtual float GrabbingCue()
	{
		return 0.0f;
	}
}
