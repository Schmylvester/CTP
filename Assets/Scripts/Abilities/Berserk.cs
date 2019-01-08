namespace Abilities
{
    public class Berserk : Ability
    {
        public Berserk()
        {
            ability_name = "Berserk";
            ability_description = "User gets an attack buff but can no longer use abilities.";
        }
        public override void useAbility(ActionFeedbackText feedback)
        {
            uses--;
            user.berserk = true;
            feedback.printMessage(user.getName() + " is really mad.");
        }
        public override bool isHighPriority()
        {
            return false;
        }


        protected override TargetRequired targetRequired()
        {
            return TargetRequired.NoTargetRequired;
        }
    }
}