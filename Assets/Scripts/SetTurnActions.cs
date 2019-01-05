using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Abilities;

public enum InputState
{
    WaitingForActionInput,
    WaitingForTargetInput,
    ActionAndTargetReceived,
    WaitingForOtherPlayer,
}

public enum PlayerActions
{
    None = -1,
    Ability_1,
    Ability_2,
    Ability_3,
    Attack,
    Move,
}

public class SetTurnActions : MonoBehaviour
{
    bool inputs_sent = false;
    Field field;
    Team team;
    InputManager input;
    InputState input_state;
    PlayerActions action_input_received = PlayerActions.None;
    int acting_unit = 0;
    [SerializeField] short player;
    [SerializeField] PlayerGrid grid;
    [SerializeField] GameObject targeter;
    [SerializeField] Transform[] target_cols;
    [SerializeField] GameObject ability_ui;
    [SerializeField] Text[] ability_texts;
    [SerializeField] AllTurnActions action_manager;
    List<Ability> action_queue;
    Unit target = null;
    char move_dir = '\0';
    short col_selected;
    Ability action;

    private void Start()
    {
        action_queue = new List<Ability>();
        if (input == null)
            input = FindObjectOfType<InputManager>();
        field = FindObjectOfType<Field>();
        team = field.getTeam(player);
        setInputState(InputState.WaitingForActionInput);
    }

    private void Update()
    {
        if (!inputs_sent)
        {
            if (team.stillInGame())
            {
                if (acting_unit < team.getUnits(true).Length)
                {
                    if (team.getUnits(true)[acting_unit].getHealth() > 0)
                    {
                        switch (input_state)
                        {
                            case InputState.WaitingForActionInput:
                                waitForActionInput();
                                break;
                            case InputState.WaitingForTargetInput:
                                waitForTargetInput();
                                break;
                            case InputState.ActionAndTargetReceived:
                                break;
                        }
                    }
                    else
                    {
                        acting_unit++;
                    }
                }
                else
                {
                    action_manager.addActions(action_queue);
                    inputs_sent = true;
                    setInputState(InputState.WaitingForOtherPlayer);
                }
            }
            else
            {
                Debug.Log("Game over and team " + (1 + (1 - player)) + " won");
                enabled = false;
            }
        }
    }

    void setInputState(InputState state)
    {
        if (state == InputState.WaitingForActionInput)
        {
            action_input_received = PlayerActions.None;
            Unit u = team.getUnits(true)[acting_unit];
            u.advanceTurn();
            grid.getSprite(u).color = Color.cyan;
            ability_ui.SetActive(true);
            showAbilityUIText(u);
            input.clearInputs();
            targeter.SetActive(false);
        }
        if (state == InputState.WaitingForTargetInput)
        {
            ability_ui.SetActive(false);
            col_selected = (short)(player * 7);
            targeter.SetActive(true);
            input.clearInputs();
        }
        if (state == InputState.ActionAndTargetReceived)
        {
            targeter.SetActive(false);
            Unit u = team.getUnits(true)[acting_unit];
            if (u.berserk)
            {
                action_input_received = PlayerActions.Attack;
            }
            else
            {
                switch (action_input_received)
                {
                    case PlayerActions.Attack:
                        action = new Attack();
                        action.setUser(u);
                        action.setTarget(target);
                        break;
                    case PlayerActions.Ability_1:
                    case PlayerActions.Ability_2:
                    case PlayerActions.Ability_3:
                        action = u.getAbility((int)action_input_received);
                        action.setTarget(target);
                        break;
                    case PlayerActions.Move:
                        action = new Move();
                        action.setUser(u);
                        (action as Move).direction = move_dir;
                        break;
                }
            }
            if (action.ability_name != "invalid" && target != null)
            {
                bool valid = (action.getRequiredTarget() && action.canUse());
                if (valid)
                {
                    action_queue.Add(action);
                    acting_unit++;
                    grid.getSprite(u).color = Color.white;
                }
                else if (!action.getRequiredTarget())
                {
                    Debug.Log("The target wasn't valid for that acion.");
                }
                else if (!action.canUse())
                {
                    Debug.Log("Hey sorry you ran out of uses for that.");
                }
            }
            else
            {
                Debug.Log("That was for some reason invalid");
            }
            setInputState(InputState.WaitingForActionInput);
            return;
        }
        input_state = state;
    }

    void waitForActionInput()
    {
        if (input.buttonDown(XboxButton.A, player) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            action_input_received = PlayerActions.Attack;
        }
        else if (input.buttonDown(XboxButton.B, player) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            action_input_received = PlayerActions.Ability_1;
        }
        else if (input.buttonDown(XboxButton.X, player) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            action_input_received = PlayerActions.Ability_2;
        }
        else if (input.buttonDown(XboxButton.Y, player) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            action_input_received = PlayerActions.Ability_3;
        }
        else if (input.buttonDown(XboxButton.RB, player) || Input.GetKeyDown(KeyCode.Alpha5))
        {
            action_input_received = PlayerActions.Move;
        }

        if (action_input_received != PlayerActions.None)
        {
            setInputState(InputState.WaitingForTargetInput);
        }
    }

    void waitForTargetInput()
    {
        target = null;
        targeter.SetActive(true);
        targeter.transform.position = target_cols[col_selected].position;
        if (input.buttonDown(XboxButton.RB, player) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            col_selected++;
            col_selected = (short)Mathf.Min(7, col_selected);
            targeter.transform.position = target_cols[col_selected].position;
            if (action_input_received == PlayerActions.Move)
            {
                move_dir = player == 0 ? 'f' : 'b';
            }
        }
        if (input.buttonDown(XboxButton.LB, player) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            col_selected--;
            col_selected = (short)Mathf.Max(0, col_selected);
            targeter.transform.position = target_cols[col_selected].position;
            if (action_input_received == PlayerActions.Move)
            {
                move_dir = player == 0 ? 'b' : 'f';
            }
        }
        if (input.buttonDown(XboxButton.A, player) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            target = field.findUnitAtPos(col_selected, 0);
        }
        if (input.buttonDown(XboxButton.B, player) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            target = field.findUnitAtPos(col_selected, 1);
        }
        if (input.buttonDown(XboxButton.X, player) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            target = field.findUnitAtPos(col_selected, 2);
        }
        if (input.buttonDown(XboxButton.Y, player) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            target = field.findUnitAtPos(col_selected, 3);
        }
        if (input.buttonDown(XboxButton.Start, player) || Input.GetKeyDown(KeyCode.Return))
        {
            targeter.SetActive(false);
            setInputState(InputState.ActionAndTargetReceived);
        }
    }

    void showAbilityUIText(Unit unit)
    {
        for (int i = 0; i < 3; i++)
        {
            Ability ability = unit.getAbility(i);
            ability_texts[i].text = ability.ability_name + '\n' + ability.getRemainingUses() + ' ' + ability.ability_description;
        }
    }

    public void turnOver()
    {
        acting_unit = 0;
        action_queue.Clear();
        inputs_sent = false;
        setInputState(InputState.WaitingForActionInput);
        target = null;
        move_dir = '\0';
    }
}