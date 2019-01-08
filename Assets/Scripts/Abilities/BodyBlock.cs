namespace Abilities
{
    public class BodyBlock : Ability
    {
        public BodyBlock()
        {
            ability_name = "Body Block";
            ability_description = "If target is attacked this turn, the user takes the attack instead.";
        }
        public override void useAbility(ActionFeedbackText feedback)
        {
            uses--;
            target.defended_by = user;
            feedback.printMessage(user.getName() + " is standing in front of " + target.getName());
        }

        public override bool isHighPriority()
        {
            return true;
        }


        protected override TargetRequired targetRequired()
        {
            return TargetRequired.AnyOtherLivingAlly;
        }
    }
}