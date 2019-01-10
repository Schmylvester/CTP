namespace Abilities
{
    public class Avenge : Ability
    {
        public Avenge()
        {
            ability_name = "Avenge";
            ability_description = "Performs an attack which scales in power for each dead ally.";
        }
        public override void useAbility(ActionFeedbackText feedback)
        {
            uses--;
            float damage = user.getStat(Stat.Attack);
            damage *= UnityEngine.Mathf.Pow(1.3f, user.getTeam().countDeadUnits());
            target.takeDamage((int)damage, feedback);
            feedback.printMessage(user.getName() + " attacked " + target.getName() + " with all the force of their dead allies.");
        }
        public override bool isHighPriority()
        {
            return false;
        }


        protected override TargetRequired targetRequired()
        {
            return TargetRequired.AnyLivingEnemy;
        }
    }
}