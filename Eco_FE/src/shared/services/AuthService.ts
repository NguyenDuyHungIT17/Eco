import { post } from 'shared/utils'
import { oauthActions } from 'store'

export type LoginRequest = {
  usernameOrEmail: string
  password: string
  deviceInfo: string
  ipAddress: string
}

export type RegisterRequest = {
  username: string
  email: string
  password: string
  fullName: string
  phoneNumber: string
}

export type AuthResponse = {
  accessToken: string
  refreshToken: string
  expiresInSeconds: number
}

class AuthService {
  async login(data: LoginRequest) {
    const token = await post<AuthResponse>('/api/auth/login', data)
    oauthActions.setToken(token)
    return token
  }

  register(data: RegisterRequest) {
    return post<{ message: string }>('/api/auth/register', data)
  }

  async refreshToken(refreshToken: string) {
    const token = await post<AuthResponse>('/api/auth/refresh-token', { refreshToken })
    oauthActions.setToken(token)
    return token
  }

  async revokeToken(refreshToken: string) {
    const result = await post<boolean>('/api/auth/revoke-token', refreshToken)
    oauthActions.removeToken()
    return result
  }
}

export const authService = new AuthService()
export { AuthService }
