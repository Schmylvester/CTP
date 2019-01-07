namespace Abilities
{
    public class Heal : Ability
    {
        public Heal()
        {
            ability_name = "Heal";
            ability_description = "Recovers some of the target's health";
        }
        public override void useAbility()
        {
            uses--;
            int healed = target.changeHealth(user.getStat(Stat.Intelligence));
            new ActionFeedbackText().printMessage(user.getName() + " healed " + target.getName() + " by " + healed + " health");
        }

        public override bool isHighPriority()
        {
            return false;
        }
        public override bool getRequiredTarget()
        {
            return user.getTeam() == target.getTeam() && target.getHealth() > 0;
        }
    }
}