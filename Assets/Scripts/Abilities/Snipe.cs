namespace Abilities
{
    public class Snipe : Ability
    {
        public Snipe()
        {
            ability_name = "Snipe";
            ability_description = "Can attack any unit with no distance penalty";
        }
        public override void useAbility(ActionFeedbackText feedback)
        {
            uses--;
            user.attack(target, feedback, false);
            feedback.printMessage(user.getName() + " used their fancy ranged weapon to attack " + target.getName());
        }
        public override bool isHighPriority()
        {
            return false;
        }

        protected override TargetRequired targetRequired()
        {
            return TargetRequired.AnyLivingEnemy;
        }
    }
}