using TodoListApplication.Models;

namespace TodoListApplication.Services.Interface
{
    public interface ITodoListService
    {
        public Task<List<TodoModel>> GetAllTodos();
        public Task AddTodo(TodoModel model);
        public Task RemoveTodo(int id);
        public Task AlterCheck(bool check, int TaskId);
    }
}
