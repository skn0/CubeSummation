using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CubeSummation.web.Models
{
    public class Cube
    {
        #region Fields

        int[,,] InternalArray;

        #endregion

        #region Public Methods

        /// <summary>
        /// Constructor, create the dimension of the cube with x, y, z given parameters.
        /// </summary>
        /// <param name="X">parameter needed to create the dimension.</param>
        /// <param name="Y">parameter needed to create the dimension.</param>
        /// <param name="Z">parameter needed to create the dimension.</param>
        public Cube(int X, int Y, int Z)
        {
            InternalArray = new int[X, Y, Z];
            //InitializeMatrix3D(X, Y, Z);
        }

        /// <summary>
        /// Performs a query in the cube, with a specific x, y, z range, then it returns the sum of the values of the result.
        /// </summary>
        /// <param name="x1">starting x coordinate in the cube.</param>
        /// <param name="y1">starting y coordinate in the cube.</param>
        /// <param name="z1">starting z coordinate in the cube.</param>
        /// <param name="x2">ending x coordinate in the cube.</param>
        /// <param name="y2">ending y coordinate in the cube.</param>
        /// <param name="z2">ending z coordinate in the cube.</param>
        /// <returns>Sum of values of the query.</returns>
        public int QueryInCube(int x1, int y1, int z1, int x2, int y2, int z2)
        {
            int sum = 0;
            x1 = x1 - 1;
            y1 = y1 - 1;
            z1 = z1 - 1;
            x2 = x2 - 1;
            y2 = y2 - 1;
            z2 = z2 - 1;



            for (int x = x1; x <= x2; x++)//X
            {
                for (int y = y1; y <= y2; y++) //Y
                {
                    for (int z = z1; z <= z2; z++)//Z
                    {
                        sum += InternalArray[x, y, z];
                    }

                }//Y

            }//X


            return sum;
        }

        /// <summary>
        /// Peforms an update in the cube, with a specific x, y, z index parameters.
        /// </summary>
        /// <param name="x">x coordinate in the cube.</param>
        /// <param name="y">y coordinate in the cube.</param>
        /// <param name="z">z coordinate in the cube.</param>
        /// <param name="w">value to be set in the cube.</param>
        public void UpdateInCube(int x, int y, int z, int w)
        {
            InternalArray[x - 1, y - 1, z - 1] = w;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// creates the dimension of the cube with x, y, x given parameters.
        /// </summary>
        /// <param name="X">X index needed to create the dimension.</param>
        /// <param name="Y">Y index needed to create the dimension.</param>
        /// <param name="Z">Z index needed to create the dimension.</param>
        private void InitializeCube(int X, int Y, int Z)
        {
            for (int x1 = 0; x1 <= X; x1++)//X
            {
                for (int y1 = 0; y1 <= Y; y1++) //Y
                {
                    for (int z1 = 0; z1 <= Z; z1++)//Z
                    {
                        InternalArray[x1, y1, z1] = 0;
                    }//Z

                }//Y

            }//X
        }
        #endregion

    }
}
