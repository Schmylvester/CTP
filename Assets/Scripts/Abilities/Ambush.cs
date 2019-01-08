namespace Abilities
{
    public class Ambush : Ability
    {
        public Ambush()
        {
            ability_name = "Ambush";
            ability_description = "If target attacks this turn, target is damaged for a portion of the damage";
        }
        public override void useAbility(ActionFeedbackText feedback)
        {
            uses--;
            target.setActiveEffect(SingleTurnEffects.Ambushed);
            feedback.printMessage(user.getName() + " is ready for " + target.getName() + " to attack.");
        }
        public override bool isHighPriority()
        {
            return true;
        }


        protected override TargetRequired targetRequired()
        {
            return TargetRequired.AnyLivingEnemy;
        }
    }
}