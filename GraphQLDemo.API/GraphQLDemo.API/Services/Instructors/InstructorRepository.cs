
using GraphQLDemo.API.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Services.Instructors
{
    public class InstructorRepository
    {
        private readonly IDbContextFactory<SchoolDBContext> _dBContexFactory;

        public InstructorRepository(IDbContextFactory<SchoolDBContext> dbContexFactory)
        {
            _dBContexFactory = dbContexFactory;
        }

        public async Task<InstructorDTO> GetInstructorById(Guid instructorId)
        {
            using (SchoolDBContext contex = _dBContexFactory.CreateDbContext())
            {
                var instructor = await contex.Instructors
                    .FirstOrDefaultAsync(c => c.Id == instructorId);
                return instructor;
            }
        }

        public async Task<Dictionary<Guid, InstructorDTO>> GetManyByIds(IReadOnlyList<Guid> instructorIds)
        {
            using (SchoolDBContext contex = _dBContexFactory.CreateDbContext())
            {
                var instructors = await contex.Instructors
                    .Where(c => instructorIds.Contains(c.Id))
                    .ToDictionaryAsync(c => c.Id);

                return instructors;
            }
        }
    }
}
