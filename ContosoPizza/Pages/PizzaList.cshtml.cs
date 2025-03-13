using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoPizza.Models;
using ContosoPizza.Services;

namespace ContosoPizza.Pages
{
    public class PizzaListModel : PageModel
    {
        private readonly PizzaService _service;
        public IList<Pizza> PizzaList { get; set; } = default!;
        
        // Add pagination properties
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public int TotalPages { get; set; }

        public PizzaListModel(PizzaService service)
        {
            _service = service;
        }

        public void OnGet()
        {
            var allPizzas = _service.GetPizzas();
            TotalPages = (int)Math.Ceiling(allPizzas.Count / (double)PageSize);
            
            // Ensure CurrentPage is within valid range
            CurrentPage = Math.Max(1, Math.Min(CurrentPage, TotalPages));
            
            // Get only pizzas for current page
            PizzaList = allPizzas
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        public IActionResult OnPostDelete(int id)
        {
            _service.DeletePizza(id);
            return RedirectToPage();
        }
    }
}