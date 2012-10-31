using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenPadMvvm.Data
{
    public class PriorityRepository : IRepository
    {
        private DataContextDataContext _dataContext;

        public PriorityRepository()
        {
            _dataContext = new DataContextDataContext();
        }

        public List<Priority> GetAllPriorities()
        {
            return _dataContext.GetTable<Priority>().ToList();
        }

        #region IDisposable Members

        public void Dispose()
        {
            _dataContext.Dispose();
        }

        #endregion
    }
}
