using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CubeSummation.web.Services
{
    public interface ICubeService
    {
        /// <summary>
        /// Creates a cube (3 dimensional matrix).
        /// </summary>
        /// <param name="X">x coordinate for the cube dimension.</param>
        /// <param name="Y">y coordinate for the cube dimension</param>
        /// <param name="Z">z coordinate for the cube dimension</param>
        void CreateCube(int X, int Y, int Z);

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
        int QueryInCube(int x1, int y1, int z1, int x2, int y2, int z2);

        /// <summary>
        /// Performs an update in the cube, uses the x, y, z coordinates to set a w value in the cube.
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <param name="z">z coordinate</param>
        /// <param name="w">value to be set in the cube</param>
        void UpdateInCube(int x, int y, int z, int w);

        /// <summary>
        /// Validates if the current line has valid x1, y1, z1, x2, y2, z2 coordinates to perform a query statement in the cube.
        /// </summary>
        /// <param name="itemLine">current line which has the query statment and the coordinates.</param>
        /// <param name="cubeDimension">N dimension of the cube.</param>
        /// <param name="result">output information of the process.</param>
        void ValidateAndExecuteQueryOperation(string itemLine, int cubeDimension, ref string result);

        /// <summary>
        /// Validates if the current line has valid x, y, z coordinates and a valid w value, then performs an update in the cube.
        /// </summary>
        /// <param name="itemLine">current line which has the update statement and the values.</param>
        /// <param name="cubeDimension">N dimension of the cube.</param>
        /// <param name="result">output information of the process.</param>
        void ValidateAndExecuteUpdateOperation(string itemLine, int cubeDimension, string result);

        /// <summary>
        /// Checks if the current number operations fits with the constraint.
        /// </summary>
        /// <param name="numberOfOperations">current number of operations to perform in the cube.</param>
        /// <returns>true: fits with the constraint. false: out of the limit.</returns>
        bool CheckConstrainNumberOfOperations(int numberOfOperations);

        /// <summary>
        /// Checks if the current cube dimension fits with the constraint.
        /// </summary>
        /// <param name="cubeDimension">n dimension of the cube.</param>
        /// <returns>true: fits with the constraint. false: out of the limit.</returns>
        bool CheckConstrainCubeDimension(int cubeDimension);

        /// <summary>
        /// Checks if the current number of test cases fit with the constraint.
        /// </summary>
        /// <param name="numberOfT">number of test cases.</param>
        /// <returns>true: fits with the constraint. false: out of the limit.</returns>
        bool CheckConstrainNumberOfTestCases(int numberOfT);

        /// <summary>
        /// Checks if the exist a particular index in the dictionary.
        /// </summary>
        /// <param name="dictItems">dictionary which contains a key and a string value of elements.</param>
        /// <param name="index">index to validate in the dictionary.</param>
        /// <returns>true: there's an index in the dictionary. false: doesn't exist the particular index in the dictionary.</returns>
        bool CheckIfExistNextItem(Dictionary<int, string> list, int index);

        /// <summary>
        /// Validates if a string value is an integer number.
        /// </summary>
        /// <param name="value">value to validate.</param>
        /// <returns>true: is a numer. false: is not a number.</returns>
        bool IsNumeric(string value);

        /// <summary>
        /// Validates if the value is a valid dimension instruction using a regexp. sample 4 5 -> # #
        /// </summary>
        /// <param name="value">value to validate.</param>
        /// <returns>true: it's a valid instruction. false: it's a invalid instruction.</returns>
        bool IsValidCubeDimension(string value);

        /// <summary>
        /// Gets all the test cases in the list of lines, using a regexp. sample ### ####.
        /// </summary>
        /// <param name="lines">list of instruction lines to process in the cube.</param>
        /// <returns>dictionary which contains the index and the values of the test cases.</returns>
        Dictionary<int, string> GetNumberTestCases(List<string> lines);

    }
}
