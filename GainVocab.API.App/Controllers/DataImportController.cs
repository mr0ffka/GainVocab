using AutoMapper;
using GainVocab.API.Core.Exceptions;
using GainVocab.API.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using System.Globalization;
using GainVocab.API.Core.Models.CourseData;
using CsvHelper.Configuration;

namespace GainVocab.API.App.Controllers
{
    [Route("api/course-data/import")]
    [ApiController]
    [Authorize]
    public class DataImportController : ControllerBase
    {
        private readonly ICourseDataService CoursesData;
        private readonly ICourseService Courses;
        private static IWebHostEnvironment WebHostEnvironment;
        private readonly ILogger<CourseDataController> Logger;
        private readonly IMapper Mapper;

        public DataImportController(ILogger<CourseDataController> logger, ICourseDataService coursesData, ICourseService courses, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            Logger = logger;
            CoursesData = coursesData;
            Courses = courses;
            WebHostEnvironment = webHostEnvironment;
            Mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<string> ImportData([FromQuery] string coursePublicId, [FromForm] IFormFile file)
        {
            if (coursePublicId == null)
                throw new BadRequestException("Provide course!");

            var course = Courses.Get(coursePublicId);
            if (course == null)
                throw new BadRequestException("Course does not exist!");

            if (file == null)
                throw new BadRequestException("Provide file to upload!");

            try
            {
                if (!Directory.Exists(WebHostEnvironment.WebRootPath + "\\DataImportsFiles\\"))
                {
                    Directory.CreateDirectory(WebHostEnvironment.WebRootPath + "\\DataImportsFiles\\");
                }

                using (FileStream fileStream = System.IO.File.Create(WebHostEnvironment.WebRootPath + "\\DataImportsFiles\\" + coursePublicId + "-" + file.FileName))
                {
                    file.CopyTo(fileStream);
                    fileStream.Flush();
                    return "\\DataImportsFiles\\" + file.FileName + "-" + coursePublicId;
                }
            }
            catch (Exception e)
            {
                throw new BadRequestException("Something went wrong during upload. Try again later.");
            }
        }

        [HttpPost("apply")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> ApplyChanges([FromQuery] string coursePublicId)
        {
            if (coursePublicId == null)
                throw new BadRequestException("Provide course!");

            var course = Courses.Get(coursePublicId);
            if (course == null)
                throw new BadRequestException("Course does not exist!");

            try
            {
                var isBadData = false;
                var dataAddModelList = new List<AddModel>();
                var importDataList = new List<ImportDataModel>();
                var importDataExampleList = new List<ImportDataExampleModel>();

                var filePaths = Directory.GetFiles(WebHostEnvironment.WebRootPath + "\\DataImportsFiles\\", $"{coursePublicId}-*.csv", SearchOption.TopDirectoryOnly);
                if (filePaths.Length == 0)
                    throw new NotFoundException("Import files", coursePublicId);

                // read data from csv files and add to lists
                foreach (var filePath in filePaths)
                {
                    using (var streamReader = new StreamReader(filePath))
                    {
                        var isRecordBad = false;
                        var csvReaderConf = new CsvConfiguration(CultureInfo.InvariantCulture) 
                        { 
                            Delimiter = ";", 
                            HasHeaderRecord = true, 
                            BadDataFound = context => { isBadData = true; },
                            Escape = '>' 
                        };
                        using (var csvReader = new CsvReader(streamReader, csvReaderConf))
                        {
                            while (csvReader.Read())
                            {
                                var record = csvReader.GetRecord<dynamic>();
                                var dict = new RouteValueDictionary(record);
                                if (dict.ContainsKey("WordId"))
                                {
                                    csvReader.Context.RegisterClassMap<ImportDataExampleModelMap>();
                                    var records = csvReader.GetRecords<ImportDataExampleModel>().ToList();
                                    importDataExampleList.AddRange(records);
                                }
                                else
                                {
                                    csvReader.Context.RegisterClassMap<ImportDataModelMap>();
                                    var records = csvReader.GetRecords<ImportDataModel>().ToList();
                                    importDataList.AddRange(records);
                                }
                                break;
                            }
                        }
                    }
                }

                // associate data with examples and add do database
                foreach (var data in importDataList)
                {
                    var examples = importDataExampleList.Where(x => x.WordId == data.Id).ToList();
                    var examplesAddModel = Mapper.Map<List<ExampleAddModel>>(examples);      
                    
                    var dataAddModel = Mapper.Map<AddModel>(data);
                    dataAddModel.CoursePublicId = coursePublicId;
                    dataAddModel.Examples = examplesAddModel;

                    dataAddModelList.Add(dataAddModel);
                }

                // add to database
                await CoursesData.Add(dataAddModelList);

                // delete files from server
                foreach (var file in filePaths)
                {
                    System.IO.File.Delete(file);
                }

                return Ok();
            }
            catch (Exception e)
            {
                throw new BadRequestException("Something went wrong during data appliance. Try again later.");
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteFiles([FromQuery] string coursePublicId)
        {
            if (coursePublicId == null)
                throw new BadRequestException("Provide course!");

            var course = Courses.Get(coursePublicId);
            if (course == null)
                throw new BadRequestException("Course does not exist!");

            try
            {
                var files = Directory.GetFiles(WebHostEnvironment.WebRootPath + "\\DataImportsFiles\\", $"{coursePublicId}-*.csv", SearchOption.TopDirectoryOnly);
                if (files.Length == 0) 
                    return Ok();

                foreach (var file in files)
                {
                    System.IO.File.Delete(file);
                }

                return Ok();
            }
            catch (Exception e)
            {
                throw new BadRequestException("Something went wrong during file deletion. Try again later.");
            }
        }
    }
}
