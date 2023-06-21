using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiDemo.Data;
using WebApiDemo.Models;

namespace WebApiDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CotactsController : Controller
    {
        private readonly ContactApiDbContext dbContext;

        public CotactsController(ContactApiDbContext DbContext)
        {
            dbContext = DbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            return Ok (await dbContext.Contacts.ToListAsync());
            
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if(contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }




        [HttpPost]
        public async Task<IActionResult> AddContacts(AddContactRequest AddContactRequest)
        {
            var Contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Address = AddContactRequest.Address,
                Email = AddContactRequest.Email,
                FullName = AddContactRequest.FullName,
                Phone = AddContactRequest.Phone,

            };
          await  dbContext.Contacts.AddAsync(Contact);
            await dbContext.SaveChangesAsync();
            return Ok(Contact);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContcatRequest updateContcatRequest)
        {
         var contact = await dbContext.Contacts.FindAsync(id);
            if (contact != null) {
                contact.FullName = updateContcatRequest.FullName;
                contact.Address = updateContcatRequest.Address;
                contact.Email = updateContcatRequest.Email;
                contact.Phone = updateContcatRequest.Phone;
                await dbContext.SaveChangesAsync();
                return Ok(contact);
            
            }
            return NotFound();
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
           var contact = await dbContext.Contacts.FindAsync(id);
            if (contact != null)
            {
                dbContext.Remove(contact);
               await dbContext.SaveChangesAsync();
                return Ok(contact);
            }
            return NotFound();

        }

    }
}
