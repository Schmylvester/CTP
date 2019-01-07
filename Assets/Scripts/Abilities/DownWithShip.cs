namespace Abilities
{
    public class DownWithShip : Ability
    {
        public DownWithShip()
        {
            ability_name = "Down With the Ship";
            ability_description = "If this unit dies this turn, they attack each of their opponent's units once.";
        }
        public override void useAbility()
        {
            uses--;
            user.setActiveEffect(SingleTurnEffects.DownWithShip);
            new ActionFeedbackText().printMessage(user.getName() + " is ready to die.");
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