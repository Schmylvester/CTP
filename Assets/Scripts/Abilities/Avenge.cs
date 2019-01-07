namespace Abilities
{
    public class Avenge : Ability
    {
        public Avenge()
        {
            ability_name = "Avenge";
            ability_description = "Performs an attack which scales in power for each dead ally.";
        }
        public override void useAbility()
        {
            uses--;
            float damage = user.getStat(Stat.Attack);
            damage *= UnityEngine.Mathf.Pow(1.3f, user.getTeam().countDeadUnits());
            target.takeDamage((int)damage);
            new ActionFeedbackText().printMessage(user.getName() + " attacked " + target.getName() + " with all the force of their dead allies.");
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