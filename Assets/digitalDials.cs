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
    public KMSelectable[] screenButtons;
    public KMSelectable mainscreenButton;
    public TextMesh[] dialScreensText;
    public TextMesh mainScreen;
    public AudioClip[] sounds;
    public AudioClip[] noteSounds;
    public MeshRenderer[] dialScreens;
    public Material[] mats;
    private string[] mainNotes;
    private int clefNum;
    private int[] DialPos;
    private int[][] DialVals;
    string[] dialScalesNote;
    private string[][] dialNotes;
    private string[][] displayNotes;
    private string[][] displayNotesText;
    private bool[][] invert;
    private int[] DialPosAns;
    private int submitTime;
    
    string[][] noteTexts =
        {
                new string[3]{ "T", "ÔT", "äT" },
                new string[3]{ "U", "ÕU", "åU" },
                new string[3]{ "V", "ÖV", "æV" },
                new string[3]{ "W", "×W", "çW" },
                new string[3]{ "X", "ØX", "èX" },
                new string[3]{ "Y", "ÙY", "éY" },
                new string[3]{ "Z", "ÚZ", "êZ" },
                new string[3]{ "[", "Û[", "ë[" },
                new string[3]{ "\\", "Ü\\", "ì\\" },

                new string[3]{ "D", "ÔD", "äD" },
                new string[3]{ "E", "ÕE", "åE" },
                new string[3]{ "F", "ÖF", "æF" },
                new string[3]{ "G", "×G", "çG" },
                new string[3]{ "H", "ØH", "èH" },
                new string[3]{ "I", "ÙI", "éI" },
                new string[3]{ "J", "ÚJ", "êJ" },
                new string[3]{ "K", "ÛK", "ëK" },
                new string[3]{ "L", "ÜL", "ìL" },

                new string[3]{ "d", "Ôd", "äd" },
                new string[3]{ "e", "Õe", "åe" },
                new string[3]{ "f", "Öf", "æf" },
                new string[3]{ "g", "×g", "çg" },
                new string[3]{ "h", "Øh", "èh" },
                new string[3]{ "i", "Ùi", "éi" },
                new string[3]{ "j", "Új", "êj" },
                new string[3]{ "k", "Ûk", "ëk" },
                new string[3]{ "l", "Ül", "ìl" },

                new string[3]{ "t", "Ôt", "ät" },
                new string[3]{ "u", "Õu", "åu" },
                new string[3]{ "v", "Öv", "æv" },
                new string[3]{ "w", "×w", "çw" },
                new string[3]{ "x", "Øx", "èx" },
                new string[3]{ "y", "Ùy", "éy" },
                new string[3]{ "z", "Úz", "êz" },
                new string[3]{ "{", "Û{", "ë{" },
                new string[3]{ "|", "Ü|", "ì|" }
        };
    string[][][] notes =
    {
            new string[9][]
            {
                new string[3]{ "E4", "E#4", "Eb4" },
                new string[3]{ "F4", "F#4", "Fb4" },
                new string[3]{ "G4", "G#4", "Gb4" },
                new string[3]{ "A5", "A#5", "Ab5" },
                new string[3]{ "B5", "B#5", "Bb5" },
                new string[3]{ "C5", "C#5", "Cb5" },
                new string[3]{ "D5", "D#5", "Db5" },
                new string[3]{ "E5", "E#5", "Eb5" },
                new string[3]{ "F5", "F#5", "Fb5" },
            },
            new string[9][]
            {
                new string[3]{ "G2", "G#2", "Gb2" },
                new string[3]{ "A3", "A#3", "Ab3" },
                new string[3]{ "B3", "B#3", "Bb3" },
                new string[3]{ "C3", "C#3", "Cb3" },
                new string[3]{ "D3", "D#3", "Db3" },
                new string[3]{ "E3", "E#3", "Eb3" },
                new string[3]{ "F3", "F#3", "Fb3" },
                new string[3]{ "G3", "G#3", "Gb3" },
                new string[3]{ "A4", "A#4", "Ab4" },
            },
            new string[9][]
            {
                new string[3]{ "F3", "F#3", "Fb3" },
                new string[3]{ "G3", "G#3", "Gb3" },
                new string[3]{ "A4", "A#4", "Ab4" },
                new string[3]{ "B4", "B#4", "Bb4" },
                new string[3]{ "C4", "C#4", "Cb4" },
                new string[3]{ "D4", "D#4", "Db4" },
                new string[3]{ "E4", "E#4", "Eb4" },
                new string[3]{ "F4", "F#4", "Fb4" },
                new string[3]{ "G4", "G#4", "Gb4" }
            }
        };
    string[] scalenames = {"", "", "", "", "", "", "", "", "", "", "", "",
                            "F#", "G", "Ab", "A", "Bb", "B", "C", "Db", "D", "Eb", "E", "F"};
    private int[][] table1 =
    {
        new int[12]{8, 1, 3, 1, 6, 9, 5, 7, 4, 0, 2, 0},
        new int[12]{1, 2, 4, 3, 6, 2, 9, 5, 1, 7, 0, 8},
        new int[12]{3, 7, 4, 2, 3, 1, 8, 5, 2, 6, 9, 0},
        new int[12]{1, 9, 5, 4, 3, 0, 7, 2, 8, 4, 3, 6},
        new int[12]{6, 4, 8, 5, 4, 2, 3, 1, 0, 5, 7, 9},
        new int[12]{2, 4, 5, 1, 6, 5, 0, 6, 8, 9, 7, 3},
        new int[12]{6, 5, 2, 0, 7, 1, 3, 8, 6, 4, 7, 9},
        new int[12]{2, 4, 8, 9, 5, 3, 7, 1, 8, 7, 0, 6},
        new int[12]{3, 0, 5, 2, 1, 9, 8, 8, 6, 9, 4, 7},
        new int[12]{7, 5, 3, 0, 9, 6, 8, 9, 4, 2, 0, 1},
        new int[12]{4, 1, 0, 8, 6, 2, 1, 7, 5, 0, 3, 9},
        new int[12]{0, 1, 1, 6, 2, 2, 8, 9, 7, 5, 3, 4}
    };
    string[][] table2 =
    {
        new string[108]{
                "Db5 Half", "F#5 Whole", "Eb5 Eighth", "C#5 Eighth", "F#4 Whole", "A#5 Eighth", "B5 Quarter", "F#4 Half", "A5 Half", "Gb4 Half", "C#5 Quarter",
                "Fb5 Whole", "F4 Quarter", "Cb5 Quarter", "C5 Whole", "A5 Eighth", "F#5 Half", "Fb5 Quarter", "Db5 Quarter", "A#5 Whole", "F#4 Eighth", "B#5 Half",
                "Bb5 Half", "B5 Eighth", "Eb4 Eighth", "Eb5 Half", "F5 Eighth", "E5 Half", "E4 Half", "E#5 Half", "Eb5 Quarter", "G#4 Eighth", "B#5 Quarter", "D5 Quarter", "E4 Quarter", "F#5 Quarter", "Fb5 Eighth",
                "D#5 Quarter", "Gb4 Eighth", "Eb4 Whole", "Db5 Eighth", "Fb4 Eighth", "B#5 Whole", "Cb5 Whole", "E#5 Whole", "F5 Whole",
                "Fb4 Half", "G#4 Half", "Fb5 Half", "C5 Half", "C5 Eighth", "G#4 Quarter", "Fb4 Quarter", "Eb4 Quarter", "Ab5 Half", "Bb5 Whole", "F#4 Quarter", "A#5 Half",
                "D5 Half", "Cb5 Eighth", "F#5 Eighth", "Fb4 Whole", "G4 Eighth", "G4 Half", "E#4 Eighth", "D#5 Eighth", "Eb5 Whole", "Gb4 Whole", "F4 Eighth", "C5 Quarter",
                "A5 Quarter", "E4 Whole", "Cb5 Half", "Bb5 Eighth", "Ab5 Eighth", "G4 Quarter", "E5 Quarter", "Bb5 Quarter", "G#4 Whole", "G4 Whole", "B#5 Eighth", "E#5 Eighth",
                "C#5 Half", "D5 Whole", "F4 Whole", "C#5 Whole", "F4 Half", "Db5 Whole", "E#5 Quarter", "B5 Half", "Ab5 Quarter",
                "E#4 Whole", "D#5 Half", "A#5 Quarter", "E5 Eighth", "E4 Eighth", "D#5 Whole", "F5 Quarter", "E5 Whole", "E#4 Half", "Ab5 Whole",
                "E#4 Quarter", "Gb4 Quarter", "A5 Whole", "Eb4 Half", "B5 Whole", "F5 Half", "D5 Eighth"
            },
        new string[108]{
            "Fb3 Half", "A#4 Whole", "Gb3 Eighth", "E#3 Eighth", "A#3 Whole", "C#3 Eighth", "D3 Quarter", "A#3 Half", "C3 Half", "Bb3 Half",
            "E#3 Quarter", "Ab4 Whole", "A3 Quarter", "Eb3 Quarter", "E3 Whole", "C3 Eighth", "A#4 Half", "Ab4 Quarter", "Fb3 Quarter", "C#3 Whole",
            "A#3 Eighth", "D#3 Half", "Db3 Half", "D3 Eighth", "Gb2 Eighth", "Gb3 Half", "A4 Eighth", "G3 Half", "G2 Half", "G#3 Half",
            "Gb3 Quarter", "B#3 Eighth", "D#3 Quarter", "F3 Quarter", "G2 Quarter", "A#4 Quarter", "Ab4 Eighth", "F#3 Quarter", "Bb3 Eighth", "Gb2 Whole",
            "Fb3 Eighth", "Ab3 Eighth", "D#3 Whole", "Eb3 Whole", "G#3 Whole", "A4 Whole", "Ab3 Half", "B#3 Half", "Ab4 Half", "E3 Half",
            "E3 Eighth", "B#3 Quarter", "Ab3 Quarter", "Gb2 Quarter", "Cb3 Half", "Db3 Whole", "A#3 Quarter", "C#3 Half", "F3 Half", "Eb3 Eighth",
            "A#4 Eighth", "Ab3 Whole", "B3 Eighth", "B3 Half", "G#2 Eighth", "F#3 Eighth", "Gb3 Whole", "Bb3 Whole", "A3 Eighth", "E3 Quarter",
            "C3 Quarter", "G2 Whole", "Eb3 Half", "Db3 Eighth", "Cb3 Eighth", "B3 Quarter", "G3 Quarter", "Db3 Quarter", "B#3 Whole", "B3 Whole",
            "D#3 Eighth", "G#3 Eighth", "E#3 Half", "F3 Whole", "A3 Whole", "E#3 Whole", "A3 Half", "Fb3 Whole", "G#3 Quarter", "D3 Half",
            "Cb3 Quarter", "G#2 Whole", "F#3 Half", "C#3 Quarter", "G3 Eighth", "G2 Eighth", "F#3 Whole", "A4 Quarter", "G3 Whole", "G#2 Half",
            "Cb3 Whole", "G#2 Quarter", "Bb3 Quarter", "C3 Whole", "Gb2 Half", "D3 Whole", "A4 Half", "F3 Eighth"
        },
        new string[108]{
            "Eb4 Half", "G#4 Whole", "Fb4 Eighth", "D#4 Eighth", "G#3 Whole", "B#4 Eighth", "C4 Quarter", "G#3 Half", "B4 Half", "Ab4 Half",
            "D#4 Quarter", "Gb4 Whole", "G3 Quarter", "Db4 Quarter", "D4 Whole", "B4 Eighth", "G#4 Half", "Gb4 Quarter", "Eb4 Quarter", "B#4 Whole",
            "G#3 Eighth", "C#4 Half", "Cb4 Half", "C4 Eighth", "Fb3 Eighth", "Fb4 Half", "G4 Eighth", "F4 Half", "F3 Half", "F#4 Half",
            "Fb4 Quarter", "A#4 Eighth", "C#4 Quarter", "E4 Quarter", "F3 Quarter", "G#4 Quarter", "Gb4 Eighth", "E#4 Quarter", "Ab4 Eighth", "Fb3 Whole",
            "Eb4 Eighth", "Gb3 Eighth", "C#4 Whole", "Db4 Whole", "F#4 Whole", "G4 Whole", "Gb3 Half", "A#4 Half", "Gb4 Half", "D4 Half",
            "D4 Eighth", "A#4 Quarter", "Gb3 Quarter", "Fb3 Quarter", "Bb4 Half", "Cb4 Whole", "G#3 Quarter", "B#4 Half", "E4 Half", "Db4 Eighth",
            "G#4 Eighth", "Gb3 Whole", "A4 Eighth", "A4 Half", "F#3 Eighth", "E#4 Eighth", "Fb4 Whole", "Ab4 Whole", "G3 Eighth", "D4 Quarter",
            "B4 Quarter", "F3 Whole", "Db4 Half", "Cb4 Eighth", "Bb4 Eighth", "A4 Quarter", "F4 Quarter", "Cb4 Quarter", "A#4 Whole", "A4 Whole",
            "C#4 Eighth", "F#4 Eighth", "D#4 Half", "E4 Whole", "G3 Whole", "D#4 Whole", "G3 Half", "Eb4 Whole", "F#4 Quarter", "C4 Half",
            "Bb4 Quarter", "F#3 Whole", "E#4 Half", "B#4 Quarter", "F4 Eighth", "F3 Eighth", "E#4 Whole", "G4 Quarter", "F4 Whole", "F#3 Half",
            "Bb4 Whole", "F#3 Quarter", "Ab4 Quarter", "B4 Whole", "Fb3 Half", "C4 Whole", "G4 Half", "E4 Eighth"
        }
};
    int[][] table3 =
    {
        new int[3]{9, 6, 7},
        new int[3]{13, 11, 12},
        new int[3]{5, 5, 13},
        new int[3]{8, 3, 4},
        new int[3]{12, 9, 8},
        new int[3]{2, 12, 11},
        new int[3]{10, 7, 6},
        new int[3]{3, 10, 2},
        new int[3]{11, 8, 10},
        new int[3]{6, 13, 9},
        new int[3]{4, 2, 3},
        new int[3]{7, 4, 5}
    };
    void Awake()
    {
        
        moduleId = moduleIdCounter++;
        dials[0].OnInteract += delegate () { Turn(0); return false; };
        dials[1].OnInteract += delegate () { Turn(1); return false; };
        dials[2].OnInteract += delegate () { Turn(2); return false; };
        submit.OnInteract += delegate () { onSubmit(); return false; };
        screenButtons[0].OnInteract += delegate () { HearSound(0); return false; };
        screenButtons[1].OnInteract += delegate () { HearSound(1); return false; };
        screenButtons[2].OnInteract += delegate () { HearSound(2); return false; };
        mainscreenButton.OnInteract += delegate () { Tuner(); return false; };
    }
    void Start()
    {
        int[] pattern = {2, 2, 1, 2, 2, 2, 1, 2, 2};
        
        DialPos = new int[3];
        dialNotes = new string[3][];
        mainNotes = new string[3];
        displayNotes = new string[3][];
        displayNotesText = new string[3][];
        dialScalesNote = new string[3];
        invert = new bool[3][];
        clefNum = UnityEngine.Random.Range(0, 3);
        string mainNotesText = "&¯ÿ"[clefNum] + "!";
        
        for(int aa = 0; aa < 3; aa++)
        {
            DialPos[aa] = 0;
            dialNotes[aa] = new string[10];
            int num = UnityEngine.Random.Range(12, 24);
            int items = UnityEngine.Random.Range(0, 10);
            dialNotes[aa][items] = noteSounds[num].name;
            dialScalesNote[aa] = scalenames[num] + " " + items;
            for (int bb = 1; bb < 10; bb++)
            {
                items++;
                num += pattern[bb - 1];
                dialNotes[aa][items % 10] = noteSounds[num].name;
            }
            num = UnityEngine.Random.Range(0, noteTexts.Length);
            int num2 = UnityEngine.Random.Range(0, noteTexts[num].Length);
            mainNotesText = mainNotesText + "'" + noteTexts[num][num2] + "!";
            mainNotes[aa] = notes[clefNum][num % 9][num2] + " " + "Quarter Eighth Half Whole".Split(' ')[num / 9];
            displayNotes[aa] = new string[10];
            displayNotesText[aa] = new string[10];
            invert[aa] = new bool[10];
            for (int bb = 0; bb < 10; bb++)
            {
                num = UnityEngine.Random.Range(0, noteTexts.Length);
                num2 = UnityEngine.Random.Range(0, noteTexts[num].Length);
                displayNotesText[aa][bb] = "'" + noteTexts[num][num2] + "!";
                displayNotes[aa][bb] = notes[clefNum][num % 9][num2] + " " + "Quarter Eighth Half Whole".Split(' ')[num / 9];
                invert[aa][bb] = (UnityEngine.Random.Range(0, 2) == 1);
            }
            getScreen(aa);
        }
        mainScreen.text = mainNotesText;
        getSolution();
    }
    void getSolution()
    {
        //Step 1
        DialPosAns = new int[3];
        for(int aa = 0; aa < 3; aa++)
        {
            Debug.LogFormat("[Scalar Dials #{0}] Main Screen Note #{1}: {2}", moduleId, aa + 1, mainNotes[aa]);
            Debug.LogFormat("[Scalar Dials #{0}] Dial Scale #{1}: {2}", moduleId, aa + 1, dialScalesNote[aa].Split(' ')[0]);
            int col = getRowCol(mainNotes[aa].Split(' ')[0].Substring(0, mainNotes[aa].Split(' ')[0].Length - 1));
            int row = getRowCol(dialScalesNote[aa].Split(' ')[0]);
            DialPosAns[aa] = table1[row][col];
            Debug.LogFormat("[Scalar Dials #{0}] Column {1}", moduleId, col + 1);
            Debug.LogFormat("[Scalar Dials #{0}] Row {1}", moduleId, row + 1);
            Debug.LogFormat("[Scalar Dials #{0}] Dial #{1} Initial Position: {2}", moduleId, aa + 1, DialPosAns[aa]);
        }
        //Step 2
        for(int aa = 0; aa < 3; aa++)
        {
            string color;
            string[][] checkNotes = new string[3][];
            if (invert[aa][DialPosAns[aa]])
            {
                color = "Black";
                checkNotes[0] = new string[4]{ "F4", "A5", "C5", "E5" };
                checkNotes[1] = new string[4]{ "A3", "C3", "E3", "G3" };
                checkNotes[2] = new string[4]{ "G3", "B4", "D4", "F4" };
            }
            else
            {
                color = "White";
                checkNotes[0] = new string[5]{ "E4", "G4", "B5", "D5", "F5" };
                checkNotes[1] = new string[5]{ "G2", "B3", "D3", "F3", "A4" };
                checkNotes[2] = new string[5]{ "F3", "A4", "C4", "E4", "G4" };
            }
            Debug.LogFormat("[Scalar Dials #{0}] Main Screen Note #{1}: {2}", moduleId, aa + 1, mainNotes[aa]);
            Debug.LogFormat("[Scalar Dials #{0}] {1} Screen Note #{2}: {3}", moduleId, color, aa + 1, displayNotes[aa][DialPosAns[aa]]);
            int n1 = -1;
            int n2 = -1;
            for (int bb = 0; bb < table2[clefNum].Length; bb++)
            {
                if (mainNotes[aa].EqualsIgnoreCase(table2[clefNum][bb]))
                    n1 = bb;
                if (displayNotes[aa][DialPosAns[aa]].EqualsIgnoreCase(table2[clefNum][bb]))
                    n2 = bb;
                if (n1 >= 0 && n2 >= 0)
                    break;
            }
            Debug.LogFormat("[Scalar Dials #{0}] Main Screen Note Position #{1}: {2}", moduleId, aa + 1, n1 + 1);
            Debug.LogFormat("[Scalar Dials #{0}] {1} Screen Note Position #{2}: {3}", moduleId, color, aa + 1, n2 + 1);
            if(n2 < n1)
            {
                int temp = n1;
                n1 = n2;
                n2 = temp;
            }
            int sum = 0;
            for (int bb = n1 + 1; bb < n2; bb++)
            {
                for (int cc = 0; cc < checkNotes[clefNum].Length; cc++)
                {
                    if (table2[clefNum][bb][0] == checkNotes[clefNum][cc][0] && table2[clefNum][bb].Contains(checkNotes[clefNum][cc][1]))
                    {
                        sum++;
                        break;
                    }
                }
            }
            Debug.LogFormat("[Scalar Dials #{0}] Sum: {1}", moduleId, sum);
            DialPosAns[aa] = sum % 10;
            Debug.LogFormat("[Scalar Dials #{0}] Dial #{1} Final Position: {2}", moduleId, aa + 1, DialPosAns[aa]);
        }
        //Step 3
        string[][] noteList =
        {
            new string[2]{"F#2", "Gb2"},
            new string[1]{"G2"},
            new string[2]{"G#2", "Ab3"},
            new string[1]{"A3"},
            new string[2]{"A#3", "Bb3"},
            new string[2]{"B3", "Cb3"},
            new string[2]{"C3", "B#3"},
            new string[2]{"C#3", "Db3"},
            new string[1]{"D3"},
            new string[2]{"D#3", "Eb3"},
            new string[2]{"E3", "Fb3"},
            new string[2]{"F3", "E#3"},
            new string[2]{"F#3", "Gb3"},
            new string[1]{"G3"},
            new string[2]{"G#3", "Ab4"},
            new string[1]{"A4"},
            new string[2]{"A#4", "Bb4"},
            new string[2]{"B4", "Cb4"},
            new string[2]{"C4", "B#4"},
            new string[2]{"C#4", "Db4"},
            new string[1]{"D4"},
            new string[2]{"D#4", "Eb4"},
            new string[2]{"E4", "Fb4"},
            new string[2]{"F4", "E#4"},
            new string[2]{"F#4", "Gb4"},
            new string[1]{"G4"},
            new string[2]{"G#4", "Ab5"},
            new string[1]{"A5"},
            new string[2]{"A#5", "Bb5"},
            new string[2]{"B5", "Cb5"},
            new string[2]{"C5", "B#5"},
            new string[2]{"C#5", "Db5"},
            new string[1]{"D5"},
            new string[2]{"D#5", "Eb5"},
            new string[2]{"E5", "Fb5"},
            new string[2]{"F5", "E#5"},
            new string[2]{"F#5", "Gb5"},
            new string[1]{"G5"},
            new string[2]{"G#5", "Ab6"},
            new string[1]{"A6"}
        };
        string colorCombo = "";
        int mult = 1;
        string[] intervals = {"P1", "m2", "M2", "m3", "M3", "P4", "TT", "P5", "m6", "M6", "m7", "M7"};
        for (int aa = 0; aa < 3; aa++)
        {
            string color = "White";
            if (invert[aa][DialPosAns[aa]])
                color = "Black";
            Debug.LogFormat("[Scalar Dials #{0}] Dial #{1} Note: {2}", moduleId, aa + 1, dialNotes[aa][DialPosAns[aa]]);
            Debug.LogFormat("[Scalar Dials #{0}] {1} Screen Note #{2}: {3}", moduleId, color, aa + 1, displayNotes[aa][DialPosAns[aa]]);
            colorCombo = colorCombo + "" + color[0];
            int n1 = -1;
            int n2 = -1;
            for(int bb = 0; bb < noteList.Length; bb++)
            {
                for(int cc = 0; cc < noteList[bb].Length; cc++)
                {
                    if (dialNotes[aa][DialPosAns[aa]].EqualsIgnoreCase(noteList[bb][cc]))
                        n1 = bb;
                    if (displayNotes[aa][DialPosAns[aa]].Split(' ')[0].EqualsIgnoreCase(noteList[bb][cc]))
                        n2 = bb;
                }
                if (n1 >= 0 && n2 >= 0)
                    break;
            }
            int diff;
            if (n1 > n2)
                diff = n1 - n2;
            else
                diff = n2 - n1;
            Debug.LogFormat("[Scalar Dials #{0}] Interval: {1}", moduleId, intervals[diff % 12]);
            Debug.LogFormat("[Scalar Dials #{0}] Table Number: {1}", moduleId, table3[diff % 12][aa]);
            mult = mult * table3[diff % 12][aa];
        }
        Debug.LogFormat("[Scalar Dials #{0}] Multiplication of all 3 numbers: {1}", moduleId, mult);
        string bin = "";
        while(mult > 0)
        {
            bin = (mult % 2) + "" + bin;
            mult = mult / 2;
        }
        while (bin.Length % 3 != 0)
            bin = "0" + bin;
        Debug.LogFormat("[Scalar Dials #{0}] Binary: {1}", moduleId, bin);
        Debug.LogFormat("[Scalar Dials #{0}] 3 Color Combination: {1}", moduleId, colorCombo);
        submitTime = 0;
        for(int aa = 0; aa < bin.Length; aa++)
        {
            if (colorCombo[aa % 3] == 'W' && bin[aa] == '1')
                submitTime++;
            else if (colorCombo[aa % 3] == 'B' && bin[aa] == '0')
                submitTime++;
        }
        Debug.LogFormat("[Scalar Dials #{0}] Number S: {1}", moduleId, submitTime);
        submitTime = submitTime % 10;
        Debug.LogFormat("[Scalar Dials #{0}] Submit when the last digit is a {1}", moduleId, submitTime);
    }
    int getRowCol(string n)
    {
        switch(n)
        {
            case "C":
            case "B#":
                return 0;
            case "C#":
            case "Db":
                return 1;
            case "D":
                return 2;
            case "D#":
            case "Eb":
                return 3;
            case "E":
            case "Fb":
                return 4;
            case "F":
            case "E#":
                return 5;
            case "F#":
            case "Gb":
                return 6;
            case "G":
                return 7;
            case "G#":
            case "Ab":
                return 8;
            case "A":
                return 9;
            case "A#":
            case "Bb":
                return 10;
            default:
                return 11;
        }
    }
    void Tuner()
    {
        if (!moduleSolved)
        {
            string note = mainNotes[2 - (((int)bomb.GetTime() / 60) % 3)].Split(' ')[0].ToUpper();
            if (note.IndexOf("E#") >= 0)
                note = "F" + note[2];
            else if (note.IndexOf("FB") >= 0)
                note = "E" + note[2];
            else if (note.IndexOf("B#") >= 0)
                note = "C" + note[2];
            else if (note.IndexOf("CB") >= 0)
                note = "B" + note[2];
            if (note[1] == 'B')
            {
                if(note[0] == 'A')
                    note = "G#" + ((note[2] - '0') - 1);
                else
                    note = "ABCDEFG"[("ABCDEFG".IndexOf(note[0]) + 6) % 7] + "#" + note[2];
            }
            audio.PlaySoundAtTransform(note, transform);
        }
    }
    void HearSound(int d)
    {
        if(!moduleSolved)
            audio.PlaySoundAtTransform(dialNotes[d][DialPos[d]], transform);
    }
    void Turn(int d)
    {
        if (!(moduleSolved))
        {
            DialPos[d] = (DialPos[d] + 1) % 10;
            dials[d].transform.Rotate(Vector3.up, 36f);
            audio.PlaySoundAtTransform(dialNotes[d][DialPos[d]], transform);
            getScreen(d);
        }
    }
    void getScreen(int d)
    {
        dialScreensText[d].text = displayNotesText[d][DialPos[d]];
        if(invert[d][DialPos[d]])
        {
            dialScreensText[d].color = Color.white;
            dialScreens[d].material = mats[0];
        }
        else
        {
            dialScreensText[d].color = Color.black;
            dialScreens[d].material = mats[1];
        }
    }
    void onSubmit()
    {
        if (!moduleSolved)
        {
            submit.AddInteractionPunch();
            audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
            if (DialPos[0] == DialPosAns[0] && DialPos[1] == DialPosAns[1] && DialPos[2] == DialPosAns[2] && (int)(bomb.GetTime() % 10) == submitTime)
            {
                audio.PlaySoundAtTransform(sounds[1].name, transform);
                Debug.LogFormat("[Scalar Dials #{0}] Module solved. Everybody boogie!", moduleId);
                mainScreen.text = "";
                for (int aa = 0; aa < 3; aa++)
                {
                    while (DialPos[aa] != 0)
                    {
                        DialPos[aa] = (DialPos[aa] + 1) % 10;
                        dials[aa].transform.Rotate(Vector3.up, 36f);
                    }
                    dialScreens[aa].material = mats[1];
                    dialScreensText[aa].text = "";
                }
                moduleSolved = true;
                module.HandlePass();
            }
            else
            {
                audio.PlaySoundAtTransform(sounds[0].name, transform);
                module.HandleStrike();
                Debug.LogFormat("[Scalar Dials #{0}] Strike! You tried to submit {1} {2} {3} at {4} seconds", moduleId, DialPos[0], DialPos[1], DialPos[2], (int)(bomb.GetTime() % 10));
            }
        }
    }
    
#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} set 9 7 1 sets the 1st, 2nd, and 3rd dials to 9, 7, and 1. !{0} cycle 1 2 3 cycles the 1st, 2nd, and 3rd dials. !{0} press M 1 2 3 presses the main screen then the smaller screens in reading order. !{0} submit 5 presses the submit button when the countdown timer's last digit is a 5.";
#pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        string[] param = command.ToUpper().Split(' ');
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
        if (Regex.IsMatch(param[0], @"^\s*cycle\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) && param.Length > 1)
        {
            string nums = "";
            for (int aa = 1; aa < param.Length; aa++)
            {
                for (int bb = 0; bb < param[aa].Length; bb++)
                {
                    if ("123".IndexOf(param[aa][bb]) >= 0)
                        nums = nums + "" + param[aa][bb];
                }
            }
            if(nums.Length > 0)
            {
                flag = false;
                for (int aa = 0; aa < nums.Length; aa++)
                {
                    for (int bb = 0; bb < 10; bb++)
                    {
                        dials["123".IndexOf(nums[aa])].OnInteract();
                        yield return new WaitForSeconds(0.4f);
                    }
                    yield return new WaitForSeconds(1.0f);
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
        if (Regex.IsMatch(param[0], @"^\s*press\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) && param.Length > 1)
        {
            string nums = "";
            for (int aa = 1; aa < param.Length; aa++)
            {
                for (int bb = 0; bb < param[aa].Length; bb++)
                {
                    if ("123M".IndexOf(param[aa][bb]) >= 0)
                        nums = nums + "" + param[aa][bb];
                }
            }
            if (nums.Length > 0)
            {
                flag = false;
                for (int aa = 0; aa < nums.Length; aa++)
                {
                    if (nums[aa] == 'M')
                        mainscreenButton.OnInteract();
                    else
                        screenButtons["123".IndexOf(nums[aa])].OnInteract();
                    yield return new WaitForSeconds(0.6f);
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
        if (Regex.IsMatch(param[0], @"^\s*set\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
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
    IEnumerator TwitchHandleForcedSolve()
    {
        for (int i = 0; i < 3; i++)
        {
            while (DialPos[i] != DialPosAns[i])
            {
                dials[i].OnInteract();
                yield return new WaitForSeconds(0.1f);
            }
        }
        while (((int)(bomb.GetTime())) % 10 != submitTime) yield return true;
        submit.OnInteract();
        yield return new WaitForSeconds(0.1f);
    }
}
