using RoCD.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoCD.Mechanics
{
    public class Creature : Actor
    {
        public Creature()
        {
            for (int i = Creature.STR; i <= Creature.LUK; i++)
            {
                stats[i] = 1;
            }

            exp_base = 0;
            exp_job = 0;

            level_base = 1;
            level_job = 1;

            exp_next_base = 4;

            updateSecondary_STR();
            updateSecondary_VIT();
            updateSecondary_AGI();
            updateSecondary_DEX();
            updateSecondary_INT();
            //updateSecondary_LUK();

            stats[CURRHP] = stats[MAXHP];
            stats[CURRSP] = stats[MAXSP];
            
            stat_buypoints = 20;
        }

        public const int STR = 0;
        public const int VIT = 1;
        public const int AGI = 2;
        public const int DEX = 3;
        public const int INT = 4;
        public const int LUK = 5;
        public const int MAXHP = 6;
        public const int MAXSP = 7;
        public const int CURRHP = 8;
        public const int CURRSP = 9;
        public const int WEIGHTLIMIT = 10;
        public const int ATK = 11;
        public const int BOWATK = 12;
        public const int MATK = 13;
        //static const int MAXMATK=14;
        public const int HARDDEF = 15;
        public const int SOFTDEF = 16;
        public const int HARDMDEF = 17;
        public const int SOFTMDEF = 18;
        public const int HIT = 19;
        public const int DODGE = 20;
        public const int ASPD = 21;
        public const int BLVL = 22;
        public const int JLVL = 23;
        public const int STATPOINTS = 24;

        ushort exp_base;
        ushort exp_job;
        ushort level_base;
        ushort level_job;

        ushort exp_next_base;

        public ushort stat_buypoints;

        public const int totalStats = 25;
        int[] stats = new int[totalStats];

        public void increaseStat(int stat)
        {
            int inc_stat = get(stat);
            if (stat < 99)
            {
                int stat_cost = (int)Math.Floor(inc_stat / 10.0) + 1;
                if (stat_cost <= stat_buypoints)
                {
                    stat_buypoints -= (ushort)stat_cost;
                    inc_stat++;

                    set(stat, inc_stat);
                }
            }
        }

        public void checkExp()
        {
            if (level_base == 99) return;

            if (exp_base > exp_next_base)
            {
                level_base++;
                exp_next_base = (ushort)(exp_next_base + (int)Math.Floor(1.02 * level_base * Math.Sqrt(exp_next_base)));
                exp_base = 0;
            }
        }

        public void addExpJob(ushort amnt)
        {
            exp_job += amnt;
            checkExp();
        }

        public void addExpBase(ushort amnt)
        {
            exp_base += amnt;
            checkExp();
        }

        public void updateSecondary_INT()
        {
            stats[MAXSP] = 6 * get(BLVL) + 4 * get(INT); //TODO proper formula
            //TODO: this should only apply to levelling up
            //float currentSPPercentage = (float)((get(CURRSP)) / get(MAXSP));
            //stats[CURRSP] = stats[MAXSP];
            stats[MATK] = (int)(get(BLVL) / 4.0 + get(INT));
        }

        public void updateSecondary_DEX()
        {
            stats[HIT] = get(BLVL) + get(DEX); //TODO proper formula
            stats[BOWATK] = (int)((get(BLVL) / 5.0) + get(DEX) + get(STR) / 6.0); //TODO proper formula
        }

        public void updateSecondary_AGI()
        {
            stats[ASPD] = 200 - 7 * (get(BLVL) - get(AGI) - (int)(get(DEX) / 4));
            stats[DODGE] = get(BLVL) + get(AGI); //TODO proper formula
        }

        public void updateSecondary_VIT()
        {
            stats[SOFTDEF] = get(VIT);
            stats[MAXHP] = 10 * get(BLVL) + 5 * get(VIT); //TODO proper formula?
            //TODO: this is only on levelup
            //float currentHPPercentage = (float)((get(CURRHP)) / get(MAXHP)); //Scale up the new current HP to be the same percentage after the max HP increase
            //stats[CURRHP] = (int)(/*currentHPPercentage * */ get(MAXHP));
        }

        public void updateSecondary_STR()
        {
            //affects ATK and Weightlimit
            stats[ATK] = (int)(get(BLVL) / 4.0 + get(STR) + get(DEX) / 5.0 + get(LUK) / 3.0);
            stats[WEIGHTLIMIT] = get(BLVL) + 30 * get(STR);
        }

        public int get(int stat)
        {
            return stats[stat];
        }

        public int meleeAttack(Creature other)
        {
            int chance_to_hit = 80 + get(HIT) - other.get(DODGE);
            if (RoCDRndm.Next(100) > chance_to_hit)
            {
                CombatLog.Log(Identity + " missed " + other.Identity);
                return 0;
            }

            int baseDamage = this.get(ATK) /*+ meleeWeapon->damage()*/; //ATK + weapon
            int finalDamage = (int)((baseDamage * other.get(HARDDEF)) / 100.0 - other.get(SOFTDEF)); //HARDDEF is percentage reduction, SOFTDEF is subtracted
            if (finalDamage < 1)
            { //there's a minimum damage of 1 per hit
                finalDamage = 1;
            }
            other.takeDamage(finalDamage);

            CombatLog.Log(Identity + " hit " + other.Identity + " for " + finalDamage.ToString());

            //tmp
            return 0;
        }

        public bool takeDamage(int damage)
        {
            stats[CURRHP] = stats[CURRHP] - damage;
            if (stats[CURRHP] <= 0)
            {
                stats[CURRHP] = 0;
            }
            return (stats[CURRHP] == 0);
        }

        public void set(int stat, int value)
        {
            if ((stat > Creature.LUK) || (stat < 0))
            {
                //std::cerr << "Only base stats may be directly modified. Please use a helper function" << std::endl;
            }
            else
            {
                if (value <= 0)
                {
                    //std::cerr << "Base stats can not be smaller than 1" << std::endl;
                }
                else
                {
                    stats[stat] = value;

                    switch (stat)
                    {
                        case Creature.AGI: updateSecondary_AGI(); break;
                        case Creature.DEX: updateSecondary_DEX(); break;
                        case Creature.INT: updateSecondary_INT(); break;
                        //case Creature.LUK: updateSecondary_LUK(); break;
                        case Creature.STR: updateSecondary_STR(); break;
                        case Creature.VIT: updateSecondary_VIT(); break;
                    }
                }
            }

        }

    }
}
