﻿using System.Data.SqlClient;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Enexure.Fire.Data.Tests
{
	[TestFixture]
	public class QueryTests
	{

		class RawRow
		{
			public int Id { get; set; }
			public string Name { get; set; }
		}

		[Test]
		public async Task SampleQueryTest()
		{
			using (var unitOfWork = new UnitOfWork(TestDatabase.GetConnection())) {

				var session = new Session(unitOfWork);

				var createTableSql = @"Create Table TableA (
	Id int primary key,
	Name varchar(20)
)";

				var insertIntoSql = @"Insert Into TableA Values (1, 'One')";

				session.CreateCommand(createTableSql).ExecuteNonQuery();
				session.CreateCommand(insertIntoSql).ExecuteNonQuery();

				using (var command = await session.CreateCommand("Select * From TableA where Id = ?", 1).ExecuteQueryAsync())
				{
					var row = await command.SingleAsync<RawRow>();

					((int) row.Id).Should().Be(1);
				}
			}

		}
	}
}
