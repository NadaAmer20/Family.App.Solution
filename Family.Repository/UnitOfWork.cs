//using Family.Core.Entities;
//using Family.Core.Repository.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Family.Service.Service
//{
//    public class UnitOfWork : IUnitOfWork
//    {
//        private readonly FamilyContext _context;
//        private IGenericRepository<Person> _persons;
//        private IGenericRepository<Clan> _clans;
//        private IGenericRepository<Branch> _branches;

//        public UnitOfWork(FamilyContext context)
//        {
//            _context = context;
//        }

//        public IGenericRepository<Person> Persons => _persons ??= new GenericRepository<Person>(_context);
//        public IGenericRepository<Clan> Clans => _clans ??= new GenericRepository<Clan>(_context);
//        public IGenericRepository<Branch> Branches => _branches ??= new GenericRepository<Branch>(_context);

//        public async Task<int> CompleteAsync()
//        {
//            return await _context.SaveChangesAsync();
//        }

//        public void Dispose()
//        {
//            _context.Dispose();
//        }
//    }
//}
