using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllTurnActions : MonoBehaviour
{
    [HideInInspector] public bool actions_complete = true;
    int received = 0;
    List<Ability> action_list_combined = new List<Ability>();
    SetTurnActions[] players;

    private void Start()
    {
        players = GetComponents<SetTurnActions>();
    }

    public void addActions(List<Ability> actions)
    {
        foreach (Ability a in actions)
            action_list_combined.Add(a);
        actions_complete = false;
        received++;

        if(received == 2)
        {
            sortActionQueue();
            StartCoroutine(triggerActions());
            actions_complete = true;
            received = 0;
        }
    }

    IEnumerator triggerActions()
    {
        foreach (Ability a in action_list_combined)
        {
            Unit u = a.getUser();

            if (u.getHealth() <= 0)
                Debug.Log("The " + u.getName() + " is dead, so this action can't happen");
            else if (a.getTarget().getHealth() <= 0)
                Debug.Log("The target is dead now, so you can't use this ability on them.");
            else if (u.getEffectActive(SingleTurnEffects.Charmed))
            {
                Debug.Log("This action can't happen because the " + u.getName() + " is under a charm");
            }
            else if (a.getUser().getTaunt())
            {
                Debug.Log(u.getName() + " was taunted.");
                u.attack(null);
            }
            else
            {
                if (a.getTarget() != null)
                {
                    Debug.Log(u.getName() + " used " + a.ability_name + " on " + a.getTarget().getName());
                }
                else
                {
                    Debug.Log(u.getName() + " used " + a.ability_name);
                }
                a.useAbility();
                if ((a as Abilities.Attack) && u.getEffectActive(SingleTurnEffects.Ambushed))
                {
                    u.modifyStat(Stat.Attack, 0.3f);
                    u.attack(u);
                }
            }

            yield return new WaitForSeconds(1.0f);
        }
        action_list_combined.Clear();
        players[0].turnOver();
        players[1].turnOver();
        yield return null;
    }

    void sortActionQueue()
    {
        List<Ability> hp_actions = new List<Ability>();
        List<Ability> lp_actions = new List<Ability>();
        foreach (Ability a in action_list_combined)
        {
            if (a.isHighPriority())
                hp_actions.Add(a);
            else
                lp_actions.Add(a);
        }
        hp_actions = sortBySpeed(hp_actions);
        lp_actions = sortBySpeed(lp_actions);

        action_list_combined.Clear();
        foreach (Ability a in hp_actions)
            action_list_combined.Add(a);
        foreach (Ability a in lp_actions)
            action_list_combined.Add(a);
    }

    List<Ability> sortBySpeed(List<Ability> list)
    {
        List<Ability> return_list = new List<Ability>();

        while (list.Count > 0)
        {
            int best_idx = -1;
            float best_speed = Mathf.NegativeInfinity;
            for (int i = 0; i < list.Count; i++)
            {
                int speed = list[i].getUser().getStat(Stat.Speed);
                if (speed > best_speed)
                {
                    best_speed = speed;
                    best_idx = i;
                }
            }
            return_list.Add(list[best_idx]);
            list.RemoveAt(best_idx);
        }

        return return_list;
    }
}
