import {
  BaseQueryFn,
  FetchArgs,
  fetchBaseQuery,
  FetchBaseQueryError,
} from '@reduxjs/toolkit/query/react';
import { Mutex } from 'async-mutex';
import { AuthResponse } from '@models/auth-model';
import { setCredentials, logOut } from '@reducers/auth-slice';
import API_END_POINTS from '@utils/constants/api-end-points';
import { RootState } from '@/store';

const baseUrl = process.env.API_BASE_URL as string; // Base URL for Swagger API
const mutex = new Mutex();

// Configure base API
const baseApi = fetchBaseQuery({
  baseUrl,
  prepareHeaders: (headers: Headers, { getState }) => {
    const state = getState() as RootState;
    const token = state.auth.accessToken;

    if (token) {
      headers.set('Authorization', `Bearer ${token}`);
    }
    return headers;
  },
});

// Define custom query function with re-authentication logic
const baseQueryWithReAuth: BaseQueryFn<
  string | FetchArgs,
  any,
  FetchBaseQueryError
> = async (args, api, extraOptions) => {
  const isFormData = args && (args as FetchArgs).body instanceof FormData;
  const headers = new Headers();

  if (!isFormData) {
    headers.set('Content-Type', 'application/json');
  }

  const requestArgs: FetchArgs =
    typeof args === 'string' ? { url: args } : { ...args };
  requestArgs.headers = headers;

  await mutex.waitForUnlock();

  let response = await baseApi(requestArgs, api, extraOptions);

  if (response.error && response.error.status === 403) {
    if (!mutex.isLocked()) {
      const release = await mutex.acquire();

      try {
        const state = api.getState() as RootState;
        const refreshToken = state.auth.refreshToken;

        if (refreshToken) {
          const refreshResponse = await baseApi(
            {
              url: API_END_POINTS.refreshToken,
              method: 'POST',
              body: { refreshToken },
            },
            api,
            extraOptions
          );

          if (refreshResponse.error) {
            api.dispatch(logOut());
            localStorage.removeItem('auth');
          } else {
            const refreshResponseData = refreshResponse.data as AuthResponse;

            // Destructure the fields
            const { token, refreshToken, employeeId, role } = refreshResponseData;

            const authData = {
              accessToken: token,
              employeeId,
              refreshToken,
              role
            };

            api.dispatch(setCredentials(authData));
            localStorage.setItem('auth', JSON.stringify(authData));

            headers.set('Authorization', `Bearer ${token}`);
            requestArgs.headers = headers;

            response = await baseApi(requestArgs, api, extraOptions);
          }
        } else {
          api.dispatch(logOut());
          localStorage.removeItem('auth');
        }
      } finally {
        release();
      }
    } else {
      await mutex.waitForUnlock();
      const state = api.getState() as RootState;
      const token = state.auth.accessToken;

      headers.set('Authorization', `Bearer ${token}`);
      requestArgs.headers = headers;

      response = await baseApi(requestArgs, api, extraOptions);
    }
  }

  return response;
};


export default baseQueryWithReAuth;