export type AppCustomer = {
    id?: number;
    customerName?: string;
    customerNumber?: string;
    email?: string;
    phone?: string;
    organizationNumber?: string;
    localeCode?: string;
}

export type AppCustomerUser = {
    id?: number
    displayName: string
    localeCode?: string;
}

export type ErrorData = {
    errorCode?: number;
    message?: string;
}

export type Sort = {
    sortColumn?: string;
    sortDescending?: boolean;
}

export type Pagination = {
    recordsPerPage?: number;
    currentPage?: number;
    totalRecords?: number;
    sortFields?: Sort[];
}

export type ResponseData<T> = {
    data?: T;
    message?: string;
    pagination?: Pagination;
    errors?: Error[];
}

export type Error = {
    errorCode?: number;
    message?: string;
}

export type AuthenticateAppUserResponse = {
    appCustomer?: AppCustomer;
    appCustomerUser?: AppCustomerUser;
}

