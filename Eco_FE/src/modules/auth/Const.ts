import { ApiError } from 'shared/types/api';

// Câu thông báo theo errorCode BE trả về (AuthErrorType + VALIDATION_ERROR)
const authErrorText: Record<string, string> = {
  InvalidCredentials: 'Sai tên đăng nhập hoặc mật khẩu.',
  AccountLocked: 'Tài khoản đang bị khóa.',
  AccountNotFound: 'Tài khoản không tồn tại.',
  DuplicateAccount: 'Tên đăng nhập hoặc email đã tồn tại.',
  InvalidRefreshToken: 'Phiên đăng nhập đã hết hạn.',
};

export function getAuthErrorMessage(error: unknown): string {
  if (error instanceof ApiError) {
    if (error.errors.length > 0) {
      return error.errors.join(' ');
    }
    return (error.errorCode && authErrorText[error.errorCode]) || error.message;
  }
  return 'Có lỗi xảy ra, vui lòng thử lại.';
}
