using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using ProgressReporting.Models;
using ProgressReporting.Services;

namespace ProgressReporting.Controllers
{
    public class JobController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GlobalJob()
        {
            var viewModel = new GlobalJobViewModel();

            var globalJob = GetExistingGlobalJob();
            if (globalJob != null && globalJob.IsComplete)
            {
                Database.Instance.StoreGlobalJobId("");
            }
            else
            {
                viewModel.Job = globalJob;
            }

            return View(viewModel);
        }

        private Job GetExistingGlobalJob()
        {
            var viewModel = new GlobalJobViewModel();

            var jobId = Database.Instance.GetGlobalJobId();
            if (!string.IsNullOrEmpty(jobId))
            {
                return JobManager.Instance.GetJob(jobId);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult DoGlobalJob()
        {
            var job = GetExistingGlobalJob();

            if (job == null || job.IsComplete)
            {
                // if we were doing this for real, we'd want to do more to protect against a race condition
                // where to global jobs could be kicked off
                job = JobManager.Instance.DoJobAsync(j =>
                {
                    for (var progress = 0; progress <= 100; progress++)
                    {
                        if (j.CancellationToken.IsCancellationRequested)
                        {
                            return;
                        }

                        Thread.Sleep(200);
                        j.ReportProgress(progress);
                    }
                });

                Database.Instance.StoreGlobalJobId(job.Id);
            }

            return Json(new
            {
                JobId = job.Id,
                Progress = job.Progress
            });
        }

        public ActionResult Job()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoJob()
        {
            var job = JobManager.Instance.DoJobAsync(j =>
            {
                for (var progress = 0; progress <= 100; progress++)
                {
                    if (j.CancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    Thread.Sleep(200);
                    j.ReportProgress(progress);
                }
            });

            return Json(new
            {
                JobId = job.Id,
                Progress = job.Progress
            });
        }
    }
}