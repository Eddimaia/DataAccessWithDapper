using Dapper;
using LearningDapper.Models;
using Microsoft.Data.SqlClient;

const string connectionString = @"Server=FDM-038\SQLEXPRESS;Database=balta;Integrated Security=SSPI;Trusted_Connection=True;Encrypt=False";


using var connection = new SqlConnection(connectionString);
UpdateCategory(connection);
//CreateCategory(connection);
ListCategories(connection);


static void ListCategories(SqlConnection connection)
{
    var categorys = connection.Query<Category>("SELECT [Id], [Title] FROM Category");
    foreach (var item in categorys)
    {
        Console.WriteLine($"{item.Id} - {item.Title}");
    }
}

static void CreateCategory(SqlConnection connection)
{
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
}

static void UpdateCategory(SqlConnection connection)
{
    var updateCategory = "UPDATE [Category] SET [Title] = @Title WHERE [Id] = @Id";

    var rows = connection.Execute(updateCategory, new
    {
        Id = new Guid("AF3407AA-11AE-4621-A2EF-2028B85507C4"),
        Title = "Frontend 2023"
    });
    Console.WriteLine(rows + " Linhas Atualizadas");
}