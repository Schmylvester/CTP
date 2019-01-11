/*
 * Make the predictions for the actions, maybe one file for each unit
 *      The inputs?
 *      All units on both teams
 *          who are they
 *          their x positions
 *          their health
 *          start with me, then my allies, then my enemies
 * A system for creating strings for what the units will do in a fight
 *      Initially, they will select a random unit, then use the predictions to create the fight
 *      When they combine and mutate, they will use % to ensure that the chars don't go over the maximums
 *      What are the maximums?
 *      Need 5 actions per team each turn, incase the ones who were dead before are not dead now
 */