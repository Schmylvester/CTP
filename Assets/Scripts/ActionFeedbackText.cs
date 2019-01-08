using UnityEngine;
using UnityEngine.UI;

public enum MessageType
{
    Action,
    Error,
}

public class ActionFeedbackText : MonoBehaviour
{
    [SerializeField] Text[] text_box;
    [SerializeField] Text player_bad;
    int messages_so_far = 0;
    int message_count;
    bool skeleton_died = false;

    public void setCount(int to)
    {
        message_count = to;
    }

    public void printMessage(string message, MessageType type = MessageType.Action)
    {
        Debug.Log(message);
        if (message == "Skeleton died.")
        {
            if (!skeleton_died)
            {
                skeleton_died = true;
                message = "Some skeletons died.";
            }
            else
            {
                return;
            }
        }
        if (messages_so_far > message_count)
            clear();
        if (type == MessageType.Action)
        {
            int text_box_idx = messages_so_far < (message_count / 2) ? 0 : 1;
            messages_so_far++;
            text_box[text_box_idx].text += message;
            text_box[text_box_idx].text += "\n\n";
        }
        else if (type == MessageType.Error)
        {
            player_bad.text = message;
        }
    }

    public void clear()
    {
        messages_so_far = 0;
        for (int i = 0; i < text_box.Length; i++)
        {
            text_box[i].text = "";
        }
        player_bad.text = "";
    }
}
