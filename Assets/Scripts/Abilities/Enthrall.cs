namespace Abilities
{

    public class Enthrall : Ability
    {
        public Enthrall()
        {
            ability_name = "Enthrall";
            ability_description = "Moves the target forward one position";
        }
        public override void useAbility()
        {
            uses--;
            target.move('f');
            new ActionFeedbackText().printMessage(user.getName() + " made " + target.getName() + " walk forwards.");
        }

        public override bool isHighPriority()
        {
            return false;
        }
        public override bool getRequiredTarget()
        {
            return user.getTeam() != target.getTeam() && target.getHealth() > 0;
        }
    }
}