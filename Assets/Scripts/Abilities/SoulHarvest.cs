namespace Abilities
{

    public class SoulHarvest : Ability
    {
        public SoulHarvest()
        {
            ability_name = "Soul Harvest";
            ability_description = "User recovers health for each dead unit in play.";
        }
        public override void useAbility(ActionFeedbackText feedback)
        {
            uses--;
            int count = 0;
            foreach (Team t in field.getTeams())
            {
                count += t.countDeadUnits();
            }
            float heal_amount = user.getStat(Stat.Max_HP) / 10;
            heal_amount *= count;
            user.changeHealth((int)heal_amount, feedback);
            feedback.printMessage(user.getName() + " absorbs " + heal_amount + " health from the dead.");
        }
        public override bool isHighPriority()
        {
            return false;
        }


        protected override TargetRequired targetRequired()
        {
            return TargetRequired.NoTargetRequired;
        }
    }
}