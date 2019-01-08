using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Speed
{
    Auto_Timer,
    Wait_For_Input,
    All_At_Once,
}

public class AllTurnActions : MonoBehaviour
{
    [HideInInspector] public bool actions_complete = true;
    int received = 0;
    List<Ability> action_list_combined = new List<Ability>();
    SetTurnActions[] players;
    [SerializeField] Speed speed;
    InputManager input;
    bool input_received = false;
    [SerializeField] PlayerGrid[] grids;
    [SerializeField] ActionFeedbackText feedback_text;

    private void Start()
    {
        input = FindObjectOfType<InputManager>();
        players = GetComponents<SetTurnActions>();
    }

    public void addActions(List<Ability> actions, bool swan)
    {
        foreach (Ability a in actions)
            action_list_combined.Add(a);
        actions_complete = false;
        if (!swan)
            received++;

        if (received == 2)
        {
            sortActionQueue();
            StartCoroutine(triggerActions());
            actions_complete = true;
            received = 0;
        }
    }

    IEnumerator triggerActions()
    {
        addActionsToTracker();
        feedback_text.setCount(action_list_combined.Count);
        foreach (Ability a in action_list_combined)
        {
            if (speed == Speed.Wait_For_Input)
            {
                yield return new WaitUntil(() => !input.isButtonPressed(XboxButton.A, 0) && !input.isButtonPressed(XboxButton.A, 1));
            }
            Unit u = a.getUser();
            Unit t = a.getTarget();
            if (a as Abilities.Move != null || t == null || a.noTarget())
                t = u;

            SpriteRenderer user_sprite = grids[u.getTeam().player_id].getSprite(u);
            SpriteRenderer target_sprite = grids[t.getTeam().player_id].getSprite(t);
            Color u_c_before = user_sprite.color;
            Color t_c_before = target_sprite.color;
            user_sprite.color = new Color(0.6f, 1.0f, 0.6f);
            if (u != t)
                target_sprite.color = new Color(1.0f, 1.0f, 0.6f);

            if (u.getHealth() <= 0)
                feedback_text.printMessage("The " + u.getName() + " is dead, so we'll skip their action for now.");
            else if (t.getHealth() <= 0 && (a as Abilities.Revive) == null)
                feedback_text.printMessage(u.getName() + " wanted to do something to " + t.getName() + " but " + t.getName() + " is dead now so maybe not.");
            else if (u.getEffectActive(SingleTurnEffects.Charmed))
            {
                feedback_text.printMessage(u.getName() + " is under a charm, so they won't do anything.");
            }
            else if (u.getTaunt())
            {
                feedback_text.printMessage(u.getName() + " was taunted.");
                u.attack(null, feedback_text);
            }
            else
            {
                a.useAbility(feedback_text);
                if ((a as Abilities.Attack) != null && u.getEffectActive(SingleTurnEffects.Ambushed))
                {
                    u.modifyStat(Stat.Attack, 0.3f);
                    u.attack(u, feedback_text);
                }
            }

            if (speed == Speed.Auto_Timer)
                yield return new WaitForSeconds(3.0f);
            else if (speed == Speed.Wait_For_Input)
            {
                yield return new WaitUntil(() => input.buttonDown(XboxButton.A, 0) || input.buttonDown(XboxButton.A, 1));
            }
            user_sprite.color = u_c_before;
            target_sprite.color = t_c_before;
        }
        yield return new WaitForSeconds(1.0f);
        action_list_combined.Clear();
        players[0].turnOver();
        players[1].turnOver();
        feedback_text.clear();
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
                else if (speed == best_speed)
                {
                    best_idx = Random.Range(0, 2) == 0 ? best_idx : i;
                }
            }
            return_list.Add(list[best_idx]);
            list.RemoveAt(best_idx);
        }

        return return_list;
    }

    void addActionsToTracker()
    {
        List<Ability> sorted_action_list = new List<Ability>();
        foreach (Ability a in action_list_combined)
        {
            if (a.getUser().getTeam().player_id == 0)
                sorted_action_list.Add(a);
        }
        foreach (Ability a in action_list_combined)
        {
            if (a.getUser().getTeam().player_id == 1)
                sorted_action_list.Add(a);
        }

        GameTracker tracker = FindObjectOfType<GameTracker>();
        foreach (Ability a in sorted_action_list)
        {
            ActionData data = new ActionData();
            if (a as Abilities.Attack != null)
            {
                data.action_idx = 0;
            }
            else if (a as Abilities.Move != null)
            {
                data.action_idx = 4;
                data.target_x = (short)((a as Abilities.Move).direction == 'f' ? 1 : 0);
                data.target_y = 0;
                data.target_team = 0;
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (a.ability_name == a.getUser().getAbility(i).ability_name)
                    {
                        data.action_idx = (short)(i + 1);
                    }
                }
            }
            if (a.noTarget())
            {
                data.target_x = 0;
                data.target_y = 0;
                data.target_team = 0;
            }
            else if (a as Abilities.Move == null)
            {
                data.target_x = (short)a.getTarget().grid_pos.x;
                data.target_y = (short)a.getTarget().grid_pos.y;
                data.target_team = a.getTarget().getTeam().player_id;
            }
            tracker.addAction(data);
        }
    }
}
