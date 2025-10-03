using Microsoft.AspNetCore.Mvc;
using PLinkageAPI.Models;
using PLinkageAPI.Repository;
using MongoDB.Driver;
using PLinkageShared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PLinkageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectOwnerController : ControllerBase
    {
        private readonly ProjectOwnerRepository _projectOwners;

        public ProjectOwnerController(ProjectOwnerRepository projectOwners)
        {
            _projectOwners = projectOwners;
        }

        // GET: api/ProjectOwner/filter?status=Active Only&category=By Specific Location&location=CebuCity
        [HttpGet("adminfilter")]
        public async Task<IActionResult> GetFiltered(
            [FromQuery] string? status = "All",
            [FromQuery] string? category = "All",
            [FromQuery] CebuLocation? location = null)
        {
            // Build MongoDB filters sequentially
            // This is the MongoDB Builders<T> syntax for making filters
            var filter = Builders<ProjectOwner>.Filter.Empty; // start with no filter

            // ne means not equal
            // Status filter
            if (status == "Active Only")
                filter &= Builders<ProjectOwner>.Filter.Ne(po => po.UserStatus, "Deactivated");
            // This means, we only add the filter to the list if the selector given through the API matches
            else if (status == "Deactivated Only")
                filter &= Builders<ProjectOwner>.Filter.Eq(po => po.UserStatus, "Deactivated");
            // Meaning, we filter those where the userStatus is eq or equal to Deactivated
            // Location filter
            if (category == "By Specific Location" && location.HasValue)
                filter &= Builders<ProjectOwner>.Filter.Eq(po => po.UserLocation, location.Value);

            // Apply filter
            var filteredProjectOwners = await _projectOwners.FilterAsync(filter);
            return Ok(filteredProjectOwners);
        }
    }
}

// For proximity based, it is simple, first we receive the location
// Then against the CebuDictionary list, we compute all the integers that should be recommended and add them to a list
// Then we check list.Contains(LocationOfDataBaseItem) if it does, match it