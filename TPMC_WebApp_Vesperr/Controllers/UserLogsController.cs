﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TPMC_WebApp_Vesperr.Models;
using TPMC_WebApp_Vesperr.Models.SuperAdminViewModels;
using TPMC_WebApp_Vesperr.Data;

namespace TPMC_WebApp_Vesperr.Controllers
{
    [Authorize(Roles = "SuperAdmins")]
    public class UserLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public ViewResult Index()
        {
            return View(_context.UserAuditEvents.ToList());
        }
    }
}
