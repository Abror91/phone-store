using AutoMapper;
using Phonix.BLL.DTO;
using Phonix.BLL.Infrastructure;
using Phonix.BLL.Interfaces;
using Phonix.DAL.Entities;
using Phonix.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonix.BLL.Services
{
    public class PhoneService : IPhoneService
    {
        private readonly IUnitOfWork _db;
        public PhoneService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<IEnumerable<PhoneDTO>> GetPhones()
        {
            var phones = await _db.Phones.GetPhones();
            var mapper = new MapperConfiguration(c => c.CreateMap<Phone, PhoneDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Phone>, IEnumerable<PhoneDTO>>(phones);
        }

        public async Task<PhoneDTO> GetPhoneById(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            var phone = await _db.Phones.GetPhone(id);
            if (phone == null)
                throw new NullReferenceException();
            var mapper = new MapperConfiguration(c => c.CreateMap<Phone, PhoneDTO>()).CreateMapper();
            return mapper.Map<Phone, PhoneDTO>(phone);
        }

        public async Task<OperationDetails> Add(PhoneDTO phone)
        {
            if (phone == null)
                throw new ArgumentNullException(nameof(phone));
            if (string.IsNullOrEmpty(phone.Model))
                return new OperationDetails(false, "Error. Phone model cannot be empty!", "");
            if (string.IsNullOrEmpty(phone.CompanyName))
                return new OperationDetails(false, "Error. Phone company name cannot be empty!", "");
            if (string.IsNullOrEmpty(phone.ReleaseDate.ToShortDateString()))
                return new OperationDetails(false, "Error. Phone release date cannot be empty!", "");
            if (string.IsNullOrEmpty(phone.CoverImagePath))
                return new OperationDetails(false, "Error. Phone image cannot be empty!", "");
            var p = new Phone
            {
                Model = phone.Model,
                CompanyName = phone.CompanyName,
                ReleaseDate = phone.ReleaseDate,
                CoverImagePath = phone.CoverImagePath
            };
            await _db.Phones.Add(p);
            return new OperationDetails(true, "Successfully created!", "");
        }

        public async Task<OperationDetails> Update(PhoneDTO phone)
        {
            if (phone == null)
                throw new ArgumentNullException(nameof(phone));
            if (string.IsNullOrEmpty(phone.Model))
                return new OperationDetails(false, "Error. Phone model cannot be empty!", "");
            if (string.IsNullOrEmpty(phone.CompanyName))
                return new OperationDetails(false, "Error. Phone company name cannot be empty!", "");
            if (string.IsNullOrEmpty(phone.ReleaseDate.ToShortDateString()))
                return new OperationDetails(false, "Error. Phone release date cannot be empty!", "");
            var p = await _db.Phones.GetPhone(phone.Id);
            if (p == null)
                throw new NullReferenceException();
            p.Model = phone.Model;
            p.CompanyName = phone.CompanyName;
            p.ReleaseDate = phone.ReleaseDate;
            await _db.Phones.Update(p);
            return new OperationDetails(true, "Successfully updated!", "");
        }

        public async Task<OperationDetails> Delete(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            var phone = await _db.Phones.GetPhone(id);
            if (phone == null)
                throw new NullReferenceException();
            await _db.Phones.Delete(phone);
            return new OperationDetails(true, "Successfully deleted!", "");
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
