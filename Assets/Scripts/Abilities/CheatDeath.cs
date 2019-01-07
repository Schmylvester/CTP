namespace Abilities
{
    public class CheatDeath : Ability
    {
        public CheatDeath()
        {
            ability_name = "Cheat Death";
            ability_description = "If the user dies this turn, there's an 80% chance that they will come back to life.";
        }
        public override void useAbility()
        {
            uses--;
            user.setActiveEffect(SingleTurnEffects.CheatingDeath);
            new ActionFeedbackText().printMessage(user.getName() + " is seeking immortality.");
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