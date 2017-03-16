using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Facebook.Unity;

public class ctrProgressClass {
	static public Dictionary<string, int> progress = new Dictionary<string, int>();
    static public string progressSaved = "";

    static public void saveProgress() {
        //progress["level6"] = 0;
        //progress["score6_1"] = 0;
        //progress["score6_2"] = 0;

        string strProgress = "";
		foreach (var item in progress ) {
			if (progressDefault.ContainsKey(item.Key)) if (progress[item.Key] != progressDefault[item.Key]) strProgress += item.Key + "=" + item.Value + ";";
		}

        if (progressSaved != strProgress)
        {
            Debug.Log("saveProgress");
            PlayerPrefs.SetString("progress", strProgress);
            PlayerPrefs.Save();
            progressSaved = strProgress;
        }
    }

	static public void getProgress() {

        string strProgress = PlayerPrefs.GetString("progress");
	    progressSaved = strProgress;

        if (strProgress == "")
	    {
            //send analytics dimentions
            ctrAnalyticsClass.sendCustomDimension(5, "1"); //sessionCount
            ctrAnalyticsClass.sendCustomDimension(4, "1"); //level
            ctrAnalyticsClass.sendCustomDimension(2, "0"); //paymentCount
            ctrAnalyticsClass.sendCustomDimension(3, "0"); //revenue
        }

        progress = new Dictionary<string, int> (progressDefault);

		string strKey = "", strValue = "";
		bool flag = true;
		for (int i = 0; i < strProgress.Length; i++) {
			if (strProgress.Substring(i, 1) == "=") flag = false;

			else if (strProgress.Substring(i, 1) == ";") {
				flag = true;
				progress[strKey] = int.Parse(strValue);
                if (strKey == "saleDate") Debug.Log(strValue);
                //Debug.Log(strKey + ": " + strValue);
                //скины и шапки. запись в статик переменную
                if (strKey == "skinCurrent") staticClass.currentSkin = "skin" + strValue;
                if (strKey == "berryCurrent") staticClass.currentBerry = "berry" + strValue;
                if (strKey == "hatCurrent") staticClass.currentHat = "hat" + strValue;
				strKey = "";
				strValue = "";
			} else if (flag) strKey += strProgress.Substring(i, 1);
			else if (!flag) strValue += strProgress.Substring(i, 1);

		}


    }


    static private Dictionary<string, int> progressDefault = new Dictionary<string, int>{
		{"googlePlay",0}, {"lastLevel",0}, {"currentLevel",1},{"coins",100},{"gems",0},{"energyTime",0},{"energy",0},{"energyInfinity",0},
        {"hints",0},{"webs",0},{"collectors",0},{"teleports",0},{"complect",0},{"music",1},{"sound",1},{"dailyBonus",0},{"language",0},
		{"everyplay",0},{"firstPurchase",0},{"fb",0},{"vk",0},{"ok",0},
        {"boostersWhite",0 }, {"boostersGreen",0 }, {"boostersBlue",0 }, {"boostersPurple",0 },
        {"berryRare", UnityEngine.Random.Range(2, 6)}, {"hatRare", UnityEngine.Random.Range(2, 6)},{"skinRare", UnityEngine.Random.Range(2, 6)},

        {"sale", 0},{"saleDate", 0}, {"adCoinsDate", 0},

        {"berry1",1},{"berry2",0},{"berry3",0},{"berry4",0},{"berry5",0},
		{"hat1",1},{"hat2",0},{"hat3",0},{"hat4",0},{"hat5",0},
		{"skin1",1},{"skin2",0},{"skin3",0},{"skin4",0},{"skin5",0},
        {"berryCurrent", 1}, {"hatCurrent", 1}, {"skinCurrent", 1},

        {"tutorialEnergy",1}, {"tutorialBuy",0}, {"tutorialHint",0}, {"tutorialBonus",0}, {"tutorialDream",0},  {"tutorialAdCoins",0},  {"tutorialSale",0}, {"tutorialMap",0},

        {"sessionStart",0}, {"sessionEnd",0}, {"sessionCount",0}, {"levelPlayCount",0}, {"winCount",0}, {"firstLaunch", 0}, {"paymentCount", 0}, {"revenue", 0},

        {"level1",0},{"level2",0},{"level3",0},{"level4",0},{"level5",0},{"level6",0},{"level7",0},{"level8",0},{"level9",0},{"level10",0},
		{"level11",0},{"level12",0},{"level13",0},{"level14",0},{"level15",0},{"level16",0},{"level17",0},{"level18",0},{"level19",0},{"level20",0},
		{"level21",0},{"level22",0},{"level23",0},{"level24",0},{"level25",0},{"level26",0},{"level27",0},{"level28",0},{"level29",0},{"level30",0},
		{"level31",0},{"level32",0},{"level33",0},{"level34",0},{"level35",0},{"level36",0},{"level37",0},{"level38",0},{"level39",0},{"level40",0},
		{"level41",0},{"level42",0},{"level43",0},{"level44",0},{"level45",0},{"level46",0},{"level47",0},{"level48",0},{"level49",0},{"level50",0},
		{"level51",0},{"level52",0},{"level53",0},{"level54",0},{"level55",0},{"level56",0},{"level57",0},{"level58",0},{"level59",0},{"level60",0},
		{"level61",0},{"level62",0},{"level63",0},{"level64",0},{"level65",0},{"level66",0},{"level67",0},{"level68",0},{"level69",0},{"level70",0},
		{"level71",0},{"level72",0},{"level73",0},{"level74",0},{"level75",0},{"level76",0},{"level77",0},{"level78",0},{"level79",0},{"level80",0},
		{"level81",0},{"level82",0},{"level83",0},{"level84",0},{"level85",0},{"level86",0},{"level87",0},{"level88",0},{"level89",0},{"level90",0},
		{"level91",0},{"level92",0},{"level93",0},{"level94",0},{"level95",0},{"level96",0},{"level97",0},{"level98",0},{"level99",0},{"level100",0},

		{"score1_1",0},{"score1_2",0},{"score2_1",0},{"score2_2",0},{"score3_1",0},{"score3_2",0},{"score4_1",0},{"score4_2",0},{"score5_1",0},{"score5_2",0},
		{"score6_1",0},{"score6_2",0},{"score7_1",0},{"score7_2",0},{"score8_1",0},{"score8_2",0},{"score9_1",0},{"score9_2",0},{"score10_1",0},{"score10_2",0},
		{"score11_1",0},{"score11_2",0},{"score12_1",0},{"score12_2",0},{"score13_1",0},{"score13_2",0},{"score14_1",0},{"score14_2",0},{"score15_1",0},{"score15_2",0},
		{"score16_1",0},{"score16_2",0},{"score17_1",0},{"score17_2",0},{"score18_1",0},{"score18_2",0},{"score19_1",0},{"score19_2",0},{"score20_1",0},{"score20_2",0},
		{"score21_1",0},{"score21_2",0},{"score22_1",0},{"score22_2",0},{"score23_1",0},{"score23_2",0},{"score24_1",0},{"score24_2",0},{"score25_1",0},{"score25_2",0},
		{"score26_1",0},{"score26_2",0},{"score27_1",0},{"score27_2",0},{"score28_1",0},{"score28_2",0},{"score29_1",0},{"score29_2",0},{"score30_1",0},{"score30_2",0},
		{"score31_1",0},{"score31_2",0},{"score32_1",0},{"score32_2",0},{"score33_1",0},{"score33_2",0},{"score34_1",0},{"score34_2",0},{"score35_1",0},{"score35_2",0},
		{"score36_1",0},{"score36_2",0},{"score37_1",0},{"score37_2",0},{"score38_1",0},{"score38_2",0},{"score39_1",0},{"score39_2",0},{"score40_1",0},{"score40_2",0},
		{"score41_1",0},{"score41_2",0},{"score42_1",0},{"score42_2",0},{"score43_1",0},{"score43_2",0},{"score44_1",0},{"score44_2",0},{"score45_1",0},{"score45_2",0},
		{"score46_1",0},{"score46_2",0},{"score47_1",0},{"score47_2",0},{"score48_1",0},{"score48_2",0},{"score49_1",0},{"score49_2",0},{"score50_1",0},{"score50_2",0},
		{"score51_1",0},{"score51_2",0},{"score52_1",0},{"score52_2",0},{"score53_1",0},{"score53_2",0},{"score54_1",0},{"score54_2",0},{"score55_1",0},{"score55_2",0},
		{"score56_1",0},{"score56_2",0},{"score57_1",0},{"score57_2",0},{"score58_1",0},{"score58_2",0},{"score59_1",0},{"score59_2",0},{"score60_1",0},{"score60_2",0},
		{"score61_1",0},{"score61_2",0},{"score62_1",0},{"score62_2",0},{"score63_1",0},{"score63_2",0},{"score64_1",0},{"score64_2",0},{"score65_1",0},{"score65_2",0},
		{"score66_1",0},{"score66_2",0},{"score67_1",0},{"score67_2",0},{"score68_1",0},{"score68_2",0},{"score69_1",0},{"score69_2",0},{"score70_1",0},{"score70_2",0},
		{"score71_1",0},{"score71_2",0},{"score72_1",0},{"score72_2",0},{"score73_1",0},{"score73_2",0},{"score74_1",0},{"score74_2",0},{"score75_1",0},{"score75_2",0},
		{"score76_1",0},{"score76_2",0},{"score77_1",0},{"score77_2",0},{"score78_1",0},{"score78_2",0},{"score79_1",0},{"score79_2",0},{"score80_1",0},{"score80_2",0},
		{"score81_1",0},{"score81_2",0},{"score82_1",0},{"score82_2",0},{"score83_1",0},{"score83_2",0},{"score84_1",0},{"score84_2",0},{"score85_1",0},{"score85_2",0},
		{"score86_1",0},{"score86_2",0},{"score87_1",0},{"score87_2",0},{"score88_1",0},{"score88_2",0},{"score89_1",0},{"score89_2",0},{"score90_1",0},{"score90_2",0},
		{"score91_1",0},{"score91_2",0},{"score92_1",0},{"score92_2",0},{"score93_1",0},{"score93_2",0},{"score94_1",0},{"score94_2",0},{"score95_1",0},{"score95_2",0},
		{"score96_1",0},{"score96_2",0},{"score97_1",0},{"score97_2",0},{"score98_1",0},{"score98_2",0},{"score99_1",0},{"score99_2",0},{"score100_1",0},{"score100_2",0},

		{"gift7",0},{"gift8",0},{"gift12",0},{"gift19",0},{"gift20",0},{"gift28",0},{"gift31",0},{"gift45",0},{"gift48",0},
		{"gift55",0},{"gift57",0},{"gift65",0},{"gift72",0},{"gift79",0},{"gift81",0},{"gift83",0},{"gift87",0},{"gift94",0},{"gift95",0},

        //0 нет, 1 первый, 2 второй, 3 оба
        { "level1_dream",0},{"level2_dream",0},{"level3_dream",0},{"level4_dream",0},{"level5_dream",0},{"level6_dream",0},{"level7_dream",0},{"level8_dream",0},{"level9_dream",0},{"level10_dream",0},
        {"level11_dream",0},{"level12_dream",0},{"level13_dream",0},{"level14_dream",0},{"level15_dream",0},{"level16_dream",0},{"level17_dream",0},{"level18_dream",0},{"level19_dream",0},{"level20_dream",0},
        {"level21_dream",0},{"level22_dream",0},{"level23_dream",0},{"level24_dream",0},{"level25_dream",0},{"level26_dream",0},{"level27_dream",0},{"level28_dream",0},{"level29_dream",0},{"level30_dream",0},
        {"level31_dream",0},{"level32_dream",0},{"level33_dream",0},{"level34_dream",0},{"level35_dream",0},{"level36_dream",0},{"level37_dream",0},{"level38_dream",0},{"level39_dream",0},{"level40_dream",0},
        {"level41_dream",0},{"level42_dream",0},{"level43_dream",0},{"level44_dream",0},{"level45_dream",0},{"level46_dream",0},{"level47_dream",0},{"level48_dream",0},{"level49_dream",0},{"level50_dream",0},
        {"level51_dream",0},{"level52_dream",0},{"level53_dream",0},{"level54_dream",0},{"level55_dream",0},{"level56_dream",0},{"level57_dream",0},{"level58_dream",0},{"level59_dream",0},{"level60_dream",0},
        {"level61_dream",0},{"level62_dream",0},{"level63_dream",0},{"level64_dream",0},{"level65_dream",0},{"level66_dream",0},{"level67_dream",0},{"level68_dream",0},{"level69_dream",0},{"level70_dream",0},
        {"level71_dream",0},{"level72_dream",0},{"level73_dream",0},{"level74_dream",0},{"level75_dream",0},{"level76_dream",0},{"level77_dream",0},{"level78_dream",0},{"level79_dream",0},{"level80_dream",0},
        {"level81_dream",0},{"level82_dream",0},{"level83_dream",0},{"level84_dream",0},{"level85_dream",0},{"level86_dream",0},{"level87_dream",0},{"level88_dream",0},{"level89_dream",0},{"level90_dream",0},
        {"level91_dream",0},{"level92_dream",0},{"level93_dream",0},{"level94_dream",0},{"level95_dream",0},{"level96_dream",0},{"level97_dream",0},{"level98_dream",0},{"level99_dream",0},{"level100_dream",0}

    };
	static private Dictionary<string, int> progressCheat = new Dictionary<string, int>{
		{"googlePlay",0}, {"lastLevel",99}, {"currentLevel",1},{"coins",10000},{"gems",200},{"energyTime", 0},{"energy",4}, {"energyInfinity", 0},
        {"hints",99},{"webs",99},{"collectors",99},{"teleports",99},{"complect",0},{"music",1},{"sound",1},{"dailyBonus",0},{"language",0},
		{"everyplay",0},{"firstPurchase",1},{"fb",0},{"vk",0},{"ok",0},

        {"boostersWhite",0 }, {"boostersGreen",220 }, {"boostersBlue",330 }, {"boostersPurple",110 },
        {"berryRare", 2 }, {"hatRare",2},{"skinRare", 4},

        {"sale", 0},{"saleDate", 0}, {"adCoinsDate", 0},

        {"berry1",1},{"berry2",2},{"berry3",1},{"berry4",5},{"berry5",1},
        {"hat1",1},{"hat2",1},{"hat3",1},{"hat4",4},{"hat5",0},
        {"skin1",1},{"skin2",50},{"skin3",2},{"skin4",12},{"skin5",1},
        {"berryCurrent", 1}, {"hatCurrent", 1}, {"skinCurrent", 1},

        {"tutorialEnergy",1}, {"tutorialBuy",0}, {"tutorialHint",0}, {"tutorialBonus",0}, {"tutorialDream",0},  {"tutorialAdCoins",0},  {"tutorialSale",0}, {"tutorialMap",0},

        { "sessionStart",0}, {"sessionEnd",0}, {"sessionCount",0}, {"levelPlayCount",0}, {"winCount",0}, {"firstLaunch", 0}, {"paymentCount", 0}, {"revenue", 0},

        {"level1",3},{"level2",3},{"level3",3},{"level4",3},{"level5",3},{"level6",3},{"level7",3},{"level8",3},{"level9",3},{"level10",3},
		{"level11",3},{"level12",3},{"level13",3},{"level14",3},{"level15",3},{"level16",3},{"level17",3},{"level18",3},{"level19",3},{"level20",3},
		{"level21",3},{"level22",3},{"level23",3},{"level24",3},{"level25",3},{"level26",3},{"level27",3},{"level28",3},{"level29",3},{"level30",3},
		{"level31",3},{"level32",3},{"level33",3},{"level34",3},{"level35",3},{"level36",3},{"level37",3},{"level38",3},{"level39",3},{"level40",3},
		{"level41",3},{"level42",3},{"level43",3},{"level44",3},{"level45",3},{"level46",3},{"level47",3},{"level48",3},{"level49",3},{"level50",3},
		{"level51",3},{"level52",3},{"level53",3},{"level54",3},{"level55",3},{"level56",3},{"level57",3},{"level58",3},{"level59",3},{"level60",3},
		{"level61",3},{"level62",3},{"level63",3},{"level64",3},{"level65",3},{"level66",3},{"level67",3},{"level68",3},{"level69",3},{"level70",3},
		{"level71",3},{"level72",3},{"level73",3},{"level74",3},{"level75",3},{"level76",3},{"level77",3},{"level78",3},{"level79",3},{"level80",3},
		{"level81",3},{"level82",3},{"level83",3},{"level84",3},{"level85",3},{"level86",3},{"level87",3},{"level88",3},{"level89",3},{"level90",3},
		{"level91",3},{"level92",3},{"level93",3},{"level94",3},{"level95",3},{"level96",3},{"level97",3},{"level98",3},{"level99",3},{"level100",3},

		{"score1_1",0},{"score1_2",0},{"score2_1",0},{"score2_2",0},{"score3_1",0},{"score3_2",0},{"score4_1",0},{"score4_2",0},{"score5_1",0},{"score5_2",0},
		{"score6_1",0},{"score6_2",0},{"score7_1",0},{"score7_2",0},{"score8_1",0},{"score8_2",0},{"score9_1",0},{"score9_2",0},{"score10_1",0},{"score10_2",0},
		{"score11_1",0},{"score11_2",0},{"score12_1",0},{"score12_2",0},{"score13_1",0},{"score13_2",0},{"score14_1",0},{"score14_2",0},{"score15_1",0},{"score15_2",0},
		{"score16_1",0},{"score16_2",0},{"score17_1",0},{"score17_2",0},{"score18_1",0},{"score18_2",0},{"score19_1",0},{"score19_2",0},{"score20_1",0},{"score20_2",0},
		{"score21_1",0},{"score21_2",0},{"score22_1",0},{"score22_2",0},{"score23_1",0},{"score23_2",0},{"score24_1",0},{"score24_2",0},{"score25_1",0},{"score25_2",0},
		{"score26_1",0},{"score26_2",0},{"score27_1",0},{"score27_2",0},{"score28_1",0},{"score28_2",0},{"score29_1",0},{"score29_2",0},{"score30_1",0},{"score30_2",0},
		{"score31_1",0},{"score31_2",0},{"score32_1",0},{"score32_2",0},{"score33_1",0},{"score33_2",0},{"score34_1",0},{"score34_2",0},{"score35_1",0},{"score35_2",0},
		{"score36_1",0},{"score36_2",0},{"score37_1",0},{"score37_2",0},{"score38_1",0},{"score38_2",0},{"score39_1",0},{"score39_2",0},{"score40_1",0},{"score40_2",0},
		{"score41_1",0},{"score41_2",0},{"score42_1",0},{"score42_2",0},{"score43_1",0},{"score43_2",0},{"score44_1",0},{"score44_2",0},{"score45_1",0},{"score45_2",0},
		{"score46_1",0},{"score46_2",0},{"score47_1",0},{"score47_2",0},{"score48_1",0},{"score48_2",0},{"score49_1",0},{"score49_2",0},{"score50_1",0},{"score50_2",0},
		{"score51_1",0},{"score51_2",0},{"score52_1",0},{"score52_2",0},{"score53_1",0},{"score53_2",0},{"score54_1",0},{"score54_2",0},{"score55_1",0},{"score55_2",0},
		{"score56_1",0},{"score56_2",0},{"score57_1",0},{"score57_2",0},{"score58_1",0},{"score58_2",0},{"score59_1",0},{"score59_2",0},{"score60_1",0},{"score60_2",0},
		{"score61_1",0},{"score61_2",0},{"score62_1",0},{"score62_2",0},{"score63_1",0},{"score63_2",0},{"score64_1",0},{"score64_2",0},{"score65_1",0},{"score65_2",0},
		{"score66_1",0},{"score66_2",0},{"score67_1",0},{"score67_2",0},{"score68_1",0},{"score68_2",0},{"score69_1",0},{"score69_2",0},{"score70_1",0},{"score70_2",0},
		{"score71_1",0},{"score71_2",0},{"score72_1",0},{"score72_2",0},{"score73_1",0},{"score73_2",0},{"score74_1",0},{"score74_2",0},{"score75_1",0},{"score75_2",0},
		{"score76_1",0},{"score76_2",0},{"score77_1",0},{"score77_2",0},{"score78_1",0},{"score78_2",0},{"score79_1",0},{"score79_2",0},{"score80_1",0},{"score80_2",0},
		{"score81_1",0},{"score81_2",0},{"score82_1",0},{"score82_2",0},{"score83_1",0},{"score83_2",0},{"score84_1",0},{"score84_2",0},{"score85_1",0},{"score85_2",0},
		{"score86_1",0},{"score86_2",0},{"score87_1",0},{"score87_2",0},{"score88_1",0},{"score88_2",0},{"score89_1",0},{"score89_2",0},{"score90_1",0},{"score90_2",0},
		{"score91_1",0},{"score91_2",0},{"score92_1",0},{"score92_2",0},{"score93_1",0},{"score93_2",0},{"score94_1",0},{"score94_2",0},{"score95_1",0},{"score95_2",0},
		{"score96_1",0},{"score96_2",0},{"score97_1",0},{"score97_2",0},{"score98_1",0},{"score98_2",0},{"score99_1",0},{"score99_2",0},{"score100_1",0},{"score100_2",0},

        {"gift7",0},{"gift8",0},{"gift12",0},{"gift19",0},{"gift20",0},{"gift28",0},{"gift31",0},{"gift45",0},{"gift48",0},
        {"gift55",0},{"gift57",0},{"gift65",0},{"gift72",0},{"gift79",0},{"gift81",0},{"gift83",0},{"gift87",0},{"gift94",0},{"gift95",0},
                
        //0 нет, 1 первый, 2 второй, 3 оба
        { "level1_dream",3},{"level2_dream",3},{"level3_dream",3},{"level4_dream",3},{"level5_dream",3},{"level6_dream",3},{"level7_dream",3},{"level8_dream",3},{"level9_dream",3},{"level10_dream",3},
        {"level11_dream",3},{"level12_dream",3},{"level13_dream",3},{"level14_dream",3},{"level15_dream",3},{"level16_dream",3},{"level17_dream",3},{"level18_dream",3},{"level19_dream",3},{"level20_dream",3},
        {"level21_dream",3},{"level22_dream",3},{"level23_dream",3},{"level24_dream",3},{"level25_dream",3},{"level26_dream",3},{"level27_dream",3},{"level28_dream",3},{"level29_dream",3},{"level30_dream",3},
        {"level31_dream",3},{"level32_dream",3},{"level33_dream",3},{"level34_dream",3},{"level35_dream",3},{"level36_dream",3},{"level37_dream",3},{"level38_dream",3},{"level39_dream",3},{"level40_dream",3},
        {"level41_dream",3},{"level42_dream",3},{"level43_dream",3},{"level44_dream",3},{"level45_dream",3},{"level46_dream",3},{"level47_dream",3},{"level48_dream",3},{"level49_dream",3},{"level50_dream",3},
        {"level51_dream",3},{"level52_dream",3},{"level53_dream",3},{"level54_dream",3},{"level55_dream",3},{"level56_dream",3},{"level57_dream",3},{"level58_dream",3},{"level59_dream",3},{"level60_dream",3},
        {"level61_dream",3},{"level62_dream",3},{"level63_dream",3},{"level64_dream",3},{"level65_dream",3},{"level66_dream",3},{"level67_dream",3},{"level68_dream",3},{"level69_dream",3},{"level70_dream",3},
        {"level71_dream",3},{"level72_dream",3},{"level73_dream",3},{"level74_dream",3},{"level75_dream",3},{"level76_dream",3},{"level77_dream",3},{"level78_dream",3},{"level79_dream",3},{"level80_dream",3},
        {"level81_dream",3},{"level82_dream",3},{"level83_dream",3},{"level84_dream",3},{"level85_dream",3},{"level86_dream",3},{"level87_dream",3},{"level88_dream",3},{"level89_dream",3},{"level90_dream",3},
        {"level91_dream",3},{"level92_dream",3},{"level93_dream",3},{"level94_dream",3},{"level95_dream",3},{"level96_dream",3},{"level97_dream",3},{"level98_dream",3},{"level99_dream",3},{"level100_dream",3}
    };
	static public void resetProgress(string nameButton) {
        //save session vars
	    var sStart = progress["sessionStart"];
        var sEnd = progress["sessionEnd"];
        var sCount = progress["sessionCount"];
        var firstLaunch = progress["firstLaunch"];
        var vk = progress["vk"];
        var fb = progress["fb"];

        //сброс прогресса
        progress = new Dictionary<string, int> (progressDefault);
		if (nameButton == "reset cheat") progress = new Dictionary<string, int> (progressCheat);

	    progress["sessionStart"] = sStart;
        progress["sessionEnd"] = sEnd;
        progress["sessionCount"] = sCount;
        progress["firstLaunch"] = firstLaunch;
        progress["vk"] = vk;
        progress["fb"] = fb;
        progress["dailyBonus"] = (int)DateTime.UtcNow.TotalSeconds();

        marketClass.instance.boostersLabel[0].text = progress["boostersWhite"].ToString();
        marketClass.instance.boostersLabel[1].text = progress["boostersGreen"].ToString();
        marketClass.instance.boostersLabel[2].text = progress["boostersBlue"].ToString();
        marketClass.instance.boostersLabel[3].text = progress["boostersPurple"].ToString();
	    staticClass.currentBerry = "berry1";
        staticClass.changeBerry();
        staticClass.currentHat = "hat1";
        staticClass.changeHat();
        staticClass.currentSkin = "skin1";
        staticClass.changeSkin();

        saveProgress();
	}

}

