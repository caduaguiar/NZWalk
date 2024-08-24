using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NZWalk.API.Data;
using NZWalk.API.Models.Domain;
using NZWalk.API.Models.DTO;

namespace NZWalk.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegionsController : ControllerBase
{
    private readonly NZWalksDbContext _dbContext;

    public RegionsController(NZWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    
    [HttpGet]
    public IActionResult GetAll()
    {
        var _regions = _dbContext.Regions.ToList();
        
        // Map Domain Models Dto
        var regionsDto = new List<RegionDto>();
        foreach (var region in _regions)
        {
            regionsDto.Add(new RegionDto()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            });
        }
        
        // return Dto
        return Ok(regionsDto);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public IActionResult GetRegionById([FromRoute] Guid id)
    {
        var _region = _dbContext.Regions.FirstOrDefault(x => x.Id == id);

        if (_region == null)
            return NotFound();

        return Ok(_region);
    }

    [HttpPost]
    public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
    {
        // Map or Convert DTO
        var regionDomainModel = new Region()
        {
            Code = addRegionRequestDto.Code,
            Name = addRegionRequestDto.Name,
            RegionImageUrl = addRegionRequestDto.RegionImageUrl
        };

        _dbContext.Regions.Add(regionDomainModel);
        _dbContext.SaveChanges();

        // Map domain model to Dto
        var regionDto = new RegionDto()
        {
            Id = regionDomainModel.Id,
            Code = regionDomainModel.Code,
            Name = regionDomainModel.Name,
            RegionImageUrl = regionDomainModel.RegionImageUrl
        };
        
        
        return CreatedAtAction(nameof(GetRegionById), new { id = regionDomainModel.Id }, regionDto);
    }

    [HttpPut]
    [Route("{id:Guid}")]
    public IActionResult Update([FromRoute]Guid id, [FromBody] AddRegionRequestDto updateRegionRequestDto)
    {
        // Check if exist
        var regionDOmainInModel = _dbContext.Regions.FirstOrDefault(x => x.Id == id);

        if (regionDOmainInModel == null)
            return NotFound();

        regionDOmainInModel.Code = updateRegionRequestDto.Code;
        regionDOmainInModel.Name = updateRegionRequestDto.Name;
        regionDOmainInModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

        _dbContext.SaveChanges();

        // Create dto
        var regionDto = new RegionDto()
        {
            Id = regionDOmainInModel.Id,
            Code = regionDOmainInModel.Code,
            Name = regionDOmainInModel.Name,
            RegionImageUrl = regionDOmainInModel.RegionImageUrl
        };
        
        return Ok(regionDto);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public IActionResult Delete([FromRoute] Guid id)
    {
        var regionDOmainModel = _dbContext.Regions.FirstOrDefault(x => x.Id == id);

        if (regionDOmainModel == null)
            return NotFound();

        _dbContext.Regions.Remove(regionDOmainModel);
        _dbContext.SaveChanges();

        return Ok();
    }
}