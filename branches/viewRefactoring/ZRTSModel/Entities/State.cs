using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.Entities
{
    /// <summary>
    /// This class represents the "state" of an Entity. The state of an Entity has two parts, Primary and Secondary.
    /// The Primary state can be thought of as what the Entity is currently doing.
    /// 
    /// The Secondary states can be thought of as miscellanious conditions the Entity is currently under.
    /// </summary>
    [Serializable()]
    public class State
    {
        /// <summary>
        /// This enum represents the possible primary states that an Entity may be in. An Entity may only be in
        /// one primary state at a time.
        /// </summary>
        public enum PrimaryState : byte{ Idle,      // Doing nothing, default state
                                        Moving,     // Entity (a Unit) is moving
                                        Attacking,  // Entity (a Unit) is attacking
                                        Dead,       // Entity is dead
                                        Harvesting, // Entity (a Unit) is harvesting a resource
                                        Building,   // Entity is performing a BuildAction
                                        Remove };   // Entity should be removed.

        /// <summary>
        /// This enum represents the possible secondary states an Entity may be in. An Entity may be in multiple or
        /// no secondary states at a time.
        /// </summary>
        public enum SecondaryState :byte { Damaged, BeingAttacked, Full };

        PrimaryState primary;
        List<SecondaryState> secondaryStates;

        /// <summary>
        /// Creates a new State with the primary state being set to Idle and the secondary state list being empty.
        /// </summary>
        public State()
        {
            this.primary = PrimaryState.Idle;

            secondaryStates = new List<SecondaryState>();
        }

        /// <summary>
        /// Given a PrimaryState pState, this function will set the primary state to pState.
        /// </summary>
        /// <param name="pState">The new PrimaryState.</param>
        public void setPrimaryState(PrimaryState pState)
        {
            this.primary = pState;
        }

        /// <summary>
        /// This function will return the primary state.
        /// </summary>
        /// <returns>The PrimaryState</returns>
        public PrimaryState getPrimaryState()
        {
            return this.primary;
        }

        /// <summary>
        /// This function will return a list of the secondary states.
        /// </summary>
        /// <returns>a List of SecondaryStates</returns>
        public List<SecondaryState> getSecondaryStates()
        {
            return this.secondaryStates;
        }

        /// <summary>
        /// This function will add the SecondaryState "state" to the SecondaryState list if they list does not aleady
        /// include that state.
        /// </summary>
        /// <param name="state">The SecondaryState to be added to the SecondaryState list.</param>
        public void addSecondaryState(SecondaryState state)
        {
            if (!(secondaryStates.IndexOf(state) >= 0))
            {
                secondaryStates.Add(state);
            }
        }

        /// <summary>
        /// This function will remove the SecondaryState "state" from the seconday state list.
        /// </summary>
        /// <param name="state">The SecondaryState to be removed.</param>
        /// <returns>Returns true if "state" was in the SecondaryState list. False if it was not.</returns>
        public bool removeSecondaryState(SecondaryState state)
        {
            int i = secondaryStates.IndexOf(state);
            if (i >= 0)
            {
                secondaryStates.RemoveAt(i);
                return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="state"></param>
        /// <returns>Returns true if the PrimaryState state of State is equal to "state"</returns>
        public bool inState(PrimaryState state)
        {
            return this.primary == state;
        }

        /// <summary>
        /// </summary>
        /// <param name="state"></param>
        /// <returns>Returns true if "state" exists in the States list of SecondaryStates.</returns>
        public bool inState(SecondaryState state)
        {
            foreach (SecondaryState s in secondaryStates)
            {

                if (s == state)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
