using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rank : MonoBehaviour
{

    public Text totaltext;
    public Text name_text;
    public Text score_text;
    public GameObject inputCanvas;
    public InputField name;
    public Text highname;
   // public InputField passwd;
    private int myScore;
    private SQLiteDB db = null;
    private string dbfilename = "my_database.sqlite";

    private void dbsetup()
    {
        db = new SQLiteDB();
        

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        string dbpath = Application.dataPath + "/StreamingAssets" + "/" + dbfilename;
#elif UNITY_WEBPLAYER
        string dbpath = "StreamingAssets/" + dbfilename;
#elif UNITY_IPHONE
        string dbpath = Application.dataPath + "/Raw/" + dbfilename;
#elif UNITY_ANDROID
        /*string dbpath = "jar:file://" + Application.dataPath + "!/assets/";

#endif
        db.Open(dbpath);
        //insertdata("", "", 1);
    }
    public void insertdata(string name1,  int score1, string passwd1="asd")
    {
        string queryInsert = "insert into ranking values(\"" + name1 + "\", \"" + passwd1 + "\", " +score1.ToString() + ");";
        //string queryInsert = "insert into ranking values(\"" + "jy4" + "\", \"" + "123" + "\", " + (myScore + 10).ToString() + ");";
        //totaltext.text = queryInsert;
        SQLiteQuery qr;
        qr = new SQLiteQuery(db, queryInsert);
        qr.Step();
        qr.Release();
        // TableCreate();
        // Insert();
        //Insert2();

        //Select();

        //db.close();
    }
   // public void getdata(string name1, s
    // base : inputdata's active = false
    // Use this for initialization

    public void SelectData()
    {
        SQLiteQuery qr;
        string querySelect = "select * from ranking order by score desc;";
        string[] testStringFromSelect = new string[10];
        int[] scoreArr = new int[10];
        qr = new SQLiteQuery(db, querySelect);
        int count = 0;
        while(qr.Step())
        {
            if (count == 10) break;
           
            //testStringFromSelect[count] = new string();
            //scoreArr = new int();

            testStringFromSelect[count] = qr.GetString("name");
            scoreArr[count] = qr.GetInteger("score");

            count++;

            /*testStringFromSelect += " : ";
            //testStringFromSelect += qr.GetInt("score").ToString();
            //testStringFromSelect += "\n";
            */
            // var name : string qr.GetString()
        }
        name_text.text =testStringFromSelect[0]+ " \n "
           +testStringFromSelect[1] + " \n "
             + testStringFromSelect[2] + " \n "
             + testStringFromSelect[3] +" \n "
             + testStringFromSelect[4] + " \n "
             + testStringFromSelect[5] + " \n "
             + testStringFromSelect[6] + " \n "
             + testStringFromSelect[7] + " \n "
             + testStringFromSelect[8] + " \n "
             + testStringFromSelect[9] + " \n ";
        score_text.text = scoreArr[0].ToString() + '\n' + scoreArr[1].ToString() + '\n' + scoreArr[2].ToString() + '\n' + scoreArr[3].ToString() + '\n' + scoreArr[4].ToString() + '\n' + scoreArr[5].ToString() + '\n' + scoreArr[6].ToString() + '\n' + scoreArr[7].ToString() + '\n' + scoreArr[8].ToString() + '\n' + scoreArr[9].ToString() + '\n';

        highname.text = testStringFromSelect[0];
        qr.Release();
    }

    public void SelectSameName(string name1)
    {
        SQLiteQuery qr;
        string querySelect = "select * from ranking where name = \"" + name1 + "\"; ";
        
    }

    void Start()
    {
        myScore = Score.score;
        if (myScore != 0)
        {
            inputCanvas.SetActive(true);
            totaltext.text = myScore.ToString();

        }
        else
        {
            inputCanvas.SetActive(false);
            totaltext.text = "No Data";
        }
        dbsetup();
        //SelectData();

    }

   
    public void ExitButton()
    {
        int score = Score.score;
        totaltext.text = score.ToString();
        inputCanvas.SetActive(false);
    }
    public void OkayButton()
    {
        int score = Score.score;

        totaltext.text = score.ToString();
        insertdata(name.text, score, "asd");
        //SelectData();

        inputCanvas.SetActive(false);
        //totaltext.text = name.text;

 

    }
    // Update is called once per frame
    void Update()
    {
        SelectData();
    }
}
