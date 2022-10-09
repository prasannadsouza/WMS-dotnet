export type Response<T> = {
    data?: T;
    message?: string;
    pagination?: Pagination;
    errors?: Error[];
}

export type Error = {
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