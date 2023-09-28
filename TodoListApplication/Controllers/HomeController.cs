using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TodoListApplication.Models;
using TodoListApplication.Services.Interface;

namespace TodoListApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITodoListService _service;
        public HomeController(ITodoListService service)
        {
            _service = service;
        }

        [HttpGet]

        public async Task<IActionResult> Index()
        {
            var todoList = await _service.GetAllTodos();
            TodoModel singleTodo = new TodoModel();

            // Create a TodoViewModel and set the list and single model
            var todoViewModel = new TodoViewModel
            {
                TodoList = todoList,
                SingleTodo = singleTodo
            };
            return View(todoViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AlterCheck(bool CheckId, int TaskId)
        {
            await _service.AlterCheck(CheckId, TaskId);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        [HttpPost]

        public async Task<IActionResult> AddTodo(TodoViewModel todo)
        {
            todo.SingleTodo.DueDate = DateTime.Now;
            todo.SingleTodo.IsCompleted = false;
            await _service.AddTodo(todo.SingleTodo);
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveTodo(int TaskId)
        {
            await _service.RemoveTodo(TaskId);
            return RedirectToAction("Index");
        }

    }
}


// @* <a class="btn btn-dark" asp-controller="YourController" asp-action="AlterCheck" asp-route-CheckId="@Todo.IsCompleted">CK</a> *@
