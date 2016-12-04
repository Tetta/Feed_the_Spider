using UnityEngine;
using System.Collections;
using System;


public class mCardClass : MonoBehaviour {
	public string functionEnable = "";
	public string functionPress = "";
	public string functionDisable = "";

    // Use this for initialization
    void Start () {
		//StartCoroutine (startCard());	
	}

	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable() {
		if (functionEnable != "") SendMessage (functionEnable);
	}	
	void OnPress(bool isPressed) {
		if (!isPressed && functionPress != "") SendMessage(functionPress, isPressed);
    }

	void OnDisable() {
		if (functionDisable != "") {
			for (int i = 0; i < 5; i++) 
			disableSkinPreview (transform.parent.parent.GetChild(1).GetChild(0).GetChild (i).gameObject, false);
		}
	}

    void enableCard() {
		//GetComponent<Animator> ().Stop ();
		//если куплен
        if (ctrProgressClass.progress.Count == 0) ctrProgressClass.getProgress();
        if (ctrProgressClass.progress[name] >= 1) {
            //frontside and backside
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }
        else {
			transform.GetChild(0).gameObject.SetActive(false);
			transform.GetChild(1).gameObject.SetActive(true);

        }


        //если выбран текущий скин или шапка или ягода
        if (ctrProgressClass.progress[name] == 2) {
            pressCard(false);
        }
    }

    void pressCard(bool isPressed) {
        if (!isPressed) {
			//stop all animation
            for (int i = 0; i < 5; i++) {
                Transform prevObject = transform.parent.GetChild(i);
                if (ctrProgressClass.progress[name] >= 1) prevObject.GetChild(3).gameObject.SetActive(false);
                /*
				if (prevObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("card selected") ||
                    prevObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("card select")
                    )
                    prevObject.GetComponent<Animator>().Play("default");
                    */
            }


            //start включаем preview текущий скин и выключаем все остальные
            string skinName = name;
            if (name.Substring(0, 3) == "hat") skinName = staticClass.currentSkin;
            Transform previewObj = transform.parent.parent.GetChild(1).GetChild(0);
            for (int i = 0; i < 5; i++) {
                if (previewObj.GetChild(i).name == skinName) {
					//previewObj.GetChild(i).gameObject.SetActive(true);
					disableSkinPreview (previewObj.GetChild (i).gameObject, true);

					 
					if (name.Length == 5) {
						previewObj.GetChild (i).GetComponent<Animator> ().Play ("spider hi");
						previewObj.GetChild (i).GetChild (1).GetChild (0).gameObject.GetComponent<AudioSource> ().Play();
					}
					if (name.Substring(0, 3) == "hat") {
						previewObj.GetChild (i).GetComponent<Animator> ().Play ("spider breath");
						//включаем текущую шапку и выключаем все остальные
                        for (int j = 0; j < 4; j++) {
							if (previewObj.GetChild (i).GetChild (0).GetChild (j).name == name) {
								previewObj.GetChild (i).GetChild (0).GetChild (j).gameObject.SetActive (true);
							} else {
								previewObj.GetChild (i).GetChild (0).GetChild (j).gameObject.SetActive (false);
							}
                        }
                    }

                }
                else { 
					disableSkinPreview (previewObj.GetChild (i).gameObject, false);
				}
            }
			//включаем описание скина или шапки
			for (int i = 0; i < 5; i++) {
				if ("label " + name == previewObj.GetChild (i + 5).name) previewObj.GetChild (i + 5).gameObject.SetActive (true);
				else previewObj.GetChild (i + 5).gameObject.SetActive (false);

			}

            //end включаем preview текущий скин и выключаем все остальные

            GetComponent<Animator>().Play("card select");
            //если куплен, то выбираем
            if (ctrProgressClass.progress[name] >= 1) {
                transform.GetChild(3).gameObject.SetActive(true);

                // = 1 и запись в static
                if (name.Substring(0, 4) == "skin") {
                    ctrProgressClass.progress[staticClass.currentSkin] = 1;
                    staticClass.currentSkin = name;
                    staticClass.changeSkin();
                }
                else if (name.Substring(0, 3) == "hat") {
                    ctrProgressClass.progress[staticClass.currentHat] = 1;
                    staticClass.currentHat = name;
                    staticClass.changeHat();
                }
                else if (name.Substring(0, 5) == "berry") {
                    ctrProgressClass.progress[staticClass.currentBerry] = 1;
                    staticClass.currentBerry = name;
                    staticClass.changeBerry();
                }
                ctrProgressClass.progress[name] = 2;
                ctrProgressClass.saveProgress();

				//выключаем get booster
				transform.parent.parent.GetChild(1).GetChild(1).gameObject.SetActive(false);
            }
            else
				transform.parent.parent.GetChild(1).GetChild(1).gameObject.SetActive(true);
			
        }
    }

	void disableSkinPreview (GameObject skinPreview, bool flag) {
		if ((skinPreview.activeSelf && !flag) || (!skinPreview.activeSelf && flag)) {
			skinPreview.SetActive (flag);
			//skin
			//возврат изменений, сделанных анимацией "spider hi" (предотвращает баг если часто переключаешься)
			if (skinPreview.transform.rotation.z != 0) {
				
				skinPreview.transform.position = new Vector3 (0, 0, 0);
				skinPreview.transform.rotation = new Quaternion (0, 0, 0, 0);
				//legs
				skinPreview.transform.GetChild (9).localPosition = new Vector3 (-51, -71, 0);
				skinPreview.transform.GetChild (9).rotation = new Quaternion (0, 0, 0, 0);
				skinPreview.transform.GetChild (10).localPosition = new Vector3 (-67, -87, 0);
				skinPreview.transform.GetChild (10).rotation = new Quaternion (0, 0, 0, 0);
				skinPreview.transform.GetChild (11).localPosition = new Vector3 (-77.3F, -67.7F, 0);
				skinPreview.transform.GetChild (11).rotation = new Quaternion (0, 0, 0, 0);
				skinPreview.transform.GetChild (12).gameObject.SetActive (true);
				skinPreview.transform.GetChild (13).localPosition = new Vector3 (69, -87, 0);
				skinPreview.transform.GetChild (13).rotation = new Quaternion (0, 0, 0, 0);
				skinPreview.transform.GetChild (14).localPosition = new Vector3 (-76, -67.3F, 0);
				skinPreview.transform.GetChild (14).rotation = new Quaternion (0, 0, 0, 0);
				//smile
				skinPreview.transform.GetChild (30).localPosition = new Vector3 (0, 0, -10000);
				skinPreview.transform.GetChild (31).localPosition = new Vector3 (0, 0, -10000);
				//bandage
				if (skinPreview.name == "skin4") {
					skinPreview.transform.GetChild (32).localPosition = new Vector3 (1, 7, 0);
					skinPreview.transform.GetChild (32).localScale = new Vector3 (1, 1, 1);
				}
			}
		}

	}

    void openCard(bool isPressed) {

		if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("card open") && !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("card open epic")) {
			if (transform.childCount > 4) {
				GetComponent<Animator> ().Play ("card open epic");
				transform.GetChild(6).GetComponent<AudioSource> ().Play ();
					
			} else GetComponent<Animator>().Play("card open");
			GetComponent<AudioSource> ().Play ();
            mBoosterClass.counterOpenCard++;
            if (mBoosterClass.counterOpenCard >= 5) {
            	transform.parent.parent.parent.parent.FindChild("exit open booster menu").localPosition = new Vector3(0, 0, -1);
				if (ctrProgressClass.progress["boosters"] > 0) transform.parent.parent.parent.parent.FindChild("button open booster").gameObject.SetActive(true);
                mBoosterClass.counterOpenCard = 0;
            }
        }
        
    }

	void openCardGift(bool isPressed) {
		
		if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("card open")) {
			GetComponent<Animator>().Play("card open");
			GetComponent<AudioSource> ().Play ();
			bool flag = true; 




			//перебор 3х карт, если хоть одна закрыта, то flag = false
			for (int i = 0; i < 3; i++) {
				if (!transform.parent.GetChild (i).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("card open") && transform.parent.GetChild (i).name != gameObject.name)
						flag = false;
			}

			if (flag) {
				transform.parent.parent.parent.GetChild(1).GetComponent<iClickClass>().functionPress = "closeMenu";
				transform.parent.parent.parent.GetChild(2).gameObject.SetActive(true);

			}
		}

	}

	void openCardDaily(bool isPressed) {
		if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("card open")) {
			GetComponent<Animator>().Play("card open");
			GetComponent<AudioSource> ().Play ();

			bool flag = true; 
			//перебор 3х карт, если это первый клик, то сохраняется результат
			for (int i = 0; i < 3; i++) {
				if (transform.parent.GetChild (i).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("card open"))
					flag = false;
			}

			if (flag) {

				//название карты и количество
				string bonusName = name.Split(new Char[] { '_' })[0];
				int bonusCount = int.Parse(name.Split(new Char[] { '_' })[1]); 

				//сохранение результата
				if (bonusName == "hints" || bonusName == "webs" || bonusName == "teleports" || bonusName == "collectors" || bonusName == "coins") ctrProgressClass.progress[bonusName] += bonusCount;
				else if (bonusName == "energy") {
					ctrProgressClass.progress["energyTime"] -= bonusCount * lsEnergyClass.costEnergy;
					ctrProgressClass.progress["energy"] += bonusCount;
				}
				else {
					if (ctrProgressClass.progress[bonusName] == 0) ctrProgressClass.progress[bonusName] = 1;
					else
						//если есть, начислять монеты. поменять количество и добавить всплывающую иконку монет.
						ctrProgressClass.progress["coins"] += 100;
				}

		
				ctrProgressClass.saveProgress();
				StartCoroutine (openCardDailyAnother());
			}
		}

	}

	private IEnumerator openCardDailyAnother(){
		yield return StartCoroutine(staticClass.waitForRealTime(0.7F));
		for (int i = 0; i < 3; i++) {
			transform.parent.GetChild (i).GetComponent<Animator>().Play("card open");
			GetComponent<AudioSource> ().Play ();

		}	
		//включаем exit daily menu
		transform.parent.parent.parent.GetChild(1).GetComponent<iClickClass>().functionPress = "closeMenu";
		transform.parent.parent.parent.GetChild(3).gameObject.SetActive(true);


	}
	private IEnumerator startCard(){
		yield return StartCoroutine(staticClass.waitForRealTime(UnityEngine.Random.value * 2));
		if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("card open") && !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("card open epic")) GetComponent<Animator> ().Play ("card idle");


	}



}
