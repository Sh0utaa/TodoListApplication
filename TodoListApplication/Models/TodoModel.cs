namespace TodoListApplication.Models
{
    public class TodoModel
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
