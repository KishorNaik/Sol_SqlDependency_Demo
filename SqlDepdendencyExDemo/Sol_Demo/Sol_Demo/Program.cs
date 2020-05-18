using Newtonsoft.Json;
using ServiceBrokerListener.Domain;
using System;
using System.Net;
using System.Xml;
using TableDependency.SqlClient;

namespace Sol_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string connectionString = @"Data Source=DESKTOP-MOL1H66\IDEATORS;Initial Catalog=Db;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            //#region SqlDependencyEx

            // https://github.com/dyatchenko/ServiceBrokerListener

            //// See constructor optional parameters to configure it according to your needs
            //var listener = new SqlDependencyEx(connectionString, "Db", "tblUsers");

            //listener.TableChanged += Listener_TableChanged;

            //listener.Start();

            //Console.ReadLine();

            //listener.Stop();
            //listener.Dispose();

            //#endregion

            #region SqlTableDependency
            // https://github.com/christiandelbianco/monitor-table-change-with-sqltabledependency
            using (var dep = new SqlTableDependency<UserModel>(connectionString, "tblUsers", executeUserPermissionCheck:false))
            {
                dep.OnChanged += Dep_OnChanged;
                dep.Start();

                Console.WriteLine("Press a key to exit");
                Console.ReadKey();

                dep.Stop();
            }
            #endregion
        }

        private static void Dep_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<UserModel> e)
        {
            var changedEntity = e.Entity;

            Console.WriteLine("DML operation: " + e.ChangeType);
            Console.WriteLine("ID: " + changedEntity.UserId);
            Console.WriteLine("Name: " + changedEntity.FirstName);
            Console.WriteLine("Surname: " + changedEntity.LastName);
        }

        //private static void Listener_TableChanged(object sender, SqlDependencyEx.TableChangedEventArgs e)
        //{
        //    var obj = e.Data;

        //    XmlDocument doc = new XmlDocument();

        //    doc.LoadXml(obj.ToString());

        //    string json = JsonConvert.SerializeXmlNode(doc);

        //    var notificationTypeObj = e.NotificationType.ToString();

        //}
    }
}
