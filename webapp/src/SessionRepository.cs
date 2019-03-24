using System;
using System.Collections.Generic;

namespace webapp
{
    public class SessionRepository
    {
        private static readonly Dictionary<string, Session> sessions = new Dictionary<string, Session>();

        public Session Create()
        {
            Session session = new Session {Id = Guid.NewGuid().ToString()};
            sessions.Add(session.Id, session);
            return session;
        }

        public Session Get(string sessionIdValue)
        {
            Session session;
            sessions.TryGetValue(sessionIdValue, out session);
            return session;
        }
    }
}