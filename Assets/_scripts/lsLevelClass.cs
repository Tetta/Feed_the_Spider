using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class lsLevelClass : MonoBehaviour {

    //public GameObject spider;
    //public Transform cameraTransform;
    public GameObject islandInactive;
    public Material materialInactive;
    public Material materialDefault;
    public GameObject block;
    public Transform stonesGift;

    public int prevLevel = 0;

    //private float maxDistance = 3.5F;
    private int level;
    private bool flagBlock = false;
    // Use this for initialization
    public void Start() {
        flagBlock = false;
        level = int.Parse(gameObject.name.Substring(6));
        //levelLabel.text = level.ToString();
        if (ctrProgressClass.progress.Count == 0) ctrProgressClass.getProgress();
        int levelProgress = ctrProgressClass.progress["level" + level];
        int lastLevel = ctrProgressClass.progress["lastLevel"];
        var gem1Active = transform.GetChild(0).GetChild(0);
        var gem2Active = transform.GetChild(0).GetChild(1);


        //камни подарка
        if (stonesGift != null)
            if (level > lastLevel)
                for (int i = 0; i < stonesGift.childCount; i++) {
                    stonesGift.GetChild(i).GetComponent<SpriteRenderer>().material = materialInactive;
                }
        //убираем или нет блокировку
        if (block != null)
        {
            if (int.Parse(block.name.Substring(6)) <= ctrProgressClass.progress["gems"] || lastLevel >= level)
                block.SetActive(false);
            else flagBlock = true;
        }

        if (!((prevLevel == 0 && lastLevel + 1 >= level) || (prevLevel != 0 && lastLevel >= prevLevel)) || flagBlock)
        {

            islandInactive.SetActive(true);
            //белый остров
            transform.GetChild(1).GetComponent<SpriteRenderer>().material = materialInactive;
            //белые камни
            for (int i = 0; i < transform.GetChild(2).childCount; i++)
            {
                transform.GetChild(2).GetChild(i).GetComponent<SpriteRenderer>().material = materialInactive;
            }
            gem1Active.GetComponent<SpriteRenderer>().material = materialInactive;
            gem2Active.GetComponent<SpriteRenderer>().material = materialInactive;
        }
        else
        {
            islandInactive.SetActive(false);
            
            //обычный остров
            transform.GetChild(1).GetComponent<SpriteRenderer>().material = materialDefault;
            //обычные камни
            for (int i = 0; i < transform.GetChild(2).childCount; i++)
            {
                transform.GetChild(2).GetChild(i).GetComponent<SpriteRenderer>().material = materialDefault;
            }
            gem1Active.GetComponent<SpriteRenderer>().material = materialDefault;
            gem2Active.GetComponent<SpriteRenderer>().material = materialDefault;

        }



        //туториал для 2го уровня (добавляем)
        if (level == 7) {
			if (lastLevel == 6 && ctrProgressClass.progress ["currentLevel"] == 6) {
				GetComponent<Animator> ().enabled = true;
				//transform.GetChild (4).gameObject.SetActive (true);
			}
		}

        //Facebook Friends
        if (ctrProgressClass.progress["fb"] == 1 || ctrProgressClass.progress["vk"] == 1 || ctrProgressClass.progress["ok"] == 1)
            ctrFbKiiClass.setFriendImgMap("level" + level, transform.GetChild(0));



        StartCoroutine(keyFly(gem1Active, gem2Active, levelProgress, level));

    }


	void OnClick() {
        if (islandInactive.activeSelf)
	    {
            Debug.Log("click on inactive island");
            if (level - 1 == ctrProgressClass.progress["lastLevel"] && flagBlock)
                initLevelMenuClass.instance.unlockСhapterMenu.SetActive(true);
            else 
                initLevelMenuClass.instance.disableLevelMenu.SetActive(true);

        }
	    else
	    {

	        int lastLevel = ctrProgressClass.progress["lastLevel"];
	        if (ctrProgressClass.progress["currentLevel"] == level)
	        {
	            GameObject.Find("root/spider").transform.GetChild(0).GetComponent<Animator>().Play("spider jump");
	            GameObject.Find("root/spider").SendMessage("selectLevelMenuCorourine");

	        }
	        else
	        {
	            //разрешаем прыгать на все острова (раскомментить потом)
	            if ((prevLevel == 0 && lastLevel + 1 >= level) || (prevLevel != 0 && lastLevel >= prevLevel))
	            {
	                ctrProgressClass.progress["currentLevel"] = level;
	                ctrProgressClass.saveProgress();
	                //туториал для 2го уровня (убираем)
	                if (level == 2)
	                {
	                    if (GetComponent<Animator>().enabled == true)
	                    {
	                        GetComponent<Animator>().enabled = false;
	                        transform.GetChild(4).gameObject.SetActive(false);
	                    }
	                }
	                GameObject.Find("root/spider")
	                    .SendMessage("clickLevel",
	                        transform.GetChild(0).localPosition + transform.localPosition + transform.parent.localPosition);
	            }
	        }
	    }

	}

	/// <summary>
	/// Checks if a line between p0 to p1 intersects a line between p2 and p3
	///
	/// returns the point of intersection
	/// </summary>
	public static Vector2 lineIntersectPos(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
	{
		float s1_x, s1_y, s2_x, s2_y;
		s1_x = p1.x - p0.x;
		s1_y = p1.y - p0.y;
		s2_x = p3.x - p2.x;
		s2_y = p3.y - p2.y;
		float s, t;
		s = (-s1_y * (p0.x - p2.x) + s1_x * (p0.y - p2.y)) / (-s2_x * s1_y + s1_x * s2_y);
		t = (s2_x * (p0.y - p2.y) - s2_y * (p0.x - p2.x)) / (-s2_x * s1_y + s1_x * s2_y);
		if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
		{
			// Collision detected
			return new Vector2(p0.x + (t * s1_x), p0.y + (t * s1_y));
		}
		return new Vector2(10000, 10000); // No collision
	}

    private IEnumerator keyFly(Transform key1, Transform key2, int levelProgress, int level)
    {
        if (staticClass.flyingKeys.Contains("level" + level + "_key1"))
        {
            yield return StartCoroutine(staticClass.waitForRealTime(0.1F));
            var startPosKey1 = key1.position;

            var keysPos = initLevelMenuClass.instance.gemsLabel.transform.position ;
            for (var i = 0; i < 20; i++)
            {
                key1.position = key1.position + (keysPos - startPosKey1)/20;
                yield return StartCoroutine(staticClass.waitForRealTime(0.01F));
            }
            staticClass.flyingKeys.Remove("level" + level + "_key1");
        }
        if (staticClass.flyingKeys.Contains("level" + level + "_key2"))
        {
            yield return StartCoroutine(staticClass.waitForRealTime(0.1F));
            var startPosKey2 = key2.position;

            var keysPos = initLevelMenuClass.instance.gemsLabel.transform.position ;
            for (var i = 0; i < 20; i++)
            {
                key2.position = key2.position + (keysPos - startPosKey2) / 20;
                yield return StartCoroutine(staticClass.waitForRealTime(0.01F));
            }
            staticClass.flyingKeys.Remove("level" + level + "_key2");
        }

        if (levelProgress == 1 || levelProgress == 3)
        {
            key1.gameObject.SetActive(false);
        }
        if ((levelProgress == 2 || levelProgress == 3) || level < 5)
        {
            key2.gameObject.SetActive(false);


        }
    }
}
