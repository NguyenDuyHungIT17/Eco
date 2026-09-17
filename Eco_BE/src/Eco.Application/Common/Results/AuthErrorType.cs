namespace Eco.Application.Common.Results;

public enum AuthErrorType
{
    InvalidCredentials,
    AccountLocked,
    AccountNotFound,
    DuplicateAccount,
    InvalidRefreshToken,
    InvalidRevokeToken
}