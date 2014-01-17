using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace States
{
    public delegate bool Reason();

    public class Transition
    {
        public State Source;
        public State Destination;
        public Reason Reason;

        public Transition(State source, State destination, Reason reason)
        {
            this.Source = source;
            this.Destination = destination;
            this.Reason = reason;
        }
    }
}
