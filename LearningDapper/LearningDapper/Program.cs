using Dapper;
using LearningDapper.Models;
using Microsoft.Data.SqlClient;

const string connectionString = @"Server=FDM-038\SQLEXPRESS;Database=balta;Integrated Security=SSPI;Trusted_Connection=True;Encrypt=False";

Category category = new()
{
    Id = Guid.NewGuid(),
    Title = "Amazon AWS",
    Url = "Amazon",
    Description = "Categoria destinada a serviços AWS",
    Order = 8,
    Summary = "AWS Cloud",
    Featured = false
};

var insertSql = @"INSERT INTO 
                        [Category] 
                   VALUES(
                            @Id
                        ,   @Title
                        ,   @Url
                        ,   @Summary
                        ,   @Order
                        ,   @Description
                        ,   @Featured)";

using var connection = new SqlConnection(connectionString);

var rows = connection.Execute(insertSql, new
{
    category.Id,
    category.Title,
    category.Url,
    category.Summary,
    category.Order,
    category.Description,
    category.Featured
});

Console.WriteLine(rows + " Linhas Inseridas");

var categorys = connection.Query<Category>("SELECT [Id], [Title] FROM Category");
foreach (var item in categorys)
{
    Console.WriteLine($"{item.Id} - {item.Title}");
}