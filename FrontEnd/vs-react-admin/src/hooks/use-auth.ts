import { useDispatch } from 'react-redux';
import useResponseError from '@hooks/utility-hooks/use-response-error';
import { AuthRequest } from '@models/auth-model';
import { setCredentials } from '@reducers/auth-slice';
import { useLoginMutation } from '@services/auth-service';

const useAuth = () => {
  const dispatch = useDispatch();
  const { displayError } = useResponseError();
  
  const [login, { isLoading }] = useLoginMutation();

  const onLogin = async (requestData: AuthRequest) => {
    try {
      const response = await login(requestData).unwrap();
      
      const authData = {
        accessToken: response.token,
        refreshToken: response.refreshToken,
        employeeId: response.employeeId,
        role: response.role
      };
      
      localStorage.setItem('auth', JSON.stringify(authData));
      dispatch(setCredentials(authData));
    } catch (error: any) {
      displayError(error.data);
    }
    
  };
  
  return {
    onLogin,
    isLoading
  };
};

export default useAuth;