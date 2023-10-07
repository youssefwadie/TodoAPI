using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Models;


namespace TodoAPI.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class TodoItemController : ControllerBase
{
    private readonly ApiDbContext _context;
    private readonly ILogger<TodoItemController> _logger;
    
    public TodoItemController(ApiDbContext context, ILogger<TodoItemController> logger)
    {
        _logger = logger;
        _context = context;
    }
    
    // GET: api/TodoItem
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
    {
        return await _context.TodoItems.ToListAsync();
    }

    // GET: api/TodoItem/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
    {
        _logger.LogDebug("Getting todo item with ID: {}", id);
        
        var todoItem = await _context.TodoItems.FindAsync(id);
        
        if (todoItem == null)
        {
            _logger.LogWarning("Todo item with ID {} not found.", id);
            return NotFound();
        }
        
        return todoItem;
    }

    // PUT: api/TodoItem/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutTodoItem(int id, TodoItem todoItem)
    {
        _logger.LogDebug("Todo item with ID {} not found.", id);
        if (id != todoItem.Id)
        {
            _logger.LogWarning("Mismatch between provided ID {} and todo item's ID {}.", id, todoItem.Id);
            return BadRequest();
        }

        if (!TodoItemExists(id))
        {
            _logger.LogWarning("Todo item with ID {} not found for updating.", id);
            return NotFound();
        }
        
        _context.Entry(todoItem).State = EntityState.Modified;
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (TodoItemExists(id)) throw;

            // It was deleted while it was updating!
            _logger.LogError("Concurrency error: Todo item with ID {} not found.", id);
            return NotFound();
        }

        return NoContent(); //204 No Content
    }

    // POST: api/TodoItem
    [HttpPost]
    public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
    {
        _logger.LogDebug("Creating a new todo item.");
        if (todoItem.Id != 0)
        {
            _logger.LogWarning("Provided ID {} should be 0 for a new todo item.", todoItem.Id);
            return BadRequest();
        }
        
        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Created a new todo item with ID: {}", todoItem.Id);
        
        return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
    }

    // DELETE: api/TodoItem/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTodoItem(int id)
    {
        _logger.LogInformation("Deleting todo item with ID: {}", id);
        
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            _logger.LogWarning("Todo item with ID {} not found for deletion.", id);
            return NotFound();
        }

        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Deleted todo item with ID: {}", id);
        
        return NoContent();
    }

    private bool TodoItemExists(int id)
    {
        return _context.TodoItems.Any(e => e.Id == id);
    }
    
}