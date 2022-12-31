using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class battleHandler2: MonoBehaviour
{
    [SerializeField] private GameObject camera;
    private List<Vector3> pos; 
    private HealthSystem healthSystem;
    private chrarcterBattle2 playerchrarcterBattle;
    private chrarcterBattle2 enemyCharacterBattle;
    private Vector2 velocity;
    private bool hasMoved;
    public static Vector3 Direction;
    private chrarcterBattle2 activeCharacterBattle;
    private static battleHandler2 instance;
    private bool  Damage;
    private bool nextEnemy=true; 
    public static battleHandler2 GetInstance()
    {
        return instance;
    }
    [SerializeField] private Transform pfCharacterBattle;
    public Texture2D playerSpritesheet;
    public Texture2D enemySpritesheet;
    private State state;
    private int killerNumber=0; 

   


    // Start is called before the first frame update
    private enum State
    {
        WaitingforPlayer,
        busy
    }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        playerchrarcterBattle = SpawnCharacter(true, new Vector3(-15.3f, -0.9f));
        camera.transform.SetParent(playerchrarcterBattle.transform);
        enemyCharacterBattle = SpawnCharacter(false, new Vector3(-0.25999999f, -1.76999998f, 0));

        /*
        pos.Add(new Vector3(-21, -3));
        pos.Add(new Vector3(-14, -5));
        pos.Add(new Vector3(-9, -8));*/
    

        //enemyCharacterBattle = SpawnCharacter(false, new Vector3(-21,-3));
      
      //  SetActiveCharacterBattle(playerchrarcterBattle);
        state = State.WaitingforPlayer;


    }
    private void Update()
    {
        if (state == State.WaitingforPlayer)
            if(!TestBattleOver())
         
            if ( Vector3.Distance( enemyCharacterBattle.transform.position, playerchrarcterBattle.transform.position) <1f  )
            {
                Damage = true;
                state = State.busy;
                enemyCharacterBattle.Attack(playerchrarcterBattle, true, () =>
                {
                    // state = State.WaitingforPlayer;
                    state = State.WaitingforPlayer;
                });

            }

            else
                Damage = false; 



        if (Input.GetKeyDown(KeyCode.Space) && !TestBattleOver())
            {
                state = State.busy;
                playerchrarcterBattle.Attack(enemyCharacterBattle, Damage,  () =>
                {
                    // state = State.WaitingforPlayer;
                    state = State.WaitingforPlayer;
                });

            }

       
        if (velocity.x == 0)
        {
            hasMoved = false;

        }
        else if (velocity.x != 0 && !hasMoved)
        {
            hasMoved = true;
            MoveByDirection();

        }
        velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
       
    }

    private chrarcterBattle2 SpawnCharacter(bool isPlayerTeam , Vector3 pos)
    {
        Vector3 position;
        if (isPlayerTeam)
        {
            position = pos;

        }
        else
        {
            position = pos;

        }
        Transform characterTransform = Instantiate(pfCharacterBattle, position, Quaternion.identity);
        chrarcterBattle2 characterBattle = characterTransform.GetComponent<chrarcterBattle2>();
        characterBattle.setup(isPlayerTeam);

        return characterBattle;
    }
    private void SetActiveCharacterBattle(chrarcterBattle2 charactereBattle)
    {
        activeCharacterBattle = charactereBattle;
    }
    private void chooseNextCharaterBattle()
    {
        if (TestBattleOver())
            return;

        if (activeCharacterBattle == playerchrarcterBattle)
        {


            state = State.busy;
            SetActiveCharacterBattle(enemyCharacterBattle);
            enemyCharacterBattle.Attack(playerchrarcterBattle,true, () =>
            {
                state = State.WaitingforPlayer;
              
            });
        }
        else
        {
            SetActiveCharacterBattle(playerchrarcterBattle);
            state = State.WaitingforPlayer;
        }
    }
    private bool TestBattleOver()
    {    
        if ( playerchrarcterBattle.isDead() ) 
        {
                nextEnemy = false;
            // player dead , enemy wins
            //Destroy(playerchrarcterBattle, 2);
             //CodeMonkey.CMDebug.TextPopupMouse("Enemy Wins!");
            BattleOverWindow1.Show_Static("Enemy Wins!");
            return true;
        }
       
        if (enemyCharacterBattle.isDead())
        {
            //enemyCharacterBattle.GetComponent<BoxCollider2D>().enabled = false;
            // player wins  , enemy dead
            //CodeMonkey.CMDebug.TextPopupMouse("player Wins!");
            Destroy(enemyCharacterBattle, 2);
            BattleOverWindow1.Show_Static("player Wins!");
            
            return true;

        }


        return false;

    }
    private void MoveByDirection()
    {
       // v = new Vector3(0.3f, 0.3f, 0);
        if (velocity.x < 0)
        {  // move left 
            if (velocity.y > 0) //  Move Upper left 
            { Direction = new Vector3(-0.5f, 0.5f); }
            else if (velocity.y < 0) // move Bottom left 
            { Direction = new Vector3(-0.5f, -0.5f); }
            else
            {
                Direction = new Vector3(-1, 0);
            }
        }





        else if (velocity.x > 0)
        {
            if (velocity.y > 0)
            {
                Direction = new Vector3(0.5f, 0.5f);

            }
            else if (velocity.y < 0)
            {
                Direction = new Vector3(0.5f, -0.5f);
            }
            else
            {
                Direction = new Vector3(1, 0);
            }




        }
        playerchrarcterBattle.transform.position += Direction;


    }
}
