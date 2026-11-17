import { createSlice, type PayloadAction } from '@reduxjs/toolkit';
import { jwtDecode } from 'jwt-decode';

export type AuthTokens = {
  accessToken: string;
  refreshToken: string;
  expiresInSeconds: number;
};

type JwtPayload = {
  sub: string;
  unique_name: string;
  email: string;
  role?: string | string[];
  permission?: string | string[];
};

export type Profile = { id: string; username: string; email: string };

export type OauthState = {
  accessToken: string | null;
  refreshToken: string | null;
  profile: Profile | null;
  roles: string[];
  permissions: string[];
};

const initialState: OauthState = {
  accessToken: null,
  refreshToken: null,
  profile: null,
  roles: [],
  permissions: [],
};

const toArray = (value?: string | string[]) =>
  value ? (Array.isArray(value) ? value : [value]) : [];

const oauthSlice = createSlice({
  name: 'oauth',
  initialState,
  reducers: {
    setToken: (_state, action: PayloadAction<AuthTokens>) => {
      const { accessToken, refreshToken } = action.payload;
      const jwt = jwtDecode<JwtPayload>(accessToken);
      return {
        accessToken,
        refreshToken,
        profile: { id: jwt.sub, username: jwt.unique_name, email: jwt.email },
        roles: toArray(jwt.role),
        permissions: toArray(jwt.permission),
      };
    },
    removeToken: () => initialState,
  },
});

export const { actions: oauthActions, reducer: oauthReducer } = oauthSlice;
