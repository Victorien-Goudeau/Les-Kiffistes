export interface User {
    id: string;
    role: UserRole;
    name: string;
    email: string;
}

export enum UserRole {
    STUDENT = 'student',
    TEACHER = 'teacher',
}