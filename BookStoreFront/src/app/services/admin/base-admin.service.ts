import { HttpClient, HttpParams } from '@angular/common/http';
import {
  DestroyRef,
  inject,
  signal,
  computed,
  WritableSignal,
} from '@angular/core';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { ApiResponse } from '../../models/apiResponse';
import { AlertService } from '../../ui-service/alert.service';
import { ErrorHandlerService } from '../error-handler.service';
import { catchError, EMPTY, map, merge, Subject, switchMap } from 'rxjs';

export abstract class BaseAdminService<T, TListResponse, TPagination> {
  protected http = inject(HttpClient);
  protected alertService = inject(AlertService);
  protected errorHandler = inject(ErrorHandlerService);
  protected destroyRef = inject(DestroyRef);

  protected abstract readonly apiUrl: string;
  protected abstract readonly entityName: string;

  protected itemsSig = signal<T[]>([]);
  protected itemSig = signal<T>({} as T);
  protected paginationSig: WritableSignal<TPagination>;

  // Internal signals for reactive listing
  protected pageNumberSig = signal<number>(1);
  protected pageSizeSig = signal<number>(20);
  protected searchSig = signal<string>('');
  protected refreshTrigger$ = new Subject<void>();

  items = computed(() => this.itemsSig());
  item = computed(() => this.itemSig());
  pagination = computed(() => this.paginationSig());

  createErrors = signal<string[]>([]);
  updateErrors = signal<string[]>([]);
  created = signal<boolean>(false);
  updated = signal<boolean>(false);

  constructor(initialPagination: TPagination) {
    this.paginationSig = signal<TPagination>(initialPagination);
    this.setupListingLogic();
  }

  protected abstract getItemsFromResponse(
    response: TListResponse
  ): T[] | undefined;
  protected abstract getPaginationFromResponse(
    response: TListResponse
  ): TPagination | undefined;

  /**
   * Reactive pipeline for data listing
   */
  private setupListingLogic() {
    const params$ = toObservable(
      computed(() => ({
        page: this.pageNumberSig(),
        size: this.pageSizeSig(),
        search: this.searchSig(),
      }))
    );

    const refresh$ = this.refreshTrigger$.pipe(
      map(() => ({
        page: this.pageNumberSig(),
        size: this.pageSizeSig(),
        search: this.searchSig(),
      }))
    );

    merge(params$, refresh$)
      .pipe(
        switchMap(({ page, size, search }) => {
          let params = new HttpParams()
            .set('PageNumber', page.toString())
            .set('PageSize', size.toString())
            .set('Search', search);

          // Merge with extra params from derived class
          const extraParams = this.getExtraParams();
          extraParams.keys().forEach((key) => {
            const value = extraParams.get(key);
            if (value !== null) {
              params = params.set(key, value);
            }
          });

          return this.http
            .get<ApiResponse<TListResponse>>(`${this.apiUrl}`, { params })
            .pipe(
              catchError((err) => {
                this.errorHandler.handleError(err);
                return EMPTY;
              })
            );
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe((response) => {
        if (response.data) {
          const items = this.getItemsFromResponse(response.data);
          const pagination = this.getPaginationFromResponse(response.data);
          if (items) this.itemsSig.set([...items]);
          if (pagination) this.paginationSig.set(pagination);
        }
      });
  }

  /**
   * Hook for derived classes to provide additional query parameters.
   */
  protected getExtraParams(): HttpParams {
    return new HttpParams();
  }

  /**
   * Unified method for fetching data. Now updates signals to trigger the reactive pipeline.
   */
  getAll(pageNumber: number = 1, pageSize: number = 20, search: string = '') {
    this.pageNumberSig.set(pageNumber);
    this.pageSizeSig.set(pageSize);
    this.searchSig.set(search);
  }

  /**
   * Manually trigger a refresh of the current listing
   */
  refresh() {
    this.refreshTrigger$.next();
  }

  getById(id: number) {
    this.http
      .get<ApiResponse<T>>(`${this.apiUrl}/${id}`)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (res) => {
          if (res.data) {
            this.itemSig.set(res.data);
          }
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }

  create(item: any) {
    this.http
      .post<ApiResponse<T>>(`${this.apiUrl}`, item)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (res) => {
          if (res.data) {
            this.itemsSig.update((val) => [res.data!, ...val]);
            this.createErrors.set([]);
            this.created.set(true);
            this.alertService.show(
              `${this.entityName} با موفقیت ایجاد شد`,
              'success'
            );
          }
        },
        error: (err) => {
          this.created.set(false);
          this.createErrors.set(this.errorHandler.handleError(err));
        },
      });
  }

  update(item: any, id: number) {
    this.http
      .put<ApiResponse<T>>(`${this.apiUrl}/${id}`, item)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (res) => {
          if (res.data) {
            this.itemsSig.update((val) =>
              val.map((i: any) => (i.id === id ? res.data! : i))
            );
            this.updateErrors.set([]);
            this.updated.set(true);
            this.alertService.show(
              `${this.entityName} با موفقیت به‌روزرسانی شد`,
              'success'
            );
          }
        },
        error: (err) => {
          this.updated.set(false);
          this.updateErrors.set(this.errorHandler.handleError(err));
        },
      });
  }

  delete(id: number) {
    this.http
      .delete<null>(`${this.apiUrl}/${id}`)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: () => {
          this.itemsSig.update((val) => val.filter((i: any) => i.id !== id));
          this.alertService.show(
            `${this.entityName} با موفقیت حذف شد`,
            'success'
          );
        },
        error: (err) => {
          this.errorHandler.handleError(err);
        },
      });
  }
}
