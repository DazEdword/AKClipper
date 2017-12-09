using Microsoft.AspNetCore.Mvc;
using AKCWebCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using NonFactors.Mvc.Grid;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace AKCWeb.Controllers {

    public class HomeController : Controller {

        public ParserWebHelper Helper;
        public IConfiguration Config;
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private const int max_rows = 99999;

        public HomeController(ParserWebHelper helper) {
            Helper = helper;
        }

        public ActionResult Index() {
            ViewData["Title"] = "Home";
            return View("Index", Helper);
        }

        [Route("/new")]
        public ActionResult New() {
            Helper.Reset();
            return RedirectToAction("Index");
        }

        [Route("/results")]
        public ActionResult Results() {
            ViewData["Title"] = "Results";
            return View("Index", Helper);
        }

        [Route("home/export")]
        public ActionResult ExportResults() {
            return ExportGrid();
        }

        [Route("home/export_all")]
        public ActionResult ExportAll() {
            return ExportGrid(max_rows);
        }

        [HttpPost]
        public IActionResult Parse(string content, string language) {
            if (content != null && content.Length > 0) {
                return ViewComponent("ParserController", new {
                    content = content,
                    language = language,
                });
            } else {
                //TODO Not found added just temporarily, return useful error message for user. 
                return NotFound();
            }  
        }

        [HttpGet]
        public ActionResult ExportGrid(int rowsPerPage = 10) {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage()) {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<AKCCore.Clipping> grid = CreateExportableGrid(rowsPerPage);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns) {
                    column.IsEncoded = false;
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;
                }

                foreach (IGridRow<AKCCore.Clipping> gridRow in grid.Rows) {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns) {
                        column.IsEncoded = false;
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);
                    }
                        
                    row++;
                }

                package.Save();

                return File(package.GetAsByteArray(), XlsxContentType, "my_clippings.xlsx");
            }
        }

        private IGrid<AKCCore.Clipping> CreateExportableGrid(int rowsPerPage = 10) {

            List<AKCCore.Clipping> clippingData = Helper.clippingData;

            IGrid<AKCCore.Clipping> grid = new Grid<AKCCore.Clipping>(clippingData);
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };
            grid.Query = Request.Query;

            grid.Columns.Add(item => item.Author).Titled("Author");
            grid.Columns.Add(item => item.BookName).Titled("Book Name");
            grid.Columns.Add(item => item.ClippingType).Titled("ClippingType");
            grid.Columns.Add(item => item.DateAdded).Titled("Date Added");
            grid.Columns.Add(item => item.Location).Titled("Location");
            grid.Columns.Add(item => item.Text).Titled("Text");

            grid.Pager = new GridPager<AKCCore.Clipping>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = rowsPerPage;

            foreach (IGridColumn column in grid.Columns) {
                column.IsFilterable = true;
                column.IsSortable = true;
            }

            return grid;
        }
    }
}