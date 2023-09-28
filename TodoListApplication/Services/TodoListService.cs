using Microsoft.Data.SqlClient;
using System.Data;
using TodoListApplication.Models;
using TodoListApplication.Services.Interface;

namespace TodoListApplication.Services
{
    public class TodoListService : ITodoListService
    {
        static string HelperConfig = @"Server=DESKTOP-8A5CQ6D\SQLEXPRESS;Database=TodoList;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
        public async Task<List<TodoModel>> GetAllTodos()
        {
            var result = new List<TodoModel>();
            using (SqlConnection connection = new SqlConnection(HelperConfig))
            {
                try
                {
                    SqlCommand command = new("sp_GetAllTodos", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();
                    var reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new TodoModel()
                            {
                                TaskId = reader.IsDBNull(0) ? 0 : Convert.ToInt32(reader[0]),
                                TaskName = reader.IsDBNull(1) ? string.Empty : Convert.ToString(reader[1]),
                                DueDate = reader.IsDBNull(2) ? DateTime.MinValue : Convert.ToDateTime(reader[2]),
                                IsCompleted = reader.IsDBNull(3) ? false : Convert.ToBoolean(reader[3]),
                            });
                        }
                    }

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    await connection.CloseAsync();
                }
                return result;
            }
        }

        public async Task AddTodo(TodoModel model)
        {
            using (SqlConnection connection = new(HelperConfig))
            {
                try
                {
                    SqlCommand command = new("sp_AddTodo", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    if(model != null)
                    {
                        command.Parameters.AddWithValue("@TaskName", model.TaskName);
                        command.Parameters.AddWithValue("@DueDate", model.DueDate);
                        command.Parameters.AddWithValue("@IsCompleted", model.IsCompleted);
                    }


                    await connection.OpenAsync();
                    command.ExecuteReader();
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    await connection.CloseAsync();
                }
            };
        }

        public async Task RemoveTodo(int id)
        {
            using (SqlConnection connection = new(HelperConfig))
            {
                try
                {
                    SqlCommand command = new("sp_RemoveTodo", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    if (id != null)
                    {
                        command.Parameters.AddWithValue("@TaskID", id);
                    }

                    await connection.OpenAsync();
                    var reader = command.ExecuteReader();
                }
                catch(Exception)
                {
                    throw;
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
        }

        public async Task AlterCheck(bool check, int TaskId)
        {
            using (SqlConnection connection = new(HelperConfig))
            {
                try
                {
                    SqlCommand command = new("sp_AlterCheck", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    check = !check;

                    command.Parameters.AddWithValue("@TaskId", TaskId);
                    command.Parameters.AddWithValue("@Check", check);

                    await connection.OpenAsync();
                    var reader = command.ExecuteReader();
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
        }
    }
}


// ExecuteProcedure