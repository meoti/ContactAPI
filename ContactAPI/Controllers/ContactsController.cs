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

namespace ContactAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class ContactsController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IMapper _mapper;
        private const string ContactOwner = "IsContactOwner";

        public ContactsController(IServiceWrapper serviceWrapper, IMapper mapper)
        {
            _serviceWrapper = serviceWrapper;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactDTO>>> GetContacts([FromQuery] PagingOptions pagingOptions)
        {
            IEnumerable<Contact> contacts = await _serviceWrapper.ContactService.GetContactsAsync(pagingOptions);
            IEnumerable<ContactDTO> contactDTOs = _mapper.Map<IEnumerable<ContactDTO>>(contacts);
            return Ok(contactDTOs);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<ContactDTO>>> GetUserContacts(Guid userId, [FromQuery] PagingOptions pagingOptions)
        {
            IEnumerable<Contact> contacts = await _serviceWrapper.ContactService.GetContactsByUserIdAsync(userId, pagingOptions);
            if ( contacts == null || !contacts.Any() )
            {
                return NotFound();
            }
            IEnumerable<ContactDTO> contactDTOs = _mapper.Map<IEnumerable<ContactDTO>>(contacts);
            return Ok(contactDTOs);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesDefaultResponseType]
        [Authorize(Policy = ContactOwner)]
        public async Task<IActionResult> PutContact(Guid id, ContactPutRequestModel contactReq)
        {
            if ( !id.Equals(contactReq.Id) )
            {
                return BadRequest("Contact Id mismatch");
            }

            Contact contact = await _serviceWrapper.ContactService.GetContactByIdAsync(id);
            if ( contact == null )
            {
                return NotFound();
            }
            contact = _mapper.Map(contactReq, contact);
            Results<Contact> results = await _serviceWrapper.ContactService.UpsetContactAsync(contact);

            return results.Succeded ? NoContent() : (IActionResult) BadRequest(results.Errors);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Contact>> CreateContact(ContactRequestModel contactReq)
        {

            Contact dbContact = _mapper.Map<Contact>(contactReq);
            Results<Contact> results = await _serviceWrapper.ContactService.UpsetContactAsync(dbContact);

            return !results.Succeded ? BadRequest(results.Errors) : (ActionResult<Contact>) StatusCode(201, results.Value);
        }

        [HttpDelete("{id}", Name = "DeleteContact")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [Authorize(Policy = ContactOwner)]
        public async Task<ActionResult<ContactDTO>> DeleteContact(Guid id)
        {
            Results<Contact> results = await _serviceWrapper.ContactService.DeleteContactByIdAsync(id);
            return results == null 
                ? NotFound() : results.Succeded 
                ? (ActionResult<ContactDTO>) _mapper.Map<ContactDTO>(results.Value) 
                : StatusCode(500, results.Errors);
        }
    }
}
