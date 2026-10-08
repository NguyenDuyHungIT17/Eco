import type { AuthResponse } from 'shared/services/AuthService'

export type AuthState = AuthResponse | null

let token: AuthState = null

export const oauthActions = {
  setToken(value: AuthResponse) {
    token = value
    localStorage.setItem('eco.auth.token', JSON.stringify(value))
  },
  removeToken() {
    token = null
    localStorage.removeItem('eco.auth.token')
  },
}

export function getToken(): AuthState {
  if (token) return token
  const saved = localStorage.getItem('eco.auth.token')
  if (!saved) return null
  try {
    token = JSON.parse(saved) as AuthResponse
    return token
  } catch {
    oauthActions.removeToken()
    return null
  }
}
