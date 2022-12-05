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
