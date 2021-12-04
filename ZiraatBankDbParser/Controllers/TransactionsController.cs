using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZiraatBankDbParser.Data;
using ZiraatBankDbParser.Models;

namespace ZiraatBankDbParser.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly MainContext _context;

        public TransactionsController(MainContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index(IFormFile uploadedFile)
        {

            //Remove this line to turn of overwriting DB
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            //Reading data from file
            try
            {
                string data = "";
                using (var reader = new StreamReader(uploadedFile.OpenReadStream()))
                {
                    data = await reader.ReadToEndAsync();
                }

                //Parsing data by transactions
                var transactionsSplit = data.Replace(Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + " F", " #F").Split("#");
                transactionsSplit = transactionsSplit.Where(l => !String.IsNullOrWhiteSpace(l)).ToArray();

                for (int a = 0; a < transactionsSplit.Length; a++)
                {
                    Transaction transaction = new Transaction();
                    //Parsing transactions and settlement categories
                    var separetedTransaction = transactionsSplit[a].Split(Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine);

                    string transactionBlock = separetedTransaction[0];
                    string categoriesBlock = separetedTransaction[1];

                    //Parsing & writing transaction data
                    transaction.FinInst = transactionBlock.Split(Environment.NewLine)[0].Split(":")[1];
                    transaction.FxSetDate = Convert.ToDateTime(transactionBlock.Split(Environment.NewLine)[1].Split(":")[1]);
                    transaction.RecFileId = transactionBlock.Split(Environment.NewLine)[2].Split(":")[1];
                    transaction.TransCurrency = transactionBlock.Split(Environment.NewLine)[3].Split(":")[1];
                    transaction.ReconCurrency = transactionBlock.Split(Environment.NewLine)[4].Split(":")[1];

                    //Parsing & writing totals line data
                    var totals = categoriesBlock.Split("Totals :")[1].Split(" ").Where(l => !String.IsNullOrWhiteSpace(l)).ToArray();
                    transaction.TransAmountCreditTotal = Convert.ToDecimal(totals[0]);
                    transaction.ReconAmountCreditTotal = Convert.ToDecimal(totals[1]);
                    transaction.FeeAmountCreditTotal = Convert.ToDecimal(totals[2]);
                    transaction.TransAmountDebitTotal = Convert.ToDecimal(totals[3]);
                    transaction.ReconAmountDebitTotal = Convert.ToDecimal(totals[4]);
                    transaction.FeeAmountDebitTotal = Convert.ToDecimal(totals[5]);
                    transaction.CountTotalTotal = Convert.ToInt32(totals[6]);
                    transaction.NetValueTotal = Convert.ToDecimal(totals[7]);

                    _context.Add(transaction);
                    await _context.SaveChangesAsync();

                    //Parsing settlement categories
                    var settCategories = categoriesBlock.Split("-+").Last().Split("--")[0].Split(Environment.NewLine).Where(l => !String.IsNullOrWhiteSpace(l)).ToArray();

                    //Parsing & writing categories data
                    for (int i = 0; i < settCategories.Length; i++)
                    {
                        settCategories[i] = settCategories[i].Replace("!", "");

                        var catLine = settCategories[i].Split("  ").Where(l => !String.IsNullOrWhiteSpace(l)).ToArray();

                        _context.SettlementCategories.Add(new SettlementCategory
                        {
                            TransactionID = transaction.ID,
                            CatName = catLine[0],
                            TransAmountCredit = Convert.ToDecimal(catLine[1]),
                            ReconAmountCredit = Convert.ToDecimal(catLine[2]),
                            FeeAmountCredit = Convert.ToDecimal(catLine[3]),
                            TransAmountDebit = Convert.ToDecimal(catLine[4]),
                            ReconAmountDebit = Convert.ToDecimal(catLine[5]),
                            FeeAmountDebit = Convert.ToDecimal(catLine[6]),
                            CountTotal = Convert.ToInt32(catLine[7]),
                            NetValue = Convert.ToDecimal(catLine[8])
                        });
                        await _context.SaveChangesAsync();
                    }
                }

                //Requesting added information from DB
                return View(await _context.Transactions.Include(s => s.SettlementCategories).ToListAsync());
            }
            catch (Exception)
            {
                return new JsonResult("Text file structure error, theck the file and try again");
            }
           
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.ID == id);
        }
    }
}
