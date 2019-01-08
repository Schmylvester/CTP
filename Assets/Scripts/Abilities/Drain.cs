namespace Abilities
{
    public class Drain : Ability
    {
        public Drain()
        {
            ability_name = "Drain";
            ability_description = "Damages target and recovers user by the amount of damage dealt";
        }
        public override void useAbility(ActionFeedbackText feedback)
        {
            uses--;
            int damage = target.takeDamage(user.getStat(Stat.Attack) + user.getStat(Stat.Intelligence) / 2, feedback);
            user.changeHealth(damage, feedback);

            feedback.printMessage(user.getName() + " drained " + damage + " health from " + target.getName());
        }

        public override bool isHighPriority()
        {
            return false;
        }


        protected override TargetRequired targetRequired()
        {
            return TargetRequired.AnythingButSelf;
        }
    }
}