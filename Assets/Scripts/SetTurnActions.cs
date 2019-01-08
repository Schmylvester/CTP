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
    bool changed_state_this_frame = false;
    public bool swan_active = false;
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
    [SerializeField] ActionFeedbackText feedback;
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
        team.setGridAndField(grid, field);
        foreach (Unit u in team.getUnits(true))
            u.actions = this;
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
                        setInputState(InputState.WaitingForActionInput);
                    }
                }
                else
                {
                    if (swan_active)
                    {
                        swan_active = false;
                        acting_unit = 0;
                        setInputState(InputState.WaitingForActionInput);
                    }
                    else
                    {
                        inputs_sent = true;
                        setInputState(InputState.WaitingForOtherPlayer);
                    }
                    action_manager.addActions(action_queue, swan_active);
                }
            }
            else
            {
                feedback.printMessage("Game over and team " + (1 + (1 - player)) + " won");
                enabled = false;
            }
        }
        changed_state_this_frame = false;
    }

    void setInputState(InputState state)
    {
        changed_state_this_frame = true;
        if (state == InputState.WaitingForActionInput)
        {
            if (acting_unit < team.getUnits(true).Length)
            {
                if (team.getUnits(true)[acting_unit].getName() == "Skeleton")
                {
                    setInputState(InputState.WaitingForTargetInput);
                    action_input_received = PlayerActions.Attack;
                    return;
                }
                action_input_received = PlayerActions.None;
                Unit u = team.getUnits(true)[acting_unit];
                if (u.getHealth() > 0)
                {
                    u.advanceTurn();
                    grid.getSprite(u).color = Color.cyan;
                }
                ability_ui.SetActive(true);
                showAbilityUIText(u);
                targeter.SetActive(false);
            }
        }
        if (state == InputState.WaitingForTargetInput)
        {
            target = null;
            ability_ui.SetActive(false);
            col_selected = (short)(player + 3);
            targeter.SetActive(true);
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
            if (action.ability_name != "invalid")
            {
                bool valid = (action.targetValid(feedback) && action.canUse());
                if (valid)
                {
                    action_queue.Add(action);
                    acting_unit++;
                    grid.getSprite(u).color = Color.white;
                }
                else if (!action.targetValid(feedback))
                {
                    feedback.printMessage("Player " + (player + 1) + "\nThat target isn't valid for that action", MessageType.Error);
                }
                else if (!action.canUse())
                {
                    feedback.printMessage("Player " + (player + 1) + "\nHey sorry you ran out of uses for that.", MessageType.Error);
                }
            }
            else
            {
                Debug.LogError("That was for some reason invalid");
            }
            setInputState(InputState.WaitingForActionInput);
            return;
        }
        input_state = state;
    }

    void waitForActionInput()
    {
        if (!changed_state_this_frame)
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
    }

    void waitForTargetInput()
    {
        if (!changed_state_this_frame)
        {
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
            short team = col_selected < 4 ? (short)0 : (short)1;
            short unit_x = team == 0 ? (short)(3 - col_selected % 4) : (short)(col_selected % 4);
            short unit_y = -1;
            if (input.buttonDown(XboxButton.A, player) || Input.GetKeyDown(KeyCode.Alpha1))
            {
                unit_y = 0;
                target = field.findUnitAtPos(unit_x, unit_y, team);
            }
            if (input.buttonDown(XboxButton.B, player) || Input.GetKeyDown(KeyCode.Alpha2))
            {
                unit_y = 1;
                target = field.findUnitAtPos(unit_x, unit_y, team);
            }
            if (input.buttonDown(XboxButton.X, player) || Input.GetKeyDown(KeyCode.Alpha3))
            {
                unit_y = 2;
                target = field.findUnitAtPos(unit_x, unit_y, team);
            }
            if (input.buttonDown(XboxButton.Y, player) || Input.GetKeyDown(KeyCode.Alpha4))
            {
                unit_y = 3;
                target = field.findUnitAtPos(unit_x, unit_y, team);
            }
            if (input.buttonUp(XboxButton.Start, player) || Input.GetKeyDown(KeyCode.Return))
            {
                targeter.SetActive(false);
                setInputState(InputState.ActionAndTargetReceived);
            }
        }
    }

    void showAbilityUIText(Unit unit)
    {
        for (int i = 0; i < 3; i++)
        {
            Ability ability = unit.getAbility(i);
            ability_texts[i].text = ability.ability_name + '\n' + ability.getRemainingUses() + ' ' + ability.ability_description;
            ability_texts[i].color = ability.getTextColour();
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