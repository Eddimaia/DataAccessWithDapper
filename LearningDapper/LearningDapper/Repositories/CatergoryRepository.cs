using Dapper;
using LearningDapper.Models;
using Microsoft.Data.SqlClient;

namespace LearningDapper.Repositories
{
    public static class CatergoryRepository
    {
        public static void ListCategories(this SqlConnection connection)
        {
            var categorys = connection.Query<Category>("SELECT [Id], [Title] FROM Category");
            foreach (var item in categorys)
            {
                Console.WriteLine($"{item.Id} - {item.Title}");
            }
        }

        public static Guid CreateCategory(this SqlConnection connection)
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

            ListCategories(connection);

            return category.Id;
        }

        public static Guid CreateCategoryMany(this SqlConnection connection)
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

            Category category2 = new()
            {
                Id = Guid.NewGuid(),
                Title = "Blazor",
                Url = "Microsoft",
                Description = "Categoria destinada a Blazor Server",
                Order = 9,
                Summary = "Blazor",
                Featured = true
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

            var rows = connection.Execute(insertSql, new[] {
                new{
                category.Id,
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured
            },
                new {
                category2.Id,
                category2.Title,
                category2.Url,
                category2.Summary,
                category2.Order,
                category2.Description,
                category2.Featured}
            });
            Console.WriteLine(rows + " Linhas Inseridas");

            ListCategories(connection);

            return category.Id;
        }

        public static void UpdateCategory(this SqlConnection connection)
        {
            var updateCategory = "UPDATE [Category] SET [Title] = @Title WHERE [Id] = @Id";

            var rows = connection.Execute(updateCategory, new
            {
                Id = new Guid("AF3407AA-11AE-4621-A2EF-2028B85507C4"),
                Title = "Frontend 2023"
            });
            Console.WriteLine(rows + " Linhas Atualizadas");
        }

        public static void DeleteCategory(this SqlConnection connection, Guid id)
        {
            var updateCategory = "DELETE [Category] WHERE [Id] = @Id";

            var rows = connection.Execute(updateCategory, new { Id = id });
            Console.WriteLine(rows + " Linhas Deletadas");
        }

        public static void SelectCategoryById(this SqlConnection connection)
        {
            var proc = "[spSelectCategoryById]";

            var param = new { Id = new Guid("2A64C978-16ED-4544-9ADE-2A346DC9AAEC") };

            var category = connection.QueryFirst<Category>(proc, param, commandType: System.Data.CommandType.StoredProcedure);

            Console.WriteLine(category.Title);
        }

        public static void OneToOne(this SqlConnection connection)
        {
            var query = @"
                        SELECT	*
                        FROM
	                        [CareerItem] CI
                        INNER JOIN
	                        [Course] CO
		                        ON CO.[Id] = CI.[CourseId]
                        INNER JOIN
	                        [Author] AU
		                        ON AU.[Id] = CO.AuthorId
                        INNER JOIN
	                        [Category] Cat
		                        ON Cat.[Id] = CO.[CategoryId]";
            var careerItems = connection.Query<CareerItem, Course, Author, Category, CareerItem>(
                query,
                (careerItem, course, author, category) =>
                {
                    careerItem.Course = course;
                    course.Author = author;
                    course.Category = category;
                    return careerItem;
                }, splitOn: "Id").ToList();

            careerItems.ForEach(c => Console.WriteLine($"Autor: {c.Course.Author.Name} - Categoria: {c.Course.Category.Title}."));
        }

        public static void OneToMany(this SqlConnection connection)
        {
            var query = @"
                        SELECT 
		                    CA.[Id]
	                    ,	CA.[Title]
	                    ,	CI.[CareerId]
	                    ,	CI.[Title]
                    FROM 
	                    [Career] CA
                    INNER JOIN
	                    [CareerItem] CI ON CI.[CareerId] = CA.[Id]
                    ORDER BY 
                            CA.[Title]";

            List<Career> careers = new();

            var items = connection.Query<Career, CareerItem, Career>(
                query,
                (career, item) =>
                {
                    var car = careers.Where(x => x.Id.Equals(career.Id)).FirstOrDefault();

                    if (car is null)
                    {
                        car = career;
                        car.Items.Add(item);
                        careers.Add(car);
                    }
                    else
                        car.Items.Add(item);

                    return career;
                }, splitOn: "CareerId");

            careers.ToList().ForEach(career =>
            {
                Console.WriteLine($"Career: {career.Title}");
                career.Items.ToList().ForEach(item =>
                {
                    Console.WriteLine($" - {item.Title}");
                });
            });
        }

        public static void QueryMultiple(this SqlConnection connection)
        {
            var query = "SELECT * FROM [Category]; SELECT * FROM [Course]";

            using var multi = connection.QueryMultiple(query);
            var categories = multi.Read<Category>().ToList();
            var course = multi.Read<Course>().ToList();

            categories.ForEach(c => Console.WriteLine("Category: " + c.Title));
            categories.ForEach(c => Console.WriteLine("Course: " + c.Title));
        }

        public static void SelectIn(this SqlConnection connection)
        {
            var query = @"SELECT * FROM [Career] WHERE [Id] IN @Id";

            var result = connection.Query<Career>(query, new
            {
                Id = new[]
                {
                    "4327AC7E-963B-4893-9F31-9A3B28A4E72B",
                    "92D7E864-BEA5-4812-80CC-C2F4E94DB1AF"
                }
            });

            result.ToList().ForEach(c => Console.WriteLine(c.Title));
        }
    }
}
