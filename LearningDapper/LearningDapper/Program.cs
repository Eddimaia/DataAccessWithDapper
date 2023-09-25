using LearningDapper.Repositories;
using Microsoft.Data.SqlClient;

const string connectionString = @"Server=FDM-038\SQLEXPRESS;Database=balta;Integrated Security=SSPI;Trusted_Connection=True;Encrypt=False";

using var connection = new SqlConnection(connectionString);
//connection.UpdateCategory();
//var id = connection.CreateCategory();
//var id = connection.CreateCategoryMany();
//connection.DeleteCategory(id);
//connection.ListCategories();
//connection.SelectCategoryById();
//connection.OneToOne();
//connection.OneToMany();
//connection.QueryMultiple();
connection.SelectIn();



