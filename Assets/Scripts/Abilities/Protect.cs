namespace Abilities
{
    public class Protect : Ability
    {
        public Protect()
        {
            ability_name = "Protect";
            ability_description = "Target's defence is buffed for this turn.";
        }
        public override void useAbility()
        {
            uses--;
            int phys_mod = UnityEngine.Mathf.Max(user.getStat(Stat.Defence) / 2, 1);
            target.modifyStat(Stat.Defence, phys_mod);
            new ActionFeedbackText().printMessage(user.getName() + " is protecting " + target.getName());
        }

        public override bool isHighPriority()
        {
            return true;
        }
        public override bool getRequiredTarget()
        {
            if (user == target)
                return false;
            return target.getTeam() == user.getTeam() && target.getHealth() > 0;
        }
    }
}