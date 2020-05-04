export interface Pagination {
    pageNumber: number;
    pageSize: number;
    totalItems: number;
    totalPages: number;
}

export class PaginationResult<T>{
    result: T;
    pagination: Pagination;
}