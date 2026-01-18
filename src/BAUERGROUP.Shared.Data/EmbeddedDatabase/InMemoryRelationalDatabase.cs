using NMemory;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAUERGROUP.Shared.Data.EmbeddedDatabase
{
    /// <summary>
    /// An in-memory relational database implementation using NMemory.
    /// </summary>
    /// <remarks>
    /// Derive from this class to create custom in-memory databases with tables and relations.
    /// See https://nmemory.net/online-examples for usage examples.
    /// </remarks>
    public class InMemoryRelationalDatabase : Database
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryRelationalDatabase"/> class.
        /// </summary>
        public InMemoryRelationalDatabase()
        {

		}
    }
}

/*
 Samples: https://nmemory.net/online-examples

 Quickstart:
	public class MyDatabase : Database
	{
		public MyDatabase()
		{
			var members = this.Tables.Create<Member, int>(x => x.Id);
			var groups = base.Tables.Create<Group, int>(g => g.Id);

			this.Members = members;
			this.Groups = groups;

			RelationOptions options = new RelationOptions(
				cascadedDeletion: true);

			var peopleGroupIdIndex = members.CreateIndex(
				new RedBlackTreeIndexFactory(),
				p => p.GroupId);

			this.Tables.CreateRelation(
				groups.PrimaryKeyIndex,
				peopleGroupIdIndex,
				x =>  x ?? -1,
				x => x,
				options);
		}

		public ITable<Member> Members { get; private set; }

		public ITable<Group> Groups { get; private set; }
	}
 */
