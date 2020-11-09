using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using ContactAPI.Data.Models;
using ContactAPI.Models;
using ContactAPI.Models.DTO;
using ContactAPI.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace ContactAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class SkillsController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IMapper _mapper;
        private const string SkillOwner = "IsSkillOwner";

        public SkillsController(IServiceWrapper serviceWrapper, IMapper mapper)
        {
            _serviceWrapper = serviceWrapper;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SkillContactDTO>>> GetSkills([FromQuery] PagingOptions pagingOptions)
        {
            IEnumerable<Skill> skills = await _serviceWrapper.SkillService.GetSkillsAsync(pagingOptions);
            IEnumerable<SkillContactDTO> skillDTOs = _mapper.Map<IEnumerable<SkillContactDTO>>(skills);
            return Ok(skillDTOs);
        }

        [HttpGet("{level:int}")]
        public async Task<ActionResult<IEnumerable<SkillContactDTO>>> GetSkillsByLevel(ContactEnum.Expertise level, [FromQuery] PagingOptions pagingOptions)
        {
            IEnumerable<Skill> skills = await _serviceWrapper.SkillService.GetSkillsWithContactByLevelAsync(level, pagingOptions);
            IEnumerable<SkillContactDTO> skillDTOs = _mapper.Map<IEnumerable<SkillContactDTO>>(skills);
            return Ok(skillDTOs);
        }


        [HttpPut("{contactId}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesDefaultResponseType]
        [Authorize(Policy = SkillOwner)]
        public async Task<IActionResult> UpdateContactSkill(Guid contactId, [FromQuery] Guid id, SkillPutRequestModel skillreq)
        {
            if ( !id.Equals(skillreq.Id) || !contactId.Equals(skillreq.ContactId) )
            {
                return BadRequest("Skill Id mismatch or Contact Id mismatch");
            }

            Skill skillWithCoctactSkills = await _serviceWrapper.SkillService.GetSkillWithContactsByIdAsync(id, contactId);

            if ( skillWithCoctactSkills == null )
            {
                return NotFound();
            }
            skillWithCoctactSkills = _mapper.Map(skillreq, skillWithCoctactSkills);
            Results<Skill> results = await _serviceWrapper.SkillService.UpdateSkillAsync(skillWithCoctactSkills);

            return results.Succeded ? NoContent() : (IActionResult) BadRequest(results.Errors);

        }


        [HttpPost("{contactId}")]
        public async Task<ActionResult<SkillDTO>> CreateSkill(Guid contactId, SkillRequestModel skillreq)
        {
            Skill skill = _mapper.Map<Skill>(skillreq);
            Results<Skill> results = await _serviceWrapper.SkillService.CreateSkillAsync(contactId, skill);

            return !results.Succeded
                ? BadRequest(results.Errors)
                : (ActionResult<SkillDTO>) StatusCode(201, results.Value);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Skill>> DeleteSkill([FromQuery] Guid id)
        {
            Skill skill = await _serviceWrapper.SkillService.GetSkillWithContactsAsync(id);
            if ( skill == null )
            {
                return NotFound();
            }

            if ( skill.Contacts.Any() )
            {
                return StatusCode(403, "Skill linked to other entities cannot be deleted ");
            }

            await _serviceWrapper.SkillService.DeleteSkillByIdAsync(skill);
            return skill;
        }

        [HttpDelete("{contactId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<SkillDTO>> DeleteContactSkill(Guid contactId, [FromQuery ]Guid id)
        {
            Skill skill = await _serviceWrapper.SkillService.GetSkillWithContactsByIdAsync(id, contactId);

            Results<Skill> results = await _serviceWrapper.SkillService.DeleteContactSkillAsync(id, contactId);

            return results == null ? NotFound() : results.Succeded ? _mapper.Map<SkillDTO>(skill.Contacts) : (ActionResult<SkillDTO>) BadRequest();
        }
    }
}
