namespace Abilities
{
    public class Bomb : Ability
    {
        public Bomb()
        {
            ability_name = "Bomb";
            ability_description = "Damages opponent's entire front row and this unit.";
        }
        public override void useAbility(ActionFeedbackText feedback)
        {
            uses--;
            System.Collections.Generic.List<Unit> targets = new System.Collections.Generic.List<Unit>();
            targets.Add(user);
            
            foreach (Team t in field.getTeams())
            {
                if (t != user.getTeam())
                {
                    foreach (Unit unit in t.getUnits(true))
                    {
                        if (unit.getPos() == 0)
                        {
                            targets.Add(unit);
                        }
                    }
                }
            }

            foreach (Unit t in targets)
            {
                t.takeDamage(UnityEngine.Random.Range(1, user.getStat(Stat.Attack) + 3), feedback);
            }
            feedback.printMessage(user.getName() + " damaged a bunch of people with a bomb.");
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