using UnityEngine;

public enum MessageType
{
    Action,
    Error,
}

public class ActionFeedbackText
{
    public void printMessage(string message, MessageType type = MessageType.Action)
    {
        Debug.Log(message);
    }
}
