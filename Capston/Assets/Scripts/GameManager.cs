using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum GameState//게임 기본 세팅
{
    menu,
    inGame,
    gameover
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get { return _instance; } }
    public bool isPause = false;
    public GameState currentGameState = GameState.menu; // 게임 시작시 설정 값 변수.
    private static GameManager _instance;
    private GameObject player;
    private SpriteRenderer playerRenderer;

    private int activelevel = 0; // 레벨설정
    //private int beforelevel; 추후 사용예정
    private string myname; // 캐릭터 이름
    private int maxHp = 0; // 최대 체력
    private int maxMp = 0; // 최대 마나
    private int maxExp = 0; // 최대 경험치
    private double maxCheck = 0; // 최대경험치 x 1.2
    private int HP = 0; // 현재 체력
    private int MP = 0; // 현재 마나
    private int STR = 0;  // 힘( 공격력 체력증가)
    private int INT = 0; //  지능( 주문력 마나 증가)
    private int FIT = 0; // 체력( 이속 및 체력 마나 회복량 증가)
    private int EXP = 0;  //경험치
    private int APPoint = 0;
    private int AD = 0;
    private int AP = 0;
    private int NormalWarriorsHP = 0;
    private int NormalWarriorsAD = 0;
    private int NormalMagiciansHP = 0;
    private int NormalMagiciansAD = 0;
    private int PotionHeal = 0;





    public void StartGame()//게임 시작 함수
    {
        SetGameState(GameState.inGame);
    }
    public void GameOver()//게임 종료 함수
    {

    }
    public void BackToMenu() // 메뉴 함수
    {

    }
    void Start()
    {
        //currentGameState = GameState.menu;// 시작시 게임상태 변경
        StartGame();
    }

    /*void Update() //test 세팅
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            EXP += 200;
            /*Debug.Log(AD);
            Debug.Log(AP);
            Debug.Log(NormalWarriorsHP);
            setWarriosMonsterHP(AP);
            Debug.Log(AD);

        }
        setLevel();
        
        APAttack();
        
    }*/
    void SetGameState(GameState newGameState)// 게임 상태
    {
        if (newGameState == GameState.menu) // 새게임시
        {
            
        }
        else if (newGameState == GameState.inGame) // 게임 진행시
        {
            activelevel = 1; // 레벨 세팅
            myname = "Charater"; // 이름 세팅
            maxHp += 50; // 체력세팅
            maxMp += 200; // 마나 세팅
            maxExp += 300; // 최대경험치 세팅(레벨업시 증가)
            HP += maxHp; // 시작시 최대체력으로 세팅
            MP += maxMp; // 시작시 최대마나로 세팅
            STR += 2; // 힘 세팅
            INT += 10; // 지능 세팅
            FIT += 2; // 체력 세팅
            EXP += 0; // 경험치 세팅
            NormalWarriorsHP += 30;
            NormalWarriorsAD += 3;
            NormalMagiciansHP += 18;
            NormalMagiciansAD += 5;
            PotionHeal += 10;


        }
        else if (newGameState == GameState.gameover) // 게임 종료시
        {

        }
    }

    public int getLevel() // 레벨불러오기
    {
        return activelevel;
    }

    public void setLevel() // 레벨업시 사용
    {
        if (EXP >= maxExp) //현재 경험치가 최대 보다 높을시(레벨업) 레벨 +1, 현재 경험치 0으로 초기화
        {
            
            activelevel += 1;
            EXP = 0;
            APPoint += 3; //레벨업 한 상태이기 때문에 스탯 포인트 3으로 설정.
            if(activelevel % 10 <= 0) //레벨 10당 1.5배로 증가
            {
                maxCheck = maxExp * 1.5;
            }
            else// 아니면 1.2배
            {
                maxCheck = maxExp * 1.2;
            }
            
            maxExp = (int)maxCheck;
        }
        
    }
    //공격력, 마력
    public void ADAttackFirst() // 1타
    {
        AD = STR * 1;
    }
    public void ADAttackSecond() // 2타
    {
        AD = STR * 2;
    }
    public void ADAttackThird() // 3타
    {
        AD = STR * 3;
    }
    public void APAttack() //스킬 데미지
    {
        AP = INT * 3;
    }
    //포션을 사용한다면 HP,MP쪽 둘다 넣어줘야함.
    public int getPotionHeal()
    {
        return PotionHeal;
    }
    
    public int usePotionHealHP()//사용시  HP오름
    {
        if (HP+PotionHeal >= maxHp) // 만약 HP가 45인데 포션 사용하면 55가 되어버리므로 maxHp를 넘지 못하게 함.
        {
            HP = maxHp; // HP를 최대 HP로 설정.
        }
        else // 아니면 HP회복.
        {
            HP += PotionHeal;
        }
        return HP;
    }
    public int usePotionHealMP()//사용시 MP오름
    {
        
        if (MP + PotionHeal >= maxMp) // 만약 HP가 195인데 포션 사용하면 205가 되어버리므로 maxHp를 넘지 못하게 함.
        {
            MP = maxMp; //  MP를 최대 MP로 설정.
        }
        else // 아니면 MP회복.
        {
            MP += (PotionHeal + PotionHeal);
        }
        return MP;
    }

    //스텟업관련
    public void UpSTR() //STR스텟업
    {
        if (APPoint <= 0)
        {
            return;
        }
        else
        {
            STR += 1;
            APPoint -= 1;
        }
    }
    public void UpINT() //INT스텟업
    {
        if (APPoint <= 0)
        {
            return;
        }
        else
        {
            INT += 1;
            APPoint -= 1;
        }
    }
    public void UpFIT() //FIT스텟업
    {
        if (APPoint <= 0)
        {
            return;
        }
        else
        {
            FIT += 1;
            maxHp += 10;
            maxMp += 10;
            APPoint -= 1;
        }
    }
    // 몬스터 관련
    public int getWarriosMonsterAD()// 전사 몬스터 공격력 
    {
        return NormalWarriorsAD;
    }
    public int getWarriosMonsterHP()// 전사 몬스터 체력
    {
        return NormalWarriorsHP;
    }
    public int setWarriosMonsterHP(int Damage) //전사몬스터 공격 받았을시
    {
        
        if (NormalWarriorsHP - Damage <= 0) // 체력보다 데미지가 더 많을시
        {
            NormalWarriorsHP = 0; //체력을 0으로 설정
        }
        else //아니면 HP에 데미지를 빼줌
        {
            NormalWarriorsHP -= Damage;
        }
        return NormalWarriorsHP;
    }
    public int getMagicianMonsterAD()
    {
        return NormalMagiciansAD;
    }
    public int getMagicianMonsterHP()
    {

        return NormalMagiciansHP;
    }
    public int setMagicianMonsterHP(int Damage) //마법사 몬스터 공격시
    {

        if (NormalMagiciansHP - Damage <= 0) //체력보다 데미지가 더 많을시
        {
            NormalMagiciansHP = 0; //체력을 0으로 설정
        }
        else // 아니면 HP에 데미지를 빼줌
        {
            NormalMagiciansHP -= Damage;
        }
        return NormalMagiciansHP;
    }


    //스텟 세팅관련
    public string getName() // 이름 불러오기
    {
        return myname;
    }

    public int getExp() // 현재 경험치 불러오기
    {
        return EXP;
    }

    public int getmaxExp() // 최대 경험치 불러오기
    {
        return maxExp;
    }

    public int getexp(int newExp) // 얻은 경험치 불러오기
    {
        EXP += newExp;
        return EXP;
    }
    public int getHp() //HP불러오기
    {
        return HP;
    }
    public int getmaxHp() //MAX HP불러오기
    {
        return maxHp;
    }
    public int getMp() //MP 불러오기
    {
        return MP;
    }
    public int getmaxMp() //MAX MP 불러오기
    {
        return maxMp;
    }
    public int getSTR() // STR값 불러오기
    {
        return STR;
    }
    public int getINT() // INT값 불러오기
    {
        return INT;
    }
    public int getFIT() // FIT값 불러오기
    {
        return FIT;
    }
    public int getAPPoint() // AP값 불러오기
    {
        return APPoint;
    }
    public int setSTR(int newSTR) // STR 증가값 설정하기
    {
        STR += newSTR;
        return STR;
    }
    public int setINT(int newINT) // INT 증가값 설정하기
    {
        INT += newINT;
        return INT;
    }
    public int setFIT(int newFIT) // FIT 증가값 설정하기
    {
        FIT += newFIT;
        return FIT;
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;

        DontDestroyOnLoad(this.gameObject);

        player = GameObject.FindWithTag("Player");
        playerRenderer = player.GetComponent<SpriteRenderer>();

        Screen.fullScreen = true;
    }

    //private void Update()
    //{
    //    //전체화면 토글
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //        Screen.fullScreen = !Screen.fullScreen;
    //    if (Input.GetButtonDown("z"))// z키 입력시 게임 시작
    //    {
    //        StartGame();
    //    }
       
    //}
}

public class GameContorl
{
    public void IsPause()
    {
        GameManager.Instance.isPause = true;
    }
}
