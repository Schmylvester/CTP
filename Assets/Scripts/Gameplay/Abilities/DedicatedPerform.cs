namespace Abilities
{
    public class DedicatedPerform : Ability
    {
        public DedicatedPerform()
        {
            ability_name = "Dedicated Performance";
            ability_description = "Target has their stats improved significantly";
        }
        public override void useAbility(ActionFeedbackText feedback)
        {
            uses--;
            //start at 1 so it doesn't change max health
            for (int i = 1; i < (int)Stat.COUNT; i++)
            {
                target.changeStat((Stat)i, 1.4f);
            }
            feedback.printMessage(user.getName() + " is singing " + target.getName() + " a lovely song.");
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