using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CubeSummation.web.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CubeSummation.web.Controllers
{
    [Route("api/[controller]")]
    public class CubeController : Controller
    {
        private ICubeService _cubeService;
        private Repositories.ICubeRepository _cubeRepository;


        public CubeController()
        {
        }

        // GET: api/cube
        [HttpGet]
        public IEnumerable<string> Get()
        {

            return new string[] { "value1", "value2" };
        }

        // GET api/cube/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/cube
        [HttpPost]
        public void Post(string value)
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
                       

            if (IsNumeric(lines[0].ToString()))
            {
                int numberTestCases = Convert.ToInt32(lines[0]);
                Dictionary<int, string> dicTestCases = checkNumberTestCases(lines);

                if (numberTestCases == dicTestCases.Count)
                {
                    for (int indexDic = 0; indexDic < dicTestCases.Count; indexDic++)
                    {
                        if (IsValidCubeDimension(dicTestCases.ElementAt(indexDic).Value))
                        {
                            //create cube dimension
                            int indexOp = dicTestCases.ElementAt(indexDic).Key + 1;

                            if (checkIfExistNextItem(dicTestCases, indexDic + 1))
                            {
                                int endIndexOpEnd = dicTestCases.ElementAt(indexDic + 1).Key;
                                for (int indexLine = indexOp; indexLine < endIndexOpEnd; indexLine++)
                                {
                                    var element = lines[indexLine];
                                    //perform query or update
                                }

                            }
                            else
                            {
                                for (int indexLine = indexOp; indexLine < lines.Count; indexLine++)
                                {
                                    var element = lines[indexLine];
                                    //perform query or update
                                }
                            }
                        }
                        else
                        {

                            //error: wrong dimension
                        }                                             

                    }
                   
                }
                else
                {
                    //error: wrong number of test cases;
                }

                //for (int i = 1; i < lines.Count; i++)
                //{
                //    if (IsValidCubeDimension(lines[i].ToString()))
                //    {
                //        var N = Convert.ToInt32(lines[1].Split(' ')[0]);
                //        var M = Convert.ToInt32(lines[1].Split(' ')[1]);
                //    }

                //}
            }







        }



        private bool checkIfExistNextItem(Dictionary<int, string> list, int index)
        {            
            try
            {
                var result = list[index];
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
            return int.TryParse("123", out n);
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
                //result+= System.Text.RegularExpressions.Regex.Matches(item.ToString(), pattern).Count;
                if (System.Text.RegularExpressions.Regex.IsMatch(lines[i].ToString(), pattern))
                {
                    dictResult.Add(i, lines[i]);
                    result += 1;
                }
            }

            //foreach (var item in lines)
            //{
            //    //result+= System.Text.RegularExpressions.Regex.Matches(item.ToString(), pattern).Count;
            //    if (System.Text.RegularExpressions.Regex.IsMatch(item.ToString(), pattern))
            //    {
            //        dictResult.Add(item)
            //        result += 1;
            //    }
            //}

            //return result;
            return dictResult;
        }


        // PUT api/cube/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/cube/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
