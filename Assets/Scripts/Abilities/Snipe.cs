namespace Abilities
{
    public class Snipe : Ability
    {
        public Snipe()
        {
            ability_name = "Snipe";
            ability_description = "Can attack any unit with no distance penalty";
        }
        public override void useAbility()
        {
            uses--;
            user.attack(target, false);
            new ActionFeedbackText().printMessage(user.getName() + " used their fancy ranged weapon to attack " + target.getName());
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