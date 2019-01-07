namespace Abilities
{
    public class Revive : Ability
    {
        public Revive()
        {
            ability_name = "Revive";
            ability_description = "User loses 50% of their health (rounded up), target is brought back to life with as much health as this unit lost.";
        }
        public override void useAbility()
        {
            uses--;
            //do it like this so it rounds it up and not down
            int damage = user.getHealth() - (user.getHealth() / 2);
            user.changeHealth(-damage);
            if (target.getHealth() <= 0)
            {
                target.changeHealth(damage);
            }
            new ActionFeedbackText().printMessage(user.getName() + " revives " + target.getName());
        }

        public override bool isHighPriority()
        {
            return false;
        }

        public override bool getRequiredTarget()
        {
            return target.getTeam() == user.getTeam() && target.getHealth() <= 0;
        }
    }
}