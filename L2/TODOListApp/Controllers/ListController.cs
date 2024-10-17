using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TODOListApp.Models;

namespace TODOListApp.Controllers
{
    public class ListController : Controller
    {
        private const string SessionKey = "TodoList";

        public IActionResult Index()
        {
            var todoList = GetTodoListFromSession();
            return View(todoList);
        }

        [HttpPost]
        public IActionResult Add(string description)
        {
            if (!string.IsNullOrWhiteSpace(description))
            {
                var todoList = GetTodoListFromSession();
                int newId = todoList.Any() ? todoList.Max(t => t.Id) + 1 : 1;
                todoList.Add(new ListItemModel { Id = newId, Description = description, IsFinished = false });
                SaveTodoListToSession(todoList);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var todoList = GetTodoListFromSession();
            var item = todoList.FirstOrDefault(t => t.Id == id);
            if (item != null)
            {
                todoList.Remove(item);
                SaveTodoListToSession(todoList);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ToggleFinished(int id)
        {
            var todoList = GetTodoListFromSession();
            var item = todoList.FirstOrDefault(t => t.Id == id);
            if (item != null)
            {
                item.IsFinished = !item.IsFinished;
                SaveTodoListToSession(todoList);
            }
            return RedirectToAction("Index");
        }

        private List<ListItemModel> GetTodoListFromSession()
        {
            var jsonString = HttpContext.Session.GetString(SessionKey);

            if (string.IsNullOrEmpty(jsonString))
            {
                return new List<ListItemModel>();
            }
            else
            {
                return JsonSerializer.Deserialize<List<ListItemModel>>(jsonString);
            }
        }

        private void SaveTodoListToSession(List<ListItemModel> todoList)
        {
            var jsonString = JsonSerializer.Serialize(todoList);
            HttpContext.Session.SetString(SessionKey, jsonString);
        }
    }
}
