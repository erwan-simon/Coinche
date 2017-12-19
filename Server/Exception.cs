using System;

namespace Server
{
    [Serializable()]
    public class UserQuit : System.Exception
    {
        public UserQuit() : base() { }
        public UserQuit(string message) : base(message) { }
        public UserQuit(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected UserQuit(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }

}