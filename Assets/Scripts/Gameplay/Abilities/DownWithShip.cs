namespace Abilities
{
    public class DownWithShip : Ability
    {
        public DownWithShip()
        {
            ability_name = "Down With the Ship";
            ability_description = "If this unit dies this turn, they attack each of their opponent's units once.";
        }
        public override void useAbility(ActionFeedbackText feedback)
        {
            uses--;
            user.setActiveEffect(SingleTurnEffects.DownWithShip);
            feedback.printMessage(user.getName() + " is ready to die.");
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