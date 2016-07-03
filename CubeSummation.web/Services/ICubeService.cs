using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CubeSummation.web.Services
{
    public interface ICubeService
    {
        void CreateCube(int X, int Y, int Z);

        int QueryInCube(int x1, int y1, int z1, int x2, int y2, int z2);

        void UpdateInCube(int x, int y, int z, int w);

    }
}
