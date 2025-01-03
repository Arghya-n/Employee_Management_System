const API_END_POINTS = {
  login: '/Login',
  refreshToken: '/refresh-token',
  getEmployeeDetails: (employeeId: number) => `/Employee/${employeeId}`,
  users: '/Employee',
  projects: '/Project',
};

export default API_END_POINTS;