namespace Abilities
{
    public class Protect : Ability
    {
        public Protect()
        {
            ability_name = "Protect";
            ability_description = "Target's defence is buffed for this turn.";
        }
        public override void useAbility(ActionFeedbackText feedback)
        {
            uses--;
            int phys_mod = UnityEngine.Mathf.Max(user.getStat(Stat.Defence) / 2, 30);
            target.modifyStat(Stat.Defence, phys_mod);
            feedback.printMessage(user.getName() + " is protecting " + target.getName());
        }

        public override bool isHighPriority()
        {
            return true;
        }

        protected override TargetRequired targetRequired()
        {
            return TargetRequired.AnyOtherLivingAlly;
        }
    }
}