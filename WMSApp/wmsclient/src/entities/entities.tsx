export type Customer = {
    id?: number;
    name?: string;
    number?: string;
    organizationNumber?: string;
    localeCode?: string;
}

export type User = {
    id?: number
    firstName?: string;
    lastName?: string;
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

