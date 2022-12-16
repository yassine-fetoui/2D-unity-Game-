using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    private Character_Base Character_Base;
    public Texture2D enemySpritesheet;
    private player player;
    private enum State
    {
        onplaying , 
        Busy 
    }  ;
    private State state; 
        
    // Start is called before the first frame update
    private void Awake()
    {
        Character_Base = GetComponent<Character_Base>();
    }
    void Start()
    {
        state=State.onplaying ;
        Character_Base.SetAnimsSwordShield();
        Character_Base.GetMaterial().mainTexture = enemySpritesheet;
    }
    
   

    // Update is called once per frame
    void Update()
    {

        if (true)
        {
            state = State.Busy;
            //Character_Base.PlayAnimIdle(player.GetComponent<player>().Direction.normalized);
            Character_Base.PlayAnimAttack(player.GetComponent<player>().Direction.normalized , null,null);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "player" && state == State.onplaying)
        { state = State.Busy;
            Character_Base.PlayAnimIdle(player.GetComponent<player>().Direction.normalized);
            Character_Base.PlayAnimAttack(player.GetComponent<player>().Direction.normalized, null, () => {state = State.onplaying ; });
        }

    }
}
