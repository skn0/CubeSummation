using CubeSummation.web.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CubeSummation.web.Services
{
    public class CubeService : ICubeService
    {
        #region fields

        private ICubeRepository _repository;

        #endregion

        #region Constructor

        /// <summary>
        ///  Constructor
        /// </summary>
        /// <param name="_repository">repository of the cube</param>
        public CubeService(ICubeRepository _repository)
        {
            this._repository = _repository;
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Creates a cube (3 dimensional matrix).
        /// </summary>
        /// <param name="X">x coordinate for the cube dimension.</param>
        /// <param name="Y">y coordinate for the cube dimension</param>
        /// <param name="Z">z coordinate for the cube dimension</param>
        public void CreateCube(int X, int Y, int Z)
        {
            _repository.CreateCube(X, Y, Z);
        }

        /// <summary>
        /// Performs a query in the cube, uses a starting and ending point per each x, y, z coordinate,
        /// then returns a numeric result.
        /// </summary>
        /// <param name="x1">x coordinate, starting point.</param>
        /// <param name="y1">y coordinate, starting point.</param>
        /// <param name="z1">z coordinate, starting point.</param>
        /// <param name="x2">x coordinate, ending point.</param>
        /// <param name="y2">y coordinate, ending point.</param>
        /// <param name="z2">z coordinate, ending point.</param>
        /// <returns>Sum of found values.</returns>
        public int QueryInCube(int x1, int y1, int z1, int x2, int y2, int z2)
        {
            return _repository.QueryInCube(x1, y1, z1, x2, y2, z2);
        }

        /// <summary>
        /// Performs an update in the cube, uses the x, y, z coordinates to set a w value in the cube.
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <param name="z">z coordinate</param>
        /// <param name="w">value to be set in the cube</param>
        public void UpdateInCube(int x, int y, int z, int w)
        {
            _repository.UpdateInCube(x, y, z, w);
        }

        /// <summary>
        /// Validates if the current line has valid x, y, z coordinates and a valid w value, then performs an update in the cube.
        /// </summary>
        /// <param name="itemLine">current line which has the update statement and the values.</param>
        /// <param name="cubeDimension">N dimension of the cube.</param>
        /// <param name="result">output information of the process.</param>
        public void ValidateAndExecuteUpdateOperation(string itemLine, int cubeDimension, string result)
        {
            if (IsValidUpdateSentence(itemLine))
            {
                //UPDATE 2 2 2 4                
                bool blnValidToProcess = false;
                int intX = 0, intY = 0, intZ = 0, intW = 0;
                ValidateTypeAndCheckConstrainOfAnUpdateItem("x", itemLine.Split(' ')[1].ToString(), cubeDimension, ref result, ref blnValidToProcess, ref intX);
                ValidateTypeAndCheckConstrainOfAnUpdateItem("y", itemLine.Split(' ')[2].ToString(), cubeDimension, ref result, ref blnValidToProcess, ref intY);
                ValidateTypeAndCheckConstrainOfAnUpdateItem("z", itemLine.Split(' ')[3].ToString(), cubeDimension, ref result, ref blnValidToProcess, ref intZ);
                ValidateTypeAndCheckConstrainOfAValueToUpdate("w", itemLine.Split(' ')[4].ToString(), cubeDimension, ref result, ref blnValidToProcess, ref intW);

                if (blnValidToProcess)
                    UpdateInCube(intX, intY, intZ, intW);

            }
            else
            {
                result = result + "<br>" + "- Update sentence is not valid.";
            }
        }

        /// <summary>
        /// Validates if the current line has valid x1, y1, z1, x2, y2, z2 coordinates to perform a query statement in the cube.
        /// </summary>
        /// <param name="itemLine">current line which has the query statment and the coordinates.</param>
        /// <param name="cubeDimension">N dimension of the cube.</param>
        /// <param name="result">output information of the process.</param>
        public void ValidateAndExecuteQueryOperation(string itemLine, int cubeDimension, ref string result)
        {
            if (IsValidQuerySentence(itemLine))
            {
                bool blnValidX1 = false, blnValidY1 = false, blnValidZ1 = false,
                    blnValidX2 = false, blnValidY2 = false, blnValidZ2 = false, blnValidConstrainX = false,
                    blnValidConstrainY = false, blnValidConstrainZ = false;
                int intX1 = 0, intY1 = 0, intZ1 = 0, intX2 = 0, intY2 = 0, intZ2 = 0;
                //QUERY 1 1 1 3 3 3
                ValidateTypeOfAQueryItem("x1", itemLine.Split(' ')[1].ToString(), ref result, ref blnValidX1, ref intX1);
                ValidateTypeOfAQueryItem("y1", itemLine.Split(' ')[2].ToString(), ref result, ref blnValidY1, ref intY1);
                ValidateTypeOfAQueryItem("z1", itemLine.Split(' ')[3].ToString(), ref result, ref blnValidZ1, ref intZ1);
                ValidateTypeOfAQueryItem("x2", itemLine.Split(' ')[4].ToString(), ref result, ref blnValidX2, ref intX2);
                ValidateTypeOfAQueryItem("y2", itemLine.Split(' ')[5].ToString(), ref result, ref blnValidY2, ref intY2);
                ValidateTypeOfAQueryItem("z2", itemLine.Split(' ')[6].ToString(), ref result, ref blnValidZ2, ref intZ2);

                if (blnValidX1 && blnValidY1 && blnValidZ1 && blnValidX2 && blnValidY2 && blnValidZ2)
                {
                    blnValidConstrainX = CheckConstrainOfTwoQueryItems("x1", intX1, "x2", intX2, cubeDimension, ref result);
                    blnValidConstrainY = CheckConstrainOfTwoQueryItems("y1", intY1, "y2", intY2, cubeDimension, ref result);
                    blnValidConstrainZ = CheckConstrainOfTwoQueryItems("z1", intZ1, "z2", intZ2, cubeDimension, ref result);
                }

                if (blnValidConstrainX && blnValidConstrainY && blnValidConstrainZ)
                {
                    int queryResult = QueryInCube(intX1, intY1, intZ1, intX2, intY2, intZ2);
                    result = result + "<br>" + $"- {queryResult}";
                }

            }
            else
            {
                result = result + "<br>" + "- Query sentence is not valid.";
            }
        }

        /// <summary>
        /// Checks if the current number operations fits with the constraint.
        /// </summary>
        /// <param name="numberOfOperations">current number of operations to perform in the cube.</param>
        /// <returns>true: fits with the constraint. false: out of the limit.</returns>
        public bool CheckConstrainNumberOfOperations(int numberOfOperations)
        {
            if (numberOfOperations > 0 && numberOfOperations < 1000)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if the current cube dimension fits with the constraint.
        /// </summary>
        /// <param name="cubeDimension">n dimension of the cube.</param>
        /// <returns>true: fits with the constraint. false: out of the limit.</returns>
        public bool CheckConstrainCubeDimension(int cubeDimension)
        {
            if (cubeDimension > 0 && cubeDimension <= 100)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if the current number of test cases fit with the constraint.
        /// </summary>
        /// <param name="numberOfT">number of test cases.</param>
        /// <returns>true: fits with the constraint. false: out of the limit.</returns>
        public bool CheckConstrainNumberOfTestCases(int numberOfT)
        {
            if (numberOfT > 0 && numberOfT <= 50)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if the exist a particular index in the dictionary.
        /// </summary>
        /// <param name="dictItems">dictionary which contains a key and a string value of elements.</param>
        /// <param name="index">index to validate in the dictionary.</param>
        /// <returns>true: there's an index in the dictionary. false: doesn't exist the particular index in the dictionary.</returns>
        public bool CheckIfExistNextItem(Dictionary<int, string> dictItems, int index)
        {
            try
            {
                var result = dictItems.ElementAt(index);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates if a string value is an integer number.
        /// </summary>
        /// <param name="value">value to validate.</param>
        /// <returns>true: is a numer. false: is not a number.</returns>
        public bool IsNumeric(string value)
        {
            int n;
            return int.TryParse(value, out n);
        }

        /// <summary>
        /// Validates if the value is a valid dimension instruction using a regexp. sample 4 5 -> # #
        /// </summary>
        /// <param name="value">value to validate.</param>
        /// <returns>true: it's a valid instruction. false: it's a invalid instruction.</returns>
        public bool IsValidCubeDimension(string value)
        {
            //sample: 4 5 -> # #
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"([0-9]{1,3}\s[0-9]{1,4})");
        }

        /// <summary>
        /// Gets all the test cases in the list of lines, using a regexp. sample ### ####.
        /// </summary>
        /// <param name="lines">list of instruction lines to process in the cube.</param>
        /// <returns>dictionary which contains the index and the values of the test cases.</returns>
        public Dictionary<int, string> GetNumberTestCases(List<string> lines)
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

        #endregion


        #region Private methods

        /// <summary>
        /// Checks if two values fit with the query constraint.
        /// </summary>
        /// <param name="nameValue1">name of the first value.</param>
        /// <param name="value1">first value.</param>
        /// <param name="nameValue2">name of the second value.</param>
        /// <param name="value2">second value.</param>
        /// <param name="cubeDimension">N dimension of the cube.</param>
        /// <param name="result">output information of the process.</param>
        /// <returns>true: fits with the constraint. false: out of the limit.</returns>
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

        /// <summary>
        /// Checks if  a value is a valid number.
        /// </summary>
        /// <param name="valueName">name of the value.</param>
        /// <param name="itemLine">current line which contains the value.</param>        
        /// <param name="result">output information of the process.</param>
        /// <param name="blnValidToProcess">reference boolean to check if it's a valid or invalid value.</param>
        /// <param name="intValue">reference numeric value of the string input.</param>
        private void ValidateTypeOfAQueryItem(string valueName, string itemLine, ref string result, ref bool blnValidToProcess, ref int intValue)
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

        /// <summary>
        /// Checks if it's a valid numeric value and if fits with the update value constrain.
        /// </summary>
        /// <param name="valueName">name of the value.</param>
        /// <param name="itemLine">current line which contains the value.</param>
        /// <param name="cubeDimension">N dimension of the cube.</param>
        /// <param name="result">output information of the process.</param>
        /// <param name="blnValidToProcess">reference boolean to check if it's a valid or invalid value.</param>
        /// <param name="intValue">reference numeric value of the string input.</param>
        private void ValidateTypeAndCheckConstrainOfAValueToUpdate(string valueName, string itemLine, int cubeDimension, ref string result, ref bool blnValidToProcess, ref int intValue)
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

        /// <summary>
        /// Checks if it's a valid numeric value and if fits with the update coordinate constrain.
        /// </summary>
        /// <param name="valueName">name of the value.</param>
        /// <param name="itemLine">current line which contains the value.</param>
        /// <param name="cubeDimension">N dimension of the cube.</param>
        /// <param name="result">output information of the process.</param>
        /// <param name="blnValidToProcess">reference boolean to check if it's a valid or invalid value.</param>
        /// <param name="intValue">reference numeric value of the string input.</param>
        private void ValidateTypeAndCheckConstrainOfAnUpdateItem(string valueName, string itemLine, int cubeDimension, ref string result, ref bool blnValidToProcess, ref int intValue)
        {
            if (IsNumeric(itemLine))
            {
                intValue = Convert.ToInt32(itemLine);
                if (intValue > 0 && intValue <= cubeDimension)
                    blnValidToProcess = true;
                else
                {
                    blnValidToProcess = false;
                    result = result + "<br>" + "- " + valueName + " value is out of limit (1<=value<=N).";
                }
            }
            else
            {
                blnValidToProcess = false;
                result = result + "<br>" + "- " + valueName + " is not a numeric value.";
            }
        }

        /// <summary>
        /// Validates if the value is a valid update instruction using a regexp. sample UPDATE # # # #
        /// </summary>
        /// <param name="value">value which contains the instruction.</param>
        /// <returns>true: is a valid update sentence. false: is not a valid update sentence.</returns>
        private bool IsValidUpdateSentence(string value)
        {
            //sample: UPDATE # # # #
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"(UPDATE)\s[0-9]{1,3}\s[0-9]{1,3}\s[0-9]{1,3}\s[0-9]{1,3}");
        }

        /// <summary>
        /// Validates if the value is a valid update instruction using a regexp. sample QUERY # # # # # #
        /// </summary>
        /// <param name="value">value which contains the instruction.</param>
        /// <returns>true: is a valid query sentence. false: is not a valid query sentence.</returns>
        private bool IsValidQuerySentence(string value)
        {
            //QUERY # # # # # #
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"(QUERY)\s[0-9]{1,3}\s[0-9]{1,3}\s[0-9]{1,3}\s[0-9]{1,3}\s[0-9]{1,3}\s[0-9]{1,3}");
        }
        
        #endregion

        
    }
}
