import { combineReducers } from '@reduxjs/toolkit';
import { oauthReducer } from './oauth.slice';

// Chỉ đưa vào store những state dùng ở nhiều nơi (token, user...).
// Dữ liệu danh sách của từng màn hình để trong component.
export const rootReducer = combineReducers({
  oauth: oauthReducer,
});
