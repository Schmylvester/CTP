namespace Abilities
{
    public class Revive : Ability
    {
        public Revive()
        {
            ability_name = "Revive";
            ability_description = "This unit loses 50% of their health (rounded up), target is brought back to life with as much health as this unit lost.";
        }
        public override void useAbility(ActionFeedbackText feedback)
        {
            uses--;
            //do it like this so it rounds it up and not down
            int damage = user.getHealth() - (user.getHealth() / 2);
            user.changeHealth(-damage, feedback);
            if (target.getHealth() <= 0)
            {
                target.changeHealth(damage, feedback);
            }
            feedback.printMessage(user.getName() + " revives " + target.getName());
        }

        public override bool isHighPriority()
        {
            return false;
        }


        protected override TargetRequired targetRequired()
        {
            return TargetRequired.AnyDeadAlly;
        }
    }
}