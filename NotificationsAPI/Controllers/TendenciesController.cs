using Microsoft.AspNetCore.Mvc;
using Python.Runtime;
using NotificationsAPI.DTO;
using NotificationsAPI.Manager;
using Microsoft.EntityFrameworkCore;
using NotificationsAPI.Models;
using System.Text.Json;

namespace NotificationsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TendenciesController : ControllerBase
    {
        private readonly FakeStoreContext _DbContext;

        public TendenciesController(FakeStoreContext DbContext) 
        {
            this._DbContext = DbContext;
        }

        [HttpPost("/Calculate")]
        public ActionResult<TendenciesDTO> CalculateTendencies(List<Dictionary<String, dynamic>> ProductsSales)
        {
            Dictionary<String, Dictionary<int, double>> MonthlyAmounts = [];
            Dictionary<String, double> TotalAmounts = [];
            DateTime CurrentDate = DateTime.Now;
            var List = ProductsSales.Where(
                (Data) => DateTime.Parse(Data["date"].GetRawText().Replace("\"", "")) > CurrentDate.Date.AddMonths(-6) 
                && DateTime.Parse(Data["date"].GetRawText().Replace("\"", "")) < new DateTime(
                    CurrentDate.Year, CurrentDate.Month, 1, 0, 0, 0, 0    
                )
            ).Select(
                Data => new ProductSaleDTO
                {
                    Category = Data["category"].GetRawText().Replace("\"", ""),
                    ProductName = Data["product_name"].GetRawText().Replace("\"", ""),
                    UnitaryPrice = Decimal.Parse(Data["unitary_price"].GetRawText().Replace("\"", "")),
                    Quantity = int.Parse(Data["quantity"].GetRawText().Replace("\"", "")),
                    TotalAmount = Decimal.Parse(Data["total_amount"].GetRawText().Replace("\"", "")),
                    Date = DateTime.Parse(Data["date"].GetRawText().Replace("\"", "")),
                }
            ).ToList();

            foreach(ProductSaleDTO ProductSale in List)
            {
                int Month = 6 - (((CurrentDate.Year - ProductSale.Date.Year) * 12) + CurrentDate.Month - 1 - ProductSale.Date.Month);

                if (TotalAmounts.ContainsKey(ProductSale.Category))
                {
                    TotalAmounts[ProductSale.Category] += Decimal.ToDouble(ProductSale.TotalAmount);
                }
                else
                {
                    MonthlyAmounts.Add(ProductSale.Category, new Dictionary<int, double>{
                        { 1, 0 },
                        { 2, 0 },
                        { 3, 0 },
                        { 4, 0 },
                        { 5, 0 },
                        { 6, 0 }
                    });
                    TotalAmounts.Add(ProductSale.Category, Decimal.ToDouble(ProductSale.TotalAmount));
                }

                MonthlyAmounts[ProductSale.Category][Month] += Decimal.ToDouble(ProductSale.TotalAmount);
            }

            var CategoriesData = TotalAmounts.ToList();

            CategoriesData.Sort((EntryA, EntryB) => EntryA.Value.CompareTo(EntryB.Value));

            var TopCategories = CategoriesData.Take(10).ToDictionary();

            foreach(var Category in MonthlyAmounts)
            {
                if (!TopCategories.ContainsKey(Category.Key))
                {
                    MonthlyAmounts.Remove(Category.Key);
                }
            }

            TendenciesDTO Tendencies = new TendenciesDTO{ 
                Results = new Dictionary<String, Dictionary<int, double>> { }
            };

            PythonEngine.Initialize();

            foreach(var Entry in MonthlyAmounts)
            {
                using (Py.GIL())
                {
                    double[] MonthlyTotals = {
                        Entry.Value[1],
                        Entry.Value[2],
                        Entry.Value[3],
                        Entry.Value[4],
                        Entry.Value[5],
                        Entry.Value[6]
                    };
                    var Scope = Py.CreateScope();

                    Scope.Set("totales", MonthlyTotals.ToPython());
                    Scope.Exec(
    @"
import numpy as np
from sklearn.linear_model import LinearRegression

meses = np.array([
    [1],
    [2],
    [3],
    [4],
    [5],
    [6],
])

modelo = LinearRegression()
modelo.fit(meses, totales)

proximosMeses = np.array([
    [7],
    [8],
    [9],
    [10],
    [11],
    [12]
])
prediccionFinal = modelo.predict(proximosMeses)
");

                    dynamic Prediction = Scope.Get("prediccionFinal");
                    
                    Tendencies.Results.Add(Entry.Key, new Dictionary<int, double>
                    {
                        {1, Math.Round(Double.Parse(Prediction[0].ToString()), 2)},
                        {2, Math.Round(Double.Parse(Prediction[1].ToString()), 2)},
                        {3, Math.Round(Double.Parse(Prediction[2].ToString()), 2)},
                        {4, Math.Round(Double.Parse(Prediction[3].ToString()), 2)},
                        {5, Math.Round(Double.Parse(Prediction[4].ToString()), 2)},
                        {6, Math.Round(Double.Parse(Prediction[5].ToString()), 2)}
                    });
                }
            }

            AppContext.SetSwitch("System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization", true);
            PythonEngine.Shutdown();
            AppContext.SetSwitch("System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization", false);

            return Tendencies;
        }
    }
}
