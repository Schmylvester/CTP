namespace Abilities
{
    public class Taunt : Ability
    {
        string[] names;
        public Taunt()
        {
            ability_name = "Taunt";
            ability_description = "Target's attack increases, accuracy decreases and they must attack this unit as their next action.";
            names = new string[10]
            {
                " a cunt.", " a wanker.", " unpleasant.",
                " a big knobber.", " Thomas, and it upset them.",
                " a nasty name.", " knob cheese.", " a prick.",
                " smelly.", " a doober."
            };
        }
        public override bool isHighPriority()
        {
            return true;
        }
        public override void useAbility(ActionFeedbackText feedback)
        {
            uses--;
            target.taunted_by = user;
            feedback.printMessage(user.getName() + " called " + target.getName() + names[UnityEngine.Random.Range(0, 10)]);
        }

        protected override TargetRequired targetRequired()
        {
            return TargetRequired.AnyLivingEnemy;
        }
    }
}