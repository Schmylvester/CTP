using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnit : MonoBehaviour
{
    #region ChangeStats
    int max_health = 20;
    float base_hit_chance = 0.8f;
    float accuracy_mod = 0.05f;
    float dodge_mod = 0.03f;
    float min_chance = 0.2f;
    int attack_mod = 3;
    int max_attack_bonus = 3;
    float defence_mod = 2;
    int min_damage = 1;
    float agility_mod = 0.85f;
    #endregion //ChangeStats

    int health;
    [SerializeField] SpriteRenderer sprite;
    List<int> stats;
    GridManager grid;
    InputManager input;
    Cell current_cell;
    bool visible;

    int team;
    static Color[] team_colours;

    private void Awake()
    {
        team_colours = new Color[4] { Color.white, Color.grey * 1.5f, Color.cyan, new Color(1, 0.4f, 0) };
    }

    public void setUp(int _team,
        Class unit_class, UnitStats unit_stats, Cell cell, GridManager _grid, InputManager _input)
    {
        health = max_health;

        grid = _grid;
        input = _input;
        stats = new List<int>();
        sprite.sprite = unit_stats.getSprite(unit_class);
        team = _team;
        sprite.color = team_colours[team];
        unit_stats.getStats(unit_class, ref stats);
        current_cell = cell;
        move(cell);
    }
    public void move(Cell new_cell)
    {
        transform.parent = new_cell.transform;
        transform.localPosition = Vector3.zero;
        current_cell = new_cell;
        grid.unitMove(this, new_cell);
    }

    public void attack(GameUnit target)
    {
        float attack_chance = base_hit_chance;
        attack_chance += accuracy_mod * getStat(Stat.ACCURACY);
        attack_chance -= target.getStat(Stat.DODGE) * dodge_mod;
        attack_chance = Mathf.Max(attack_chance, min_chance);

        if (attack_chance >= Random.Range(0.0f, 1.0f))
        {
            int dam = 0;
            for (int i = 0; i < attack_mod; i++)
            {
                dam += getStat(Stat.ATTACK) + Random.Range(0, max_attack_bonus);
            }
            target.defend(dam);
        }
        else
        {
            Debug.Log("Attack missed.");
        }
    }

    public void defend(int damage)
    {
        damage -= (int)((float)getStat(Stat.DEFENCE) * defence_mod);
        damage = Mathf.Max(min_damage, damage);
        Debug.Log("Hit for " + damage + " damage.");
        health -= damage;

        if(health <= 0)
        {
            grid.unitKilled(this);
            Destroy(gameObject);
        }
        else
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, ((float)health / max_health));
        }
    }

    public void isVisible()
    {
        visible = false;
        if(input.getActivePlayer() == team)
        {
            visible = true;
        }
        else if(current_cell.getVisible() == Visibility.Clear)
        {
            Debug.Log("Hi I am in a visible cell");
            visible = true;
        }
        else if(current_cell.getVisible() == Visibility.Foggy)
        {
            float seen = 1.0f;
            for (int i = 0; i < getStat(Stat.AGILITY); i++)
            {
                seen *= agility_mod;
            }
            if(Random.Range(0.0f, 1.0f) < seen)
            {
                visible = true;
            }
        }

        sprite.enabled = visible;
    }

    public bool getVisible()
    {
        return visible;
    }

    #region constants
    public int getStat(Stat stat)
    {
        return stats[(int)stat];
    }
    public int getRange()
    {
        return stats[(int)Stat.ATTACK_RANGE] + stats[(int)Stat.MOVE];
    }
    public int getTeam()
    {
        return team;
    }
    public Cell getCell()
    {
        return current_cell;
    }
    #endregion
}