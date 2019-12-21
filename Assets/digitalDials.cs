using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using System.Text.RegularExpressions;

public class digitalDials : MonoBehaviour
{
    public KMAudio audio;
    public KMBombInfo bomb;
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    public KMBombModule module;
    public KMSelectable[] dials;
    public KMSelectable submit;
    public TextMesh[] dialScreens;
    public TextMesh mainScreen;
    public AudioClip[] sounds;
    private int[] DialPos;
    private int[][] DialVals;
    private int[][][] dialCharts =
    {
        new int[10][]
        {
            new int[10] {6, 7, 0, 9, 3, 8, 4, 1, 2, 5},
            new int[10] {0, 2, 9, 1, 8, 7, 6, 4, 3, 5},
            new int[10] {8, 1, 5, 7, 3, 2, 9, 4, 6, 0},
            new int[10] {2, 0, 4, 8, 9, 5, 6, 3, 1, 7},
            new int[10] {8, 1, 5, 0, 7, 2, 3, 9, 6, 4},
            new int[10] {4, 1, 5, 8, 3, 7, 6, 2, 0, 9},
            new int[10] {3, 7, 9, 4, 5, 0, 8, 2, 6, 1},
            new int[10] {8, 2, 1, 7, 5, 4, 6, 9, 3, 0},
            new int[10] {9, 2, 5, 3, 8, 6, 7, 4, 0, 1},
            new int[10] {5, 6, 2, 7, 9, 3, 8, 0, 4, 1}
        },
        new int[10][]
        {
            new int[10]{2, 8, 5, 3, 9, 6, 1, 7, 4, 0},
            new int[10]{4, 5, 6, 1, 0, 7, 2, 9, 8, 3},
            new int[10]{3, 0, 6, 7, 1, 4, 8, 2, 9, 5},
            new int[10]{1, 8, 6, 0, 4, 5, 3, 9, 2, 7},
            new int[10]{5, 0, 7, 4, 2, 9, 1, 8, 3, 6},
            new int[10]{9, 8, 0, 1, 5, 7, 6, 4, 2, 3},
            new int[10]{3, 9, 8, 2, 4, 6, 7, 0, 5, 1},
            new int[10]{5, 3, 1, 9, 8, 0, 2, 7, 6, 4},
            new int[10]{3, 5, 1, 2, 9, 4, 0, 7, 8, 6},
            new int[10]{9, 1, 0, 6, 8, 3, 5, 4, 7, 2}
        },
        new int[10][]
        {
            new int[10]{9, 8, 2, 5, 4, 0, 1, 7, 6, 3},
            new int[10]{7, 5, 2, 0, 6, 1, 8, 4, 9, 3},
            new int[10]{5, 3, 1, 7, 8, 4, 0, 6, 2, 9},
            new int[10]{0, 8, 7, 2, 9, 5, 3, 1, 6, 4},
            new int[10]{2, 1, 6, 9, 0, 3, 4, 8, 7, 5},
            new int[10]{8, 5, 0, 4, 3, 6, 1, 2, 7, 9},
            new int[10]{7, 0, 1, 8, 2, 5, 6, 4, 3, 9},
            new int[10]{4, 6, 1, 7, 2, 8, 9, 3, 5, 0},
            new int[10]{2, 5, 4, 6, 7, 0, 1, 9, 8, 3},
            new int[10]{1, 2, 5, 3, 8, 9, 4, 0, 6, 7}
        }
    };
    private int[] DialPosAns;
    private int submitTime;
    void Awake()
    {
        moduleId = moduleIdCounter++;
        dials[0].OnInteract += delegate () { Turn(0); return false; };
        dials[1].OnInteract += delegate () { Turn(1); return false; };
        dials[2].OnInteract += delegate () { Turn(2); return false; };
        submit.OnInteract += delegate () { onSubmit(); return false; };
    }
    void Start()
    {
        DialVals = new int[3][];
        DialPos = new int[3];
        mainScreen.text = "";
        for (int aa = 0; aa < 3; aa++)
        {
            DialVals[aa] = new int[10];
            DialPos[aa] = 0;
            mainScreen.text = mainScreen.text + "" + UnityEngine.Random.Range(0, 10);
            Debug.LogFormat("[Digital Dials {0}] Dial {1} Values:", moduleId, aa + 1);
            for (int bb = 0; bb < 10; bb++)
            {
                DialVals[aa][bb] = UnityEngine.Random.Range(0, 100);
                Debug.LogFormat("[Digital Dials {0}] {1}", moduleId, DialVals[aa][bb]);
            }
            getScreen(aa);
        }
        Debug.LogFormat("[Digital Dials {0}] Main Screen Value: {1}", moduleId, mainScreen.text);
        getSolution();
    }
    void getSolution()
    {
        DialPosAns = new int[3];
        int[] nums = new int[3];
        string sn = bomb.GetSerialNumber();
        string snn = "";
        for (int aa = 0; aa < sn.Length; aa++)
        {
            if ("0123456789".IndexOf(sn[aa]) >= 0)
                snn = snn + "" + sn[aa];
        }
        if (snn.Length == 2)
            snn = snn + "" + bomb.GetPortCount();
        for (int aa = 0; aa < 3; aa++)
        {
            nums[aa] = (snn[aa] - '0') + (mainScreen.text[aa] - '0');
            Debug.LogFormat("[Digital Dials {0}] 1{1}: {2}", moduleId, "ABC"[aa], snn[aa] + " + " + mainScreen.text[aa] + " = " + nums[aa]);
        }
        Debug.LogFormat("[Digital Dials {0}] 1D: ({1} * {2} * {3}) % 1000 = {4}", moduleId, nums[0], nums[1], nums[2], (nums[0] * nums[1] * nums[2]) % 1000);
        nums[0] = (nums[0] * nums[1] * nums[2]) % 1000;
        DialPosAns[0] = nums[0] / 100;
        DialPosAns[1] = (nums[0] / 10) % 10;
        DialPosAns[2] = nums[0] % 10;
        Debug.LogFormat("[Digital Dials {0}] Initial Dial Positions: {1} {2} {3}", moduleId, DialPosAns[0], DialPosAns[1], DialPosAns[2]);
        for (int aa = 0; aa < 3; aa++)
        {
            int num = dialCharts[aa][DialVals[aa][DialPosAns[aa]] / 10][DialVals[aa][DialPosAns[aa]] % 10];
            Debug.LogFormat("[Digital Dials {0}] Dial {1}: {2} => ({3} + {4}) % 10 = {5}", moduleId, aa + 1, DialVals[aa][DialPosAns[aa]], num, DialPosAns[aa], (num + DialPosAns[aa]) % 10);
            DialPosAns[aa] = (DialPosAns[aa] + num) % 10;
        }
        Debug.LogFormat("[Digital Dials {0}] Final Dial Positions: {1} {2} {3}", moduleId, DialPosAns[0], DialPosAns[1], DialPosAns[2]);
        submitTime = DialVals[0][DialPosAns[0]] * DialVals[1][DialPosAns[1]] * DialVals[2][DialPosAns[2]];
        Debug.LogFormat("[Digital Dials {0}] 2A: {1} * {2} * {3} = {4}", moduleId, DialVals[0][DialPosAns[0]], DialVals[1][DialPosAns[1]], DialVals[2][DialPosAns[2]], submitTime);
        while (submitTime > 9)
        {
            string conv = submitTime + "";
            submitTime = 0;
            for (int aa = 0; aa < conv.Length; aa++)
                submitTime += "0123456789".IndexOf(conv[aa]);
            Debug.LogFormat("[Digital Dials {0}] 2B: {1} => {2}", moduleId, conv, submitTime);
        }
        Debug.LogFormat("[Digital Dials {0}] Submit when the last digit is a {1}", moduleId, submitTime);
    }
    void Turn(int d)
    {
        if (!(moduleSolved))
        {
            audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
            DialPos[d] = (DialPos[d] + 1) % 10;
            dials[d].transform.Rotate(Vector3.up, 36f);
            getScreen(d);
        }
    }
    void getScreen(int d)
    {
        string display = "" + DialVals[d][DialPos[d]];
        if (display.Length < 2)
            display = "0" + display;
        dialScreens[d].text = display.ToUpper();
    }
    void onSubmit()
    {
        if (!moduleSolved)
        {
            submit.AddInteractionPunch();
            audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
            if (DialPos[0] == DialPosAns[0] && DialPos[1] == DialPosAns[1] && DialPos[2] == DialPosAns[2] && (int)(bomb.GetTime() % 10) == submitTime)
            {
                audio.PlaySoundAtTransform(sounds[0].name, transform);
                Debug.LogFormat("[Digital Dials {0}] Module solved. Everybody boogie!", moduleId);
                mainScreen.text = "GG";
                for(int aa = 0; aa < 3; aa++)
                {
                    while (DialPos[aa] != 0)
                        Turn(aa);
                    dialScreens[aa].text = "";
                }
                moduleSolved = true;
                module.HandlePass();
            }
            else
            {
                audio.PlaySoundAtTransform(sounds[1].name, transform);
                module.HandleStrike();
                Debug.LogFormat("[Digital Dials {0}] Strike! You tried to submit {1} {2} {3} at {4} seconds", moduleId, DialPos[0], DialPos[1], DialPos[2], (int)(bomb.GetTime() % 10));
            }
        }
    }
    
#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} set 9 7 1 sets the 1st, 2nd, and 3rd dials to 9, 7, and 1. !{0} submit 5 presses the submit button when the countdown timer's last digit is a 5.";
#pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        string[] param = command.Split(' ');
        bool flag = true;
        if (Regex.IsMatch(param[0], @"^\s*submit\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) && param.Length == 2)
        {
            if ("0123456789".IndexOf(param[1]) >= 0 && param[1].Length == 1)
            {
                flag = false;
                int timepress = "0123456789".IndexOf(param[1]);
                while(((int)(bomb.GetTime())) % 10 != timepress)
                    yield return "trycancel The button was not pressed due to a request to cancel.";
                submit.OnInteract();
                yield return new WaitForSeconds(0.1f);
            }
        }
        if(Regex.IsMatch(param[0], @"^\s*set\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            string nums = "";
            for(int aa = 1; aa < param.Length; aa++)
            {
                for(int bb = 0; bb < param[aa].Length; bb++)
                {
                    if("0123456789".IndexOf(param[aa][bb]) >= 0)
                        nums = nums + "" + param[aa][bb];
                }
            }
            if (nums.Length == 3)
            {
                flag = false;
                for (int aa = 0; aa < 3; aa++)
                {
                    while (DialPos[aa] != (nums[aa] - '0'))
                    {
                        dials[aa].OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                        
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
        if(flag)
            yield return "sendtochat The command you sent wasn't executed because the dials were confused/scared.";
    }
}
