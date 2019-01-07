namespace Abilities
{
    public class RapidFire : Ability
    {
        public RapidFire()
        {
            ability_name = "Rapid Fire";
            ability_description = "Attacks each unit on enemy team once with reduced damage.";
        }
        public override void useAbility()
        {
            user.modifyStat(Stat.Attack, 0.6f);
            foreach (Team t in field.getTeams())
            {
                if (t != user.getTeam())
                {
                    foreach (Unit u in t.getUnits(true))
                    {
                        user.attack(u);
                    }
                }
            }
            new ActionFeedbackText().printMessage(user.getName() + " attacked everyone on the opposing team.");
        }

        public override bool isHighPriority()
        {
            return false;
        }
        public override bool getRequiredTarget()
        {
            return true;
        }
    }
}