using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADSController : MonoBehaviour {

	public ADStatus adStatus;
	public enum ADStatus {

		REWARDAPPROVEDINFO,
		LOADEDREWARDED,
		LOADREWARDEDFAILED,
		HIDDENREWARDED,
		USERDECLINED,
		NULL,
	}

	//Delegate
	public delegate void OnAdStatusChanged(ADStatus adStatus);
	public  static event OnAdStatusChanged onAdStatusChanged;



	private void Start () {

//		Debug.Log ("ads controller");

		if (Application.platform != RuntimePlatform.OSXEditor){

			AppLovin.SetSdkKey("EarbB063rSJBWRQLRk8BvtPwVJoxXwcnURNgSTlHw3TRUunwcv6WnWQGrCCN7p9cEWHg_0zw4RLTJ7Tx0aRn6s");
			AppLovin.InitializeSdk ();

			AppLovin.PreloadInterstitial ();
			AppLovin.LoadRewardedInterstitial();

			AppLovin.SetUnityAdListener(this.gameObject.name);

		}

	}


	public static void ShowInterstitial() {

		if (Application.platform == RuntimePlatform.OSXEditor) {
			return;
		}

		if(AppLovin.HasPreloadedInterstitial()) {
			
			AppLovin.ShowInterstitial();
			AppLovin.PreloadInterstitial ();
		}
	}


	public static void ShowRewardedVideo() {

		if (Application.platform == RuntimePlatform.OSXEditor) {
			return;
		}

		if(AppLovin.HasPreloadedInterstitial()){
			
			AppLovin.ShowRewardedInterstitial ();
			AppLovin.LoadRewardedInterstitial();
		}

	}



	private void onAppLovinEventReceived(string ev){

		ADStatus adStatus = ADStatus.NULL;

		if (ev.Contains ("REWARDAPPROVEDINFO")) {

			adStatus = ADStatus.REWARDAPPROVEDINFO;

		} else if (ev.Contains ("LOADEDREWARDED")) {

			adStatus = ADStatus.LOADEDREWARDED;

		} else if (ev.Contains ("LOADREWARDEDFAILED")) {

			adStatus = ADStatus.LOADREWARDEDFAILED;

		} else if (ev.Contains ("HIDDENREWARDED")) {

			adStatus = ADStatus.HIDDENREWARDED;
			AppLovin.LoadRewardedInterstitial ();

		} else if (ev.Contains ("USERDECLINED")) {

			adStatus = ADStatus.USERDECLINED;

		}


		if (onAdStatusChanged != null) {
			onAdStatusChanged (adStatus);
		}

	}
}
