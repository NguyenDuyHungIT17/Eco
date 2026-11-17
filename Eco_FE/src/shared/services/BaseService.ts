import { http } from 'shared/utils/http';
import type { PagedRequest, PagedResult } from 'shared/types/api';

// CRUD dùng chung — service của từng module chỉ cần `extends BaseService<T>`
export class BaseService<T, TKey = string> {
  protected http = http;

  constructor(protected url: string) {}

  getMany(req: PagedRequest) {
    return http.get<PagedResult<T>>(this.url, {
      ...req,
      sort: JSON.stringify(req.sort ?? {}),
      filter: JSON.stringify(req.filter ?? {}),
    });
  }

  getById(id: TKey) {
    return http.get<T>(`${this.url}/${id}`);
  }

  create(data: Partial<T>) {
    return http.post<T>(this.url, data);
  }

  update(id: TKey, data: Partial<T>) {
    return http.put<T>(`${this.url}/${id}`, data);
  }

  del(id: TKey) {
    return http.delete<boolean>(`${this.url}/${id}`);
  }
}
