export interface User {
    username: string;
    firstName: string;
    lastName: string;
    lockedOut: boolean;
    password: string;
    canDelete: boolean;
}