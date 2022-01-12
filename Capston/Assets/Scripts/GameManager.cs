using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum GameState
{
    menu,
    inGame,
    gameover
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get { return _instance; } }
    public bool isPause = false;
    public GameState currentGameState = GameState.menu; // 게임상태 설정
    private static GameManager _instance;
    private GameObject player;
    private SpriteRenderer playerRenderer;

    private int activelevel = 0; // 레벨설정
    
    private string myname; // 닉네임설정
    private int maxHp = 0; // 최대체력 설정
    private int maxMp = 0; // 최대마나 설정
    private int maxExp = 0; // 레벨업에 필요한 경험치 설정
    private double maxCheck = 0; // 레벨업시 필요경험치 = 경험치 x 1.2
    private int HP = 0; // 현재체력
    private int MP = 0; // 현재마나
    private int STR = 0;  // 공격력(평타 공격)
    private int INT = 0; //  주문력(스킬 공격력)
    private int FIT = 0; // 캐릭터 체력 마나 스텟
    private int EXP = 0;  //현재경험치
    private int APPoint = 0;
    private int AD = 0;
    
    private int AP = 0;
    private int NormalWarriorsHP = 0;
    private int NormalWarriorsAD = 0;
    private int NormalMagiciansHP = 0;
    private int NormalMagiciansAD = 0;





    public void StartGame()//게임시작시
    {
        SetGameState(GameState.inGame);
    }
    public void GameOver()//게임 끝날시
    {

    }
    public void BackToMenu() // 메뉴로 돌아갈시
    {

    }
    void Start()
    {
        //currentGameState = GameState.menu;// 게임시작시 메뉴로 설정. (차후사용)
        StartGame();
    }

    void Update() //test 세팅
    {
        AD = 10;
        if (Input.GetKeyDown(KeyCode.Z))
        {
            EXP += 200;
            Debug.Log(HP);
            PlayerDamage(NormalMagiciansAD);


        }
        setLevel();
        setPlayerHP(HP);
        APAttack();
        
    }
    void SetGameState(GameState newGameState)// 게임상태 설정
    {
        if (newGameState == GameState.menu) // 메뉴일때
        {
            
        }
        else if (newGameState == GameState.inGame) // 게임 시작했을때
        { 
            activelevel = 1; // 레벨 설정
            myname = "Charater"; // 닉네임 설정
            maxHp += 50; // 최대 체력 설정.
            maxMp += 200; // 최대마나
            maxExp += 300; // 1랩때 최대 경험치 
            HP += maxHp; // 초기 체력 설정
            MP += maxMp; // 초기 마나 설정
            STR += 2; // 초기 공격력 설정
            INT += 10; // 초기 주문력 설정
            FIT += 2; // 초기 체력 마나 스텟 설정
            EXP += 0; // 초기 경험치 세팅
            NormalWarriorsHP += 30;
            NormalWarriorsAD += 3;
            NormalMagiciansHP += 18;
            NormalMagiciansAD += 5;


        }
        else if (newGameState == GameState.gameover) // 게임 끝날시
        {

        }
    }

    public int getLevel() // 레벨 불러오기
    {
        return activelevel;
    }

    public void setLevel() // 레벨 세팅
    {
        if (EXP >= maxExp) //현재 경험치가 최대경험치 이상일때 레벨 +1
        {
            
            activelevel += 1;
            EXP = 0;
            APPoint += 3; //스텟 포인트 3추가
            HP = maxHp;
            MP = maxMp;
            if(activelevel % 10 <= 0) // 10렙당 경험치 1.5배
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
    //플레이어 공격 분리
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
    public void APAttack() // 주문력
    {
        AP = INT * 3;
    }


    //포션사용시 HP,MP 따로 증가하게 설정
    public int usePotionHealHP(int PotionHeal)//사용시 HP 증가
    {
        if (HP+PotionHeal >= maxHp) // HP+힐량이 최대체력보다 높을시 현재 체력을 최대 체력으로 설정.
        {
            HP = maxHp; 
        }
        else // 아니면 HP에 힐량 추가
        {
            HP += PotionHeal;
        }
        return HP;
    }
    public int usePotionHealMP(int PotionHeal)//사용시 MP 증가
    {
        
        if (MP + PotionHeal >= maxMp) // MP+힐량이 최대마나보다 높을시 현재 마나을 최대 마나으로 설정.
        {
            MP = maxMp; 
        }
        else // 아니면 MP에 힐량 추가
        {
            MP += PotionHeal;
        }
        return MP;
    }

    //스텟증가 관련
    public void UpSTR() //STR증가시
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
    public void UpINT() //INT증가시
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
    public void UpFIT() //FIT증가시
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
            HP = maxHp;
            MP = maxMp;
        }
    }

    // 플레이어 피격시
    public void PlayerDamage(int PlayerHit) //PlayerHIt에는 몬스터 몬스터 공격력을 추가.
    {
        if (HP - PlayerHit >= 0)
        {
            HP = HP-PlayerHit;
            setPlayerHP(HP);
        }
        else
        {
            HP = 0;
            setPlayerHP(HP);
            Debug.Log("DIE!");
        }
    }
    public int setPlayerHP(int setHP)
    {
        HP = setHP;
        return HP;
    }




    // 몬스터 설정
    public int getWarriosMonsterAD()// 전사몬스터 공격력 설정
    {
        return NormalWarriorsAD;
    }
    public int getWarriosMonsterHP()// 전사몬스터 체력 설정
    {
        return NormalWarriorsHP;
    }
    
    public int getMagicianMonsterAD()//마법몬스터 공격력 설정
    {
        return NormalMagiciansAD;
    }
    public int getMagicianMonsterHP()//마법몬스터 체력설정
    {

        return NormalMagiciansHP;
    }
    public int setWarriosMonsterHP(int HIT)//전사몬스터 체력설정
    {
        NormalWarriorsHP = HIT;
        return NormalWarriorsHP;
    }
    public int setMagicianMonsterHP(int HIT)//마법몬스터 체력설정
    {
        NormalMagiciansHP = HIT;
        return NormalMagiciansHP;
    }
    public void setDamage(int Damage, int MonsterHP) //몬스터 피격설정
    {
        if (MonsterHP - Damage <= 0 && MonsterHP == NormalMagiciansHP) 
        {
            MonsterHP = 0; 
            setMagicianMonsterHP(MonsterHP);
        }
        else if(MonsterHP - Damage <= 0 && MonsterHP == NormalWarriorsHP) 
        {
            MonsterHP = 0; 
            setWarriosMonsterHP(MonsterHP);
        }
        else if(MonsterHP - Damage >= 0 && MonsterHP == NormalWarriorsHP) 
        {
            MonsterHP = MonsterHP - Damage;
            setWarriosMonsterHP(MonsterHP);
        }
        else if(MonsterHP - Damage >= 0 && MonsterHP ==NormalMagiciansHP)
        {
            MonsterHP = MonsterHP - Damage;
            setMagicianMonsterHP(MonsterHP);
        }
    }



    //초기 세팅
    public string getName() // 닉네임 불러오기
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

    public int getexp(int newExp) // 경험치 추가
    {
        EXP += newExp;
        return EXP;
    }
    public int getHp() //HP불러오기 및 세팅
    {
        if (HP <= 0)
        {
            HP = 0;
        }
        else if (HP >= maxHp)
        {
            HP = maxHp;
        }
        return HP;
    }
    public int getmaxHp() //MAX HP 불러오기
    {
        return maxHp;
    }
    public int getMp() //MP 불러오기 및 세팅
    {
        if( MP <= 0)
        {
            MP = 0;
        }
        else if (MP >= maxMp)
        {
            MP = maxMp;
        }
        return MP;
    }
    public int getmaxMp() //MAX MP 불러오기
    {
        return maxMp;
    }
    public int getSTR() // STR불러오기
    {
        return STR;
    }
    public int getINT() // INT불러오기
    {
        return INT;
    }
    public int getFIT() // FIT불러오기
    {
        return FIT;
    }
    public int getAPPoint() // AP불러오기
    {
        return APPoint;
    }
    public int setSTR(int newSTR) // STR 스텟 증가
    {
        STR += newSTR;
        return STR;
    }
    public int setINT(int newINT) // INT 스텟증가
    {
        INT += newINT;
        return INT;
    }
    public int setFIT(int newFIT) // FIT 스텟증가
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
    //    
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //        Screen.fullScreen = !Screen.fullScreen;
    //    if (Input.GetButtonDown("z"))
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
