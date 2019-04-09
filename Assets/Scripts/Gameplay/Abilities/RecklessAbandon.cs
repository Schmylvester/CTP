namespace Abilities
{

    public class RecklessAbandon : Ability
    {
        public RecklessAbandon()
        {
            ability_name = "Reckless Abandon";
            ability_description = "Causes increased damage to target, but risks damaging units on user's team.";
        }
        public override void useAbility(ActionFeedbackText feedback)
        {
            uses--;
            target.takeDamage((int)(user.getStat(Stat.Attack) * 2.5f), feedback);
            string allies_hit = "";
            foreach (Unit unit in user.getTeam().getUnits(true))
            {
                if (UnityEngine.Random.Range(0, 3) == 0)
                {
                    allies_hit += ", " + unit.getName();
                    unit.takeDamage(user.getStat(Stat.Attack) / 2, feedback);
                }
            }
            feedback.printMessage(user.getName() + " attacked with reckless abandon and damaged " + target.getName() + allies_hit);
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