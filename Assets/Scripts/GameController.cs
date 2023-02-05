using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    
   //計時器
    public float m_Seconds,sec; //總秒數
    //public Text m_Timer;//時間文字呈現
    public Text timeUp;//時間到
    public Image timeImage;
    public int fractionDoudleSecond = 20;
    
    //場上蘿蔔
    public int maxRadish;
    public int countRadish;
    
    //遊戲開始
    public bool gameStart;
    
    //玩家分數
    public Text nowTxt_P1, nowTxt_P2;
    public int int_P1, int_P2;
    
    //結算圖及文字
    public Image image_whoWin;
    public Text txt_whoWin;
    public Sprite P1_win;
    public Sprite P2_win;
    public Sprite tie;
    
     //開始畫面
     public GameObject startmenu;
     //遊戲畫面
     public GameObject game;
    //結束畫面
    public GameObject end;
    
   

    public void Start()
    {
        sec = m_Seconds;
      startmenu.SetActive(true);
        game.SetActive(false);
        end.SetActive(false);
 }

    public void click()
    {
        gameStart = true; 
        startmenu.SetActive(false);
        game.SetActive(true);
        AudioManagerScript.Instance.PlayAudioClip("click");
        StartCoroutine("CountDown");
        AudioManagerScript.Instance.PlayAudioClip("StartBgm");
        timeImage.color = new Color(255, 255,255);

    }
    
    public void Update()
    {
        if (gameStart == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameStart = true;
                click();
            }
        }
        nowTxt_P1.text = int_P1.ToString();
        nowTxt_P2.text = int_P2.ToString();
        if (gameStart == true)
        {
          
            if (countRadish==maxRadish)
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
        yield return new WaitForSeconds(1);
        m_Seconds--;
     //  m_Timer.text = string.Format("{0}:{1}", m_Min.ToString("00"), m_Sec.ToString("00"));
     //  m_Seconds = (m_Min * 60) + m_Sec;

     while (m_Seconds>0)
        {
         yield return new WaitForSeconds(1);
         m_Seconds--;
         float f = m_Seconds / sec;
         timeImage.fillAmount = f;
         Debug.Log(f);
         if (f== 0.3f)
         {
             timeImage.color = new Color(255, 0, 0);
         }
         //      m_Sec--;
         //      if (m_Sec < 0 && m_Min > 0)
         //      {
         //          m_Min -= 1;
         //          m_Sec += 59;
         //      }
         //      else if(m_Sec<0&& m_Min==0)
         //      {
         //          m_Sec = 0;
         //      }
         //      m_Timer.text = string.Format("{0}:{1}", m_Min.ToString("00"), m_Sec.ToString("00"));
        }
        yield return new WaitForSeconds(1);
        timeUp.text = "TIME'S UP!!!!";
        yield return new WaitForSeconds(1);
        End();
    }
    
    //誰得分數
    public void RadishFraction(PlayerType who,int fraction)
    {
        if (m_Seconds < fractionDoudleSecond)
        {
            fraction = fraction * 2;
        }
        switch (who)
        {
            case PlayerType.Player1:
                countRadish++;
                int_P1 += fraction;
                break;
            case PlayerType.Player2:
                countRadish++;
                int_P2 += fraction;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(who), who, null);
        }
    }
    
    //結算畫面
    public void End()
    {
        AudioManagerScript.Instance.Stop(0);
        AudioManagerScript.Instance.PlayAudioClip("End");
        WinGame();
        end.SetActive(true);
    }
    
    //勝利判斷
    public void WinGame()
    {
        if (int_P1 > int_P2)
        {
            image_whoWin.sprite = P1_win;
            txt_whoWin.text = int_P1.ToString();
        }
        else if(int_P2>int_P1)
        {
            image_whoWin.sprite = P2_win;
            txt_whoWin.text = int_P2.ToString();
        }
        else
        {
            image_whoWin.sprite = tie;
            txt_whoWin.text = int_P1.ToString();
        }
        
    }
  
    //按鈕控制
    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
