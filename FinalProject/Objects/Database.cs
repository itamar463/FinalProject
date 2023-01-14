using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Objects
{
    
    internal class Database
    {
        private static DataTable PersonsDataTable;
        private static DataTable ExamsDataTable;

        static Database()
        {
            PersonsDataTable = new DataTable();
            ExamsDataTable = new DataTable();
        }

        public static IEnumerable<DataRow> GetPersonsData()
        {
            // Use LINQ to query the data in the DataTable
            return from row in PersonsDataTable.AsEnumerable()
                   select row;
        }
        public static IEnumerable<DataRow> GetExamsData()
        {
            // Use LINQ to query the data in the DataTable
            return from row in ExamsDataTable.AsEnumerable()
                   select row;
        }
    }

}
