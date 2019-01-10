namespace Abilities
{
    public class Perform : Ability
    {
        public Perform()
        {
            ability_name = "Performance";
            ability_description = "User's team has their stats improved slightly.";
        }
        public override void useAbility(ActionFeedbackText feedback)
        {
            uses--;
            user.getTeam().buffTeam(1.1f, user);
            feedback.printMessage(user.getName() + " sung a nice song.");
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