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
            //method to test api
            return new string[] { "value1", "value2", DateTime.Now.ToString() };
        }        

        // POST api/cube
        [HttpPost]
        public ActionResult Post(string value)
        {
            List<string> lines = new List<string>();
            
            using (System.IO.StringReader reader = new System.IO.StringReader(value))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {                    
                    lines.Add(line);
                }
            }

            var result = string.Empty;

            if (IsNumeric(lines[0].ToString()))
            {
                int numberTestCases = Convert.ToInt32(lines[0]);
                Dictionary<int, string> dicTestCases = checkNumberTestCases(lines);

                if (numberTestCases == dicTestCases.Count)
                {
                    if (CheckConstrainNumberOfTestCases(numberTestCases))
                    {
                        for (int indexDic = 0; indexDic < dicTestCases.Count; indexDic++)
                        {
                            if (IsValidCubeDimension(dicTestCases.ElementAt(indexDic).Value))
                            {
                                //create cube dimension
                                var cubeDimension = Convert.ToInt32(dicTestCases.ElementAt(indexDic).Value.Split(' ')[0]);

                                if (CheckConstrainCubeDimension(cubeDimension))
                                {
                                    _cubeService.CreateCube(cubeDimension, cubeDimension, cubeDimension);

                                    var numberOfOperations = Convert.ToInt32(dicTestCases.ElementAt(indexDic).Value.Split(' ')[1]);

                                    if (CheckConstrainNumberOfOperations(numberOfOperations))
                                    {
                                        int indexOp = dicTestCases.ElementAt(indexDic).Key + 1;

                                        if (CheckIfExistNextItem(dicTestCases, indexDic + 1))
                                        {
                                            int endIndexOp = dicTestCases.ElementAt(indexDic + 1).Key;
                                            for (int indexLine = indexOp; indexLine < endIndexOp; indexLine++)
                                            {
                                                string itemLine = lines[indexLine];
                                                if (itemLine.ToLower().StartsWith("update"))
                                                {
                                                    ValidateAndExecuteUpdateOperation(itemLine, cubeDimension, result);
                                                }

                                                if (itemLine.ToLower().StartsWith("query"))
                                                {
                                                    ValidateAndExecuteQueryOperation(itemLine, cubeDimension, ref result);
                                                }
                                                //perform query or update
                                            }

                                        }
                                        else
                                        {
                                            for (int indexLine = indexOp; indexLine < lines.Count; indexLine++)
                                            {
                                                //perform query or update
                                                string itemLine = lines[indexLine];
                                                if (itemLine.ToLower().StartsWith("update"))
                                                {
                                                    ValidateAndExecuteUpdateOperation(itemLine, cubeDimension, result);
                                                }

                                                if (itemLine.ToLower().StartsWith("query"))
                                                {
                                                    ValidateAndExecuteQueryOperation(itemLine, cubeDimension, ref result);
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


            return Content(result);
        }


        private void ValidateAndExecuteQueryOperation(string itemLine, int cubeDimension, ref string result)
        {
            if (IsValidQuerySentence(itemLine))
            {                
                bool blnValidX1 = false, blnValidY1 = false, blnValidZ1 = false, 
                    blnValidX2 = false, blnValidY2 = false, blnValidZ2 = false, blnValidConstrainX = false,
                    blnValidConstrainY = false, blnValidConstrainZ = false;
                int intX1 = 0, intY1 = 0, intZ1 = 0, intX2 = 0, intY2 = 0, intZ2 = 0;
                //QUERY 1 1 1 3 3 3
                ValidateTypeOfAQueryItem("x1", itemLine.Split(' ')[1].ToString(), cubeDimension, ref result, ref blnValidX1, ref intX1);
                ValidateTypeOfAQueryItem("y1", itemLine.Split(' ')[2].ToString(), cubeDimension, ref result, ref blnValidY1, ref intY1);
                ValidateTypeOfAQueryItem("z1", itemLine.Split(' ')[3].ToString(), cubeDimension, ref result, ref blnValidZ1, ref intZ1);
                ValidateTypeOfAQueryItem("x2", itemLine.Split(' ')[4].ToString(), cubeDimension, ref result, ref blnValidX2, ref intX2);
                ValidateTypeOfAQueryItem("y2", itemLine.Split(' ')[5].ToString(), cubeDimension, ref result, ref blnValidY2, ref intY2);
                ValidateTypeOfAQueryItem("z2", itemLine.Split(' ')[6].ToString(), cubeDimension, ref result, ref blnValidZ2, ref intZ2);

                if (blnValidX1 && blnValidY1 && blnValidZ1 && blnValidX2 && blnValidY2 && blnValidZ2)
                {
                    blnValidConstrainX = CheckConstrainOfTwoQueryItems("x1", intX1, "x2", intX2, cubeDimension, ref result);
                    blnValidConstrainY = CheckConstrainOfTwoQueryItems("y1", intY1, "y2", intY2, cubeDimension, ref result);
                    blnValidConstrainZ = CheckConstrainOfTwoQueryItems("z1", intZ1, "z2", intZ2, cubeDimension, ref result);
                }

                if (blnValidConstrainX && blnValidConstrainY && blnValidConstrainZ)
                {
                    int queryResult = _cubeService.QueryInCube(intX1, intY1, intZ1, intX2, intY2, intZ2);
                    result = result + "<br>" + $"- {queryResult}";
                }
                
            }
            else
            {
                result = result + "<br>" + "- Query sentence is not valid.";
            }
        }

        private bool CheckConstrainOfTwoQueryItems(string nameValue1, int value1, string nameValue2, int value2, int cubeDimension, ref string result)
        {
            if (value1 > 0 && value1 <= value2 && value2 <= cubeDimension)
                return true;
            else
            {
                result = result + "<br>" + $"- {nameValue1} and/or {nameValue2} out of the limit (1 <= {nameValue1} <= {nameValue2} <= N ) ";
                return false;
            }
        }

        private void ValidateTypeOfAQueryItem(string valueName, string itemLine, int cubeDimension, ref string result, ref bool blnValidToProcess, ref int intValue)
        {
            if (IsNumeric(itemLine))
            {
                intValue = Convert.ToInt32(itemLine);                
                blnValidToProcess = true;
            }
            else
            {
                blnValidToProcess = false;
                result = result + "<br>" + "- " + valueName + " is not a numeric value.";
            }
        }
        
        private void ValidateAndExecuteUpdateOperation(string itemLine,int cubeDimension, string result)
        {
            if (IsValidUpdateSentence(itemLine))
            {
                //UPDATE 2 2 2 4                
                bool blnValidToProcess = false;
                int intX = 0, intY = 0, intZ = 0, intW = 0;
                ValidateTypeAndCheckConstraintOfAnUpdateItem("x", itemLine.Split(' ')[1].ToString(), cubeDimension, ref result, ref blnValidToProcess, ref intX);
                ValidateTypeAndCheckConstraintOfAnUpdateItem("y", itemLine.Split(' ')[2].ToString(), cubeDimension, ref result, ref blnValidToProcess, ref intY);
                ValidateTypeAndCheckConstraintOfAnUpdateItem("z", itemLine.Split(' ')[3].ToString(), cubeDimension, ref result, ref blnValidToProcess, ref intZ);
                ValidateTypeAndCheckConstraintOfAValueToUpdate("w", itemLine.Split(' ')[4].ToString(), cubeDimension, ref result, ref blnValidToProcess, ref intW);
                                
                if(blnValidToProcess)
                    _cubeService.UpdateInCube(intX, intY, intZ, intW);

            }
            else
            {
                result = result + "<br>" + "- Update sentence is not valid.";
            }            
        }

        private void ValidateTypeAndCheckConstraintOfAValueToUpdate(string valueName, string itemLine, int cubeDimension, ref string result, ref bool blnValidToProcess, ref int intValue)
        {
            if (IsNumeric(itemLine))
            {
                intValue = Convert.ToInt32(itemLine);
                double minusValue = Math.Pow(-10, 9);
                double maxValue = Math.Pow(10, 9);
                if (intValue >= minusValue && intValue <= maxValue)
                    blnValidToProcess = true;
                else
                {
                    blnValidToProcess = false;
                    result = result + "<br>" + "- " + valueName + " value is out of limit (-10exp9 <= W <= 10exp9).";
                }
            }
            else
            {
                blnValidToProcess = false;
                result = result + "<br>" + "- " + valueName + " is not a numeric value.";
            }
        }

        private void ValidateTypeAndCheckConstraintOfAnUpdateItem(string valueName, string itemLine, int cubeDimension, ref string result, ref bool blnValidToProcess, ref int intValue)
        {
            if (IsNumeric(itemLine))
            {
                intValue = Convert.ToInt32(itemLine);
                if (intValue > 0 && intValue <= cubeDimension)
                    blnValidToProcess = true;
                else
                {
                    blnValidToProcess = false;
                    result = result + "<br>" + "- "+ valueName + " value is out of limit (1<=value<=N).";
                }
            }
            else
            {
                blnValidToProcess = false;
                result = result + "<br>" + "- " + valueName + " is not a numeric value.";
            }
        }

        private bool CheckConstrainNumberOfOperations(int numberOfOperations)
        {
            if (numberOfOperations > 0 && numberOfOperations < 1000)
                return true;
            else
                return false;   
        }

        private bool CheckConstrainCubeDimension(int cubeDimension)
        {
            if (cubeDimension > 0 && cubeDimension<= 100)
                return true;
            else
                return false;
        }

        private bool CheckConstrainNumberOfTestCases(int numberOfT)
        {
            if (numberOfT > 0 && numberOfT <= 50)
                return true;
            else
                return false;
        }

        private bool CheckIfExistNextItem(Dictionary<int, string> list, int index)
        {            
            try
            {
                var result = list.ElementAt(index);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private bool IsNumeric(string value)
        {
            int n;
            return int.TryParse(value, out n);
        }

        private bool IsValidCubeDimension(string value)
        {
            //sample: 4 5 -> # #
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"([0-9]{1,3}\s[0-9]{1,4})");
        }

        private bool IsValidUpdateSentence(string value)
        {
            //sample: UPDATE # # # #
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"(UPDATE)\s[0-9]{1,3}\s[0-9]{1,3}\s[0-9]{1,3}\s[0-9]{1,3}");
        }

        private bool IsValidQuerySentence(string value)
        {
            //QUERY # # # # # #
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"(QUERY)\s[0-9]{1,3}\s[0-9]{1,3}\s[0-9]{1,3}\s[0-9]{1,3}\s[0-9]{1,3}\s[0-9]{1,3}");
        }

        private Dictionary<int, string> checkNumberTestCases(List<string> lines)
        {            
            string pattern = @"(^[0-9]{1,3}\s[0-9]{1,4}$)";
            //string pattern = @"([0-9]{1,3}\s[0-9]{1,4})";

            var result = 0;
            Dictionary<int, string> dictResult = new Dictionary<int, string>();
            for (int i = 0; i < lines.Count; i++)
            {                
                if (System.Text.RegularExpressions.Regex.IsMatch(lines[i].ToString(), pattern))
                {
                    dictResult.Add(i, lines[i]);
                    result += 1;
                }
            }
            return dictResult;
        }
        
        
    }
}
