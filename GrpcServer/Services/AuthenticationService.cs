using Grpc.Core;

namespace GrpcServer.Services
{
    public class AuthenticationService : Authentication.AuthenticationBase
    {
        public override async Task<AuthResponse> Authenticate(AuthRequest request, ServerCallContext context)
        {
            var response = AuthenticationManager.Authenticate(request);
            if (response == null)
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid user credentials"));

            return response;
        }
    }
}