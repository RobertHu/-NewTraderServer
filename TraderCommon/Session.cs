using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trader.Common
{
    public struct Session:IEquatable<Session>
    {
        public static Session InvalidSession = new Session(0);
        private readonly long _Id;
        public Session(long id)
        {
            _Id = id;
        }
        public long Id
        {
            get { return _Id; }
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Session))
            {
                return false;
            }
            return Equals((Session)obj);
           
        }
        public static bool operator ==(Session s1, Session s2)
        {
            return s1.Id == s2.Id;
        }

        public static bool operator !=(Session s1, Session s2)
        {
            return s1.Id != s2.Id;
        }

        public override int GetHashCode()
        {
            return _Id.GetHashCode();
        }

        public override string ToString()
        {
            return _Id.ToString();
        }

        public static bool TryParse(string sessionstr,out Session session )
        {
            long id;
            bool result;
            if (long.TryParse(sessionstr, out id))
            {
                session = new Session(id);
                result = true;
            }
            else
            {
                session = InvalidSession;
                result = false;
            }
            return result;
        }

        public bool Equals(Session other)
        {
            return _Id == other.Id;
        }
    }
}
