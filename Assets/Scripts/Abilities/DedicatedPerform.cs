namespace Abilities
{
    public class DedicatedPerform : Ability
    {
        public DedicatedPerform()
        {
            ability_name = "Dedicated Performance";
            ability_description = "Target has their stats improved significantly";
        }
        public override void useAbility()
        {
            uses--;
            //start at 1 so it doesn't change max health
            for (int i = 1; i < (int)Stat.COUNT; i++)
            {
                target.changeStat((Stat)i, 1.4f);
            }
            new ActionFeedbackText().printMessage(user.getName() + " is singing " + target.getName() + " a lovely song.");
        }

        public override bool isHighPriority()
        {
            return true;
        }

        public override bool getRequiredTarget()
        {
            if (user == target)
                return false;
            return user.getTeam() == target.getTeam() && target.getHealth() > 0;
        }
    }
}