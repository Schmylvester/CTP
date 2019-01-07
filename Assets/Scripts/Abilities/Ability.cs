using UnityEngine;

public abstract class Ability
{
    protected Field field;
    protected Unit user;
    protected Unit target;
    public string ability_name = "invalid";
    public string ability_description;
    protected int uses = 5;
    protected int max_uses = 5;

    public abstract bool isHighPriority();
    public abstract void useAbility();
    public abstract bool getRequiredTarget();
    public Unit getUser()
    {
        return user;
    }
    public Unit getTarget()
    {
        return target;
    }
    public bool canUse()
    {
        return uses > 0;
    }
    public void setUser(Unit _user)
    {
        user = _user;
    }
    public void setTarget(Unit _target)
    {
        target = _target;
    }
    public bool userAndTargetStillAlive()
    {
        return (user.getHealth() > 0 && target.getHealth() > 0);
    }
    public string getRemainingUses()
    {
        return uses + "/" + max_uses;
    }
    public void setField(Field _field)
    {
        field = _field;
    }
}
namespace Abilities
{
    public class Attack : Ability
    {
        public Attack()
        {
            ability_name = "Attack";
        }
        public override void useAbility()
        {
            int dam = user.attack(target);
            if(user == target)
                new ActionFeedbackText().printMessage(user.getName() + " was caught in an ambush and took " + dam + " damage.");
            else if (dam > 0)
                new ActionFeedbackText().printMessage(user.getName() + " attacked " + target.getName() + " for " + dam + " damage.");
        }

        public override bool isHighPriority()
        {
            return false;
        }


        public override bool getRequiredTarget()
        {
            if (target == null)
                return false;
            return user.getTeam() != target.getTeam() && target.getHealth() > 0;
        }
    }
    public class Move : Ability
    {
        public char direction = '\0';
        public Move()
        {
            ability_name = "Move";
        }
        public override bool isHighPriority()
        {
            return false;
        }
        public override void useAbility()
        {
            user.move(direction);
            if (direction == 'b')
                new ActionFeedbackText().printMessage(user.getName() + " moved away from the enemy.");
            if (direction == 'f')
                new ActionFeedbackText().printMessage(user.getName() + " moved towards the enemy.");
        }
        public override bool getRequiredTarget()
        {
            return user.moveValid(direction);
        }
    }
}