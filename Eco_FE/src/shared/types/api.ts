// Khớp với ApiResponse<T> bên Eco.Application
export type ApiResponse<T> = {
  success: boolean;
  message?: string;
  data?: T;
  errors?: string[];
  errorCode?: string;
};

export type PagedResult<T> = {
  data: T[];
  meta: { page: number; pageSize: number; total: number };
};

export type PagedRequest = {
  page: number;
  pageSize: number;
  search?: string;
  sort?: Record<string, 1 | -1>;
  filter?: Record<string, unknown>;
};

export class ApiError extends Error {
  constructor(
    public status: number,
    message: string,
    public errorCode?: string,
    public errors: string[] = [],
  ) {
    super(message);
  }
}
