namespace Abilities
{
    public class Berserk : Ability
    {
        public Berserk()
        {
            ability_name = "Berserk";
            ability_description = "User gets an attack buff but can no longer use abilities.";
        }
        public override void useAbility()
        {
            uses--;
            user.berserk = true;
            new ActionFeedbackText().printMessage(user.getName() + " is really mad.");
        }
        public override bool isHighPriority()
        {
            return false;
        }

        public override bool getRequiredTarget()
        {
            return true;
        }
    }
}