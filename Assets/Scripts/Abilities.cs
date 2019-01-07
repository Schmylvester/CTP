using UnityEngine;

public abstract class Ability : UnityEngine.MonoBehaviour
{
    protected Unit user;
    protected Unit target;
    public string ability_name = "invalid";
    public string ability_description;
    protected int uses = 3;

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
        return uses + "/3";
    }
}

namespace Abilities
{
    public class Ambush : Ability
    {
        public Ambush()
        {
            ability_name = "Ambush";
            ability_description = "If target attacks this turn, target is damaged for a portion of the damage";
        }
        public override void useAbility()
        {
            uses--;
            target.setActiveEffect(SingleTurnEffects.Ambushed);
        }
        public override bool isHighPriority()
        {
            return true;
        }

        public override bool getRequiredTarget()
        {
            return user.getTeam() != target.getTeam() && target.getHealth() > 0;
        }
    }
    public class Attack : Ability
    {
        public Attack()
        {
            ability_name = "Attack";
        }
        public override void useAbility()
        {
            user.attack(target);
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
    public class Avenge : Ability
    {
        public Avenge()
        {
            ability_name = "Avenge";
            ability_description = "Performs an attack which scales in power for each dead ally.";
        }
        public override void useAbility()
        {
            uses--;
            float damage = user.getStat(Stat.Attack);
            damage *= Mathf.Pow(1.3f, user.getTeam().countDeadUnits());
            target.takeDamage((int)damage);
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
    public class Berserk : Ability
    {
        public Berserk()
        {
            ability_name = "Berserk";
            ability_description = "User gets an attack buff but can no longer use abilities.";
        }
        public override void useAbility()
        {
            uses--;
            user.berserk = true;
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
    public class BodyBlock : Ability
    {
        public BodyBlock()
        {
            ability_name = "Body Block";
            ability_description = "If target is attacked this turn, the user takes the attack instead.";
        }
        public override void useAbility()
        {
            uses--;
            target.defended_by = user;
        }

        public override bool isHighPriority()
        {
            return true;
        }

        public override bool getRequiredTarget()
        {
            if (user == target)
                return false;
            return user.getTeam() == target.getTeam() && target.getHealth() > 0;
        }
    }
    public class Bomb : Ability
    {
        public Bomb()
        {
            ability_name = "Bomb";
            ability_description = "Damages opponent's entire front row and this unit.";
        }
        public override void useAbility()
        {
            uses--;
            System.Collections.Generic.List<Unit> targets = new System.Collections.Generic.List<Unit>();
            targets.Add(user);

            Field field = FindObjectOfType<Field>();
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
                t.takeDamage(Random.Range(1, user.getStat(Stat.Attack) + 3));
            }
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
    public class Charm : Ability
    {
        public Charm()
        {
            ability_name = "Charm";
            ability_description = "May prevent target from performing action this turn";
        }
        public override void useAbility()
        {
            uses--;
            float chance = Mathf.Pow(0.9f, (target.getStat(Stat.Defence) + target.getStat(Stat.Intelligence) / 2));
            if (Random.Range(0.0f, 1.0f) < chance)
            {
                target.setActiveEffect(SingleTurnEffects.Charmed);
            }
        }

        public override bool isHighPriority()
        {
            return true;
        }
        public override bool getRequiredTarget()
        {
            return user.getTeam() != target.getTeam() && target.getHealth() > 0;
        }
    }
    public class CheatDeath : Ability
    {
        public CheatDeath()
        {
            ability_name = "Cheat Death";
            ability_description = "If the user dies this turn, there's an 80% chance that they will come back to life.";
        }
        public override void useAbility()
        {
            uses--;
            user.setActiveEffect(SingleTurnEffects.CheatingDeath);
        }

        public override bool isHighPriority()
        {
            return true;
        }

        public override bool getRequiredTarget()
        {
            return true;
        }
    }
    public class DedicatedPerform : Ability
    {
        public DedicatedPerform()
        {
            ability_name = "Dedicated Performance";
            ability_description = "Target has their stats improved significantly";
        }
        public override void useAbility()
        {
            uses--;
            //start at 1 so it doesn't change max health
            for (int i = 1; i < (int)Stat.COUNT; i++)
            {
                target.changeStat((Stat)i, 1.4f);
            }
        }

        public override bool isHighPriority()
        {
            return true;
        }

        public override bool getRequiredTarget()
        {
            if (user == target)
                return false;
            return user.getTeam() == target.getTeam() && target.getHealth() > 0;
        }
    }
    public class Defend : Ability
    {
        public Defend()
        {
            ability_name = "Defend";
            ability_description = "User's defence is buffed for this turn.";
        }
        public override void useAbility()
        {
            uses--;
            user.modifyStat(Stat.Defence, 1.2f);
        }

        public override bool isHighPriority()
        {
            return true;
        }

        public override bool getRequiredTarget()
        {
            return true;
        }
    }
    public class DownWithShip : Ability
    {
        public DownWithShip()
        {
            ability_name = "Down With the Ship";
            ability_description = "If this unit dies this turn, they attack each of their opponent's units once.";
        }
        public override void useAbility()
        {
            uses--;
            user.setActiveEffect(SingleTurnEffects.DownWithShip);
        }

        public override bool isHighPriority()
        {
            return true;
        }

        public override bool getRequiredTarget()
        {
            return true;
        }
    }
    public class Drain : Ability
    {
        public Drain()
        {
            ability_name = "Drain";
            ability_description = "Damages target and recovers user by the amount of damage dealt";
        }
        public override void useAbility()
        {
            uses--;
            int damage = target.takeDamage(user.getStat(Stat.Attack) + user.getStat(Stat.Intelligence) / 2);
            user.changeHealth(damage);
        }

        public override bool isHighPriority()
        {
            return false;
        }

        public override bool getRequiredTarget()
        {
            return target.getHealth() > 0 && user != target;
        }
    }
    public class Enthrall : Ability
    {
        public Enthrall()
        {
            ability_name = "Enthrall";
            ability_description = "Moves the target forward one position";
        }
        public override void useAbility()
        {
            uses--;
            target.move('f');
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
    public class Heal : Ability
    {
        public Heal()
        {
            ability_name = "Heal";
            ability_description = "Recovers some of the target's health";
        }
        public override void useAbility()
        {
            uses--;
            target.changeHealth(user.getStat(Stat.Intelligence));
        }

        public override bool isHighPriority()
        {
            return false;
        }
        public override bool getRequiredTarget()
        {
            return user.getTeam() == target.getTeam() && target.getHealth() > 0;
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
        }
        public override bool getRequiredTarget()
        {
            return true;
        }
    }
    public class Perform : Ability
    {
        public Perform()
        {
            ability_name = "Performance";
            ability_description = "User's team has their stats improved slightly.";
        }
        public override void useAbility()
        {
            uses--;
            user.getTeam().buffTeam(1.1f, user);
        }

        public override bool isHighPriority()
        {
            return true;
        }

        public override bool getRequiredTarget()
        {
            return user.getTeam() == target.getTeam() && target.getHealth() > 0;
        }
    }
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
            int phys_mod = Mathf.Max(user.getStat(Stat.Defence) / 2, 1);
            target.modifyStat(Stat.Defence, phys_mod);
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
            Field field = FindObjectOfType<Field>();
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
            foreach (Unit unit in user.getTeam().getUnits(true))
            {
                if (Random.Range(0, 3) == 0)
                {
                    unit.takeDamage(user.getStat(Stat.Attack) / 2);
                }
            }
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
    public class Revive : Ability
    {
        public Revive()
        {
            ability_name = "Revive";
            ability_description = "User loses 50% of their health (rounded up), target is brought back to life with as much health as this unit lost.";
        }
        public override void useAbility()
        {
            uses--;
            //do it like this so it rounds it up and not down
            int damage = user.getHealth() - (user.getHealth() / 2);
            user.changeHealth(-damage);
            if (target.getHealth() <= 0)
            {
                target.changeHealth(damage);
            }
        }

        public override bool isHighPriority()
        {
            return false;
        }

        public override bool getRequiredTarget()
        {
            return target.getTeam() == user.getTeam() && target.getHealth() <= 0;
        }
    }
    public class Sneak : Ability
    {
        public Sneak()
        {
            ability_name = "Sneak";
            ability_description = "Increases user's evasion this turn, user's next attack is buffed proportional to the number of consecutive times this move has been used without being hit.";
        }
        public override void useAbility()
        {
            uses--;
            if (user as Rogue)
            {
                (user as Rogue).sneak_turns++;
            }
            else
            {
                Debug.LogError("Sneak is not implemented for that unit, and should not be useable.");
            }
            user.modifyStat(Stat.Agility, 1.5f);
        }

        public override bool isHighPriority()
        {
            return true;
        }

        public override bool getRequiredTarget()
        {
            return true;
        }
    }
    public class Snipe : Ability
    {
        public Snipe()
        {
            ability_name = "Snipe";
            ability_description = "Can attack any unit with no distance penalty";
        }
        public override void useAbility()
        {
            uses--;
            user.attack(target, false);
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
    public class SoulHarvest : Ability
    {
        public SoulHarvest()
        {
            ability_name = "Soul Harvest";
            ability_description = "User recovers health for each dead unit in play.";
        }
        public override void useAbility()
        {
            uses--;
            int count = 0;
            Field field = FindObjectOfType<Field>();
            foreach (Team t in field.getTeams())
            {
                count += t.countDeadUnits();
            }
            float heal_amount = user.getStat(Stat.Max_HP) / 10;
            heal_amount *= count;
            user.changeHealth((int)heal_amount);
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
    public class Summon : Ability
    {
        public Summon()
        {
            ability_name = "Raise Dead";
            ability_description = "Summons some skeletons to fight alongside you.";
        }
        public override void useAbility()
        {
            uses--;
            user.getTeam().addSkeletons(user.grid_pos.x);
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
    public class SwanSong : Ability
    {
        public SwanSong()
        {
            ability_name = "Swan Song";
            ability_description = "If this unit dies this turn, each unit on the team can take an extra action.";
        }
        public override void useAbility()
        {
            uses--;
            user.setActiveEffect(SingleTurnEffects.SwanSong);
        }
        public override bool isHighPriority()
        {
            return true;
        }
        public override bool getRequiredTarget()
        {
            return true;
        }
    }
    public class Taunt : Ability
    {
        public Taunt()
        {
            ability_name = "Taunt";
            ability_description = "Target's attack increases, accuracy decreases and they must attack this unit as their next action.";
        }
        public override bool isHighPriority()
        {
            return true;
        }
        public override void useAbility()
        {
            uses--;
            target.taunted_by = user;
        }
        public override bool getRequiredTarget()
        {
            return target.getTeam() != user.getTeam() && target.getHealth() > 0;
        }
    }
}