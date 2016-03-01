using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.DDb.SDKApp
{
    class Program
    {
        //1.
        static  string endpointUrl;
        static string authorizationKey;
        //2.
        static DocumentClient   ddbClient = null;
        static DocumentCollection docCCollection = null;
        static Document document = null; 
        //3.
        static void Main(string[] args)
        {
            //4.
            endpointUrl = ConfigurationManager.AppSettings["DDbEndPoint"];
            authorizationKey = ConfigurationManager.AppSettings["DDbMasterKey"];

            try
            {
                //5.
                    var database =  createDatabase("CollegesDb").Result;
                //6.
                    //  createDocumentCollection(database, "CollegesInfo");
                //8
               getDatafromDocument(database, "CollegesInfo", "MSEngg");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Occured " + ex.Message);
            }
            Console.ReadLine();
        }

        //Method to Create a Database in DocumentDb
        static async Task<Database> createDatabase(string ddbName)
        {
            //5a.  
              ddbClient = new DocumentClient(new Uri(endpointUrl),authorizationKey);
            //5b. 
            Database ddbDatabase = ddbClient.CreateDatabaseQuery()
                            .Where(d=>d.Id==ddbName).AsEnumerable().FirstOrDefault();
            if (ddbDatabase == null)
            {
                //5c.
                ddbDatabase = await ddbClient.CreateDatabaseAsync(new Database()
                {
                    Id = ddbName
                });

                Console.WriteLine("The DocumentDB of name " + ddbName + " created successfully.");
            }
            else {
                Console.WriteLine("This Database is already available");
            }
            return ddbDatabase;
        }

        static async void createDocumentCollection(Database ddb,string colName)
        {
            //6a.  
            docCCollection = ddbClient.CreateDocumentCollectionQuery("dbs/" + ddb.Id)
                        .Where(c => c.Id == colName).AsEnumerable().FirstOrDefault();

            
            if (docCCollection == null)
            {
                //6b. 
                docCCollection = await ddbClient.CreateDocumentCollectionAsync("dbs/" + ddb.Id,
                    new DocumentCollection
                    {
                        Id = colName
                    });

                Console.WriteLine ("Created dbs"+ ddb.Id +"Collection "+ colName);

                
            }
            //7
            createDocumentData(ddb, docCCollection.Id, "Colleges");

        }

        static async void createDocumentData(Database ddb,string collectionName,string docname)
        {
            // 7a.
            document = ddbClient.CreateDocumentQuery("dbs/"+ddb.Id+"/colls/"+ collectionName)
                                .Where(d=>d.Id==docname).AsEnumerable().FirstOrDefault();
            if (document == null)
            {
                //7b.
                //Record 1
                College msEngg = new College()
                {
                     collegeId="MSEngg",
                     collegeName="MS College of Engineeering",
                     city="Pune",
                     state="Maharashtra",
                     branches=new Branch[] {
                         new Branch() {
                              branchId="Mech",branchName="Mechanical",capacity=2,
                               courses = new Course[] {
                                    new Course() {courseId="EngDsgn",courseName="Engineering Design",isOtional=false },
                                    new Course() {courseId="MacDw",courseName="Machine Drawing",isOtional=true }
                               }
                         },
                         new Branch() {
                              branchId="CS",branchName="Computer Science",capacity=1,
                               courses = new Course[] {
                                    new Course() {courseId="DS",courseName="Data Structure",isOtional=false },
                                    new Course() {courseId="TOC",courseName="Theory of Computation",isOtional=true },
                                    new Course() {courseId="CPD",courseName="Compiler Design",isOtional=false }
                               }
                         }
                     } 
                };

                //7c.
                await ddbClient.CreateDocumentAsync("dbs/" + ddb.Id + "/colls/" + collectionName, msEngg);


                //Record 2
                College lsEngg = new College()
                {
                    collegeId = "LSEngg",
                    collegeName = "LS College of Engineeering",
                    city = "Nagpur",
                    state = "Maharashtra",
                    branches = new Branch[] {
                         new Branch() {
                              branchId="Cvl",branchName="Civil",capacity=2,
                               courses = new Course[] {
                                    new Course() {courseId="EngDsgn",courseName="Engineering Design",isOtional=false },
                                    new Course() {courseId="TOM",courseName="Theory of Mechanics",isOtional=true }
                               }
                         },
                         new Branch() {
                              branchId="CS",branchName="Computer Science",capacity=3,
                               courses = new Course[] {
                                    new Course() {courseId="APS",courseName="Application Software",isOtional=false },
                                    new Course() {courseId="TOC",courseName="Theory of Computation",isOtional=true },
                                    new Course() {courseId="TNW",courseName="Theory of Computer Networks",isOtional=false }
                               }
                         },
                         new Branch() {
                              branchId="IT",branchName="Information Technology",capacity=3,
                               courses = new Course[] {
                                    new Course() {courseId="DS",courseName="Data Structure",isOtional=false },
                                    new Course() {courseId="CSX",courseName="Computer Security Extension",isOtional=true },
                                    new Course() {courseId="APS",courseName="Application Software",isOtional=false }
                               }
                         }
                     }
                };
                await ddbClient.CreateDocumentAsync("dbs/" + ddb.Id + "/colls/" + collectionName, lsEngg);


                Console.WriteLine("Created dbs/"+ddb.Id+"/colls/"+ collectionName + "/docs/"+docname);
            }

        }

        static void getDatafromDocument(Database ddb,string collectioName, string docId)
        {
            //8a.
            var colleages = ddbClient.CreateDocumentQuery("dbs/" + ddb.Id + "/colls/" + collectioName,
                "SELECT * " +
                "FROM CollegesInfo ci " +
                "WHERE ci.id = \"MSEngg\"");

            foreach (var collage in colleages)
            {
                Console.WriteLine("\t Result is"+ collage);
            }
        }

    }
}
