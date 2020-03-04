using System;
using System.Collections.Generic;
using System.Text;
using WebApplication2.Entities;
using WebApplication2.Persistence;

namespace WebApplication2.Tests
{
    public class TestMemoryTeamRepository : MemoryTeamRepository
    {
        public TestMemoryTeamRepository() : base(CreateInitialFake())
        {

        }

        private static ICollection<Team> CreateInitialFake()
        {
            var teams = new List<Team>();
            teams.Add(new Team("one"));
            teams.Add(new Team("two"));

            return teams;
        }
    }
}
