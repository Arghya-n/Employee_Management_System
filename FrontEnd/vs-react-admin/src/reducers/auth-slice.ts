import { createSlice, PayloadAction } from '@reduxjs/toolkit';

import { AuthState } from '@models/auth-model';

const initialState = {
  accessToken: '',
  refreshToken: '',
  employeeId: 0,
  role: ''
} as AuthState;

const authSlice = createSlice({
  name: 'auth',
  initialState: initialState,
  reducers: {
    setCredentials: (state, action: PayloadAction<AuthState>) => {
      const { accessToken, refreshToken, employeeId, role } = action.payload;

      state.accessToken = accessToken;
      state.refreshToken = refreshToken;
      state.employeeId = employeeId;
      state.role = role;
    },
    logOut: () => initialState
  }
});

export const { setCredentials, logOut } = authSlice.actions;

export default authSlice.reducer;