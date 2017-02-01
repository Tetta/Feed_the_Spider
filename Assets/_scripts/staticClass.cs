using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

	/*
		stars = 0 - 3;
		--------------
		none = 0;
		time = 1 - 99;
		web = 100 - 199;
		sluggish = 201;
		destroyer = 202;
		yeti = 203;
		groot = 204;
	*/

public class staticClass
{

    //public static staticClass instance = new staticClass();

    //public static List<level> levels = new List<level>();
    public static int[,] levels = new int[101, 2];
    public static int[] levelBlocks = new int[101];


    public static void initLevels()
    {
        //всегда есть stars [0] + что-то еще [1]
        levels[1, 0] = 1;
        levels[1, 1] = 10;
        levels[2, 0] = 1;
        levels[2, 1] = 10;
        levels[3, 0] = 1;
        levels[3, 1] = 10;
        levels[4, 0] = 1;
        levels[4, 1] = 10;
        levels[5, 0] = 2;
        levels[5, 1] = 103;
        levels[6, 0] = 2;
        levels[6, 1] = 14;
        levels[7, 0] = 3;
        levels[7, 1] = 7;
        levels[8, 0] = 1;
        levels[8, 1] = 11;
        levels[9, 0] = 2;
        levels[9, 1] = 102;
        levels[10, 0] = 0;
        levels[10, 1] = 4;
        levels[11, 0] = 2;
        levels[11, 1] = 102;
        levels[12, 0] = 1;
        levels[12, 1] = 103;
        levels[13, 0] = 0;
        levels[13, 1] = 8;
        levels[14, 0] = 1;
        levels[14, 1] = 0;
        levels[15, 0] = 1;
        levels[15, 1] = 9;
        levels[16, 0] = 2;
        levels[16, 1] = 3;
        levels[17, 0] = 2;
        levels[17, 1] = 0;
        levels[18, 0] = 2;
        levels[18, 1] = 10;
        levels[19, 0] = 2;
        levels[19, 1] = 8;
        levels[20, 0] = 1;
        levels[20, 1] = 14;
        levels[21, 0] = 1;
        levels[21, 1] = 7;
        levels[22, 0] = 1;
        levels[22, 1] = 8;
        levels[23, 0] = 2;
        levels[23, 1] = 7;
        levels[24, 0] = 0;
        levels[24, 1] = 9;
        levels[25, 0] = 1;
        levels[25, 1] = 201;
        //----------------------------
        levels[26, 0] = 0;
        levels[26, 1] = 5;

		levels[27, 0] = 0;
		levels[27, 1] = 7;
		levels[28, 0] = 2;
		levels[28, 1] = 6;
		levels[29, 0] = 2;
		levels[29, 1] = 8;
		levels[30, 0] = 1;
		levels[30, 1] = 102;
		levels[31, 0] = 2;
		levels[31, 1] = 7;
		levels[32, 0] = 0;	// сложно возможно
		levels[32, 1] = 12;
		levels[33, 0] = 1;
		levels[33, 1] = 6;	
		levels[34, 0] = 2;
		levels[34, 1] = 15;
		levels[35, 0] = 2;
		levels[35, 1] = 101;
		levels[36, 0] = 0;
		levels[36, 1] = 10;

        levels[37, 0] = 2;
        levels[37, 1] = 0;
        levels[38, 0] = 1;
        levels[38, 1] = 7;
        levels[39, 0] = 0;
        levels[39, 1] = 6;
        levels[40, 0] = 3;

		levels[40, 1] = 8;
		levels[41, 0] = 2;
		levels[41, 1] = 4;
		levels[42, 0] = 3;
		levels[42, 1] = 204;
		levels[43, 0] = 1;
		levels[43, 1] = 8;
		levels[44, 0] = 0;
		levels[44, 1] = 11;
		levels[45, 0] = 2;
		levels[45, 1] = 204;
		levels[46, 0] = 1;
		levels[46, 1] = 204;
		levels[47, 0] = 1;
		levels[47, 1] = 7;
		levels[48, 0] = 2;
		levels[48, 1] = 9;
		levels[49, 0] = 3;
		levels[49, 1] = 102;
		levels[50, 0] = 1;
		levels[50, 1] = 9;
		//----------------------------
        levels[51, 0] = 3;	
        levels[51, 1] = 16;
		levels[52, 0] = 0;
		levels[52, 1] = 15;
		levels[53, 0] = 3;
		levels[53, 1] = 14;
		levels[54, 0] = 1;
		levels[54, 1] = 101;
		levels[55, 0] = 3;
		levels[55, 1] = 14;
		levels[56, 0] = 3;
		levels[56, 1] = 16;
		levels[57, 0] = 3;
		levels[57, 1] = 204;
		levels[58, 0] = 2;
		levels[58, 1] = 103;
		levels[59, 0] = 3;
		levels[59, 1] = 21;
		levels[60, 0] = 2;
		levels[60, 1] = 5;
		levels[61, 0] = 2;
		levels[61, 1] = 106;
		levels[62, 0] = 1;
		levels[62, 1] = 101;
		levels[63, 0] = 1;
		levels[63, 1] = 9;
		levels[64, 0] = 1;
		levels[64, 1] = 201;
		levels[65, 0] = 2;
		levels[65, 1] = 8;
		levels[66, 0] = 3;
		levels[66, 1] = 104;
		levels[67, 0] = 1;
		levels[67, 1] = 9;
		levels[68, 0] = 3;
		levels[68, 1] = 13;
		levels[69, 0] = 2;
		levels[69, 1] = 12;
		levels[70, 0] = 2;
		levels[70, 1] = 18;
		levels[71, 0] = 3;
		levels[71, 1] = 104;
		levels[72, 0] = 2;
		levels[72, 1] = 13;
		levels[73, 0] = 2;
		levels[73, 1] = 100;
		levels[74, 0] = 1;
		levels[74, 1] = 9;
		levels[75, 0] = 0;
		levels[75, 1] = 6;
		//----------------------------
		levels[76, 0] = 2;
		levels[76, 1] = 10;
		levels[77, 0] = 3;
		levels[77, 1] = 12;
		levels[78, 0] = 3;
		levels[78, 1] = 201;
		levels[79, 0] = 2;
		levels[79, 1] = 7;
		levels[80, 0] = 0;
		levels[80, 1] = 8;
		levels[81, 0] = 3;
		levels[81, 1] = 12;
		levels[82, 0] = 3;
		levels[82, 1] = 10;
		levels[83, 0] = 3;
		levels[83, 1] = 19;
		levels[84, 0] = 3;
		levels[84, 1] = 15;
		levels[85, 0] = 1;
		levels[85, 1] = 11;
		levels[86, 0] = 2;
		levels[86, 1] = 9;
		levels[87, 0] = 2;
		levels[87, 1] = 6;
		levels[88, 0] = 1;
		levels[88, 1] = 100;
		levels[89, 0] = 3;
		levels[89, 1] = 13;
		levels[90, 0] = 1;
		levels[90, 1] = 8;
		levels[91, 0] = 3;
		levels[91, 1] = 204;
		levels[92, 0] = 3;
		levels[92, 1] = 11;
		levels[93, 0] = 1;
		levels[93, 1] = 0;
		levels[94, 0] = 0;
		levels[94, 1] = 9;
		levels[95, 0] = 1;
		levels[95, 1] = 7;
		levels[96, 0] = 1;
		levels[96, 1] = 12;
		levels[97, 0] = 3;
		levels[97, 1] = 12;
		levels[98, 0] = 2;
		levels[98, 1] = 5;
		levels[99, 0] = 1;
		levels[99, 1] = 9;
		levels[100, 0] = 2;
		levels[100, 1] = 11;




        levelBlocks[5] = 3;

    }

    public static int useWeb = 0;
    public static int timer = 0;
    public static bool useSluggish = false;
    public static bool useDestroyer = false;
    public static bool useYeti = false;
    public static bool useGroot = false;
    public static bool loadAd = false;

    public static int testCounter = 0;


    //level1 = 0-3: 0-3 звезд
    //level1 = 4: пройдено испытание
    public static string currentSkin = "skin1";
    public static string currentHat = "hat1";
    public static string currentBerry = "berry1";

    public static string strProgressDefault =
        "googlePlay=0;lastLevel=0;currentLevel=1;coins=0;gems=0;energyTime=0;energy=30;" +

        "boosters=0;hints=0;webs=0;collectors=0;teleports=0;complect=0;music=1;sound=1;dailyBonus=0;language=0;tutorialBuy=0;everyplay=1;" +


        "berry1=2;berry2=0;berry3=0;berry4=0;berry5=0;" +
        "hat1=2;hat2=0;hat3=0;hat4=0;hat5=0;" +
        "skin1=2;skin2=0;skin3=0;skin4=0;skin5=0;" +
        "level1=0;level2=0;level3=0;level4=0;level5=0;level6=0;level7=0;level8=0;level9=0;level10=0;" +
        "level11=0;level12=0;level13=0;level14=0;level15=0;level16=0;level17=0;level18=0;level19=0;level20=0;" +
        "level21=0;level22=0;level23=0;level24=0;level25=0;level26=0;level27=0;level28=0;level29=0;level30=0;" +
        "level31=0;level32=0;level33=0;level34=0;level35=0;level36=0;level37=0;level38=0;level39=0;level40=0;" +
        "level41=0;level42=0;level43=0;level44=0;level45=0;level46=0;level47=0;level48=0;level49=0;level50=0;" +
        "level51=0;level52=0;level53=0;level54=0;level55=0;level56=0;level57=0;level58=0;level59=0;level60=0;" +
        "level61=0;level62=0;level63=0;level64=0;level65=0;level66=0;level67=0;level68=0;level69=0;level70=0;" +
        "level71=0;level72=0;level73=0;level74=0;level75=0;level76=0;level77=0;level78=0;level79=0;level80=0;" +
        "level81=0;level82=0;level83=0;level84=0;level85=0;level86=0;level87=0;level88=0;level89=0;level90=0;" +
        "level91=0;level92=0;level93=0;level94=0;level95=0;level96=0;level97=0;level98=0;level99=0;level100=0;" +
        "level101=0;level102=0;level103=0;" +
        "score1_1=0;score1_2=0;score2_1=0;score2_2=0;score3_1=0;score3_2=0;score4_1=0;score4_2=0;score5_1=0;score5_2=0;" +
        "score6_1=0;score6_2=0;score7_1=0;score7_2=0;score8_1=0;score8_2=0;score9_1=0;score9_2=0;score10_1=0;score10_2=0;" +
        "score11_1=0;score11_2=0;score12_1=0;score12_2=0;score13_1=0;score13_2=0;score14_1=0;score14_2=0;score15_1=0;score15_2=0;" +
        "score16_1=0;score16_2=0;score17_1=0;score17_2=0;score18_1=0;score18_2=0;score19_1=0;score19_2=0;score20_1=0;score20_2=0;" +
        "score21_1=0;score21_2=0;score22_1=0;score22_2=0;score23_1=0;score23_2=0;score24_1=0;score24_2=0;score25_1=0;score25_2=0;" +
        "score26_1=0;score26_2=0;score27_1=0;score27_2=0;score28_1=0;score28_2=0;score29_1=0;score29_2=0;score30_1=0;score30_2=0;" +
        "score31_1=0;score31_2=0;score32_1=0;score32_2=0;score33_1=0;score33_2=0;score34_1=0;score34_2=0;score35_1=0;score35_2=0;" +
        "score36_1=0;score36_2=0;score37_1=0;score37_2=0;score38_1=0;score38_2=0;score39_1=0;score39_2=0;score40_1=0;score40_2=0;" +
        "score41_1=0;score41_2=0;score42_1=0;score42_2=0;score43_1=0;score43_2=0;score44_1=0;score44_2=0;score45_1=0;score45_2=0;" +
        "score46_1=0;score46_2=0;score47_1=0;score47_2=0;score48_1=0;score48_2=0;score49_1=0;score49_2=0;score50_1=0;score50_2=0;" +
        "score51_1=0;score51_2=0;score52_1=0;score52_2=0;score53_1=0;score53_2=0;score54_1=0;score54_2=0;score55_1=0;score55_2=0;" +
        "score56_1=0;score56_2=0;score57_1=0;score57_2=0;score58_1=0;score58_2=0;score59_1=0;score59_2=0;score60_1=0;score60_2=0;" +
        "score61_1=0;score61_2=0;score62_1=0;score62_2=0;score63_1=0;score63_2=0;score64_1=0;score64_2=0;score65_1=0;score65_2=0;" +
        "score66_1=0;score66_2=0;score67_1=0;score67_2=0;score68_1=0;score68_2=0;score69_1=0;score69_2=0;score70_1=0;score70_2=0;" +
        "score71_1=0;score71_2=0;score72_1=0;score72_2=0;score73_1=0;score73_2=0;score74_1=0;score74_2=0;score75_1=0;score75_2=0;" +
        "score76_1=0;score76_2=0;score77_1=0;score77_2=0;score78_1=0;score78_2=0;score79_1=0;score79_2=0;score80_1=0;score80_2=0;" +
        "score81_1=0;score81_2=0;score82_1=0;score82_2=0;score83_1=0;score83_2=0;score84_1=0;score84_2=0;score85_1=0;score85_2=0;" +
        "score86_1=0;score86_2=0;score87_1=0;score87_2=0;score88_1=0;score88_2=0;score89_1=0;score89_2=0;score90_1=0;score90_2=0;" +
        "score91_1=0;score91_2=0;score92_1=0;score92_2=0;score93_1=0;score93_2=0;score94_1=0;score94_2=0;score95_1=0;score95_2=0;" +
        "score96_1=0;score96_2=0;score97_1=0;score97_2=0;score98_1=0;score98_2=0;score99_1=0;score99_2=0;score100_1=0;score100_2=0;" +
        "gift7=0;gift8=0;gift11=0;gift19=0;gift21=0;gift31=0;gift32=0;gift40=0;gift47=0;" +
        "gift53=0;gift56=0;gift63=0;gift69=0;gift71=0;gift73=0;gift84=0;gift87=0;gift94=0;gift95=0;";

    public static Animator currentSkinAnimator;

    public static IEnumerator waitForRealTime(float delay)
    {
        while (true)
        {
            float pauseEndTime = Time.realtimeSinceStartup + delay;
            while (Time.realtimeSinceStartup < pauseEndTime)
            {
                yield return 0;
            }
            break;
        }
    }


    //включаем текущий скин и выключаем все остальные
    //public static void changeSkin(out Animator currentSkinAnimator){
    public static void changeSkin()
    {
        GameObject spider = GameObject.Find("root/spider");
        //currentSkinAnimator = new Animator ();
        if (spider == null) return;
        Transform spiderTr = spider.transform;
        for (int i = 0; i < 5; i++)
        {
            if (spiderTr.GetChild(i).name == staticClass.currentSkin)
            {
                spiderTr.GetChild(i).gameObject.SetActive(true);
                currentSkinAnimator = spiderTr.GetChild(i).GetComponent<Animator>();
            }
            else
                spiderTr.GetChild(i).gameObject.SetActive(false);
        }
        changeHat();
    }

    //включаем текущую шапку и выключаем все остальные
    public static void changeHat()
    {
        if (GameObject.Find("root/spider") == null) return;
        Transform spider = GameObject.Find("root/spider").transform;
        for (int i = 0; i < 5; i++)
        {
            if (spider.GetChild(i).name == staticClass.currentSkin)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (spider.GetChild(i).GetChild(0).GetChild(j).name == staticClass.currentHat)
                    {
                        spider.GetChild(i).GetChild(0).GetChild(j).gameObject.SetActive(true);
                    }
                    else
                        spider.GetChild(i).GetChild(0).GetChild(j).gameObject.SetActive(false);
                }
            }
        }
    }

    //включаем текущую ягоду и выключаем все остальные
    public static void changeBerry()
    {
        if (GameObject.Find("root/berry") == null) return;
        Transform berry = GameObject.Find("root/berry").transform;
        for (int i = 0; i < 5; i++)
        {
            if (berry.GetChild(i).name == staticClass.currentBerry)
                berry.GetChild(i).gameObject.SetActive(true);
            else berry.GetChild(i).gameObject.SetActive(false);
        }

    }

    //прибавляем монеты и энергию
    public static void plusCoinsAndEnergy(int coins, int energy)
    {


    }


    public static string strProgressCheat =
        "googlePlay=0;lastLevel=100;currentLevel=100;coins=10000;gems=200;energyTime=0;energy=30;" +

        "boosters=100;hints=99;webs=99;collectors=99;teleports=99;complect=0;music=0;sound=1;dailyBonus=0;language=0;tutorialBuy=0;everyplay=1;" +


        "berry1=2;berry2=1;berry3=1;berry4=1;berry5=1;" +
        "hat1=2;hat2=1;hat3=1;hat4=1;hat5=1;" +
        "skin1=2;skin2=1;skin3=1;skin4=1;skin5=1;" +
        "level1=0;level2=0;level3=0;level4=0;level5=0;level6=0;level7=0;level8=0;level9=0;level10=0;" +
        "level11=0;level12=0;level13=0;level14=0;level15=0;level16=0;level17=0;level18=0;level19=0;level20=0;" +
        "level21=0;level22=0;level23=0;level24=0;level25=0;level26=0;level27=0;level28=0;level29=0;level30=0;" +
        "level31=0;level32=0;level33=0;level34=0;level35=0;level36=0;level37=0;level38=0;level39=0;level40=0;" +
        "level41=0;level42=0;level43=0;level44=0;level45=0;level46=0;level47=0;level48=0;level49=0;level50=0;" +
        "level51=0;level52=0;level53=0;level54=0;level55=0;level56=0;level57=0;level58=0;level59=0;level60=0;" +
        "level61=0;level62=0;level63=0;level64=0;level65=0;level66=0;level67=0;level68=0;level69=0;level70=0;" +
        "level71=0;level72=0;level73=0;level74=0;level75=0;level76=0;level77=0;level78=0;level79=0;level80=0;" +
        "level81=0;level82=0;level83=0;level84=0;level85=0;level86=0;level87=0;level88=0;level89=0;level90=0;" +
        "level91=0;level92=0;level93=0;level94=0;level95=0;level96=0;level97=0;level98=0;level99=0;level100=0;" +
        "level101=0;level102=0;level103=0;" +
        "score1_1=0;score1_2=0;score2_1=0;score2_2=0;score3_1=0;score3_2=0;score4_1=0;score4_2=0;score5_1=0;score5_2=0;" +
        "score6_1=0;score6_2=0;score7_1=0;score7_2=0;score8_1=0;score8_2=0;score9_1=0;score9_2=0;score10_1=0;score10_2=0;" +
        "score11_1=0;score11_2=0;score12_1=0;score12_2=0;score13_1=0;score13_2=0;score14_1=0;score14_2=0;score15_1=0;score15_2=0;" +
        "score16_1=0;score16_2=0;score17_1=0;score17_2=0;score18_1=0;score18_2=0;score19_1=0;score19_2=0;score20_1=0;score20_2=0;" +
        "score21_1=0;score21_2=0;score22_1=0;score22_2=0;score23_1=0;score23_2=0;score24_1=0;score24_2=0;score25_1=0;score25_2=0;" +
        "score26_1=0;score26_2=0;score27_1=0;score27_2=0;score28_1=0;score28_2=0;score29_1=0;score29_2=0;score30_1=0;score30_2=0;" +
        "score31_1=0;score31_2=0;score32_1=0;score32_2=0;score33_1=0;score33_2=0;score34_1=0;score34_2=0;score35_1=0;score35_2=0;" +
        "score36_1=0;score36_2=0;score37_1=0;score37_2=0;score38_1=0;score38_2=0;score39_1=0;score39_2=0;score40_1=0;score40_2=0;" +
        "score41_1=0;score41_2=0;score42_1=0;score42_2=0;score43_1=0;score43_2=0;score44_1=0;score44_2=0;score45_1=0;score45_2=0;" +
        "score46_1=0;score46_2=0;score47_1=0;score47_2=0;score48_1=0;score48_2=0;score49_1=0;score49_2=0;score50_1=0;score50_2=0;" +
        "score51_1=0;score51_2=0;score52_1=0;score52_2=0;score53_1=0;score53_2=0;score54_1=0;score54_2=0;score55_1=0;score55_2=0;" +
        "score56_1=0;score56_2=0;score57_1=0;score57_2=0;score58_1=0;score58_2=0;score59_1=0;score59_2=0;score60_1=0;score60_2=0;" +
        "score61_1=0;score61_2=0;score62_1=0;score62_2=0;score63_1=0;score63_2=0;score64_1=0;score64_2=0;score65_1=0;score65_2=0;" +
        "score66_1=0;score66_2=0;score67_1=0;score67_2=0;score68_1=0;score68_2=0;score69_1=0;score69_2=0;score70_1=0;score70_2=0;" +
        "score71_1=0;score71_2=0;score72_1=0;score72_2=0;score73_1=0;score73_2=0;score74_1=0;score74_2=0;score75_1=0;score75_2=0;" +
        "score76_1=0;score76_2=0;score77_1=0;score77_2=0;score78_1=0;score78_2=0;score79_1=0;score79_2=0;score80_1=0;score80_2=0;" +
        "score81_1=0;score81_2=0;score82_1=0;score82_2=0;score83_1=0;score83_2=0;score84_1=0;score84_2=0;score85_1=0;score85_2=0;" +
        "score86_1=0;score86_2=0;score87_1=0;score87_2=0;score88_1=0;score88_2=0;score89_1=0;score89_2=0;score90_1=0;score90_2=0;" +
        "score91_1=0;score91_2=0;score92_1=0;score92_2=0;score93_1=0;score93_2=0;score94_1=0;score94_2=0;score95_1=0;score95_2=0;" +
        "score96_1=0;score96_2=0;score97_1=0;score97_2=0;score98_1=0;score98_2=0;score99_1=0;score99_2=0;score100_1=0;score100_2=0;" +
        "gift7=0;gift8=0;gift11=0;gift19=0;gift21=0;gift31=0;gift32=0;gift40=0;gift47=0;" +
        "gift53=0;gift56=0;gift63=0;gift69=0;gift71=0;gift73=0;gift84=0;gift87=0;gift94=0;gift95=0;";

    public static string scenePrev = "menu";
    public static bool sceneLoading = false;
    public static float isTimePlay = 1;
    public static bool bonusesView = false;

    public static int levelRestartedCount = 0;
    public static int levelAdViewed = 0;

    public class saleTimer
    {
        public TimeSpan pause;
        public TimeSpan duration;
        public saleTimer(TimeSpan pause, TimeSpan duration)
        {
            this.pause = pause;
            this.duration = duration;
        }
    }
    /*
    static public Dictionary<string, saleTimer> sales = new Dictionary<string, saleTimer>
    {
      {"sale_1_free", new saleTimer(new TimeSpan(72, 0,0), new TimeSpan(72, 0,0))},

    };
    */
    //for test
    static public Dictionary<string, saleTimer> sales = new Dictionary<string, saleTimer>
    {
      {"sale_0_free", new saleTimer(new TimeSpan(0, 0, 5), new TimeSpan(0, 0, 30))},
      {"sale_1_free", new saleTimer(new TimeSpan(0, 0, 20), new TimeSpan(0, 0, 30))},
      {"sale_2_free", new saleTimer(new TimeSpan(0, 0, 20), new TimeSpan(0, 0, 30))},
      {"sale_3_free", new saleTimer(new TimeSpan(0, 1, 20), new TimeSpan(0, 0, 30))},
      {"sale_0_payers", new saleTimer(new TimeSpan(0, 0, 5), new TimeSpan(0, 0, 30))},
      {"sale_1_payers", new saleTimer(new TimeSpan(0, 0, 5), new TimeSpan(0, 0, 30))},
      {"sale_2_payers", new saleTimer(new TimeSpan(0, 0, 5), new TimeSpan(0, 0, 30))},

    };

    public static int getLanguage()
    {

        if (ctrProgressClass.progress["language"] == 0)
        {
            if (Application.systemLanguage.ToString() == "Russian" ||
                Application.systemLanguage.ToString() == "Ukrainian" ||
                Application.systemLanguage.ToString() == "Belarusian"
            )
            {

                Localization.language = "Russian";
                ctrProgressClass.progress["language"] = 2;

            }
            else
            {
                Localization.language = "English";
                ctrProgressClass.progress["language"] = 1;
            }
            ctrProgressClass.saveProgress();
        }
        else
        {
            if (ctrProgressClass.progress["language"] == 2)
            {
                Localization.language = "Russian";
                ctrProgressClass.progress["language"] = 2;
            }
            else
            {
                Localization.language = "English";
                ctrProgressClass.progress["language"] = 1;
            }
        }
        return ctrProgressClass.progress["language"];
    }
}

