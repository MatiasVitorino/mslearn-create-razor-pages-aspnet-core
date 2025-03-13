using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoPizza.Models;
using ContosoPizza.Services;

namespace ContosoPizza.Pages
{
    public class PizzaAddModel : PageModel
    {
        private readonly PizzaService _service;
        [BindProperty]
        public Pizza Pizza { get; set; } = default!;
        [TempData]
        public string ErrorMessage { get; set; }

        public PizzaAddModel(PizzaService service)
        {
            _service = service;
        }

        public void OnGet()
        {
            Pizza = new Pizza();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Check if a pizza with the same properties already exists
            var existingPizza = _service.GetPizzas().FirstOrDefault(p => 
                p.Name.Equals(Pizza.Name, StringComparison.OrdinalIgnoreCase) &&
                p.Size == Pizza.Size &&
                p.IsGlutenFree == Pizza.IsGlutenFree &&
                p.Price == Pizza.Price
            );

            if (existingPizza != null)
            {
                ModelState.AddModelError(string.Empty, "A pizza with these exact properties already exists!");
                return Page();
            }

            _service.AddPizza(Pizza);
            return RedirectToPage("PizzaList");
        }
    }
}