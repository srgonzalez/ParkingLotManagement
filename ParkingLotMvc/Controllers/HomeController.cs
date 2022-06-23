using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ParkingLotMvc.Models;
using Packt.Shared;
using Microsoft.EntityFrameworkCore;

namespace ParkingLotMvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private ParkingLot db;
    private readonly IHttpClientFactory clientFactory;

    public HomeController(ILogger<HomeController> logger, ParkingLot injectedContext, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        db = injectedContext;
        clientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var model = new HomeIndexViewModel
        {
            ParkingSpots = await db.ParkingSpots.ToListAsync()
        };
        return View(model);
    }

    public async Task<IActionResult> SpotDetail(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound("You must pass a slot ID in the route, for example, /Home/SlotDetail/21");
        }
        var model = await db.ParkingSpots.SingleOrDefaultAsync(p => p.Id == id);
        if (model == null)
        {
            return NotFound($"Slot with ID of {id} not found.");
        }
        TimeSpan duration = model.DepartureDate.Subtract(model.DateEntered);
        ViewData["duration"] = Math.Round(duration.Hours + (duration.Minutes / 60.0), 2);
        ViewData["totalDue"] = Math.Round(duration.Hours + (decimal)(duration.Minutes / 60.0)) * model.Rate;
        return View(model); // pass model to view and then return result
    }

    public async Task<IActionResult> ParkingSpots(string licensePlate)
    {
        string uri;
        if (string.IsNullOrEmpty(licensePlate))
        {
            ViewData["Title"] = "All Parking Spots";
            uri = "api/parkingspots/";
        }
        else
        {
            ViewData["Title"] = $"Parking Spot occupied by Licence Plate {licensePlate}";
            uri = $"api/parkingspots/?licensePlate={licensePlate}";
        }
        var client = clientFactory.CreateClient(
          name: "ParkingLotService");
        var request = new HttpRequestMessage(
          method: HttpMethod.Get, requestUri: uri);
        HttpResponseMessage response = await client.SendAsync(request);
        var model = await response.Content
          .ReadFromJsonAsync<IEnumerable<ParkingSpot>>();
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
