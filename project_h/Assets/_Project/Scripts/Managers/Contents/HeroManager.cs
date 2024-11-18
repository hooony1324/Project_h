using UnityEngine;

public class HeroManager
{
    public Hero MainHero { get; private set;}


    public void SetMainHero(Hero hero)
    {
        //TODO : if MainHero와 hero가 다르면 changeHero
        MainHero = hero;
    }

    public void TeleportHero(Vector3 position)
    {
        MainHero.Movement.AgentEnabled = false;
        MainHero.transform.position = position;

        MainHero.Movement.AgentEnabled = true;
    }
}