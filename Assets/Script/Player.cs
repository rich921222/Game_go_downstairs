using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
     public float moveSpeed = 7.0f;
     [SerializeField] int Hp;
     [SerializeField] Text scoreText;
     [SerializeField] GameObject HpBar;
     GameObject currentfloor;
     int score;
     float scoreTime;
     Animator anim;
     SpriteRenderer render;
     AudioSource deathSound,backgroundMusic;
     [SerializeField] GameObject replayBotton;
    // 也可以寫成 [SerializeField] float //屬性為private
    void Start()
    {
        Hp = 10;
        score = 0;
        scoreTime = 0f;
        anim = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        AudioSource[] audioSources = GetComponents<AudioSource>();
        deathSound = audioSources[0];
        backgroundMusic = audioSources[1];
        backgroundMusic.loop = true;
        backgroundMusic.Play();
    }   

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(0,movespeed*Time.deltaTime,0);
        // 改變速度 = 0.01 * (1次所需時間)*(電腦更新速度)
        // advanced computer processing velocity = 0.01 * 0.01(時間) * 100(次/時間) = 0.01
        // elementary computer processing velocity = 0.01 * 0.1 * 10 = 0.01
        if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(moveSpeed*Time.deltaTime,0,0);
            render.flipX = false;
            anim.SetBool("run",true);
        }
        else if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(-moveSpeed*Time.deltaTime,0,0);
            render.flipX = true;
            anim.SetBool("run",true);
        }
        else
        {
            anim.SetBool("run",false);
        }
        UpdateScore();
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Normal")
        {
            if(other.contacts[0].normal == new Vector2(0f,1f))
            {
                currentfloor = other.gameObject;  
                ModifyHp(1);    
                other.gameObject.GetComponent<AudioSource>().Play();         
            }
        }
        else if(other.gameObject.tag == "Nails")
        {
            if(other.contacts[0].normal == new Vector2(0f,1f))
            {
                currentfloor = other.gameObject;
                ModifyHp(-3);  
                anim.SetTrigger("hurt");   
                other.gameObject.GetComponent<AudioSource>().Play();               
            }
        }
        else if(other.gameObject.tag == "Ceiling")
        {
            currentfloor.GetComponent<BoxCollider2D>().enabled = false;
            ModifyHp(-3);  
            anim.SetTrigger("hurt");
            other.gameObject.GetComponent<AudioSource>().Play(); 
        }        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "deathline")
        {
            Die();
        }
    }
    void ModifyHp(int num)
    {
        Hp += num;
        if(Hp > 10)
        {
            Hp = 10;
        }
        else if(Hp <= 0)
        {
            Hp = 0;
            Die();
        }
        UpdateHpBar();
    }
    void UpdateHpBar()
    {
        for(int i = 0;i < HpBar.transform.childCount;i++)
        {
            if(Hp > i)
            {
                HpBar.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                HpBar.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    void UpdateScore()
    {
        scoreTime += Time.deltaTime;
        if(scoreTime > 2f)
        {
            score++;
            scoreTime = 0f;
            scoreText.text = "地下" + score.ToString() + "層";
        }
    }
    void Die()
    {
        backgroundMusic.Stop();
        deathSound.Play();
        Time.timeScale = 0f;     
        replayBotton.SetActive(true);   
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }
}
