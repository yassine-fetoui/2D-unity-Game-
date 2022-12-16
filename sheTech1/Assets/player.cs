using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CodeMonkey.Utils;

public class player : MonoBehaviour
{
    private Character_Base Character_Base;
    public Texture2D playerSpritesheet;
    private Vector2 velocity;
    public Vector3 Direction;
    private bool hasMoved;
    private HealthSystem healthSystem;
    private World_Bar healthBar;
    public Transform enemey;
    private Vector3 v;
    private enum State
    {
        waitingForPlayer,
        Busy
    }
    private State state;

    // Start is called before the first frame update
    private void Awake()
    {
        Character_Base = GetComponent<Character_Base>();
    }
    void Start()
    {
        state = State.waitingForPlayer;
        Character_Base.SetAnimsSwordShield();
        Character_Base.GetMaterial().mainTexture = playerSpritesheet;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && state == State.waitingForPlayer)
        {
            state = State.Busy;
            Attack(() => { state = State.waitingForPlayer; });
       
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
         Character_Base.PlayAnimIdle(Direction);

        /* if (Input.GetKeyDown(KeyCode.Space))
         {
             Character_Base.PlayAnimAttack(Direction, () =>
             {
                 // Targit Hit 

                 Damage(40);
             }, null);
         }*/
        
    }
    private void MoveByDirection()
    {
        v = new Vector3(0.3f, 0.3f, 0);
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
        transform.position += Direction;


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.name == "Obstacles")
        {
            transform.position -= Direction;
            Debug.Log("ffeeee");
        }
         if (collision.gameObject.tag == "enemy")
        {
            transform.position -= Direction;
            Debug.Log("fff");
        }
    }
    private void Damage(int damageAmount)
    {
        CodeMonkey.Utils.UtilsClass.ShakeCamera(1f, .1f);
        healthSystem.Damage(damageAmount);
        // Vector3 dirFromAttacker = (Getpostion() - attacker.Getpostion()).normalized;
        // CodeMonkey.CMDebug.TextPopup("Hit " + healthSystem.GetHealthAmount(), Getpostion()); 
        DamagePopup.Create(Direction, damageAmount, false);
        Character_Base.SetColorTint(new Color(1, 0, 0, 1f));
        Blood_Handler.SpawnBlood(Direction, Direction);


        if (healthSystem.IsDead())
            Character_Base.PlayAnimLyingUp();


        /*
          private void UpdateFog()
           {
               Vector3 currentPlayerPos = tilmapFog.WorldToCell(transform.position);//cell  position  converts  to world  position 

               for (int i = -3; i < 3; i++)
               {
                   for (int j = -3; j < 3; j++)
                   {

                       tilmapFog.SetTile(currentPlayerPos + new Vector3(i, j, 0), null);
                   }
               }
           }
       }

          */
    }
    private void Attack(Action OncomleteAttack)
    {
       
        
            Character_Base.PlayAnimAttack((Direction * 2).normalized, null, 
                () => { Character_Base.PlayAnimIdle(Direction);  });
        OncomleteAttack();


        // yield return new WaitForSeconds(2f);

    }
    private IEnumerator wait()
    {
        yield return new WaitForSeconds(1f);
    } 
}

 