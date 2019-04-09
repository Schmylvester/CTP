using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum XboxButton
{
    A = 1,
    B = 2,
    X = 4,
    Y = 8,
    LB = 16,
    RB = 32,
    Back = 64,
    Start = 128,
    Left_Stick = 256,
    Right_Stick = 512,
}
public enum Axis
{
    Left_Horizontal,
    Left_Vertical,
    Triggers,
    Right_Horizontal,
    Right_Vertical,
    D_Horizontal,
    D_Vertical
}
public enum AxisState
{
    At_Rest,                //wasn't doing anything last frame, isn't doing anything this frame
    Going_Steady,           //was going last frame and is still going this frame
    Increased,              //going harder this frame
    Decreased,              //not going as hard this frame

    Rested_This_Frame,      //was going and now it's not
    Flipped_This_Frame,     //gone the whole other way
    Triggered_This_Frame,   //wasn't going and now it is

    Null                    //lol who knows?
}

public class GamePad
{
    public int buttons_last_frame = 0;
    public int buttons_this_frame = 0;
    public float[] axes_last_frame = new float[7];
    public float[] axes_this_frame = new float[7];
}

public class InputManager : MonoBehaviour
{
    [SerializeField] int max_players = 2;
    List<GamePad> controllers;
    float dead_zone = 0.3f;

    private void Start()
    {
        //destroy this if there's another one
        if (FindObjectOfType<InputManager>() != this) { Destroy(gameObject); }
        //otherwise never destroy it
        DontDestroyOnLoad(gameObject);

        //add a controller object for each player
        controllers = new List<GamePad>();
        for (int i = 0; i < max_players; i++)
        {
            controllers.Add(new GamePad());
        }
    }

    void Update()
    {
        //for everyone connected
        for (int player_idx = 0; player_idx < controllers.Count; player_idx++)
        {
            //get their axes and changes in axes
            for (int axis = 0; axis < 7; axis++)
            {
                controllers[player_idx].axes_last_frame[axis] = controllers[player_idx].axes_this_frame[axis];
                controllers[player_idx].axes_this_frame[axis] = getAxis(axis, player_idx);
            }

            //get their buttons and changes in buttons
            controllers[player_idx].buttons_last_frame = controllers[player_idx].buttons_this_frame;
            controllers[player_idx].buttons_this_frame = 0;
            for (int button = 0; button < 10; button++)
            {
                //joystick enumeration starts at 350, and each player gets 20
                //so the key is 350, plus 20 for the player index, plus whatever button you're checking for
                int key_as_int = (350 + (player_idx * 20) + button);
                if (Input.GetKey((KeyCode)key_as_int))
                {
                    // buttons are stored as an int because I think bools waste like 7 bits at a time, so if this ever ends up being scalable to many controllers it will save like 10 whole bytes or something, this means I gotta pow them, which might be worse idk
                    buttonPressed((int)Mathf.Pow(2, button), player_idx);
                }
            }
        }
    }
    /// <summary>
    /// Called when a button is pressed
    /// </summary>
    /// <param name="button">The button that was pressed</param>
    /// <param name="player_id">The player who pressed it</param>
    void buttonPressed(int button, int player_id)
    {
        if (!isButtonPressed((XboxButton)button, player_id))
            controllers[player_id].buttons_this_frame += button;
    }
    /// <summary>
    /// Checks if a button is being pressed
    /// </summary>
    /// <param name="button">The button checking</param>
    /// <param name="player_id">The player you want to know about</param>
    /// <returns></returns>
    public bool isButtonPressed(XboxButton button, int player_id)
    {
        return (controllers[player_id].buttons_this_frame / (int)button) % 2 == 1;
    }
    /// <summary>
    /// Checks whether a button was pressed this frame
    /// </summary>
    /// <param name="button">The button</param>
    /// <param name="player_id">The player</param>
    /// <returns>Press or nah?</returns>
    public bool buttonDown(XboxButton button, int player_id)
    {
        return (controllers[player_id].buttons_this_frame / (int)button) % 2 == 1
            && (controllers[player_id].buttons_last_frame / (int)button) % 2 == 0;
    }
    /// <summary>
    /// Checks if a button was released this frame
    /// </summary>
    /// <param name="button">The button</param>
    /// <param name="player_id">The player</param>
    /// <returns>Yeah or nah?</returns>
    public bool buttonUp(XboxButton button, int player_id)
    {
        return (controllers[player_id].buttons_this_frame / (int)button) % 2 == 0
            && (controllers[player_id].buttons_last_frame / (int)button) % 2 == 1;
    }
    /// <summary>
    /// Checks a controller axis and gets its state
    /// </summary>
    /// <param name="axis">The axis you want</param>
    /// <param name="player_id">The player you're checking</param>
    /// <param name="state">Pass a state in by reference and it will tell you how this axis compares to last frame</param>
    /// <returns>The axis value</returns>
    public float getAxisAndState(Axis axis, int player_id, ref AxisState state)
    {
        float this_frame = controllers[player_id].axes_this_frame[(int)axis];
        state = getAxisState(this_frame, axis, player_id);
        return this_frame;
    }
    /// <summary>
    /// Gets the axis of a player's input
    /// </summary>
    /// <param name="axis">The axis</param>
    /// <param name="player_id">The player</param>
    /// <returns>The value</returns>
    float getAxis(int axis, int player_id)
    {
        string axis_name;
        switch (axis)
        {
            case (int)Axis.Left_Horizontal:
                axis_name = "L_Horizontal";
                break;
            case (int)Axis.Left_Vertical:
                axis_name = "L_Vertical";
                break;
            case (int)Axis.Triggers:
                axis_name = "Triggers";
                break;
            case (int)Axis.Right_Horizontal:
                axis_name = "R_Horizontal";
                break;
            case (int)Axis.Right_Vertical:
                axis_name = "R_Vertical";
                break;
            case (int)Axis.D_Horizontal:
                axis_name = "D_Horizontal";
                break;
            case (int)Axis.D_Vertical:
                axis_name = "D_Vertical";
                break;
            default:
                axis_name = "X";
                break;
        }
        float value = (Input.GetAxis((axis_name) + "_P" + (player_id + 1)));
        if (Mathf.Abs(value) < dead_zone)
            return 0.0f;
        return value;
    }
    /// <summary>
    /// tells you whether the axis is doing the same thing or something different as it was last frame
    /// </summary>
    /// <param name="this_frame">What's it doing now</param>
    /// <param name="axis">Which axis</param>
    /// <param name="player_id">Which player</param>
    /// <returns>What's happened since last frame</returns>
    AxisState getAxisState(float this_frame, Axis axis, int player_id)
    {
        float last_frame = controllers[player_id].axes_last_frame[(int)axis];

        if (last_frame == 0 && this_frame == 0)
            return AxisState.At_Rest;
        else if (last_frame != 0 && this_frame == last_frame)
            return AxisState.Going_Steady;
        else if (last_frame * this_frame < 0)
            return AxisState.Flipped_This_Frame;
        else if (last_frame != 0 && this_frame < last_frame)
            return AxisState.Decreased;
        else if (last_frame != 0 && this_frame > last_frame)
            return AxisState.Increased;
        else if (last_frame != 0 && this_frame == 0)
            return AxisState.Rested_This_Frame;
        else if (last_frame == 0 && this_frame != 0)
            return AxisState.Triggered_This_Frame;
        else
        {
            Debug.LogError("Shouldn't have triggered this");
            return AxisState.Null;
        }
    }

    public void clearInputs()
    {
        for(int controller = 0; controller < controllers.Count; controller++)
        {
            controllers[controller].buttons_last_frame = 0;
            controllers[controller].buttons_this_frame = 0;
            for(int axis = 0; axis < 7; axis++)
            {
                controllers[controller].axes_this_frame[axis] = 0;
                controllers[controller].axes_last_frame[axis] = 0;
            }
        }
    }
}