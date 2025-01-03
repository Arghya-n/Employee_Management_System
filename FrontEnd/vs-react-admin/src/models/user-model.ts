export interface User {
  employeeId?: number;
  name: string;
  email: string;
  password?: string;
  role: string;
  stack: string;
}

export type UserPartial = Partial<User>;

export interface Users {
  users: User[];
}
