namespace Abilities
{

    public class RecklessAbandon : Ability
    {
        public RecklessAbandon()
        {
            ability_name = "Reckless Abandon";
            ability_description = "Causes increased damage to target, but risks damaging units on user's team.";
        }
        public override void useAbility()
        {
            uses--;
            target.takeDamage((int)(user.getStat(Stat.Attack) * 2.5f));
            string allies_hit = "";
            foreach (Unit unit in user.getTeam().getUnits(true))
            {
                if (UnityEngine.Random.Range(0, 3) == 0)
                {
                    allies_hit += ", " + unit.getName();
                    unit.takeDamage(user.getStat(Stat.Attack) / 2);
                }
            }
            new ActionFeedbackText().printMessage(user.getName() + " attacked with reckless abandon and damaged " + target.getName() + allies_hit);
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