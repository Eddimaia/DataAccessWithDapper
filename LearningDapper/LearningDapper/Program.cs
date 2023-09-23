using Dapper;
using LearningDapper.Models;
using Microsoft.Data.SqlClient;

const string connectionString = @"Server=FDM-038\SQLEXPRESS;Database=balta;Integrated Security=SSPI;Trusted_Connection=True;Encrypt=False";

using var connection = new SqlConnection(connectionString);
var categorys = connection.Query<Category>("SELECT [Id], [Title] FROM Category");
foreach (var category in categorys)
{
    Console.WriteLine($"{category.Id} - {category.Title}");
}