using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CodeMonkey.Utils;
public class chrarcterBattle2 : MonoBehaviour
{

    private Vector3 slidTargetposition;
    private Character_Base Character_Base;
    private State state;
    private Action onSlideComplete;
    private bool isPlayerTeam;
    private HealthSystem healthSystem;
    private World_Bar healthBar;
    private enum State
    {
        Idle,
        Sliding,
        Busy
    }

    // Start is called before the first frame update
    private void Awake()
    {
        Character_Base = GetComponent<Character_Base>();
    }
    private void Start()
    {
        // Character_Base.PlayAnimMove(new Vector3(1, 0));

        state = State.Idle;
    }
    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Sliding:
                float SlidingSpeed = 10f;
                transform.position += (slidTargetposition - Getpostion()) * SlidingSpeed * Time.deltaTime;
                float reachedDistance = 1f;
                if (Vector3.Distance(Getpostion(), slidTargetposition) < reachedDistance)
                {
                    //arrivde at slide 
                    //  transform.position = slidTargetposition;
                   // onSlideComplete();
                }
                break;
            case State.Busy:
                break;


        }
    }


    // Update is called once per frame

    public void setup(bool isPlayerteam)
    {
        this.isPlayerTeam = isPlayerteam;
        if (isPlayerteam)
        {
            Character_Base.SetAnimsSwordTwoHandedBack();
            Character_Base.GetMaterial().mainTexture = battleHandler2.GetInstance().playerSpritesheet;
        }
        else
        {
            Character_Base.SetAnimsSwordShield();
            Character_Base.GetMaterial().mainTexture = battleHandler2.GetInstance().enemySpritesheet;
        }
        healthSystem = new HealthSystem(100);
        healthBar = new World_Bar(transform, new Vector3(0, 10), new Vector3(1, 0.2f), Color.grey, Color.red, 1f, 100, new World_Bar.Outline { color = Color.black, size = 0.2f });
        healthSystem.OnHealthChanged += HealthSystem_OnHealthchanged;

        playAnimIdle();
    }
    private void HealthSystem_OnHealthchanged(object sender, EventArgs e)
    {
        healthBar.SetSize(healthSystem.GetHealthPercent());
    }
    private void playAnimIdle()
    {
        if (isPlayerTeam)
            Character_Base.PlayAnimIdle(new Vector3(1, 0));
        else
            Character_Base.PlayAnimIdle(new Vector3(-1, 0));

    }
    public void Damage(chrarcterBattle2 attacker, int damageAmount)
    {
        CodeMonkey.Utils.UtilsClass.ShakeCamera(0.4f, .1f);
        healthSystem.Damage(damageAmount);
        Vector3 dirFromAttacker = (Getpostion() - attacker.Getpostion()).normalized;
        // CodeMonkey.CMDebug.TextPopup("Hit " + healthSystem.GetHealthAmount(), Getpostion()); 
        DamagePopup.Create(Getpostion(), damageAmount, false).transform.localScale= new Vector3(0.26f, 0.26f);
        Character_Base.SetColorTint(new Color(1, 0, 0, 1f));
        Blood_Handler.SpawnBlood(Getpostion() , -dirFromAttacker);


        if (healthSystem.IsDead())
            Character_Base.PlayAnimLyingUp();



    }
    public bool isDead()
    {
        return healthSystem.IsDead();
    }
    public Vector3 Getpostion()
    {
        return transform.position;
    }
    public void Attack(chrarcterBattle2 targetcharacterBattle,  bool Damage ,  Action onAttackComplete)
    {   //if(targetcharacterBattle==)
        slidTargetposition = targetcharacterBattle.Getpostion() + (Getpostion() - targetcharacterBattle.Getpostion()).normalized * 10f;
        Vector3 startingposition = Getpostion();

        Vector3 attackDir = (targetcharacterBattle.Getpostion() - Getpostion()).normalized;
        Character_Base.PlayAnimAttack(attackDir, () =>
        {
            // Targit Hit 
             if (Damage)
            targetcharacterBattle.Damage(this, 40);
        }, ()=>{ Character_Base.PlayAnimIdle(attackDir);
            onAttackComplete(); });

    
        /*Vector3 attackDir = (targetcharacterBattle.Getpostion() - Getpostion()).normalized; 
        Character_Base.PlayAnimAttack(attackDir, null , () =>
        {
            Character_Base.PlayAnimIdle(attackDir);
            onAttackComplete();
        });*/
    }


    private void SlideToPosition(Vector3 SlideTargetPosition, Action onSlideComplete)
    {
        this.slidTargetposition = SlideTargetPosition;
        this.onSlideComplete = onSlideComplete;
        state = State.Sliding;
        if (SlideTargetPosition.x > 0)
        {
            Character_Base.PlayAnimSlideRight();

        }
        else
        {
            Character_Base.PlayAnimSlideLeft();

        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.name == "Obstacles")
        {
            transform.position -=battleHandler2.Direction;
            Debug.Log("ffeeee");
        }
        if (collision.gameObject.name == "enemy")
        {
            transform.position -= battleHandler2.Direction;
            Debug.Log("fff");
        }
    }

}
