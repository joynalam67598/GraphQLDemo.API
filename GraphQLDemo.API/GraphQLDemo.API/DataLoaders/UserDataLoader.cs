using FirebaseAdmin;
using FirebaseAdmin.Auth;
using GraphQLDemo.API.Schema.Queries;
using GreenDonut;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GraphQLDemo.API.DataLoaders
{
    public class UserDataLoader : BatchDataLoader<string, UserType>
    {
        private readonly FirebaseAuth _firebaseAuth;

        public UserDataLoader(FirebaseApp firebseApp,
            IBatchScheduler batchScheduler, DataLoaderOptions options = null) : base(batchScheduler, options)
        {
            _firebaseAuth = FirebaseAuth.GetAuth(firebseApp);
        }

        protected override async Task<IReadOnlyDictionary<string, UserType>> LoadBatchAsync(
            IReadOnlyList<string> userIds, CancellationToken cancellationToken)
        {
            List<UidIdentifier> userIdentifiers = userIds.Select(u => new UidIdentifier(u)).ToList();

            GetUsersResult usersResult = await _firebaseAuth.GetUsersAsync(userIdentifiers);

            return usersResult.Users.Select(u => new UserType()
            {
                Id = u.Uid,
                UserName = u.DisplayName,
                PhotoUrl = u.PhotoUrl
            }).ToDictionary(u => u.Id);

        }
    }
}
