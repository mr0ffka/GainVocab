export interface ErrorResponse {
  statusCode: string;
  title: string;
  exception: string;
  errors: ErrorEntry[];
}

export interface ErrorEntry {
  code: string;
  title: string;
  source: string;
}

export interface GenericResponse {
  succeeded: any;
  status: string;
  message: string;
}

export interface IPagerParams {
  pageSize: number;
  pageNumber: number;
  sortBy: string;
  sortDirection: string;
}

export interface IPagedResult {
  totalCount: number;
  pageNumber: number;
  recordNumber: number;
  items: [];
}

export interface IPager {
  totalCount: number;
  pageNumber: number;
  recordNumber: number;
}
