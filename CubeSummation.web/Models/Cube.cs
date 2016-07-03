using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CubeSummation.web.Models
{
    public class Cube
    {
        #region Fields

        public int[,,] InternalArray { get; set; }

        #endregion
        
        /// <summary>
        /// Constructor, create the dimension of the cube with x, y, z given parameters.
        /// </summary>
        /// <param name="X">parameter needed to create the dimension.</param>
        /// <param name="Y">parameter needed to create the dimension.</param>
        /// <param name="Z">parameter needed to create the dimension.</param>
        public Cube(int X, int Y, int Z)
        {
            InternalArray = new int[X, Y, Z];
        }
             

    }
}
