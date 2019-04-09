using UnityEngine;

namespace Abilities
{
    public class Sneak : Ability
    {
        public Sneak()
        {
            ability_name = "Sneak";
            ability_description = "Increases user's evasion this turn, user's next attack is buffed proportional to the number of consecutive times this move has been used without being hit.";
        }
        public override void useAbility(ActionFeedbackText feedback)
        {
            uses--;
            if ((user as Rogue) != null)
            {
                (user as Rogue).sneak_turns++;
            }
            else
            {
                Debug.LogError("Sneak is not implemented for that unit, and should not be useable.");
            }
            user.modifyStat(Stat.Agility, 1.5f);
            feedback.printMessage(user.getName() + " is sneaking around.");
        }

        public override bool isHighPriority()
        {
            return true;
        }


        protected override TargetRequired targetRequired()
        {
            return TargetRequired.NoTargetRequired;
        }
    }
}