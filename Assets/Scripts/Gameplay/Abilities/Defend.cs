namespace Abilities
{
    public class Defend : Ability
    {
        public Defend()
        {
            ability_name = "Defend";
            ability_description = "User's defence is buffed for this turn.";
        }
        public override void useAbility(ActionFeedbackText feedback)
        {
            uses--;
            user.modifyStat(Stat.Defence, 1.2f);
            feedback.printMessage(user.getName() + " is protecting their face.");
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