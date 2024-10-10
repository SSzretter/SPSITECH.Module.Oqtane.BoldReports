using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Net;
using Oqtane.Controllers;
using Oqtane.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.IO;
using BoldReports.Web.ReportViewer;
using BoldReports.Web.ReportDesigner;
using BoldReports.Web;

namespace SPSITECH.Module.BoldReports.Server.Controllers
{
    [Route("api/{controller}/{action}/{id?}")]
    public class ReportingAPIController : ModuleControllerBase, IReportDesignerController, IReportHelperSettings
    {
        private Microsoft.Extensions.Caching.Memory.IMemoryCache _cache;
        private IWebHostEnvironment _hostingEnvironment;
        internal ReportingExternalServer Server { get; set; }
        public string ServerURL { get; set; }
        public ICredentials ReportServerCredential { get; set; }
       // private readonly AppSettingsServer _appSettings;





        public ReportingAPIController(Microsoft.Extensions.Caching.Memory.IMemoryCache memoryCache,
            IWebHostEnvironment hostingEnvironment,
            ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _cache = memoryCache;
            _hostingEnvironment = hostingEnvironment;
           // _appSettings = appSettings.Value;


           // _reportingAPI = SettingService.GetSetting(ModuleState.Settings, "UseSyncfusion", "False");


            ReportingExternalServer externalServer = new ReportingExternalServer(_hostingEnvironment);
            this.Server = externalServer;
            this.ServerURL = "/api/ReportingAPI";
      
            //this.ReportServerCredential = new System.Net.NetworkCredential("dummy", "dummy");
        }

        [NonAction]
        public void InitializeSettings(ReportHelperSettings helperSettings)
        {
            helperSettings.ReportingServer = this.Server;
        }

        /// <summary>
        /// Get the path of specific file
        /// </summary>
        /// <param name="itemName">Name of the file to get the full path</param>
        /// <param name="key">The unique key for report designer</param>
        /// <returns>Returns the full path of file</returns>
        [NonAction]
        private string GetFilePath(string itemName, string key)
        {
            string targetFolder = this._hostingEnvironment.WebRootPath + "\\";
            targetFolder += "Cache";

            if (!System.IO.Directory.Exists(targetFolder))
            {
                System.IO.Directory.CreateDirectory(targetFolder);
            }

            if (!System.IO.Directory.Exists(targetFolder + "\\" + key))
            {
                System.IO.Directory.CreateDirectory(targetFolder + "\\" + key);
            }

            return targetFolder + "\\" + key + "\\" + itemName;

        }

        /// <summary>
        /// Action (HttpGet) method for getting resource of images in the report.
        /// </summary>
        /// <param name="key">The unique key for request identification.</param>
        /// <param name="image">The name of requested image.</param>
        /// <returns>Returns the image as HttpResponseMessage content.</returns>
        [HttpGet("{key}/{image}")]
        public object GetImage(string key, string image)
        {
            return ReportDesignerHelper.GetImage(key, image, this);
        }

        /// <summary>
        /// Send a GET request and returns the requested resource for a report.
        /// </summary>
        /// <param name="resource">Contains report resource information.</param>
        /// <returns> Resource object for the given key</returns>
        [HttpGet("{resource}")]
        public object GetResource(ReportResource resource)
        {
            return ReportHelper.GetResource(resource, this, _cache);
        }

        /// <summary>
        /// Report Initialization method that is triggered when report begin processed.
        /// </summary>
        /// <param name="reportOptions">The ReportViewer options.</param>
        [NonAction]
        public void OnInitReportOptions(ReportViewerOptions reportOption)
        {

            //You can update report options here
            reportOption.ReportModel.ReportServerUrl = this.ServerURL;
            //reportOption.ReportModel.ReportServerCredential = this.ReportServerCredential;


            /*
			string basePath = _hostingEnvironment.WebRootPath;
             FileStream inputStream = new FileStream(basePath + @"\reports\" + reportOption.ReportModel.ReportPath, FileMode.Open, FileAccess.Read);
            MemoryStream reportStream = new MemoryStream();
            inputStream.CopyTo(reportStream);
            reportStream.Position = 0;
            inputStream.Close();
            reportOption.ReportModel.Stream = reportStream;
            */
        }

        /// <summary>
        /// Report loaded method that is triggered when report and sub report begins to be loaded.
        /// </summary>
        /// <param name="reportOptions">The ReportViewer options.</param>
        [NonAction]
        public void OnReportLoaded(ReportViewerOptions reportOption)
        {
            List<ReportParameter> userParameters = new List<ReportParameter>();
            StringValues authHeaders = Request.Headers["Authorization"];

            if (StringValues.IsNullOrEmpty(authHeaders)) return;

            userParameters.Add(new ReportParameter()
            {
                Name = "Token",
                Values = new List<string>() { authHeaders }
            });
            reportOption.ReportModel.Parameters = userParameters;
        }

        /// <summary>
        /// Action (HttpPost) method for posting the request for designer actions.
        /// </summary>
        /// <param name="jsonData">A collection of keys and values to process the designer request.</param>
        /// <returns>Json result for the current request.</returns>
        [HttpPost]
        public object PostDesignerAction([FromBody] Dictionary<string, object> jsonResult)
        {
            // if(jsonResult["designerAction"].Equals("CategoryCatalog"))
            // {
            //     jsonResult["directoryPath"] = "/reports/";

            //	}
            return ReportDesignerHelper.ProcessDesigner(jsonResult, this, null, this._cache);
        }

        /// <summary>Action (HttpPost) method for posting the request for designer actions.</summary>
        /// <returns>Json result for the current request.</returns>
        [HttpPost]
        public object PostFormDesignerAction()
        {
            return ReportDesignerHelper.ProcessDesigner(null, this, null, this._cache);
        }

        [HttpPost]
        public object PostFormReportAction()
        {
            return ReportHelper.ProcessReport(null, this, this._cache);
        }

        /// <summary>
        /// Action (HttpPost) method for posting the request for report process.
        /// </summary>
        /// <param name="jsonResult">The JSON data posted for processing report.</param>
        /// <returns>The object data.</returns>
        [HttpPost]
        public object PostReportAction([FromBody] Dictionary<string, object> jsonResult)
        {
            return ReportHelper.ProcessReport(jsonResult, this, this._cache);
        }

        /// <summary>
        /// Sets the resource into storage location.
        /// </summary>
        /// <param name="key">The unique key for request identification.</param>
        /// <param name="itemId">The unique key to get the required resource.</param>
        /// <param name="itemData">Contains the resource data.</param>
        /// <param name="errorMessage">Returns the error message, if the write action is failed.</param>
        /// <returns>Returns true, if resource is successfully written into storage location.</returns>
        [NonAction]
        public bool SetData(string key, string itemId, ItemInfo itemData, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (itemData.Data != null)
            {
                System.IO.File.WriteAllBytes(this.GetFilePath(itemId, key), itemData.Data);
            }
            else if (itemData.PostedFile != null)
            {
                var fileName = itemId;
                if (string.IsNullOrEmpty(itemId))
                {
                    fileName = System.IO.Path.GetFileName(itemData.PostedFile.FileName);
                }

                using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                {
                    itemData.PostedFile.OpenReadStream().CopyTo(stream);
                    byte[] bytes = stream.ToArray();
                    var writePath = this.GetFilePath(fileName, key);

                    System.IO.File.WriteAllBytes(writePath, bytes);
                    stream.Close();
                    stream.Dispose();
                }
            }
            return true;
        }

        /// <summary>
        /// Gets the resource from storage location.
        /// </summary>
        /// <param name="key">The unique key for request identification.</param>
        /// <param name="itemId">The unique key to get the required resource.</param>
        ///  <returns>Returns the resource data and error message.</returns>
        [NonAction]
        public ResourceInfo GetData(string key, string itemId)
        {
            var resource = new ResourceInfo();
            try
            {
                var filePath = this.GetFilePath(itemId, key);
                if (itemId.Equals(Path.GetFileName(filePath), StringComparison.InvariantCultureIgnoreCase) && System.IO.File.Exists(filePath))
                {
                    resource.Data = System.IO.File.ReadAllBytes(filePath);
                }
                else
                {
                    resource.ErrorMessage = "File not found from the specified path";
                }
            }
            catch (Exception ex)
            {
                resource.ErrorMessage = ex.Message;
            }
            return resource;
        }

        /// <summary>
        /// Action (HttpPost) method for posted or uploaded file actions.
        /// </summary>
        [HttpPost]
        public void UploadReportAction()
        {
            ReportDesignerHelper.ProcessDesigner(null, this, this.Request.Form.Files[0], this._cache);
        }
    }

}
