using CubeSummation.web.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CubeSummation.web.Services
{
    public class CubeService : ICubeService
    {
        private ICubeRepository _repository;

        public CubeService(ICubeRepository _repository)
        {
            this._repository = _repository;
        }

        public void CreateCube(int X, int Y, int Z)
        {
            _repository.CreateCube(X, Y, Z);
        }

        public int QueryInCube(int x1, int y1, int z1, int x2, int y2, int z2)
        {
            return _repository.QueryInCube(x1, y1, z1, x2, y2, z2);
        }


        public void UpdateInCube(int x, int y, int z, int w)
        {
            _repository.UpdateInCube(x, y, z, w);
        }


    }
}
