using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CubeSummation.web.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CubeSummation.web.Controllers
{
    [Route("api/[controller]")]
    public class CubeController : Controller
    {
        private ICubeService _cubeService;

        public CubeController(ICubeService _cubeService)
        {
            this._cubeService = _cubeService;
        }

        // GET: api/cube
        [HttpGet]
        public IEnumerable<string> Get()
        {
            // Method to test api
            return new string[] { "value1", "value2", DateTime.Now.ToString() };
        }        

        // POST api/cube
        [HttpPost]
        public ActionResult Post(string value)
        {
            var result = string.Empty;
            

            if (!string.IsNullOrEmpty(value))
            {
                result = ProcessInput(value);
            }
            else
            {
                result = "There's no input data to be processed.";
            }

            return Content(result);
        }

        /// <summary>
        /// get the input and process each line of the information to create the cube and make update and query sentences
        /// according to the input lines.
        /// </summary>
        /// <param name="inputData">information about the instructions to create and manipulate the cube.</param>
        /// <returns>string value with the results of the processing.</returns>
        private string ProcessInput(string inputData)
        {
            var result = string.Empty;

            try
            {
                // Create a list for each line of the input
                List<string> lines = new List<string>();

                using (System.IO.StringReader reader = new System.IO.StringReader(inputData))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }

                if (_cubeService.IsNumeric(lines[0].ToString()))
                {
                    int numberTestCases = Convert.ToInt32(lines[0]);
                    Dictionary<int, string> dicTestCases = _cubeService.GetNumberTestCases(lines);

                    if (numberTestCases == dicTestCases.Count)
                    {
                        if (_cubeService.CheckConstrainNumberOfTestCases(numberTestCases))
                        {
                            for (int indexDic = 0; indexDic < dicTestCases.Count; indexDic++)
                            {
                                if (_cubeService.IsValidCubeDimension(dicTestCases.ElementAt(indexDic).Value))
                                {
                                    //create cube dimension
                                    var cubeDimension = Convert.ToInt32(dicTestCases.ElementAt(indexDic).Value.Split(' ')[0]);

                                    if (_cubeService.CheckConstrainCubeDimension(cubeDimension))
                                    {
                                        _cubeService.CreateCube(cubeDimension, cubeDimension, cubeDimension);

                                        var numberOfOperations = Convert.ToInt32(dicTestCases.ElementAt(indexDic).Value.Split(' ')[1]);

                                        if (_cubeService.CheckConstrainNumberOfOperations(numberOfOperations))
                                        {
                                            int indexOp = dicTestCases.ElementAt(indexDic).Key + 1;

                                            if (_cubeService.CheckIfExistNextItem(dicTestCases, indexDic + 1))
                                            {
                                                int endIndexOp = dicTestCases.ElementAt(indexDic + 1).Key;
                                                for (int indexLine = indexOp; indexLine < endIndexOp; indexLine++)
                                                {
                                                    string itemLine = lines[indexLine];
                                                    // Perform query or update
                                                    if (itemLine.ToLower().StartsWith("update"))
                                                    {
                                                        _cubeService.ValidateAndExecuteUpdateOperation(itemLine, cubeDimension, result);
                                                    }

                                                    if (itemLine.ToLower().StartsWith("query"))
                                                    {
                                                        _cubeService.ValidateAndExecuteQueryOperation(itemLine, cubeDimension, ref result);
                                                    }                                                    
                                                }

                                            }
                                            else
                                            {
                                                for (int indexLine = indexOp; indexLine < lines.Count; indexLine++)
                                                {
                                                    // Perform query or update
                                                    string itemLine = lines[indexLine];
                                                    if (itemLine.ToLower().StartsWith("update"))
                                                    {
                                                        _cubeService.ValidateAndExecuteUpdateOperation(itemLine, cubeDimension, result);
                                                    }

                                                    if (itemLine.ToLower().StartsWith("query"))
                                                    {
                                                        _cubeService.ValidateAndExecuteQueryOperation(itemLine, cubeDimension, ref result);
                                                    }
                                                }
                                            }

                                        }
                                        else
                                        {
                                            result = result + "<br>" + "- Number of operations out of the limit.";
                                        }
                                    }
                                    else
                                    {
                                        result = result + "<br>" + "- Cube dimension out of the limit.";
                                    }

                                }
                                else
                                {
                                    //error: wrong dimension
                                    result = result + "<br>" + "- Wrong cube dimension.";
                                }

                            }


                        }
                        else
                        {
                            result = result + "<br>" + "- Number of test cases out of the limit.";
                        }



                    }
                    else
                    {
                        //error: wrong number of test cases;
                        result = result + "<br>" + "- Wrong number of test cases.";
                    }

                }
                else
                {
                    result = result + "<br>" + "- Invalid number of test cases.";
                }


            }
            catch (Exception ex)
            {
                result = result + "<br>" + $"- Exception: {ex.ToString()}.";
                throw;
            }            

            return result;
        }
        
        
    }
}
