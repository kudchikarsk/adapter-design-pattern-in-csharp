using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace Example3
{
    class Program
    {
        public class DataRenderer
        {
            private readonly IDbDataAdapter _dataAdapter;

            public DataRenderer(IDbDataAdapter dataAdapter)
            {
                _dataAdapter = dataAdapter;
            }

            public void Render(TextWriter writer)
            {
                writer.WriteLine("Rendering Data:");
                var myDataSet = new DataSet();

                _dataAdapter.Fill(myDataSet);

                foreach (DataTable table in myDataSet.Tables)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        writer.Write(column.ColumnName.PadRight(20) + " ");
                    }
                    writer.WriteLine();
                    foreach (DataRow row in table.Rows)
                    {
                        for (int i = 0; i < table.Columns.Count; i++)
                        {
                            writer.Write(row[i].ToString().PadRight(20) + " ");
                        }
                        writer.WriteLine();
                    }
                }
            }
        }

        class PersonCollectionDbAdapter : IDbDataAdapter
        {
            private readonly IEnumerable<Person> _persons;

            public PersonCollectionDbAdapter(IEnumerable<Person> patterns)
            {
                _persons = patterns;
            }

            public int Fill(DataSet dataSet)
            {
                var myDataTable = new DataTable();
                myDataTable.Columns.Add(new DataColumn("Name", typeof(string)));
                myDataTable.Columns.Add(new DataColumn("Description", typeof(int)));

                foreach (var person in _persons)
                {
                    var myRow = myDataTable.NewRow();
                    myRow[0] = person.Name;
                    myRow[1] = person.Age;
                    myDataTable.Rows.Add(myRow);
                }
                dataSet.Tables.Add(myDataTable);
                dataSet.AcceptChanges();

                return myDataTable.Rows.Count;
            }

            //Below methods are not implemented because they are useless in our scenario

            public DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType)
            {
                throw new NotImplementedException();
            }

            public IDataParameter[] GetFillParameters()
            {
                throw new NotImplementedException();
            }

            public int Update(DataSet dataSet)
            {
                throw new NotImplementedException();
            }

            public MissingMappingAction MissingMappingAction
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public MissingSchemaAction MissingSchemaAction
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public ITableMappingCollection TableMappings
            {
                get { throw new NotImplementedException(); }
            }

            public IDbCommand SelectCommand
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public IDbCommand InsertCommand
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public IDbCommand UpdateCommand
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public IDbCommand DeleteCommand
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }
        }

        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        static void Main(string[] args)
        {
            List<Person> persons = new List<Person>() {
                new Person(){ Name ="Foo", Age = 25} ,
                new Person(){ Name ="Bar", Age = 25}
            };

            var renderer = new DataRenderer(new PersonCollectionDbAdapter(persons));
            renderer.Render(Console.Out);
            Console.ReadLine();
        }
    }
}