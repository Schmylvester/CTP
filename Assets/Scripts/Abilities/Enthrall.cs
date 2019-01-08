namespace Abilities
{

    public class Enthrall : Ability
    {
        public Enthrall()
        {
            ability_name = "Enthrall";
            ability_description = "Moves the target forward one position";
        }
        public override void useAbility(ActionFeedbackText feedback)
        {
            uses--;
            target.move('f', feedback);
            feedback.printMessage(user.getName() + " made " + target.getName() + " walk forwards.");
        }

        public override bool isHighPriority()
        {
            return false;
        }

        protected override TargetRequired targetRequired()
        {
            return TargetRequired.AnythingButSelf;
        }
    }
}