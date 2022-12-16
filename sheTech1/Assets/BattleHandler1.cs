using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class BattleHandler1 : MonoBehaviour
{
    private HealthSystem healthSystem;
    private CharactereBattle1 playerchrarcterBattle;
    private CharactereBattle1 enemyCharacterBattle;

    private CharactereBattle1 activeCharacterBattle;
    private static BattleHandler1 instance;
    public static BattleHandler1 GetInstance()
    {
        return instance;
    }
    [SerializeField] private Transform pfCharacterBattle;
    public Texture2D playerSpritesheet;
    public Texture2D enemySpritesheet;
    private State state;


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
        playerchrarcterBattle = SpawnCharacter(true);

        enemyCharacterBattle = SpawnCharacter(false);
        SetActiveCharacterBattle(playerchrarcterBattle);
        state = State.WaitingforPlayer;


    }
    private void Update()
    {
        if (state == State.WaitingforPlayer)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                state = State.busy;
                playerchrarcterBattle.Attack(enemyCharacterBattle, () =>
                {
                    // state = State.WaitingforPlayer;
                    chooseNextCharaterBattle();
                });
            }
    }

    private CharactereBattle1 SpawnCharacter(bool isPlayerTeam)
    {
        Vector3 position;
        if (isPlayerTeam)
        {
            position = new Vector3(-50, 0);

        }
        else
        {
            position = new Vector3(+50, 0);

        }
        Transform characterTransform = Instantiate(pfCharacterBattle, position, Quaternion.identity);
        CharactereBattle1 characterBattle = characterTransform.GetComponent<CharactereBattle1>();
        characterBattle.setup(isPlayerTeam);

        return characterBattle;
    }
    private void SetActiveCharacterBattle(CharactereBattle1 charactereBattle)
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
            enemyCharacterBattle.Attack(playerchrarcterBattle, () =>
            {
                // state = State.WaitingforPlayer;
                chooseNextCharaterBattle();
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
        if (playerchrarcterBattle.isDead())
        {
            // player dead , enemy wins

            //CodeMonkey.CMDebug.TextPopupMouse("Enemy Wins!");
            BattleOverWindow1.Show_Static("Enemy Wins!");
            return true;
        }
        if (enemyCharacterBattle.isDead())
        {
            // player wins  , enemy dead
            //CodeMonkey.CMDebug.TextPopupMouse("player Wins!");
            BattleOverWindow1.Show_Static("player Wins!");
            return true;
        }


        return false;
    }
}

