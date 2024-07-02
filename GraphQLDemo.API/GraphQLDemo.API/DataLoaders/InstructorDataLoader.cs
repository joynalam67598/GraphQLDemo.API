using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Services.Instructors;
using GreenDonut;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GraphQLDemo.API.DataLoaders
{
    public class InstructorDataLoader : BatchDataLoader<Guid, InstructorDTO>
    {
        private readonly InstructorRepository _instructorsRepository;

        public InstructorDataLoader(InstructorRepository instructorsRepository,
            IBatchScheduler batchScheduler, DataLoaderOptions options = null)
            : base(batchScheduler, options)
        {
            _instructorsRepository = instructorsRepository;
        }

        protected override async Task<IReadOnlyDictionary<Guid, InstructorDTO>>
            LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            return await _instructorsRepository.GetManyByIds(keys);
        }
    }
}
