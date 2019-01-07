namespace Abilities
{
    public class Drain : Ability
    {
        public Drain()
        {
            ability_name = "Drain";
            ability_description = "Damages target and recovers user by the amount of damage dealt";
        }
        public override void useAbility()
        {
            uses--;
            int damage = target.takeDamage(user.getStat(Stat.Attack) + user.getStat(Stat.Intelligence) / 2);
            user.changeHealth(damage);

            new ActionFeedbackText().printMessage(user.getName() + " drained " + damage + " health from " + target.getName());
        }

        public override bool isHighPriority()
        {
            return false;
        }

        public override bool getRequiredTarget()
        {
            return target.getHealth() > 0 && user != target;
        }
    }
}