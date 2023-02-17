using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper_Experience.DbGenerator;

public class Generator
{
	static public void CreateDb(SqlConnection? sqlConnection)
	{

        sqlConnection.Open();
        string checkDatabaseQuery = $"SELECT COUNT(*) FROM sys.databases WHERE name = 'BookShopDapper'";
        SqlCommand checkDatabaseCommand = new SqlCommand(checkDatabaseQuery,sqlConnection);

        int databaseCount = (int)checkDatabaseCommand.ExecuteScalar();


        if (databaseCount == 0)
		{

			sqlConnection?.Execute("CREATE DATABASE BookShopDapper");

			sqlConnection.Execute(@"
							use [BookShopDapper]
							Create Table [dbo].[Book] (
							[Id]	INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
							[Name]	NVARCHAR(100) NOT NULL,
							[Page]	INT NOT NULL,
							[Author] NVARCHAR(MAX) NOT NULL,
							[Price] INT NOT NULL,
							[Stock] INT NOT NULL)");
			sqlConnection.Execute(@"
							use [BookShopDapper]
							INSERT INTO [BOOK] VALUES('book1', 20, 'Author1', 1, 22);
							INSERT INTO [BOOK] VALUES('book2', 15, 'Author2', 2, 14);
							INSERT INTO [BOOK] VALUES('book3', 35, 'Author3', 3, 11);
							INSERT INTO [BOOK] VALUES('book4', 65, 'Author4', 4, 11);");
            sqlConnection.Execute(@"
							
							CREATE PROCEDURE [dbo].[sp_AddBook] ( @Name nvarchar(100), @Page int, @Author nvarchar(max), @Price int,@Stock int)
							AS
							BEGIN
							    INSERT INTO Book (Name, Page, Author, Price,Stock)
							    VALUES (@Name, @Page, @Author, @Price,@Stock)
							END");
        }
		else
            Console.WriteLine($"The database already exists.");
		sqlConnection?.Close();
    }
}
