namespace Abilities
{
    public class Defend : Ability
    {
        public Defend()
        {
            ability_name = "Defend";
            ability_description = "User's defence is buffed for this turn.";
        }
        public override void useAbility()
        {
            uses--;
            user.modifyStat(Stat.Defence, 1.2f);
            new ActionFeedbackText().printMessage(user.getName() + " is defending.");
        }

        public override bool isHighPriority()
        {
            return true;
        }

        public override bool getRequiredTarget()
        {
            return true;
        }
    }
}