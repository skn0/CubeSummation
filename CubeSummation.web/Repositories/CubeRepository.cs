using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CubeSummation.web.Repositories
{
    public class CubeRepository : ICubeRepository
    {
        private Models.Cube _cubeEntity;
        
        #region Public Methods

        /// <summary>
        /// Constructor, create the dimension of the cube with x, y, z given parameters.
        /// </summary>
        /// <param name="X">parameter needed to create the dimension.</param>
        /// <param name="Y">parameter needed to create the dimension.</param>
        /// <param name="Z">parameter needed to create the dimension.</param>
        public void CreateCube(int X, int Y, int Z)
        {
            _cubeEntity = new Models.Cube(X, Y, Z);                
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
                        sum += _cubeEntity.InternalArray[x, y, z];
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
            _cubeEntity.InternalArray[x - 1, y - 1, z - 1] = w;
        }

        #endregion
               

    }
}
