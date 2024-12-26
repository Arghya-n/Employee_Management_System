export interface User {
  id?: number;
  name: string;
  email: string;
  password?: string;
  role: string;
  avatar?: string;
  workingStack: string;
  phoneNumber:string;
}

export type UserPartial = Partial<User>;

export interface Users {
  users: User[];
}
