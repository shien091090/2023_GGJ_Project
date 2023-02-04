using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    
    //音樂
    public AudioSource my_audio;
    public AudioClip ac_End;
    
    //計時器
    public int m_Seconds,m_Min,m_Sec;//總秒數,分鐘,秒
    public Text m_Timer;//時間文字呈現
    public Text timeUp;//時間到
    
    //場上蘿蔔
    public int maxRadish;
    
    //遊戲開始
    public bool gameStart;
    
    //玩家分數
    public Text nowTxt_P1, nowTxt_P2;
    public Text txt_P1, txt_P2;
    public int int_P1, int_P2;
    
    //結算圖及文字
    public Text howWin;
    
    //結束畫面
    public GameObject end;

    public void Start()
    {
        my_audio = GetComponent<AudioSource>();
        gameStart = true;
        end.SetActive(false);
        StartCoroutine("CountDown");
        
    }
    
    
    public void Update()
    {
        nowTxt_P1.text = int_P1.ToString();
        nowTxt_P2.text = int_P2.ToString();
        if (gameStart == true)
        {
            int i = int_P1 + int_P2;
            if (i==maxRadish)
              {
                StopCoroutine("CountDown");
                End();
                gameStart = false;
              }
        }
       
    }
    
  
    
    //倒數計時器
    IEnumerator CountDown()
    {
        m_Timer.text = string.Format("{0}:{1}", m_Min.ToString("00"), m_Sec.ToString("00"));
        m_Seconds = (m_Min * 60) + m_Sec;

        while (m_Seconds>0)
        {
            yield return new WaitForSeconds(1);
            m_Seconds--;
            m_Sec--;
            if (m_Sec < 0 && m_Min > 0)
            {
                m_Min -= 1;
                m_Sec += 59;
            }
            else if(m_Sec<0&& m_Min==0)
            {
                m_Sec = 0;
            }
            m_Timer.text = string.Format("{0}:{1}", m_Min.ToString("00"), m_Sec.ToString("00"));
        }
        yield return new WaitForSeconds(1);
        timeUp.text = "TIME'S UP!!!!";
        yield return new WaitForSeconds(1);
        End();
    }
    
    //誰得分數
    public void RadishFraction(string who,int fraction)
    {
        switch (who)
        {
            case "P1":
                int_P1 += fraction;
                break;
            case "P2":
                int_P2 += fraction;
                break;
        }
    }
    
    //結算畫面
    public void End()
    {
        my_audio.clip= ac_End;
        my_audio.Play();
        WinGame();
        txt_P1.text = int_P1.ToString();
        txt_P2.text = int_P2.ToString();
        end.SetActive(true);
    }
    
    //勝利判斷
    public void WinGame()
    {
        if (int_P1 > int_P2)
        {
            howWin.text = "P1 WIN!!!!";
         }
        else if(int_P2>int_P1)
        {
            howWin.text = "P2 WIN!!!!";
        }
        else
        {
            howWin.text = "Tie";
        }
        
    }
  
    //按鈕控制
    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
